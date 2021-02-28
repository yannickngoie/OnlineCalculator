using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineCalculator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace OnlineCalculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            var inputValue = Request.Query["txt"].ToString();
            var charInputValue = inputValue.ToCharArray();
            var memoryIndicator = Request.Query["memoryType"].ToString();
            string MemoryValue = Request.Query["memoryValue"].ToString();
            string calcHistory = Request.Query["txtHistory"].ToString();
            ServiceResponse service = new ServiceResponse();
            Execute execute = new Execute();

            var hasMemoryInfo = new Func<bool>(() => (MemoryValue != null && MemoryValue != string.Empty)
                   && (inputValue != string.Empty && inputValue != null)
                   );

            service = Execute.HasValidInput(inputValue, memoryIndicator, hasMemoryInfo, MemoryValue,calcHistory);
           
            if (service.Result)
            {
                ViewBag.memoryData = service.MemoryResult == 0 ? string.Empty:service.MemoryResult.ToString();
                ViewBag.result = service.EquationResult.ToString();
                ViewBag.calcHistoryInfo = service.CalcHistory;

               
                return View();
            }
            else
            
            { 
                if (service.Message == MessageType.MissingValue.ToString() && service.Result == false)
                {
                    ViewBag.memoryData = service.MemoryResult == 0 ? string.Empty : service.MemoryResult.ToString();

                    ViewBag.flag = 0;

                    ViewBag.calcHistoryInfo = service.CalcHistory;
                }

                ViewBag.result = Request.Query["txt"].ToString();

                return View();
            }          
        }


 
        public IActionResult Privacy()
        {
            ViewBag.result = "Yes";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
