using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace DevEnv_Semester_Project.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }             
        public string Description { get; set; }
        public string AdministratorId { get; set; }
        public virtual ICollection<ApplicationUser> Employees { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
        public string ReferenceCode { get; set; }
        [Display(Name = "Link to company logo")]
        public string CompanyLogoUrl { get; set; }

        public Company ()
        {
            this.ReferenceCode = Guid.NewGuid().ToString();
            this.Employees = new List<ApplicationUser>();
            this.Skills = new List<Skill>();
        }

    }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<UserSkillLevels> SkillLevels { get; set; }
        public bool Admin { get; set; }
        public int? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Focus> Focus { get; set; }
        public ApplicationUser ()
        {
            this.EmailConfirmed = true;
            this.SkillLevels = new List<UserSkillLevels>();
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillLevel> SkillLevels {get;set;}
        public DbSet<UserSkillLevels> UserSkillLevels { get; set; }
        public DbSet<Focus> Foci { get; set; }
    }

    public class Skill
    {
        public int SkillId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserSkillLevels> SkillLevels { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }

    public class SkillLevel
    {
        public int SkillLevelId { get; set; }
        public string SkillMastery { get; set; }
        public virtual ICollection<UserSkillLevels> SkillLevels { get; set; }
    }

    public class UserSkillLevels
    {
        public int UserSkillLevelsId { get; set; }
        public int SkillId { get; set; }
        public virtual Skill Skill { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int SkillLevelId { get; set; }
        public virtual SkillLevel SkillLevel { get; set; }
    }

    public class Focus
    {
        public int FocusId { get; set; }
        public virtual Skill Skill { get; set; }
        public int SkillId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }

}