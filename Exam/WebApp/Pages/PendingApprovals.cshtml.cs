using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages
{
    public class PendingApprovalsModel : PageModel
    {
        private readonly AppDbContext _context;

        public PendingApprovalsModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Booking> PendingBookings { get; set; } = default!;

        public async Task OnGetAsync()
        {
            // Get all bookings that are pending approval
            PendingBookings = await _context.Bookings
                .Include(b => b.Student)
                .Include(b => b.Room)
                .Where(b => b.Status == BLL.EBookingStatus.PendingApproval)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostApproveAsync(Guid bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Student)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            // Approve the booking
            booking.Status = BLL.EBookingStatus.Confirmed;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Bookings/Index");
        }

        public async Task<IActionResult> OnPostRejectAsync(Guid bookingId)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            // Get student to refund hours
            var student = await _context.People.FindAsync(booking.StudentId);
            if (student != null)
            {
                student.CurrentWeekUsedHours -= booking.DurationHours;
                student.TotalPracticeHours -= booking.DurationHours;

                // Ensure hours never go negative
                if (student.CurrentWeekUsedHours < 0)
                    student.CurrentWeekUsedHours = 0;
                if (student.TotalPracticeHours < 0)
                    student.TotalPracticeHours = 0;

                _context.People.Update(student);
            }

            // Reject the booking
            booking.Status = BLL.EBookingStatus.Rejected;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Bookings/Index");
        }
    }
}