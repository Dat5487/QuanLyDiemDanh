using System.Web.Mvc;

namespace QLDD_MVC.Areas.QTV
{
    public class QTVAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QTV";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QTV_default",
                "QTV/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}