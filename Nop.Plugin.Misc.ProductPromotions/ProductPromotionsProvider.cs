using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Shipping;
using Nop.Web.Framework.Infrastructure;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.ProductPromotions
{
    public class ProductPromotionsProvider : BasePlugin, IMiscPlugin, IWidgetPlugin
    {
        #region Fields

        private readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public ProductPromotionsProvider(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        public override void Install()
        {
            _localizationService.AddPluginLocaleResource(new Dictionary<string, string>
            {
                ["Nop.Plugins.Misc.ProductPromotions.Discount.Percentage"] = "Discount Percentage",
                ["Nop.Plugins.Misc.ProductPromotions.Discount.Amount"] = "Discount Amount",
                ["Nop.Plugins.Misc.ProductPromotions.Discount.Coupon.Code"] = "Coupon Code",
                ["Nop.Plugins.Misc.ProductPromotions.Discount.Coupon.NotRequired"] = "Not Required",
                ["Nop.Plugins.Misc.ProductPromotions.Discount.NotApplicable"] = "No promotion is applicable",
                ["Nop.Plugins.Misc.ProductPromotions.Product.Id.Invalid"] = "This is an invalid product id",
                ["Nop.Plugins.Misc.ProductPromotions.Promotions.List"] = "Available Promotions",
                ["Nop.Plugins.Misc.ProductPromotions.Discount.Name"] = "Promotion Name",
            });

            base.Install();
        }
        public override void Uninstall()
        {
            base.Uninstall();
        }
        public IList<string> GetWidgetZones()
        {
            return new List<string> { PublicWidgetZones.ProductDetailsAddInfo };
        }
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "ProductPromotions";
        }

        #endregion

        #region Properties

        public bool HideInWidgetList => false;

        #endregion
    }
}
