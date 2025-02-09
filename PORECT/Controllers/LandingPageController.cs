using Microsoft.AspNetCore.Mvc;

namespace PORECT.Controllers
{
	public class LandingPageController : ParentController
    {
        public LandingPageController(IServiceProvider serviceProvider, IHttpContextAccessor contextAccessor) : base(contextAccessor, serviceProvider)
        {

        }

		public IActionResult Index()
		{
			try
            {
                var fullname = HttpContext.Session.GetString("Fullname");
                if (string.IsNullOrEmpty(fullname))
                {
                    return RedirectToAction("Login", "Login");
                }

                return View();
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "LandingPage", "Form Index");
                throw;
            }
        }
	}
}
