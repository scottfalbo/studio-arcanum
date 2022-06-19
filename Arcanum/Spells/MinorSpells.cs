using Arcanum.Auth.Models;
using System.Collections.Generic;
using System.Linq;

namespace Arcanum.Spells
{
    public class MinorSpells
    {
        public static List<string> ConvertUsers(IQueryable<ApplicationUser> applicationUsers)
        {
            var users = new List<string>();
            foreach (var user in applicationUsers)
            {
                users.Add(user.UserName);
            }
            return users;
        }

        public static string GetFirstName(string name)
        {
            var splitName = name.Split(" ");
            return splitName[0];
        }
    }
}
