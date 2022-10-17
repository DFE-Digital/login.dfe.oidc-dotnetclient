using DfeSigninDemo.DsiAuthentication;
using DfeSigninDemo.DsiAuthentication.DsiClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DfeSigninDemo.Controllers
{
	[Authorize]

    //Add authorisation with a policy
    //[Authorize(Policy = #policy_name#)]
    public class HomeController : Controller
	{
		private readonly IDfESignInConfig _dfESignInConfig;
        
		public HomeController(IDfESignInConfig dfESignInConfig)
        {
			_dfESignInConfig = dfESignInConfig;

		}

		[HttpGet]
		public async Task<IActionResult> Claims()
		{
            ViewData["Username"] = User.Claims.Where(c => c.Type.Contains("emailaddress"))
                   .Select(c => c.Value).SingleOrDefault();
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            string idToken = await HttpContext.GetTokenAsync("id_token");

            ViewData["IdToken"] = idToken;
            ViewData["AccessToken"] = accessToken;
            ViewData["APICall"] = "";


            //API call demonstration code from Controller
            //Get userId and organization id from claims
            string userId = User.Claims.Where(c => c.Type.Contains(Constants.Claim.NameIdentifier)).Select(c => c.Value).SingleOrDefault();
            var userOrganization = JsonConvert.DeserializeObject<Organisation>
            (
                User.Claims.Where(c => c.Type == Constants.Claim.Organisation)
                .Select(c => c.Value)
                .FirstOrDefault()
            );
            //Create DfE Signin Client
            var clientFactory = new DfESignInClientFactory(_dfESignInConfig);
            DfESignInClient dfeSignInClient = clientFactory.CreateDfESignInClient(userId, userOrganization.Id.ToString());

            //Call the API
            //See appsettings for config used to call it
            HttpResponseMessage response = await dfeSignInClient.HttpClient.GetAsync(new Uri($"{dfeSignInClient.ServiceUrl}/users/{dfeSignInClient.UserId}/organisations"));
            if (response.IsSuccessStatusCode)
            {
                //Read result
                ViewData["APICall"] = await response.Content.ReadAsStringAsync();
            }

            return View();
		}
	}
}
