﻿using CI_PLATFORM.Entities.Data;
using CI_PLATFORM.Entities.Models;
using CI_PLATFORM.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using CI_PLATFORM.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using MailKit.Security;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace CI_PLATFORM.Repository.Repositories
{
    public class PlatformRepository : IPlatformRepository
    {

        public readonly CiPlatformContext _CiPlatformContext;
        public PlatformRepository(CiPlatformContext CiPlatformContext)
        {
            _CiPlatformContext = CiPlatformContext;
        }
        public List<Country> GetCountryData()
        {

            List<Country> country = _CiPlatformContext.Countries.ToList();
            return country;

        }
        public List<City> GetCitys()
        {
            List<City> cities = _CiPlatformContext.Cities.ToList();
            return cities;
        }
        public List<City> GetCityData(int countryId)
        {

            List<City> city = _CiPlatformContext.Cities.Where(i => i.CountryId == countryId).ToList();
            return city;

        }
        public List<MissionTheme> GetMissionThemes()
        {
            List<MissionTheme> themes = _CiPlatformContext.MissionThemes.ToList();
            return themes;

        }

       
        public List<MissionSkill> GetSkills()
        {

            var skills = _CiPlatformContext.MissionSkills.Include(m => m.Skill).ToList();
            return skills;

        }

       
        public List<Mission> GetMissionDetails()
        {
            List<Mission> missionDetails = _CiPlatformContext.Missions.Include(m => m.City).Include(m => m.Theme).Include(m => m.MissionMedia).Include(m => m.MissionRatings).Include(m => m.GoalMissions).Include(m => m.MissionSkills).Include(m => m.FavoriteMissions).ToList();
            return missionDetails;
        }

        public CardsViewModel getCards()
        {
            List<Mission> missions = _CiPlatformContext.Missions.ToList();
            List<MissionMedium> media = _CiPlatformContext.MissionMedia.Where(x => x.Default == 1).ToList();
            List<MissionSkill> missionSkills = _CiPlatformContext.MissionSkills.ToList();
            List<MissionTheme> missionThemes = _CiPlatformContext.MissionThemes.ToList();
            List<MissionRating> rating = _CiPlatformContext.MissionRatings.ToList();
            List<City> cities = _CiPlatformContext.Cities.ToList();
            List<Country> countries = _CiPlatformContext.Countries.ToList();




            CardsViewModel missionCards = new CardsViewModel();
            {

                missionCards.missions = missions;
                missionCards.missionthemes = missionThemes;
                missionCards.missionskill = missionSkills;
                missionCards.media = media;
                missionCards.rating = rating;
                missionCards.countries = countries;
                missionCards.cities = cities;
            }
            return missionCards;

        }
        public int GetMissionCount()
        {

            int missionNumber = _CiPlatformContext.Missions.Count();
            return missionNumber;

        }
        public List<Mission> Filter(List<int>? cityId, List<int>? countryId, List<int>? themeId, List<int>? skillId, string? search, int? sort)
        {
            List<Mission> cards = new List<Mission>();
            var missioncards = GetMissionDetails();
            var Missionskills = GetSkills();
            List<int> temp = new List<int>();


            if (cityId.Count != 0 || countryId.Count != 0 || themeId.Count != 0 || skillId.Count != 0)
            {
                foreach (var n in countryId)
                {
                    foreach (var item in missioncards)
                    {
                        bool countrychek = cards.Any(x => x.MissionId == item.MissionId);
                        if (item.CountryId == n && countrychek == false)
                        {
                            cards.Add(item);
                        }
                    }

                }

                if (cityId.Count != 0)
                {
                    cards.Clear();
                    foreach (var n in cityId)
                    {
                        foreach (var item in missioncards)
                        {
                            bool citychek = cards.Any(x => x.MissionId == item.MissionId);
                            if (item.CityId == n && citychek == false)
                            {
                                cards.Add(item);
                            }

                        }
                    }
                }

                foreach (var n in themeId)
                {
                    foreach (var item in missioncards)
                    {
                        bool themechek = cards.Any(x => x.MissionId == item.MissionId);
                        if (item.ThemeId == n && themechek == false)
                        {
                            cards.Add(item);
                        }
                    }
                }

                foreach (var n in skillId)
                {
                    foreach (var item in Missionskills)
                    {
                        bool skillchek = cards.Any(x => x.MissionId == item.MissionId);
                        if (item.SkillId == n && skillchek == false)
                        {

                            cards.Add(missioncards.FirstOrDefault(x => x.MissionId == item.MissionId));
                        }
                    }
                    //foreach (var item in Missionskills)
                    //{
                    //    if (item.SkillId == n)
                    //    {
                    //        temp.Add((int)item.MissionId);
                    //    }
                    //    foreach (var item2 in temp)
                    //    {
                    //        bool skillchek = missionDetails.Any(x => x.MissionId == item2);
                    //        if (skillchek == false)
                    //        {
                    //            cards.Add(missioncards.FirstOrDefault(x => x.MissionId == item2));
                    //        }
                    //    }

                    //}
                }

                if (search != null)
                {
                    foreach (var n in missioncards)
                    {
                        var title = n.Title.ToLower();
                        if (title.Contains(search.ToLower()))
                        {
                            cards.Add(n);
                        }
                    }
                }
                if (sort != null)
                {


                    if (sort == 1)
                    {
                        //if (cards.Count != 0)
                        //{
                        cards = cards.OrderByDescending(x => x.CreatedAt).ToList();
                        //}

                        //else
                        //{
                        //    missioncards = missioncards.OrderByDescending(x => x.CreatedAt).ToList();
                        //    return missioncards;
                        //}
                    }
                    if (sort == 2)
                    {
                        //if (cards.Count != 0)
                        //{
                        cards = cards.OrderBy(x => x.CreatedAt).ToList();
                        //}

                        //else
                        //{
                        //    missioncards = missioncards.OrderBy(x => x.CreatedAt).ToList();
                        //    return missioncards;
                        //}
                    }




                }
                return cards;
            }

            //else if (cityId.Count == 0 && countryId.Count == 0 && themeId.Count == 0 && skillId.Count == 0 && search == null)
            //{
            //    foreach (var item in missioncards)
            //    {
            //        cards.Add(item);
            //    }
            //    //return missioncards;
            //}

            if (search != null)
            {

                foreach (var n in missioncards)
                {
                    var title = n.Title.ToLower();
                    if (title.Contains(search.ToLower()))
                    {
                        cards.Add(n);
                    }
                }

            }
            if (sort != null)
            {


                if (sort == 1)
                {
                    //if (cards.Count != 0)
                    //{
                    //    cards = cards.OrderByDescending(x => x.CreatedAt).ToList();
                    //}

                    //else
                    //{
                    missioncards = missioncards.OrderByDescending(x => x.CreatedAt).ToList();
                    return missioncards;
                    //}
                }
                if (sort == 2)
                {
                    //if (cards.Count != 0)
                    //{
                    //    cards = cards.OrderBy(x => x.CreatedAt).ToList();
                    //}

                    //else
                    //{
                    missioncards = missioncards.OrderBy(x => x.CreatedAt).ToList();
                    return missioncards;
                    //}
                }




            }
            else if (cityId.Count == 0 && countryId.Count == 0 && themeId.Count == 0 && skillId.Count == 0 && search == null)
            {
                foreach (var item in missioncards)
                {
                    cards.Add(item);
                }
                //return missioncards;
            }
            return cards;

        }


        public List<MissionMedium> media(int mid)
        {
            List<MissionMedium> photos = _CiPlatformContext.MissionMedia.Where(x => x.MissionId == mid).ToList();
            return photos;
        }

        public List<StoryMedium> smedia(int sid)
        {
            List<StoryMedium> photos = _CiPlatformContext.StoryMedia.Where(x => x.StoryId == sid).ToList();
            return photos;
        }
        public void addComment(int mid, int uid, string comnt)
        {
            Comment comment = new Comment();
            {
                comment.MissionId = mid;
                comment.UserId = uid;
                comment.CommentDescription = comnt;
            }
            _CiPlatformContext.Comments.Add(comment);
            _CiPlatformContext.SaveChanges();

            //return true;

        }
        public bool applyMission(int mid, int uid)
        {
            MissionApplication application = new();
            application.MissionId = mid;
            application.UserId = uid;



            var applicable = _CiPlatformContext.MissionApplications.FirstOrDefault(a => a.MissionId == mid && a.UserId == uid );

            if (applicable == null)
            {
                application.CreatedAt = DateTime.Now;
                application.AppliedAt = DateTime.Now;

                _CiPlatformContext.MissionApplications.Add(application);

                _CiPlatformContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool addToFav(int missionId, int userId)
        {
            FavoriteMission favorite = new();
            favorite.MissionId = missionId; 
            favorite.UserId = userId;   

            var  fM = _CiPlatformContext.FavoriteMissions.FirstOrDefault(f => f.MissionId == missionId && f.UserId == userId);

            if (fM == null)
            {

                _CiPlatformContext.FavoriteMissions.Add(favorite);
                return true;
            }

            else
            {
                _CiPlatformContext.FavoriteMissions.Remove(fM);
                return false;
               
            }
        }
        public List<MissionDocument> document(int mid)
        {
            List<MissionDocument> documents = _CiPlatformContext.MissionDocuments.Where(x => x.MissionId == mid).ToList();
            return documents;
        }
        public void RecommandToCoWorker(int FromUserId, List<int> ToUserId, int mid)
        {
            var fromUser = _CiPlatformContext.Users.FirstOrDefault(u => u.UserId == FromUserId && u.DeletedAt == null);
            var fromEmailId = fromUser.Email;
            //if (user1 == null)
            //{
            //    return null;
            //}

            foreach (var user in ToUserId)
            {
                var toUser = _CiPlatformContext.Users.FirstOrDefault(u => u.UserId == user && u.DeletedAt == null);
                var toEmailId = toUser.Email;

                MissionInvite invite = new MissionInvite();
                {
                    invite.FromUserId = FromUserId;
                    invite.ToUserId = user;
                    invite.MissionId = mid;
                }
                _CiPlatformContext.Add(invite);
                _CiPlatformContext.SaveChanges();



                #region Send Mail
                var mailBody = "<h1></h1><br><h2><a href='" + "https://localhost:7251/Platform/MissionListing?mid=" + mid + "'>Check Out this Mission!</a></h2>";

                // create email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(fromEmailId));
                email.To.Add(MailboxAddress.Parse(toEmailId));
                email.Subject = "Reset Your Password";
                email.Body = new TextPart(TextFormat.Html) { Text = mailBody };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("hetshah2207@gmail.com", "lpoqtojvkcgkwdms");
                smtp.Send(email);
                smtp.Disconnect(true);
                #endregion Send Mail
            }
        }


        public void RecommandStory(int FromUserId, List<int> ToUserId, int sid)
        {
            var fromUser = _CiPlatformContext.Users.FirstOrDefault(u => u.UserId == FromUserId && u.DeletedAt == null);
            var fromEmailId = fromUser.Email;
            //if (user1 == null)
            //{
            //    return null;
            //}

            foreach (var user in ToUserId)
            {
                var toUser = _CiPlatformContext.Users.FirstOrDefault(u => u.UserId == user && u.DeletedAt == null);
                var toEmailId = toUser.Email;

                StoryInvite invite = new StoryInvite();
                {
                    invite.FromUserId = FromUserId;
                    invite.ToUserId = user;
                    invite.StoryId = sid;
                }
                _CiPlatformContext.Add(invite);
                _CiPlatformContext.SaveChanges();



                #region Send Mail
                var mailBody = "<h1></h1><br><h2><a href='" + "https://localhost:7251/Platform/StoryDetail?sid=" + sid + "'>Check Out this Mission!</a></h2>";

                // create email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(fromEmailId));
                email.To.Add(MailboxAddress.Parse(toEmailId));
                email.Subject = "Reset Your Password";
                email.Body = new TextPart(TextFormat.Html) { Text = mailBody };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("hetshah2207@gmail.com", "lpoqtojvkcgkwdms");
                smtp.Send(email);
                smtp.Disconnect(true);
                #endregion Send Mail
            }
        }
        public MissionListingViewModel GetCardDetail(int mid)
        {
            List<Mission> missions = GetMissionDetails();
            Mission mission = missions.FirstOrDefault(x => x.MissionId == mid);
            List<MissionMedium> photos = media(mid);
            List<MissionDocument> documents = document(mid);
       
            List <Mission> relatedMissions = missions.Where(x =>  x.OrganizationName == mission.OrganizationName || x.ThemeId == mission.ThemeId || x.CountryId == mission.CountryId ).ToList();
            relatedMissions.Remove(mission);
            List<MissionSkill> missionSkills = _CiPlatformContext.MissionSkills.Include(m => m.Skill).Where(x => x.MissionId == mid).ToList();
            List<MissionApplication> applications = _CiPlatformContext.MissionApplications.Include(m => m.User).Where(x => x.MissionId == mid).ToList();
            List<Comment> comments = _CiPlatformContext.Comments.Include(m => m.User).Where(x => x.MissionId == mid).ToList();
            List<FavoriteMission> favoriteMission = _CiPlatformContext.FavoriteMissions.ToList();
            MissionListingViewModel CardDetail = new MissionListingViewModel();
            List<User> users = _CiPlatformContext.Users.ToList();

            {
                CardDetail.missions = mission;
                CardDetail.missionmedias = photos;
                CardDetail.relatedmissions = relatedMissions;
                CardDetail.missiondocuments = documents;
                CardDetail.favoriteMissions = favoriteMission;
                CardDetail.missionskills = missionSkills;
                CardDetail.missionapplications = applications;
                CardDetail.comments = comments;
                CardDetail.coworkers = users;
            }

            return CardDetail;
        }

        public StoryListingViewModel GetStoryDetail()
        {
            List<Story> stories = _CiPlatformContext.Stories.Include(m => m.User).Include(m => m.StoryMedia).Include(m => m.Mission).ToList();
            


            StoryListingViewModel StoryDetail = new StoryListingViewModel();
            {
                StoryDetail.stories = stories;
               


            }
            return StoryDetail;
        }
        
        public StoryListingViewModel GetStory(int sid)
        {
            Story story = _CiPlatformContext.Stories.Include(m => m.User).FirstOrDefault(m => m.StoryId == sid);
            List<StoryMedium> photos = smedia(sid);
            List<User> users = _CiPlatformContext.Users.ToList();

            StoryListingViewModel StoryDetail = new StoryListingViewModel();
            {
                StoryDetail.storymedias = photos;
                StoryDetail.story = story;
                StoryDetail.coworkers = users;
            }
            return StoryDetail;
        }


    }
}