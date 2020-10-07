using System.Web.Mvc;

namespace WebApplication1.Areas.ClassDondur
{
    public class ClassDondurAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ClassDondur";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ClassDondur_default",
                "ClassDondur/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}