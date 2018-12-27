using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SerilogTimings;
using Microsoft.AspNetCore.Authentication;
using MVC.Models;
using System.Diagnostics;
using IdentityModel.Client;
using Shared.Enum;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.HttpClient;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using Microsoft.Extensions.Localization;

namespace MVC.Controllers
{


    public abstract class BaseController : Controller
    {
        protected IStringLocalizer _localizer;

        int _userId = int.MinValue;

        internal string BearerToken { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            GetTokenFromContext();
        }


        [HttpGet("Logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }


        [HttpGet("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        protected void GetTokenFromContext()
        {
            using (Operation.At(Serilog.Events.LogEventLevel.Debug).Time("get access token"))
            {
                BearerToken = HttpContext.GetTokenAsync("access_token").Result;
            }
        }

        protected async Task<string> GetToken(string name, string password)
        {
            string accessToken = string.Empty;
            using (var op = Operation.At(Serilog.Events.LogEventLevel.Debug).Begin("get access token"))
            {
                var tokenClient = new TokenClient(APIUrls.IdentityServerConnectToken, name, password);
                accessToken = (await tokenClient.RequestClientCredentialsAsync()).AccessToken;
                op.Complete();
            }
            return accessToken;
        }


        protected int UserId
        {
            get
            {
                if (_userId == int.MinValue && User.Identity.IsAuthenticated)
                {
                    var userId = User.Claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value;

                    _userId = Convert.ToInt32(userId);
                }

                return _userId;
            }
        }

        /// <summary>
        /// обробка результату виклику RestAPI сервісу
        /// </summary>
        /// <param name="res"></param>
        protected void AddResponseErrorsToModelState(ParsedResponse res)
        {
            if (!res.HasError)
            {
                ModelState.AddModelError("Model", res.Code.ToString());
                return;
            }

            var obj = res.Error;
            var foundInState = false;

            foreach (var entry in
                from entry in ModelState
                let matchSuffix = entry.Key
                where obj.ContainsKey(matchSuffix)
                select entry)
            {
                if (obj.ContainsKey(entry.Key))
                {
                    ModelState.AddModelError(entry.Key, obj.GetValue(entry.Key).ToString());
                    foundInState = true;
                }
            }
            const string MSG = "message";
            const string Msg = "Message";
            const string ERR = "error";
            const string MODEL = "Model";

            if (obj.ContainsKey(ERR))
            {
                ModelState.AddModelError(MODEL, obj.GetValue(ERR)[MSG].ToString());
            }
            else if (obj.ContainsKey(Msg))
            {
                ModelState.AddModelError(MODEL, obj.GetValue(Msg).ToString());
            }
            else if (!foundInState)
            {
                foreach (var item in obj.Children())
                {
                    ModelState.AddModelError(MODEL, $"{item.Path} has {obj.GetValue(item.Path).First.ToString()}");
                }
            }
        }

        protected string ToQueryString(object obj)
        {
            const string AMP = "&";
            var properties = new List<string>();

            var objType = obj.GetType();

            // check if it's collection
            if (objType.GetInterface(nameof(IEnumerable)) != null)
            {
                foreach (var item in (obj as IEnumerable))
                {
                    properties.AddRange(ToQueryToken(item));
                }
            }
            else
            {
                properties.AddRange(ToQueryToken(obj));
            }

            return String.Join(AMP, properties);
        }

        protected IEnumerable<string> ToQueryToken(object obj)
        {
            return from p in obj.GetType().GetProperties()
                   where p.GetValue(obj, null) != null
                   select $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(obj, null).ToString())}";
        }
    }
}