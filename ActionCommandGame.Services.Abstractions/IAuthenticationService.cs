using ActionCommandGame.DTO.Requests;
using ActionCommandGame.DTO.Results;
using System.Threading.Tasks;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> Register(UserRegisterRequestDto request);
        Task<AuthenticationResult> SignIn(UserSignInRequestDto request);
    }
}
