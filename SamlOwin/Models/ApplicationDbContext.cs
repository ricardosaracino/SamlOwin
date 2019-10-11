using System;
using System.Collections.Generic;

namespace SamlOwin.Models
{
    public class ApplicationDbContext : IDisposable
    {
        private ApplicationDbContext(IList<ApplicationUser> users)
        {
            Users = users;
        }

        public IList<ApplicationUser> Users { get; set; }

        public void Dispose()
        {
        }

        public static ApplicationDbContext Create()
        {
            //You can use any database and hook it here

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "a@a.com",
                    Email = "a@a.com",
                    Password = "test",
                    Roles = new List<string> {"Admin", "Admin2"}
                },
                new ApplicationUser
                {
                    UserName = "a@a2.com",
                    Email = "a@a2.com",
                    Password = "test2",
                    Roles = new List<string> {"Admin"}
                }
            };

            return new ApplicationDbContext(users);
        }
    }

}