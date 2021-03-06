﻿using FluentValidation;
using Project_DotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_DotNET.Utils
{
    //see: http://www.exceptionnotfound.net/use-fluentvalidation-for-better-validation-framework-in-mvc/
    // and same thing plus tests: http://devkimchi.com/1901/validating-asp-net-mvc-models-with-fluent-validation/
    public class AddJobToUserVMValidator : AbstractValidator<AddJobToUserViewModel>
    {
        public AddJobToUserVMValidator()
        {
            var db = new ApplicationDbContext();
            RuleFor(x => x.SelectedUser).Must(selectedUser => { return db.Users.Find(selectedUser) != null; }).WithMessage("Sélectionnez un utilisateur.");
            RuleFor(x => x.SelectedCompany).Must(selectedCompany => { return db.Companies.Find(selectedCompany) != null; }).WithMessage("Sélectionnez une entreprise.");
            RuleFor(x => x.SelectedJob).Must(selectedJob => { return db.Jobs.Find(selectedJob) != null; }).WithMessage("Sélectionnez le métier exercé durant cette période.");
            //La date de début ne peut être nulle 
            RuleFor(x => x.Debut).NotNull().WithMessage("La date de début ne peut être nulle");
            RuleFor(x => x.Fin).NotNull().WithMessage("La date de fin ne peut être nulle.");
            RuleFor(x => x.Fin).LessThanOrEqualTo(DateTime.Now).WithMessage("La date de fin ne peut être inférieure à aujourd'hui.");
            RuleFor(x => x.Debut).LessThan(x => x.Fin).WithMessage("La date de debut doit être inférieure à la date de fin.");
            //La date de début ne peut être supérieure ou égale à la date de fin (si non nulle) ou courante (aujourd'hui)
            //RuleFor(x => x.debut).LessThanOrEqualTo(DateTime.Now).When(x => x.fin == null).WithMessage("La date de début doit être antérieure à la date d'aujourd'hui.");
            //La période se trouve avant ou après toutes les périodes existantes
            //RuleFor(x => x.debut).Must(noIntersection).WithMessage("L'utilisateur travaillait déjà dans une autre Company à cette date.");
            //RuleFor(x => new DateTime[]{ x.debut, x.fin}).Must(noIntersection).WithMessage("Une partie de cette période est déjà utilisée.");
            RuleFor(x => x.Debut).Must((x, debut) => { return noIntersection(db.Users.Find(x.SelectedUser), x.Debut, x.Fin); }).WithMessage("La période se croise avec une autre période existante.");
        }

        private bool noIntersection(ApplicationUser user, DateTime debut, DateTime fin)
        {
            var _db = new ApplicationDbContext();
            var Periods = 0;

            if (debut.CompareTo(user.firstDay) >= 0 || fin.CompareTo(user.firstDay) >= 0 || debut.CompareTo(user.birthday) == -1)
                return false;

            //La période est correcte pour 2 cas:
            // - soit il n'existe aucune période pour laquelle la date de début est inférieure à la date de fin en param.
            // - soit il n'existe aucune période pour laquelle la date de fin est supérieure à la date de deb. en param.
            //Périodes qui croisent pas la nouvelle: http://stackoverflow.com/questions/13513932/algorithm-to-detect-overlapping-periods
            Periods = 0;
            /*Periods =
                _db.Periods
                .Select(x => x)
                .Where(x => x.User.Id == user.Id && x.fin != null && (x.debut.CompareTo(fin) == 1 && x.fin.CompareTo(debut) == -1)).Count();
            if (p_en_cours != null)
                Periods++;
                */
            Periods =
                _db.Periods
                .Select(x => x)
                .Where(x => x.User.Id == user.Id && (Tools.InclusiveBetween(debut, x.debut, x.fin) || Tools.InclusiveBetween(fin, x.debut, x.fin) || (Tools.InclusiveBetween(x.debut, debut, fin) || Tools.InclusiveBetween(x.fin, debut, fin))))
                .Count();

            //Si'il n'y a aucune intersection alors le nb de périodes est au nb de périodes en bases.
            return (Periods == 0) ? true : false;
        }
    }
}
