using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace AspReact
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;

        public ProfileService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            var roles = await _userManager.GetRolesAsync(user);

            IList<Claim> roleClaims = new List<Claim>();
            foreach (var role in roles) roleClaims.Add(new Claim(JwtClaimTypes.Role, role));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Name, user.UserName));
            context.IssuedClaims.AddRange(roleClaims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}