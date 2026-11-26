using Download.Domain.Entities;
using Download.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;



namespace Download.Infrastructure.Identity
{
    public static class SeedData
    {
        

        public static async Task SeedAsync(DownloadsDBContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    UserName = "yourname",
                    Email = "yourmail@gmail.com",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null!, "PASS123456"),
                    NormalizedUserName = "YOURNAME",
                    NormalizedEmail = "YOURMAIL@GMAIL.COM",
                    EmailConfirmed = false,
                    LockoutEnd = null

                });
                await context.SaveChangesAsync();
            }
        }




    }
}
