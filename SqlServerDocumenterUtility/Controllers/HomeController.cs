using System;
using System.Web;
using System.Web.Mvc;

namespace SqlServerDocumenterUtility.Controllers
{
    public class HomeController : Controller
    {
        private const string titleTemplate = "SQL Server Utility | {0}";

        /// <summary>
        /// Controller action to get the default view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = String.Format(titleTemplate, "Default") ;
            return View();
        }

        /// <summary>
        /// Controller action to get the partial view for the about section
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Title = String.Format(titleTemplate, "About");
            return PartialView("About");            
        }
        
        /// <summary>
        /// Controller action to get the partial view for the ipsum section
        /// </summary>
        /// <returns></returns>
        public ActionResult Ipsum()
        {
            ViewBag.Title = String.Format(titleTemplate, "Ipsum");
            return PartialView("ForceIpsum");
        }
       
        /// <summary>
        /// Controller action to get the partial view for the Readme section. 
        /// It returns the html file path for the compiled README markdown file
        /// in the apps root.
        /// </summary>
        /// <returns></returns>
        public ActionResult Readme()
        {
            HttpContext.Response.AddHeader("Content-Disposition", new System.Net.Mime.ContentDisposition
            {
                Inline = true,
                FileName = "README.html"
            }.ToString());
            return File("~/README.html", "text/plain");
        }
    }
}
