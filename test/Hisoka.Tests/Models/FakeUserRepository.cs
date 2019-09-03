using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hisoka.Tests.Models
{
    public class FakeUserRepository
    {
        public IQueryable<User> GetUsers()
        {
            var users = new Faker<User>("pt_BR")
                        .RuleFor(x => x.Id, f => f.IndexFaker)
                        .RuleFor(x => x.Email, f => f.Person.Email)
                        .RuleFor(x => x.DateOfBirth, f => f.Date.Between(new DateTime(1994, 5, 1), new DateTime(2010, 12, 31)))
                        .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                        .RuleFor(x => x.LastName, f => f.Name.LastName())
                        .RuleFor(x => x.Roles, f => GetRoles(f.IndexFaker));

            return users.Generate(100).AsQueryable();
        }

        private List<Role> GetRoles(int index)
        {
            var roles = new List<Role>()
            {
                new Role { Name = "User", Permissions = new List<Permission> { new Permission { Name = "User" } } },
                new Role { Name = "Guest", Permissions = new List<Permission> { new Permission { Name = "Guest" } } },
            };

            if (index % 2 == 0)
                roles.Add(new Role { Name = "Admin", Permissions = new List<Permission> { new Permission { Name = "Admin" } } });
            else
                roles.Add(new Role { Name = "Developer", Permissions = new List<Permission> { new Permission { Name = "Developer" } } });

            return roles;
        }
    }
}
