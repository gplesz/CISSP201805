using InputValidationDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Repos.CISSP201805.InputValidationDemo.Controllers
{
    public class TestController : Controller
    {

        [HttpGet]
        public IActionResult Input()
        {
            var inputModel = new InputModel();
            return View(inputModel);
        }


        [HttpPost]
        public IActionResult Input(InputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}