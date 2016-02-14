using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;

namespace Project_DotNET.Models
{
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
        [StringLength(100, ErrorMessage = "Le {0} doit compter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le nouveau mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe actuel")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit compter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le nouveau mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Numéro de téléphone")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Numéro de téléphone")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    public class CreateJobViewModel
    {
        [Required]
        [Display(Name = "Nom du métier")]
        public string Name { get; set; }

        [Display(Name = "Description du métier")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Catégorie du métier")]
        public int SelectedCategory { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }

    public class AddJobToUserViewModel
    {
        [Required]
        [Display(Name = "Utilisateur")]
        public int SelectedUser { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }

        [Required]
        [Display(Name = "Métier exercé")]
        public int SelectedJob { get; set; }
        public IEnumerable<Job> Jobs { get; set; }

        [Required]
        [Display(Name = "Début")]
        public DateTime Debut { get; set; }

        [Required]
        [Display(Name = "Fin")]
        public DateTime Fin { get; set; }

        [Required]
        [Display(Name = "Entreprise")]
        public int SelectedCompany { get; set; }
        public IEnumerable<Company> Companies { get; set; }
    }
}