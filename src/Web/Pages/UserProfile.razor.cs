using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Therasim.Application.Common.Interfaces;
using Therasim.Infrastructure.Data;
using Therasim.Web.Models;

namespace Therasim.Web.Pages
{
    public partial class UserProfile : ComponentBase
    {
        [SupplyParameterFromForm] private CreateUserProfile CreateUserProfile { get; set; } = new();
        [Inject] private IApplicationDbContext ApplicationDbContext { get; set; } = null!;
        [CascadingParameter] private Task<AuthenticationState>? AuthenticationState { get; set; }

        private Domain.Entities.UserProfile _userProfile = null!;

        protected override async Task OnInitializedAsync()
        {
            if (AuthenticationState is not null)
            {
                var state = await AuthenticationState;
                var principal = state.User;
                if (principal.Identity?.IsAuthenticated == true)
                {
                    var userId = state.User.Claims
                        .Where(c => c.Type.Equals(@"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))
                        .Select(c => c.Value)
                        .FirstOrDefault() ?? string.Empty;

                    var email = state.User.Claims
                        .Where(c => c.Type.Equals(@"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))
                        .Select(c => c.Value)
                        .FirstOrDefault() ?? string.Empty;

                    var name = state.User.Claims
                        .Where(c => c.Type.Equals(@"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/"))
                        .Select(c => c.Value)
                        .FirstOrDefault() ?? string.Empty;

                    var userProfile = new Domain.Entities.UserProfile
                    {
                        UserId = userId,
                        Email = email,
                        Name = name
                    };

                    ApplicationDbContext.UserProfiles.Add(userProfile);
                    await ApplicationDbContext.SaveChangesAsync(CancellationToken.None);

                    _userProfile = userProfile;
                }
            }
        }
    }
}
