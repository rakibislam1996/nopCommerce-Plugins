using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.DiscountRules.DaysOfWeek.Models
{
    public class RequirementModel
    {
        #region Ctor
        
        public RequirementModel()
        {
            SelectedWeekdaysId = new List<int>();
            AvailableWeekdays = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.DiscountRules.DaysOfWeek.Fields.Weekdays.Select")]
        public IList<int> SelectedWeekdaysId { get; set; }
        public IList<SelectListItem> AvailableWeekdays { get; set; }
        public int DiscountId { get; set; }
        public int RequirementId { get; set; }

        #endregion
    }
}
