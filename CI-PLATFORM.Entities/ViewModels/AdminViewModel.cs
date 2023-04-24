﻿using CI_PLATFORM.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_PLATFORM.Entities.ViewModels
{
    public class AdminViewModel
    {
        public List<User> users { get; set; } = new List<User>();
        public User user { get; set; } = new User();
        public List<City> cities { get; set; } = new List<City>();
        public List<Country> countries { get; set; } = new List<Country>();
        public IFormFile? Avatarfile { get; set; }
        public List<Mission> missions { get; set; } = new List<Mission>();
        
        public List<CmsPage> CmsPages { get; set; } = new List<CmsPage>();
        public CmsPage CmsPage { get; set; } = new CmsPage();
        public List<MissionApplication>? missionapplications { get; set; } = new List<MissionApplication>();
        public List<Story> stories { get; set; } = new List<Story>();
        public List<Skill> skills { get; set; } = new List<Skill>();
        public Skill Skill { get; set; }  = new Skill();
        public List<MissionTheme> missionThemes { get; set; } = new List<MissionTheme>();
        public MissionTheme missionTheme { get; set; } = new MissionTheme();

    }
}
