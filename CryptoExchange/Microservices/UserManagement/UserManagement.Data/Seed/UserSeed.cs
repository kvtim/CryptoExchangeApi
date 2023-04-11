using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Models;

namespace UserManagement.Data.Seed
{
    public class UserSeed : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = 1,
                    FirstName = "admin",
                    LastName = "admin",
                    Email = "admin@gmail.com",
                    UserName = "admin",
                    Password = "EA3E878ECF1BEA198F692A6B7C779A637C125DC7CD651447CB0E335BD030620F:313E4270A0204A72B73AFAE74E85D7D7",
                    Role = Role.Admin,
                }
                );

            builder.HasData(
                new User
                {
                    Id = 2,
                    FirstName = "user",
                    LastName = "user",
                    Email = "user@gmail.com",
                    UserName = "user",
                    Password = "2181B73726BAF731410388404BB9FDB409F7DEDA08F52562A9079537AE794C57:F759490F73BA694FDE0344691CA47230",
                    Role = Role.User,
                }
                );
        }
    }
}

