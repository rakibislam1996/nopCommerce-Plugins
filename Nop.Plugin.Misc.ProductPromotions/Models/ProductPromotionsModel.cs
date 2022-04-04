using Nop.Web.Framework.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Misc.ProductPromotions.Models
{
    public class ProductPromotionsModel : BaseNopModel
    {
        #region Ctor

        public ProductPromotionsModel()
        {

        }

        #endregion

        #region Properties

        public string Name { get; set; }
        public bool UsePercentage { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool RequiresCouponCode { get; set; }
        public string CouponCode { get; set; }

        #endregion
    }
}