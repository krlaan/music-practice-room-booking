using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace WebApp.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopulateSelectLists();
            return Page();
        }
        
        private void PopulateSelectLists()
        {
            var rooms = _context.Rooms.ToList();
            var roomSelectList = rooms.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = FormatRoomDisplay(r)
            }).ToList();
            ViewData["RoomId"] = roomSelectList;
            ViewData["StudentId"] = new SelectList(_context.People.Where(p => p.UserType == BLL.EUserType.Student), "Id", "FullName");
        }

        private string FormatRoomDisplay(Domain.Room room)
        {
            var features = new List<string> { room.RoomNumber, room.Size.ToString() };
            
            if (room.HasGrandPiano) features.Add("Grand Piano");
            if (room.HasUprightPiano) features.Add("Upright Piano");
            if (room.IsSoundproofed) features.Add("Soundproofed");
            
            return string.Join(" | ", features);
        }

        [BindProperty]
        public Booking Booking { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Booking.Student");
            ModelState.Remove("Booking.Room");
            
            if (!ModelState.IsValid)
            {
                PopulateSelectLists();
                return Page();
            }

            // Validate that EndTime is after StartTime
            if (Booking.EndTime <= Booking.StartTime)
            {
                ModelState.AddModelError("Booking.EndTime", "End time must be after start time.");
                PopulateSelectLists();
                return Page();
            }

            // Calculate duration
            var durationHours = Booking.DurationHours;
            
            // Auto-detect recital prep based on date (within 2 weeks before jury exam)
            Booking.IsRecitalPrep = BLL.BookingService.IsWithinRecitalPrepPeriod(Booking.StartTime);
            
            // 1. Validate duration using BLL service
            var (isValidDuration, durationError) = BLL.BookingService.ValidateBookingDuration(durationHours, Booking.IsRecitalPrep);
            if (!isValidDuration)
            {
                ModelState.AddModelError("Booking.EndTime", durationError!);
                PopulateSelectLists();
                return Page();
            }
            
            // Load student with their data
            var student = await _context.People.FindAsync(Booking.StudentId);
            if (student == null)
            {
                ModelState.AddModelError("Booking.StudentId", "Student not found.");
                PopulateSelectLists();
                return Page();
            }
            
            // Check if week has passed and reset quota
            if (student.WeekStartDate.HasValue && (DateTime.Now.Date - student.WeekStartDate.Value).Days >= 7)
            {
                student.CurrentWeekUsedHours = 0;
                student.WeekStartDate = DateTime.Now.Date;
            }
            
            // 2. Check weekly quota using BLL service
            var availableHours = BLL.PersonService.CalculateAvailableHours(
                student.WeeklyQuotaHours, 
                student.CurrentWeekUsedHours, 
                student.NoShowPenaltyHours);
                
            var (isValidQuota, quotaError) = BLL.BookingService.ValidateQuota(durationHours, availableHours);
            if (!isValidQuota)
            {
                ModelState.AddModelError("", quotaError!);
                PopulateSelectLists();
                return Page();
            }
            
            // 3. Check room availability (no overlapping bookings)
            var hasOverlap = await _context.Bookings.AnyAsync(b =>
                b.RoomId == Booking.RoomId &&
                b.Status != BLL.EBookingStatus.NoShow &&
                b.StartTime < Booking.EndTime &&
                b.EndTime > Booking.StartTime);
            
            if (hasOverlap)
            {
                ModelState.AddModelError("Booking.RoomId", "Room is already booked for this time period.");
                PopulateSelectLists();
                return Page();
            }

            // Set default values using BLL service
            // If recital prep - requires approval, otherwise confirmed
            if (Booking.IsRecitalPrep)
            {
                Booking.Status = BLL.EBookingStatus.PendingApproval;
                Booking.RequiresApproval = true;
            }
            else
            {
                Booking.Status = BLL.EBookingStatus.Confirmed;
                Booking.RequiresApproval = false;
            }
            
            // 4. Update student hours
            student.CurrentWeekUsedHours += durationHours;
            student.TotalPracticeHours += durationHours;
            
            // Ensure hours never go negative
            if (student.CurrentWeekUsedHours < 0)
                student.CurrentWeekUsedHours = 0;
            if (student.TotalPracticeHours < 0)
                student.TotalPracticeHours = 0;

            _context.People.Update(student);
            _context.Bookings.Add(Booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
