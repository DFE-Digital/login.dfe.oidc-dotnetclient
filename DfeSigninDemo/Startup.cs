//using DfE.DC.Core.Web.Authentication.DfESignIn;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using DfeSigninDemo.DsiAuthentication;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using DfeSigninDemo.DsiAuthentication.DsiClient;

namespace DfeSigninDemo
{
    public class Startup
    {
        #region Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Publics

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region DfESignIn

            var dfeSignInConfig = new DfESignInConfig();
            Configuration.GetSection("DfESignInConfig").Bind(dfeSignInConfig);
            AddDfESignInAuthentication(services, dfeSignInConfig);
            services.AddSingleton<IDfESignInConfig>(dfeSignInConfig);

            #endregion

            services.AddAuthorization();
            services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(120);
            });

            services.AddRouting();
            services.AddMvc();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Start}/{action=Index}");
            });
        }

        #endregion

        #region Privates


        private void AddDfESignInAuthentication(IServiceCollection services, DfESignInConfig dfeSignInConfiguration)
        {
            services.AddHttpClient();
            services.AddHttpClient<DfESignInClient>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    UseDefaultCredentials = false,
                    PreAuthenticate = true,
                    Proxy = string.IsNullOrWhiteSpace(dfeSignInConfiguration.APIServiceProxyUrl) ? new WebProxy() : new WebProxy(new Uri(dfeSignInConfiguration.APIServiceProxyUrl, UriKind.Absolute))
                });
            services.AddHttpContextAccessor();
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
           .AddOpenIdConnect(options =>
           {
               options.ClientId = dfeSignInConfiguration.ClientId;
               options.ClientSecret = dfeSignInConfiguration.ClientSecret;
               options.Authority = dfeSignInConfiguration.Authority;
               options.MetadataAddress = dfeSignInConfiguration.MetaDataUrl;
               options.CallbackPath = new PathString(dfeSignInConfiguration.CallbackUrl);
               options.SignedOutRedirectUri = new PathString(dfeSignInConfiguration.SignoutRedirectUrl);
               options.SignedOutCallbackPath = new PathString(dfeSignInConfiguration.SignoutCallbackUrl);
               options.ResponseType = OpenIdConnectResponseType.Code;

               options.Scope.Clear();
               foreach (string scope in dfeSignInConfiguration.Scopes)
               {
                   options.Scope.Add(scope);
               }

               options.GetClaimsFromUserInfoEndpoint = dfeSignInConfiguration.GetClaimsFromUserInfoEndpoint;
               options.SaveTokens = dfeSignInConfiguration.SaveTokens;

               //API call demonstration code from Startup by extending options to include OpenIfConnectEvents
               //Get role details from claims
               options.Events = new OpenIdConnectEvents()
               {
                   OnTokenValidated = async (tokenValidatedContext) =>
                   {
                       if (tokenValidatedContext.Principal.Identity.IsAuthenticated)
                       {
                           var claims = tokenValidatedContext.Principal.Claims;
                           var userOrganization = JsonConvert.DeserializeObject<Organisation>
                               (
                                   claims.Where(c => c.Type == Constants.Claim.Organisation)
                                   .Select(c => c.Value)
                                   .FirstOrDefault()
                               );

                           var clientFactory = new DfESignInClientFactory(dfeSignInConfiguration);
                           string userId = claims.Where(c => c.Type.Contains(Constants.Claim.NameIdentifier)).Select(c => c.Value).SingleOrDefault();
                           DfESignInClient dfeSignInClient = clientFactory.CreateDfESignInClient(userId, userOrganization.Id.ToString());

                           var res = await dfeSignInClient.HttpClient.GetAsync(dfeSignInClient.TargetAddress);

                           if (res.IsSuccessStatusCode)
                           {
                               var resMessage = await res.Content.ReadAsStringAsync();

                               var apiServiceResponse = JsonConvert.DeserializeObject<ApiServiceResponse>(resMessage);
                               var roleClaims = new List<Claim>();
                               foreach (var role in apiServiceResponse.Roles)
                               {
                                   if (role.Status.Id.Equals(1))
                                   {
                                       roleClaims.Add(new Claim(Constants.Claim.RoleCode, role.Code, ClaimTypes.Role, tokenValidatedContext.Options.ClientId));
                                       roleClaims.Add(new Claim(Constants.Claim.RoleId, role.Id.ToString(), ClaimTypes.Role, tokenValidatedContext.Options.ClientId));
                                       roleClaims.Add(new Claim(Constants.Claim.RoleName, role.Name, ClaimTypes.Role, tokenValidatedContext.Options.ClientId));
                                       roleClaims.Add(new Claim(Constants.Claim.RoleNumericId, role.NumericId.ToString(), ClaimTypes.Role, tokenValidatedContext.Options.ClientId));
                                   }
                               }

                               var roleIdentity = new ClaimsIdentity(roleClaims);
                               tokenValidatedContext.Principal.AddIdentity(roleIdentity);
                           }
                       }
                   }
               };
           })
           .AddCookie(options =>
           {
               options.Cookie.Name = dfeSignInConfiguration.CookieName;
               options.ExpireTimeSpan = TimeSpan.FromMinutes(dfeSignInConfiguration.CookieExpiration);
               options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
               options.SlidingExpiration = dfeSignInConfiguration.SlidingExpiration;
           });

           //Sample to add authorisation to restrict user access to service based on a claim value
           //services.AddAuthorization(options =>
           //{
           //    options.AddPolicy("#policy_name#",
           //         policy => policy.RequireClaim("#claim_name#", "#claim_value#"));
           //});
        }

        #endregion
    }
}
