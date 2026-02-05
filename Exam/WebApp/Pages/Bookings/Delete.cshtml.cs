using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Bookings
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Booking Booking { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking is not null)
            {
                Booking = booking;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (booking != null)
            {
                Booking = booking;
                
                // Refund hours back to student
                var student = booking.Student;
                student.CurrentWeekUsedHours -= booking.DurationHours;
                student.TotalPracticeHours -= booking.DurationHours;
                
                // Ensure hours never go negative
                if (student.CurrentWeekUsedHours < 0)
                    student.CurrentWeekUsedHours = 0;
                if (student.TotalPracticeHours < 0)
                    student.TotalPracticeHours = 0;
                
                _context.Bookings.Remove(Booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
