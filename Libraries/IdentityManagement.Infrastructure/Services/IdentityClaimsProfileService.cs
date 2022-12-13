using IdentityManagement.Infrastructure.Persistence;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityManagement.Infrastructure.Services
{
    public class IdentityClaimsProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<AppUser> _factory;
        private readonly UserManager<AppUser> _userManager;

        public IdentityClaimsProfileService(IUserClaimsPrincipalFactory<AppUser> factory, UserManager<AppUser> userManager)
        {
            _factory = factory;
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.FindByIdAsync(context.Subject.Identity.GetSubjectId());
            var principal = await _factory.CreateAsync(user);

            var claims = principal.Claims.ToList();

            claims = claims.Where(p => context.RequestedClaimTypes.Contains(p.Type)).ToList();

            claims.Add(new Claim(JwtClaimTypes.GivenName, user.Name));
            claims.Add(new Claim(JwtClaimTypes.Id, user.Id.ToString()));
            claims.Add(new Claim("userEmailAddress", user.Email));
            claims.Add(new Claim(ClaimTypes.Role, "user"));

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);

            context.IsActive = user != null;
        }
    }
}
