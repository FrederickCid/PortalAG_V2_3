using PortalAG_V2.Shared.Requests.User;
using PortalAG_V2.Shared.Wrapper;
using System.Security.Claims;

namespace PortalAG_V2.Auth
{
    public interface IAuthenticationManager
    {

        Task<IResult> Login(TokenRequest model);

        Task<IResult> Logout();

        Task<string> RefreshToken();

        Task<string> TryRefreshToken();

        Task<string> TryForceRefreshToken();

        Task<ClaimsPrincipal> CurrentUser();
    }
}
