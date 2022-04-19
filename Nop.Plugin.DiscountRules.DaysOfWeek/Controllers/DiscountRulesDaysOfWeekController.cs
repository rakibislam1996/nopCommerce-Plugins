using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Discounts;
using Nop.Plugin.DiscountRules.DaysOfWeek.Models;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.DiscountRules.DaysOfWeek.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class DiscountRulesDaysOfWeekController : BasePluginController
    {
        #region Fields

        private readonly IDiscountService _discountService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public DiscountRulesDaysOfWeekController( IDiscountService discountService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ISettingService settingService)
        {
            _discountService = discountService;
            _localizationService = localizationService;
            _permissionService = permissionService;
            _settingService = settingService;
        }

        #endregion

        #region Methods

        public IActionResult Configure(int discountId, int? discountRequirementId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDiscounts))
                return Content("Access denied");

            //load the discount
            var discount = _discountService.GetDiscountById(discountId);
            if (discount == null)
                throw new ArgumentException("Discount could not be loaded");

            //check whether the discount requirement exists
            if (discountRequirementId.HasValue && _discountService.GetDiscountRequirementById(discountRequirementId.Value) is null)
                return Content("Failed to load requirement.");

            //try to get previously saved restricted customer role identifier
            var restrictedWeekdayIds = _settingService.GetSettingByKey<string>(string.Format(DiscountRequirementDefaults.SettingsKey, discountRequirementId ?? 0));

            var model = new RequirementModel
            {
                RequirementId = discountRequirementId ?? 0,
                DiscountId = discountId,
                SelectedWeekdaysId = restrictedWeekdayIds!=null ? restrictedWeekdayIds.Split(',').Select(x => int.Parse(x)).ToList() : null
            };

            //set available customer roles
            model.AvailableWeekdays = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = DayOfWeek.Sunday.ToString(),
                    Value = ((int)DayOfWeek.Sunday).ToString(),
                    Selected = model.SelectedWeekdaysId!=null ? model.SelectedWeekdaysId.Contains((int)DayOfWeek.Sunday) : false
                },
                new SelectListItem
                {
                    Text = DayOfWeek.Monday.ToString(),
                    Value = ((int)DayOfWeek.Monday).ToString(),
                    Selected = model.SelectedWeekdaysId!=null ? model.SelectedWeekdaysId.Contains((int)DayOfWeek.Monday) : false
                },
                new SelectListItem
                {
                    Text = DayOfWeek.Tuesday.ToString(),
                    Value = ((int)DayOfWeek.Tuesday).ToString(),
                    Selected = model.SelectedWeekdaysId!=null ? model.SelectedWeekdaysId.Contains((int)DayOfWeek.Tuesday) : false
                },
                new SelectListItem
                {
                    Text = DayOfWeek.Wednesday.ToString(),
                    Value = ((int)DayOfWeek.Wednesday).ToString(),
                    Selected = model.SelectedWeekdaysId!=null ? model.SelectedWeekdaysId.Contains((int)DayOfWeek.Wednesday) : false
                },
                new SelectListItem
                {
                    Text = DayOfWeek.Thursday.ToString(),
                    Value = ((int)DayOfWeek.Thursday).ToString(),
                    Selected = model.SelectedWeekdaysId!=null ? model.SelectedWeekdaysId.Contains((int)DayOfWeek.Thursday) : false
                },
                new SelectListItem
                {
                    Text = DayOfWeek.Friday.ToString(),
                    Value = ((int)DayOfWeek.Friday).ToString(),
                    Selected = model.SelectedWeekdaysId!=null ? model.SelectedWeekdaysId.Contains((int)DayOfWeek.Friday) : false
                },
                new SelectListItem
                {
                    Text = DayOfWeek.Saturday.ToString(),
                    Value = ((int)DayOfWeek.Saturday).ToString(),
                    Selected = model.SelectedWeekdaysId!=null ? model.SelectedWeekdaysId.Contains((int)DayOfWeek.Saturday) : false
                }
            };
            
            //set the HTML field prefix
            ViewData.TemplateInfo.HtmlFieldPrefix = string.Format(DiscountRequirementDefaults.HtmlFieldPrefix, discountRequirementId ?? 0);

            return View("~/Plugins/DiscountRules.DaysOfWeek/Views/Configure.cshtml", model);
        }

        [HttpPost]        
        public IActionResult Configure(RequirementModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDiscounts))
                return Content("Access denied");

            if (ModelState.IsValid)
            {
                //load the discount
                var discount = _discountService.GetDiscountById(model.DiscountId);
                if (discount == null)
                    return NotFound(new { Errors = new[] { "Discount could not be loaded" } });

                //get the discount requirement
                var discountRequirement = _discountService.GetDiscountRequirementById(model.RequirementId);

                //the discount requirement does not exist, so create a new one
                if (discountRequirement == null)
                {
                    discountRequirement = new DiscountRequirement
                    {
                        DiscountId = discount.Id,
                        DiscountRequirementRuleSystemName = DiscountRequirementDefaults.SystemName
                    };

                    _discountService.InsertDiscountRequirement(discountRequirement);
                }
                var weekdaysId = string.Join(",", model.SelectedWeekdaysId);  

                //var weekdaysId =  model.SelectedWeekdaysId.ToArray();

                //save restricted customer role identifier
                _settingService.SetSetting(string.Format(DiscountRequirementDefaults.SettingsKey, discountRequirement.Id), weekdaysId);

                return Ok(new { NewRequirementId = discountRequirement.Id });
            }

            return BadRequest(new { Errors = GetErrorsFromModelState(ModelState) });
        }

        #endregion

        #region Utilities

        private IEnumerable<string> GetErrorsFromModelState(ModelStateDictionary modelState)
        {
            return ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
        }

        //private IList<int> ParseSeparatedNumbers(string separated, char separator = ',')
        //{
        //    if (string.IsNullOrEmpty(separated))
        //        return new List<int>();

        //    return separated.Split(separator).Select(x =>
        //    {
        //        int.TryParse(x, out int result);
        //        return result;
        //    }).ToList();
        //}

        #endregion
    }
}