using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages
{
    public class NoShowManagementModel : PageModel
    {
        private readonly AppDbContext _context;

        public NoShowManagementModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Booking> PastBookings { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var now = DateTime.Now;
            
            PastBookings = await _context.Bookings
                .Include(b => b.Student)
                .Include(b => b.Room)
                .Where(b => b.Status == BLL.EBookingStatus.Confirmed && b.StartTime < now)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostMarkNoShowAsync(Guid bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Student)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
            
            if (booking == null)
            {
                return NotFound();
            }

            // Get student to apply penalty
            var student = booking.Student;
            
            // Mark as no-show
            booking.Status = BLL.EBookingStatus.NoShow;
            
            // Apply penalties
            student.NoShowCount++;
            
            _context.Bookings.Update(booking);
            _context.People.Update(student);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
