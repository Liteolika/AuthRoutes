using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AuthRoutes.Core;
using AuthRoutes.IdServer.Models;

namespace AuthRoutes.IdServer.Data
{
    public static class IdenitySeeder
    {
        private const string OwnerEmail = "test@test.se";

        public static IServiceProvider SeedWithAdminUser(this IServiceProvider provider)
        {
            SeedWithAdminUserAsync(provider).Wait();
            return provider;
        }

        private static async Task SeedWithAdminUserAsync(IServiceProvider provider)
        {
            var context = provider.GetService<ApplicationDbContext>();

            var roles = new[]
            {
                IdentityConfig.Role.PublicUser,
                IdentityConfig.Role.AdminUser
            };

            var adminRoles = new[]
            {
                IdentityConfig.Role.AdminUser
            };

            var claims = AccessClaims.GetAccessClaimsWithValue(AccessClaimValues.ReadWrite);

            await SaveRolesAsync(roles, context);

            var user = context.Users.FirstOrDefault(u => u.UserName == OwnerEmail);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = OwnerEmail,
                    Id = Guid.NewGuid().ToString(),
                    Email = OwnerEmail,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    EmailConfirmed = true
                };

                var passwordHasher = provider.GetService<IPasswordHasher<ApplicationUser>>();
                user.PasswordHash = passwordHasher.HashPassword(user, "Kakan123!");
                var userStore = new UserStore<ApplicationUser>(context);
                await userStore.CreateAsync(user);
            }

            await AssignClaimsAsync(provider, user.Id, claims);
            await AssignRolesAsync(provider, user.Id, adminRoles);

            await context.SaveChangesAsync();
        }

        private static async Task SaveRolesAsync(IEnumerable<string> roles, ApplicationDbContext context)
        {
            foreach (var role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    await roleStore.CreateAsync(new IdentityRole(role) { NormalizedName = role.ToUpperInvariant() });
                }
            }
        }

        private static async Task<IdentityResult> AssignClaimsAsync(IServiceProvider provider, string userId, IEnumerable<Claim> claims)
        {
            var userManager = provider.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByIdAsync(userId);
            var existingClaims = await userManager.GetClaimsAsync(user);
            var missingClaims = claims.Where(c => existingClaims.All(exc => exc.Type != c.Type));
            var result = await userManager.AddClaimsAsync(user, missingClaims);

            return result;
        }

        private static async Task<IdentityResult> AssignRolesAsync(IServiceProvider provider, string userId, IEnumerable<string> roles)
        {
            var userManager = provider.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByIdAsync(userId);
            var existingRoles = await userManager.GetRolesAsync(user);
            var missingRoles = roles.Where(r => existingRoles.All(exr => exr != r));
            var result = await userManager.AddToRolesAsync(user, missingRoles);

            return result;
        }
    }
}
