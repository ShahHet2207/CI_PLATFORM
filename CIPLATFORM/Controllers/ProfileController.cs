﻿using CI_PLATFORM.Entities.Models;
using CI_PLATFORM.Entities.ViewModels;
using CI_PLATFORM.Repository.Interface;
using Microsoft.AspNetCore.Mvc;


namespace CIPLATFORM.Controllers
{
    public class ProfileController : Controller
    {
        public readonly IProfileRepository _ProfileRepository;
        public readonly IPlatformRepository _PlatformRepository;
        public ProfileController(IProfileRepository ProfileRepository, IPlatformRepository PlatformRepository)
        {
            _ProfileRepository = ProfileRepository;
            _PlatformRepository = PlatformRepository;
        }
        public IActionResult Profile()
        {
            string? name = HttpContext.Session.GetString("Uname");
            ViewBag.Uname = name;

            string? avtar = HttpContext.Session.GetString("Avtar");
            ViewBag.Avtar = avtar;

            List<Country> countries = _PlatformRepository.GetCountryData();
            ViewBag.countries = countries;

            List<City> Cities = _PlatformRepository.GetCitys();
            ViewBag.Cities = Cities;

            if (name != null)
            {
                int? UserId = (int)HttpContext.Session.GetInt32("UId");
                ViewBag.UId = UserId;

                ProfileViewModel pm = _ProfileRepository.getUser(@ViewBag.UId);
                return View(pm);
            }

            return View();
        }
        [HttpPost]
        public IActionResult Profile(ProfileViewModel obj,int save)
        {
            List<Country> countries = _PlatformRepository.GetCountryData();
            ViewBag.countries = countries;

            List<City> Cities = _PlatformRepository.GetCitys();
            ViewBag.Cities = Cities;
            string? name = HttpContext.Session.GetString("Uname");
            ViewBag.Uname = name;

            string? avtar = HttpContext.Session.GetString("Avtar");
            ViewBag.Avtar = avtar;

            if (name != null)
            {
                int? UserId = (int)HttpContext.Session.GetInt32("UId");
                ViewBag.UId = UserId;
            }
            if (!ModelState.ContainsKey("resetPass"))
            {
                bool resetpass = _ProfileRepository.saveProfile(obj, save, @ViewBag.UId);

                if (resetpass)
                {
                    TempData["true"] = "password updated";

                }
                else
                {
                    TempData["false"] = "entered password is wrong";
                    
                }

                return View();
            }

            if (save == 1)
            {
                save = 0;
            }

            bool saveprofile = _ProfileRepository.saveProfile(obj, save, @ViewBag.UId);

            return View();
        }
    }
}