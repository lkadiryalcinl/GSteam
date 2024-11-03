using BasketService.Base;
using BasketService.Models;

namespace BasketService.Repository
{
    public interface IBasketRepository
    {
        Task<ResponseModel<bool>> AddBasket(BasketModel model);
        Task<ResponseModel<BasketModel>> GetBasketItem(long index);
        Task<ResponseModel<List<BasketModel>>> GetBasketItems();
        Task<ResponseModel<bool>> RemoveBasketItem(long index);
        Task<ResponseModel<BasketModel>> UpdateBasketItem(BasketModel model,long index);

        Task<ResponseModel<bool>> Checkout();
    }
}
