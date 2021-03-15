using System;
using System.Diagnostics;
using System.Linq;
using Application.Dal.Domain.Settings;
using Application.Services.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MainSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        // GET: SettingsController
        [Route("Admin/Settings")]
        public ActionResult Index()
        {
            var settings = _settingsService.GetAllSettings();
            return View(settings);
        }

        [Route("Admin/Settings/Create")]
        public ActionResult Create()
        {


            return View();
        }

        // POST: SettingsController/Create
        [HttpPost("Admin/Settings/Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] Setting setting)
        {
            if (ModelState.IsValid)
            {
                _settingsService.SetParameter(setting);
                return RedirectToAction(nameof(Index));
            }
            return View(setting);

        }

        [HttpGet("Admin/Settings/Update")]
        public IActionResult Update(string id)
        {
            return View(_settingsService.GetSettingById(id));
        }

        [HttpPost("Admin/Settings/Update")]
        public IActionResult Update(Setting setting)
        {
            var entity = _settingsService.GetSettingById(setting.Id);
            if (entity == null) ModelState.AddModelError("", "Запись не найдена");
            if (ModelState.IsValid)
            {
                entity.Value = setting.Value;
                _settingsService.SetParameter(entity);
                return RedirectToAction(nameof(Index));
            }

            return View(setting);
        }



        // GET: SettingsController/Delete/5
        [HttpGet]
        [Route("Admin/Settings/Delete")]
        public ActionResult Delete(string id)
        {
            _settingsService.DeleteSetting(id);
            return RedirectToAction("Index");
        }


    }
}
