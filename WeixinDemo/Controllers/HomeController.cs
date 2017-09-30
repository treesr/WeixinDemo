using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeixinDemo.Filters;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace WeixinDemo.Controllers
{
    [OAuthFilter]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var userinfo = Session["userinfo"] as OAuthUserInfo;

            return View(userinfo);
        }
        public ActionResult Share()
        {
            return View();
        }
    }
}