using System.Web.Mvc;

namespace QLDD_MVC.Areas.CBDT
{
    public class CBDTAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CBDT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CBDT_default",
                "CBDT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}