using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace ServerSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public ActionResult Index()
        {
            string authority = new Uri(this.Request.GetDisplayUrl())
                .GetLeftPart(UriPartial.Authority);

            if (_environment.IsDevelopment())
            {
                var dictionary = new Dictionary<string, object>
                {
                    ["oidc_discovery_url"] = authority + "/.well-known/openid-configuration",
                    ["debug"] = new Dictionary<string, string>
                    {
                        ["client"] = this.Url.Link("debug.client.get", null),
                        ["user"] = this.Url.Link("debug.user.get", null),
                        ["user_claim"] = this.Url.Link("debug.user_claim.get", null),
                        ["api_scope"] = this.Url.Link("debug.api_scope.get", null),
                        ["api_resource"] = this.Url.Link("debug.api_resource.get", null),
                        ["identity_resource"] = this.Url.Link("debug.identity_resource.get", null)
                    }
                };
                return Json(dictionary);
            }
            else
            {
                return Ok();
            }
        }
    }
}
