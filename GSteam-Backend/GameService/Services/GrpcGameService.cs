using GameService.Data;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace GameService.Services
{
    public class GrpcGameService(GameDbContext gameDbContext) : GrpcGame.GrpcGameBase
    {
        public override async Task<GrpcGameResponse> GetGame(GetGameRequest request,ServerCallContext context)
        {
            
            Console.WriteLine("==>> Grpc Recieved call service started");

            var game = await gameDbContext.Games.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.Id) && x.UserId == request.UserId);

            if (game == null)
            {

            }

            var response = new GrpcGameResponse
            {
                Game = new GrpcGameModel
                {
                    GameName = game.Name,
                    Price = Convert.ToDouble(game.Price),
                    VideoUrl = game.VideoUrl,
                    GameDescription = game.Description,
                    MinimumSystemRequirement = game.MinimumSystemRequirement,
                    RecommendedSystemRequirement = game.RecommendedSystemRequirement,
                    UserId = game.UserId,
                    CategoryId = game.CategoryId.ToString()
                }
            };

            return response;
        }
    }
}
