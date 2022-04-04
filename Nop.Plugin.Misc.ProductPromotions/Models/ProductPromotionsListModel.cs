using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Misc.ProductPromotions.Models
{
    public partial class ProductPromotionsListModel
    {
        #region Ctor

        public ProductPromotionsListModel()
        {
            Promotions = new List<ProductPromotionsModel>();
            Errors = new List<string>();
        }

        #endregion

        #region Properties

        public List<ProductPromotionsModel> Promotions { get; set; }
        public IList<string> Errors { get; set; }
        public bool Success => !Errors.Any();

        #endregion
    }
}