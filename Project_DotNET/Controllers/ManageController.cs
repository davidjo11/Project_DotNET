using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Project_DotNET.Models;
using FluentValidation.Results;
using Project_DotNET.Utils;

namespace Project_DotNET.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Votre mot de passe a été changé."
                : message == ManageMessageId.SetPasswordSuccess ? "Votre mot de passe a été défini."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Votre fournisseur d'authentification à 2 facteurs a été défini."
                : message == ManageMessageId.Error ? "Une erreur s'est produite."
                : message == ManageMessageId.AddPhoneSuccess ? "Votre numéro de téléphone a été ajouté."
                : message == ManageMessageId.RemovePhoneSuccess ? "Votre numéro de téléphone a été supprimé."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Générer le jeton et l'envoyer
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Votre code de sécurité est : " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Envoyer un SMS via le fournisseur SMS afin de vérifier le numéro de téléphone
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            //Si nous sommes arrivés là, quelque chose a échoué, réafficher le formulaire
            ModelState.AddModelError("", "La vérification du téléphone a échoué");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            //Si nous sommes arrivés là, quelque chose a échoué, réafficher le formulaire
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "La connexion externe a été supprimée."
                : message == ManageMessageId.Error ? "Une erreur s'est produite."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Demander une redirection vers le fournisseur de connexion externe afin de lier une connexion pour l'utilisateur actuel
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        [AllowAnonymous]
        // GET: /Manage/CreateJob
        public ActionResult CreateCategory()
        {
            var db = new ApplicationDbContext();
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: /Manage/CreateJob
        public ActionResult CreateCategory(CreateCategoryViewModel model)
        {
            CreateCategoryVMValidator validator = new CreateCategoryVMValidator();
            ValidationResult result = validator.Validate(model);

            //Traitement
            var db = new ApplicationDbContext();
            if (result.IsValid)
            {
                var CategoryDb = db.Categories;

                var newCat = new Category { CategoryName = model.Name, CategoryDesc = model.Description };
                //Ajout à la bdd
                CategoryDb.Add(newCat);
                //Commit!
                db.SaveChanges();

                return RedirectToAction("ListCategories", "Manage");
            }

            foreach (ValidationFailure failer in result.Errors)
            {
                ModelState.AddModelError(failer.PropertyName, failer.ErrorMessage);
            }
            //Redirection vers la liste des périodes pour l'utilisateur concerné
            return View(model);
        }

        [AllowAnonymous]
        // GET: /Manage/ListCategories
        public ActionResult ListCategories()
        {
            return View();
        }

        [AllowAnonymous]
        // GET: /Manage/JsonListCategories
        public ActionResult JsonListCategories()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var data = db.Categories.Select(c => new { c.CategoryName, c.CategoryDesc }).ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        // GET: /Manage/CreateJob
        public ActionResult CreateJob()
        {
            var db = new ApplicationDbContext();
            var vm = new CreateJobViewModel()
            {
                Categories = db.Categories.ToList(),
            };
            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: /Manage/CreateJob
        public ActionResult CreateJob(CreateJobViewModel model)
        {
            CreateJobVMValidator validator = new CreateJobVMValidator();
            ValidationResult result = validator.Validate(model);

            //Traitement
            var db = new ApplicationDbContext();
            if (result.IsValid)
            {
                var JobDb = db.Jobs;

                var newJob = new Job { JobName = model.Name, JobDesc = model.Description, CategoryId = model.SelectedCategory };
                //Ajout à la bdd
                JobDb.Add(newJob);
                //Commit!
                db.SaveChanges();

                return RedirectToAction("ListJobs", "Manage");
            }

            foreach (ValidationFailure failer in result.Errors)
            {
                ModelState.AddModelError(failer.PropertyName, failer.ErrorMessage);
            }
            //Redirection vers la liste des périodes pour l'utilisateur concerné
            model.Categories = db.Categories.ToList();
            return View(model);
        }

        [AllowAnonymous]
        // GET: /Manage/CreateJob
        public ActionResult CreateRole()
        {
            var db = new ApplicationDbContext();
            var vm = new CreateRoleViewModel()
            {
                RolesInc = db.CustomRoles.ToList(),
            };
            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: /Manage/CreateRole
        public ActionResult CreateRole(CreateRoleViewModel model)
        {
            CreateRoleVMValidator validator = new CreateRoleVMValidator();
            ValidationResult result = validator.Validate(model);

            //Traitement
            var db = new ApplicationDbContext();
            var RolesDb = db.CustomRoles;
            var RolesIncDb = db.RolesInc;

            var exists = RolesDb.Select(r => r).Where(r => r.RoleName == model.Name).First();
            if (result.IsValid || exists == null)
            {

                /*RolesIncomptabilities rolesInc = new RolesIncomptabilities();
                //On ajoute les différents rôles incompatibles sélectionnés par l'utilisateur.
                foreach(var roleId in model.SelectedRolesInc)
                {
                    var role = RolesDb.Find(roleId);
                    rolesInc.addIncompatibilite(role);
                }
                RolesIncDb.Add(rolesInc);
                db.SaveChanges();

                rolesInc = RolesIncDb.Find(rolesInc);*/
                var newRole = new CustomRole { RoleName = model.Name, RoleDesc = model.Description, };
                //Ajout à la bdd
                RolesDb.Add(newRole);
                //Commit!
                db.SaveChanges();

                //Ajout des incompatibilités
                newRole = RolesDb.Select(r => r).Where(r => r.RoleName == model.Name).First();
                foreach (var roleId in model.SelectedRolesInc)
                {
                    var role = RolesDb.Find(roleId);
                    
                    //A affiner: ctrl si le rôle n'est pas déjà dedans even though that's a creation
                    newRole.RolesInc.addIncompatibilite(role);
                }
                RolesIncDb.Add(newRole.RolesInc);
                db.SaveChanges();

                return RedirectToAction("ListRoles", "Manage");
            }

            foreach (ValidationFailure failer in result.Errors)
            {
                ModelState.AddModelError(failer.PropertyName, failer.ErrorMessage);
            }
            model.RolesInc = db.CustomRoles.ToList();
            return View(model);
        }

        [AllowAnonymous]
        // GET: /Manage/ListJobs
        public ActionResult ListRoles()
        {
            var RolesDb = new ApplicationDbContext().CustomRoles;
            ViewBag.roles = RolesDb.Select(r => r).ToList();
            return View();
        }

        [AllowAnonymous]
        // GET: /Manage/JsonListJobs
        public ActionResult JsonListRoles()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var data = db.CustomRoles.Select(r => new { r.RoleName, r.RoleDesc, }).ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        // GET: /Manage/ListJobs
        public ActionResult ListJobs()
        {
            return View();
        }

        [AllowAnonymous]
        // GET: /Manage/JsonListJobs
        public ActionResult JsonListJobs()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var data = db.Jobs.Select(j => new { j.JobName, j.Category.CategoryName, j.JobDesc }).ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        // GET: /Manage/AddJobToUser (creates a period)
        public ActionResult AddJobToUser()
        {
            var db = new ApplicationDbContext();
            var vm = new AddJobToUserViewModel()
            {
                Jobs = db.Jobs.ToList(),
                Users = db.Users.ToList(),
                Companies = db.Companies.ToList(),
            };
            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: /Manage/CreateJob
        public ActionResult AddJobToUser(AddJobToUserViewModel model)
        {
            AddJobToUserVMValidator validator = new AddJobToUserVMValidator();
            ValidationResult result = validator.Validate(model);

            //Traitement
            var db = new ApplicationDbContext();
            if (result.IsValid)
            {
                var PeriodsDb = db.Periods;

                var period = new Period { En_Cours = false, debut = model.Debut, fin = model.Fin, CompanyId = model.SelectedCompany, JobId = model.SelectedJob, UserId = model.SelectedUser };

                PeriodsDb.Add(period);
                db.SaveChanges();

                return RedirectToAction("List", "Account");
            }

            foreach (ValidationFailure failer in result.Errors)
            {
                ModelState.AddModelError(failer.PropertyName, failer.ErrorMessage);
            }
            //Redirection vers la liste des jobs pour l'utilisateur concerné
            model.Jobs = db.Jobs.ToList();
            model.Users = db.Users.ToList();
            model.Companies = db.Companies.ToList();
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Programmes d'assistance
        // Utilisé pour la protection XSRF lors de l'ajout de connexions externes
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}