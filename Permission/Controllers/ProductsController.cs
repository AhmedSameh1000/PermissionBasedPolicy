using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Permission.Constant;

namespace Permission.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = Permissions.Products.Edit)]
        public IActionResult Edit()
        {
            return View();
        }
    }
}