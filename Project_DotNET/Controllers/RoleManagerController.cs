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
    public class RoleManagerController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public RoleManagerController()
        {
        }

        public RoleManagerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        // GET: /RoleManager/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.SetAppRoledSuccess ? "Le rôle a été ajouté à l'utilisateur."
                : message == ManageMessageId.AddAvailableRole ? "Le nouveau rôle a été ajouté à la lsite des roles disponibles."
                : message == ManageMessageId.IncompatbileRole ? "Vous voulez ajouter un rôle incomptabible avec rôle précédent."
                : message == ManageMessageId.Error ? "Une erreur s'est produite."
                : message == ManageMessageId.RemoveAppRoleSuccess ? "Le rôle selectionné a été supprimé."
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


        [AllowAnonymous]
        //
        // GET: /RoleManager/ListAvailableRoles
        public ActionResult ListAvailableRoles()
        {
            return View();
        }

        [AllowAnonymous]
        //
        // GET: /RoleManager/JsonListAvailableRoles
        public ActionResult JsonListAvailableRoles()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var data = db.AvailableRoles.Select(ar => new { ar.AvailableRoleName, ar.AvailableRoleDesc}).ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }
       
        [AllowAnonymous]
        // GET: /RoleManager/CreateAvailableRole
        public ActionResult CreateAvailableRole()
        {
            var db = new ApplicationDbContext();
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: /RoleManager/CreateAvailableRole
        public ActionResult CreateAvailableRole(CreateAvailableRoleViewModel model)
        {
            
            //CreateAppRoleVMValidator validator = new CreateAppRoleVMValidator();
            //ValidationResult result = validator.Validate(model);

            //Traitement
            var db = new ApplicationDbContext();
            if (true) //result.IsValid)
            {
                var AvailableRoleDb = db.AvailableRoles;

                var newRole = new AvailableRole { AvailableRoleName = model.Name, AvailableRoleDesc = model.Description };
                //Ajout à la bdd
                AvailableRoleDb.Add(newRole);
                //Commit!
                db.SaveChanges();

                return RedirectToAction("ListAvailableRoles", "RoleManager");
            }
            
            /*
            foreach (ValidationFailure failer in result.Errors)
            {
                ModelState.AddModelError(failer.PropertyName, failer.ErrorMessage);
            }
            */
            //Redirection vers la liste des périodes pour l'utilisateur concerné
            return View(model);
        }

      

    

        [AllowAnonymous]
        // GET: /Manage/AddJobToUser (creates a period)
        public ActionResult AddAppRoleToUser()
        {
            var db = new ApplicationDbContext();
            var vm = new AddAppRoleToUserViewModel()
            {
               AvailableRoles = db.AvailableRoles.ToList(),
               Users = db.Users.ToList()
            };
            return View(vm);
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
            AddAvailableRole,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error,
            SetAppRoledSuccess,
            IncompatbileRole,
            RemoveAppRoleSuccess
        }

#endregion
    }
}