using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.People
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Person> Person { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchProgram { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.People.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchName))
            {
                query = query.Where(p => p.FullName.Contains(SearchName));
            }
            if (!string.IsNullOrWhiteSpace(SearchProgram))
            {
                query = query.Where(p => p.ProgramType != null && p.ProgramType.ToString() == SearchProgram);
            }

            Person = await query.ToListAsync();
        }
    }
}
