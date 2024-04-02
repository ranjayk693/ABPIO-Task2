using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;

namespace PasswordlessAuthentication.OpenIddict
{
    public class RoleDataSeed : IDataSeedContributor, ITransientDependency
    {
        private readonly IdentityRole _defaultRole;
        private readonly IdentityRoleManager _roleManager;
        private readonly IdentityUserManager _userManager;
        
        public RoleDataSeed(IdentityRoleManager roleManager, IdentityUserManager userManager , IdentityRole defaultRole)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _defaultRole = defaultRole;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            /*Adding role*/
            var admin = await _roleManager.FindByNameAsync("admin");
            if (admin == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), "admin"));
            }

            var hr = await _roleManager.FindByNameAsync("hr");
            if (hr == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), "hr"));
                _defaultRole.IsDefault = true;   //setting the defualt role as true
            }


            /*Creating user of admin and seeding the data*/

            var user = new IdentityUser(Guid.NewGuid(), "ranjay", "ranjay@admin.com");


            var admin_name = await _userManager.CreateAsync(user, "Ranjay1!");

            if (admin_name.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "admin");
            }
            else
            {
                throw new Exception("Failed to create user");
            }
        }
    }
    
}
