using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zeje.Staticize_.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? pageIndex, int? pageSize)
        {
            pageIndex = pageIndex ?? 0;
            pageSize = pageSize ?? 10;

            using (Model.BlogEntities db = new Model.BlogEntities())
            {
                var lstPost = db.BlogPost.OrderBy(it => it.updateTime).Skip(pageIndex.GetValueOrDefault() * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToList();
                ViewBag.lstPost = lstPost;
                int totalCount = db.BlogPost.Count();
                return View(new AppCode.SimplePageObj<Model.BlogPost>()
                {
                    pageIndex = pageIndex.GetValueOrDefault(),
                    pageSize = pageSize.GetValueOrDefault(),
                    rows = lstPost,
                    totalCount = totalCount
                });
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}