using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OpenIdConnectPoC.Services
{
    public class AuthService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;


        public AuthService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Authorise()
        {
            var redirectUrl = "./auth/handle-authorisation";
            var provider = "Google";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task HandleAuthorisation()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if(info == null)
            {
                // Not authorised
                // redirect to error or something
                return;
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            // Sign in the user with this external login provider if the user already has a login.
            if (signInResult.Succeeded)
            {
                // Store the access token and resign in so the token is included in the cookie
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                var props = new AuthenticationProperties();
                props.StoreTokens(info.AuthenticationTokens);
                await _signInManager.SignInAsync(user, props, info.LoginProvider);
            }
            else
            {
                // auto register if not yet registered
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = new IdentityUser { UserName = email, Email = email };
                var identityResult = await _userManager.CreateAsync(user);

                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {
                        // Include the access token in the properties
                        var props = new AuthenticationProperties();
                        props.StoreTokens(info.AuthenticationTokens);
                        await _signInManager.SignInAsync(user, props, info.LoginProvider);
                    }
                }
            }
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }        
    }
}