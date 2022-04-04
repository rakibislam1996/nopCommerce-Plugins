using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.ProductPromotions.Models;
using Nop.Plugin.Misc.ProductPromotions.Services;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;
using static Nop.Web.Models.Catalog.ProductDetailsModel;

namespace Nop.Plugin.Misc.ProductPromotions.Components
{
    [ViewComponent(Name = "ProductPromotions")]
    public class ProductPromotionsViewComponent : NopViewComponent
    {
        #region Fields

        private readonly IProductPromotionsService _productPromotionsService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ProductPromotionsViewComponent(IProductPromotionsService productPromotionsService,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            _productPromotionsService = productPromotionsService;
            _localizationService = localizationService;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var customer = _workContext.CurrentCustomer;

            AddToCartModel addToCartModel = (AddToCartModel)additionalData;

            if (addToCartModel == null || addToCartModel.ProductId==0)
            {
                return View("~/Plugins/Misc.ProductPromotions/Views/Default.cshtml", new ProductPromotionsListModel
                {
                    Errors = { _localizationService.
                     GetResource("Nop.Plugins.Misc.ProductPromotions.Product.Id.Invalid") }
                });
            }
            var model = _productPromotionsService.GetProductPromotions(customer, addToCartModel.ProductId);

            if (model == null)
            {
                return View("~/Plugins/Misc.ProductPromotions/Views/Default.cshtml", new ProductPromotionsListModel
                {
                    Errors = { _localizationService.
                    GetResource("Nop.Plugins.Misc.ProductPromotions.Discount.NotApplicable") }
                });
            }

            return View("~/Plugins/Misc.ProductPromotions/Views/Default.cshtml", model);
        }

        #endregion
    }
}
