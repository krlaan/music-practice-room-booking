using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain;

namespace WebApp.Pages.People
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
            return Page();
        }

        [BindProperty]
        public Person Person { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Set weekly quota based on program type using BLL service
            if (Person.UserType == BLL.EUserType.Student && Person.ProgramType.HasValue)
            {
                Person.WeeklyQuotaHours = (int?) BLL.PersonService.GetWeeklyQuotaForProgram(Person.ProgramType);
            }
            else
            {
                Person.WeeklyQuotaHours = null;
            }
            
            // Initialize tracking fields for students
            if (Person.UserType == BLL.EUserType.Student)
            {
                Person.CurrentWeekUsedHours = 0;
                Person.TotalPracticeHours = 0;
                Person.NoShowCount = 0;
                Person.PenaltyMultiplier = 1.0;
                Person.WeekStartDate = DateTime.Now.Date;
            }

            _context.People.Add(Person);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
