using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Helpers;


namespace WeixinDemo.Controllers
{
    public class OAuthController : Controller
    {
        private string _appId = "wx3fd874185144a158";
        private string _appsecret = "a4effc4676f40aad5797a688971ea930";
        private string _domain = "http://wx.guodeshijie.top";
     

        // GET: OAuth
        public ActionResult Login(string requestUrl)
        {
            //生成一个回调url，供微信授权完成后，返回给我们信息的接受地址
            var redirectUrl = $"{_domain}{Url.Action("CallBack",new { requestUrl})}";
            //生成一个验证码
            var state = "wx" + DateTime.Now.Millisecond;
            Session["state"] = state;
            //生成微信授权页面URL
            var url = OAuthApi.GetAuthorizeUrl(_appId,redirectUrl, state, OAuthScope.snsapi_base);
            return Redirect(url);
        }
        public ActionResult CallBack(string code, string state, string requestUrl)
        {
            //判断验证码
            if (state != (string)Session["state"])
            {
                return Content("非法访问！");
            }
            //判断code是不是有数据，这里有时可能为空，空的话就是失败了
            if (string.IsNullOrEmpty(code))
            {
                return Content("授权失败！");
            }
            //这是通过code获取accessToken令牌
            var OAuthAccessToken = OAuthApi.GetAccessToken(_appId,_appsecret,code);
            //判断有没有获取成功
            if (OAuthAccessToken.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                return Content("授权失败！");
            }
            //获取令牌成功，就说明我们以登陆
            Session["AccessToken"] = OAuthAccessToken;
            //常识获取用户信息，如果能获取到，就说明我们这个令牌是有权限的，如果没有获取到，令牌就没有权限。
            //但是不管令牌是否有权限，OpenID都是一样的
            
            try
            {
               var userInfo = OAuthApi.GetUserInfo(OAuthAccessToken.access_token, OAuthAccessToken.openid);
                //如果不为控，就说明获取成功，同时也说明了，我们的令牌是有权限的
                Session["userinfo"] = userInfo;
                return Redirect(requestUrl);
            }
            catch (Exception)
            {
                var redirectUrl = $"{_domain}{ Url.Action("CallBack", new { requestUrl })}";
                var url = OAuthApi.GetAuthorizeUrl(_appId, redirectUrl, state, OAuthScope.snsapi_userinfo);
                return Redirect(url);
            }
     
     ;

        }

        public ActionResult JsSdkConfig()
        {
            var url = _domain+Request.RawUrl;
            var config = JSSDKHelper.GetJsSdkUiPackage(_appId, _appsecret, url);
            return PartialView(config);
        }
    }
}