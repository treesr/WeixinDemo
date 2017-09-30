using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeixinDemo.Filters
{
    public class OAuthFilterAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //判断是否登陆授权 AccessToken标识代表着用户有登陆， 这里放着用户的令牌
            if (filterContext.HttpContext.Session["AccessToken"] != null) return;
            //开始授权 

            //requestUrl代表用户访问的请求地址，需要保存，以便于授权完成后，跳到这个地址
            var requestUrl = filterContext.HttpContext.Request.RawUrl;
            //未登陆，所以跳转登陆页
            var urlHelper = new UrlHelper(filterContext.RequestContext);
            filterContext.Result = new RedirectResult(urlHelper.Action("Login", "OAuth", new { requestUrl }));
        }
    }
}