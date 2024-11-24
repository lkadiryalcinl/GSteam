using AutoMapper;
using Contracts;
using MassTransit;
using OrderService.Data;
using OrderService.Entities;

namespace OrderService.Consumers
{
    public class CheckoutBasketConsumer(IMapper mapper,ApplicationDbContext dbContext) : IConsumer<CheckoutBasketModel>
    {
        public async Task Consume(ConsumeContext<CheckoutBasketModel> context)
        {
            Console.WriteLine("Checout basket consuming with order");

            var item = mapper.Map<Order>(context.Message);
            await dbContext.Orders.AddAsync(item);
            await dbContext.SaveChangesAsync();
        }
    }
}
