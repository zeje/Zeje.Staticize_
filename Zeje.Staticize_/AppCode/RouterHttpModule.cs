using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Zeje.Staticize_.AppCode;

namespace Zeje.Staticize_.AppCode
{
    public class RouterHttpModule : IHttpModule
    {
        public void Init(HttpApplication application)
        {
            application.BeginRequest += this.Application_BeginRequest; //注册事件
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            string filePath = context.Request.FilePath;
            string fileExtension = VirtualPathUtility.GetExtension(filePath);
            bool isFile = !string.IsNullOrEmpty(fileExtension);

            //如果当前请求的不是资源文件、不是后台管理、请求中没有表单信息、静态页面存在，则返回静态页面
            if ((!isFile || fileExtension == ".zeje") && !filePath.ToLower().StartsWith("/admin") && context.Request.Form.Count.Equals(0))
            {
                string htmlPath = HtmlPageHelper.GetHtmlPathFromUrl(context.Request.Url.AbsoluteUri);
                if (File.Exists(htmlPath))
                {
                    context.Response.WriteFile(htmlPath);
                    context.Response.End();
                }
                else
                {
                    context.Response.Filter = new HttpResponseFilterWrapper(context.Response.Filter, context);
                }
            }
        }

        public void Dispose() { }

    }
}