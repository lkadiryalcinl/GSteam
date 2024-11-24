using DiscountService.Data;
using DiscountService.Entities;
using DiscountService.Models;
using DiscountService.Services;
using System.Security.Claims;

namespace DiscountService.Repository
{
    public class DiscountRepository(AppDbContext context, GrpcGameClient grpcClient, IHttpContextAccessor contextAccessor) : IDiscountRepository
    {
        private readonly string UserId = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public async Task<bool> CreateDiscount(DiscountModel model)
        {
            try
            {
                if (model != null)
                {
                    Console.WriteLine("trigger inner discount ----> " + model.UserId);
                    var game = grpcClient.GetGame(model.GameId, UserId);
                    if (!string.IsNullOrEmpty(game.Name))
                    {
                        Discount discount = new()
                        {
                            CouponCode = model.CouponCode,
                            DiscountAmount = model.DiscountAmount,
                            GameId = model.GameId,
                            UserId = game.UserId,
                        };
                        await context.Discounts.AddAsync(discount);
                        if (await context.SaveChangesAsync() > 0)
                        {
                            return true;

                        }

                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }
    }
}
