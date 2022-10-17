using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DfeSigninDemo.Controllers
{
    public class StartController : Controller
	{
		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
		}

		[Authorize]
        public async Task<IActionResult> SignOut()
        {
            return new SignOutResult(new[] { OpenIdConnectDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme });
        }
	}
}