using Nop.Core.Domain.Customers;
using Nop.Plugin.Misc.ProductPromotions.Models;

namespace Nop.Plugin.Misc.ProductPromotions.Services
{
    public interface IProductPromotionsService
    {
        #region Methods
        ProductPromotionsListModel GetProductPromotions(Customer customer, int productId);

        #endregion
    }
}
