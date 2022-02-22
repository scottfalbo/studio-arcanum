using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Arcanum.Pages
{
    public class MovingImagesTestModel : PageModel
    {

        public Parent Things;

        public void OnGet()
        {
            Things = new Parent()
            {
                Id = 1,
                Items = new List<Child>()
            };
            for (int i = 0; i < 10; i++)
            {
                Child child = new Child()
                {
                    Id = i,
                    Description = GetLetter(i),
                    IsEnabled = true,
                    Order = i
                };
                Things.Items.Add(child);
            }
        }

        public IActionResult OnPostMoveItems([FromBody] List<Child> itemsOrder)
        {
            Things = new Parent();
            Things.Items = new List<Child>();
            Things.Items = itemsOrder;
            return Redirect("MovingImageTest");
        }

        private string GetLetter(int i) =>
            ((char)(i + 65)).ToString().ToUpper();
    }

    public class Parent
    {
        public int Id { get; set; }
        public List<Child> Items { get; set; }
    }

    public class Child
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public int Order { get; set; }
    }
}
