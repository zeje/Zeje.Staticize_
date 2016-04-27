using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zeje.Staticize_.AppCode;

namespace Zeje.Staticize_.Areas.admin.Controllers
{
    public class sysController : Controller
    {
        // GET: admin/sys
        public ActionResult Index()
        {
            HtmlPageHelper hph = new HtmlPageHelper();
            try
            {
                hph.SaveHtmlCode(HtmlPageHelper.GetUrlRoot(HttpContext.Request.Url));
                return Content("递归处理");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}