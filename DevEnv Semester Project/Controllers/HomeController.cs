using DevEnv_Semester_Project.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevEnv_Semester_Project.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        

        public ActionResult Test()
        {
            //connect to db and serve a view, then handle it inside the view 
            //db.skills.where 
            return View();
        }
        [HttpPost]
        public ActionResult Test(Dictionary<string, string> level)
        {

            //get user based on loggedin user.
            var CurrentUserId = User.Identity.GetUserId();
            
            
            foreach (var item in level)
            {
                ////add item as skilluserlevel to user.
                db.UserSkillLevels.Add(new UserSkillLevels { UserId = CurrentUserId, SkillId = Int32.Parse(item.Key), SkillLevelId = Int32.Parse(item.Value) });
            }
            db.SaveChanges();
            return View();
        }
        [Authorize]
        public ActionResult JoinOrCreateCompany()
        {
            var currentUserId = User.Identity.GetUserId();           
            var currentUser = db.Users.Where(x => x.Id == currentUserId).First();
            if (currentUser.CompanyId == null)
            {
                return View();
            }
            if (currentUser.CompanyId != null && currentUser.Admin == true)
            {
                return RedirectToAction("AdminIndex");
            } 
                return RedirectToAction("UserIndex");
            
        }

        [HttpPost]
        public ActionResult JoinCompany(string referenceCode)
        {
            try
            {
                Company ExistingCompany = db.Companies.Where(x => x.ReferenceCode == referenceCode).First();
                var CurrentUserId = User.Identity.GetUserId();
                ApplicationUser CurrentUser = db.Users.Where(x => x.Id == CurrentUserId).First();
                ExistingCompany.Employees.Add(CurrentUser);
                db.SaveChanges();

            }
            catch
            {
                ViewBag.ReferenceErrorMessage = "Invalid reference code.";
                return View("JoinOrCreateCompany");
            }

            return RedirectToAction("UserIndex", "Home");

        }

        [HttpPost]
        public ActionResult CreateCompany(Company Company)
        {
            if (!String.IsNullOrWhiteSpace(Company.Name) && !String.IsNullOrWhiteSpace(Company.Description))
            {
                var currentUserId = User.Identity.GetUserId();
                Company.AdministratorId = currentUserId;
                var currentUser = db.Users.Where(x => x.Id == currentUserId).First();
                currentUser.Admin = true;
                Company.Employees.Add(currentUser);
                db.Companies.Add(Company);
                db.SaveChanges();
                return RedirectToAction("AdminIndex", "Home");
            }
            ViewBag.CompanyErrorMessage = "Invalid name or description.";
            return View("JoinOrCreateCompany");
            
        }

        [Authorize]
        public ActionResult AdminIndex()
        {
            var userId = User.Identity.GetUserId();
            var currentUser = db.Users.Where(x => x.Id == userId).First();
           
            if (currentUser.Admin == true) 
            {
                var adminCompany = db.Companies.Where(x => x.AdministratorId == userId).First();
                var advm = new AdminDashboardViewModel();
                advm.Employees = db.Users.Where(x => x.CompanyId == adminCompany.CompanyId).ToList();
                advm.Skills = db.Skills.Where(x => x.CompanyId == adminCompany.CompanyId).ToList();
                advm.Company = adminCompany;

                return View(advm);
            }
            return RedirectToAction("Index");
           
        }


        [Authorize]
        public ActionResult UserIndex()
        {
            var currentUserId = User.Identity.GetUserId();
            var user = db.Users.Where(i=> i.Id == currentUserId).First();
            if (user.CompanyId != null)
            {
                var udvm = new UserDashboardViewModel();
                udvm.AvailableSkills = db.Skills.Where(x=>x.CompanyId == user.CompanyId).Select(x => new SelectListItem { Value = x.SkillId.ToString(), Text = x.Name });
                udvm.AvailableLevels = db.SkillLevels.Select(x => new SelectListItem { Value = x.SkillLevelId.ToString(), Text = x.SkillMastery });
                
                udvm.Employees = db.Users.Where(x => x.CompanyId == user.CompanyId).ToList();
                // = from t in db.UserSkillLevels where t.UserId == CurrentUserId select t;
                var CurrentSkills = db.UserSkillLevels.Where(i => i.UserId == currentUserId).ToList();
                udvm.SkillsForFocus = db.Skills.Where(x => x.CompanyId == user.CompanyId).ToList();

                udvm.UserCurrentSkills = CurrentSkills.ToList();
                udvm.Focus = db.Foci.Where(x => x.UserId == currentUserId);
                udvm.Company = db.Companies.Where(x => x.CompanyId == user.CompanyId).First();
                if (user.Admin)
                {
                    udvm.IsAdmin = true;
                }
                //uslvm.CurrentSkills = db.UserSkillLevels.Where(x => x.UserId == User.Identity.GetUserId().Select());
                return View(udvm);
            }
            return RedirectToAction("JoinOrCreateCompany"); 
                       
        }
        [Authorize]
        [HttpPost]
        public ActionResult UserAddSkill(UserDashboardViewModel model)
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUserSkills = db.UserSkillLevels.Where(x => x.UserId == currentUserId).ToList();
            foreach (var skill in currentUserSkills)
            {
                if (model.SelectedSkill == skill.SkillId)
                {
                    return RedirectToAction("UserIndex", "Home");
                }
            }
            db.UserSkillLevels.Add(new UserSkillLevels { UserId = currentUserId, SkillId = model.SelectedSkill, SkillLevelId = model.SelectedLevel });
            db.SaveChanges();
            return RedirectToAction("UserIndex", "Home");
        }
        [Authorize]
        public ActionResult ShowSkillLevels(string id)
        {
            var uslvm = new UserSkillLevelsViewModel();
            uslvm.UserSkillLevels = db.UserSkillLevels.Where(x => x.UserId == id).ToList();
            uslvm.PartialViewOpen = true;

            return PartialView("_ShowSkillLevels", uslvm);
        }
        [Authorize]
        public ActionResult ShowFocus(string id)
        {

            var userFocus = db.Foci.Where(x => x.UserId == id).ToList();
            return PartialView("_ShowFocus", userFocus);
        }
        [HttpPost]
        public JsonResult SetFocus(List<int> arrayOfSkills)
        {
            var currentUserId = User.Identity.GetUserId();
            var user = db.Users.Where(i => i.Id == currentUserId).First();
            var focus = db.Foci.Where(i => i.UserId == currentUserId).ToList();
            db.Foci.RemoveRange(focus);
            
            foreach (var item in arrayOfSkills)
            {
                
                Focus Focus = new Focus();
                Focus.SkillId = item;
                Focus.UserId = currentUserId;
                db.Foci.Add(Focus);
                db.SaveChanges();
            }

            return Json(new {result = "success"}); 
        }
        

            public ActionResult Index()
            {
            var currentUserId = User.Identity.GetUserId();
            
            if (currentUserId != null)
            {
                return RedirectToAction("JoinOrCreateCompany", "Home");
            }
                return View();
            }

            public ActionResult About()
            {
                ViewBag.Message = "Your application description page.";

                return View();
            }

            public ActionResult Contact()
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }
            //[Authorize]
            //public ActionResult Test()
            //{
            //    return Content("Hello!");
            //}
        }
}