using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using Plan_current_repairs.Data.Interface;
using Plan_current_repairs.Data.Models;
using System;
using System.Linq;


namespace Plan_current_repairs.Controllers.Admin
{
    [Authorize("IsAdmin")]
    public class AdminSettingsController: Controller
    {
       
        public readonly IYears years;
        public readonly Settings settings;
        public AdminSettingsController(IYears years, IOptionsMonitor<Settings> _settings)
        {
            this.years = years;
            settings = _settings.CurrentValue;
        }

        public IActionResult IndexAdminSettings()
        {
            ViewBag.AllYears = new SelectList(years.GetAllYears, "YearID", "Years");
            return View(settings);
        }

        [HttpPost]
        public IActionResult IndexAdminSettings(Settings setting)
        {
            settings.LoadSetting(setting);
            return RedirectToAction("IndexAdmin", "Admin");
        }

        //добавить год
        public IActionResult AddYear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddYear(Year year)
        {
            this.years.AddAndEditYear(year);
            return RedirectToAction("IndexAdminSettings", "AdminSettings");
        }
    }
}
