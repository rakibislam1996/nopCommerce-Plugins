using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.DiscountRules.DaysOfWeek.Models
{
    public class RequirementModel
    {
        public RequirementModel()
        {
            AvailableWeekdays = new List<SelectListItem>();
            SelectedWeekdaysId = new List<int>();
        }

        [NopResourceDisplayName("Plugins.DiscountRules.DaysOfWeek.Fields.Weekdays.Select")]
        public IList<int> SelectedWeekdaysId { get; set; }
		
        public int DiscountId { get; set; }

        public int RequirementId { get; set; }

        public IList<SelectListItem> AvailableWeekdays { get; set; }
		
    }
}
