using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace WebApp.Pages.Bookings
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty] public Booking Booking { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            Booking = booking;
            ViewData["ApprovedByInstructorId"] = new SelectList(_context.People, "Id", "FullName");

            var rooms = _context.Rooms.ToList();
            var roomSelectList = rooms.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = FormatRoomDisplay(r)
            }).ToList();

            ViewData["RoomId"] = roomSelectList;
            ViewData["StudentId"] = new SelectList(_context.People, "Id", "FullName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Booking.Student");
            ModelState.Remove("Booking.Room");
            ModelState.Remove("Booking.ApprovedByInstructor");

            if (!ModelState.IsValid)
            {
                var rooms = _context.Rooms.ToList();
                var roomSelectList = rooms.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = FormatRoomDisplay(r)
                }).ToList();

                ViewData["RoomId"] = roomSelectList;
                ViewData["StudentId"] = new SelectList(_context.People, "Id", "FullName");

                return Page();
            }

            // Set default values for fields not in the form
            Booking.Status = BLL.EBookingStatus.Confirmed;
            Booking.RequiresApproval = false;

            _context.Attach(Booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(Booking.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToPage("./Index");
        }

        private string FormatRoomDisplay(Domain.Room room)
        {
            var features = new List<string> { room.RoomNumber, room.Size.ToString() };

            if (room.HasGrandPiano) features.Add("Grand Piano");
            if (room.HasUprightPiano) features.Add("Upright Piano");
            if (room.IsSoundproofed) features.Add("Soundproofed");

            return string.Join(" | ", features);
        }

        private bool BookingExists(Guid id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}