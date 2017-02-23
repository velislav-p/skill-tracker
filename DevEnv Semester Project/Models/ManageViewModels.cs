using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace DevEnv_Semester_Project.Models
{
    public class UserDashboardViewModel
    {
        [Required] //data anotation used for validation
        [Display(Name = "Select a skill")]
        public int SelectedSkill { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AvailableSkills { get; set; }

        [Required]
        [Display(Name = "Select a level of mastery")]
        public int SelectedLevel { get; set; }        
        public IEnumerable<System.Web.Mvc.SelectListItem> AvailableLevels { get; set; }
        public List<UserSkillLevels> UserCurrentSkills { get; set; }
        public IEnumerable<ApplicationUser> Employees { get; set; }
        public IEnumerable<Skill> SkillsForFocus { get; set; }
        public IEnumerable<Focus> Focus { get; set; }
        public Company Company { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class UserSkillLevelsViewModel
    {
        public IEnumerable<UserSkillLevels> UserSkillLevels { get; set; }
        public bool PartialViewOpen { get; set; }

    }
    public class AdminDashboardViewModel
    {
        public IEnumerable<Skill> Skills { get; set; }
        public IEnumerable<ApplicationUser> Employees { get; set; }
        public IEnumerable<Focus> Focus { get; set; }
        public Company Company { get; set; }
        
    }
   
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}