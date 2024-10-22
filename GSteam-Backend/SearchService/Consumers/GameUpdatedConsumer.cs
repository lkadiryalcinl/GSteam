using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class GameUpdatedConsumer(IMapper mapper) : IConsumer<GameUpdated>
    {
        private readonly IMapper _mapper = mapper;
        public async Task Consume(ConsumeContext<GameUpdated> context)
        {
            Console.WriteLine("Game Updated consuming "+context.Message.Name);
            var objDTO = _mapper.Map<GameItem>(context.Message);

            var result = await DB.Update<GameItem>()
                .Match(a => a.ID == context.Message.Id)
                .ModifyOnly(a => new
                {
                    a.Name,
                    a.Description,
                    a.MinimumSystemRequirement,
                    a.Author,
                    a.RecommendedSystemRequirement,
                    a.Price,
                    a.CategoryId,
                },objDTO)
                .ExecuteAsync();

            if (!result.IsAcknowledged)
            {
                throw new Exception("Something went wrong!");
            }        
        }
    }
}
