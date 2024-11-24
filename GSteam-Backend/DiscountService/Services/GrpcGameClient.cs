using DiscountService.Models;
using GameService;
using Grpc.Net.Client;

namespace DiscountService.Services
{
    public class GrpcGameClient(ILogger<GrpcGameClient> logger, IConfiguration configuration)
    {

        public Game GetGame(string gameId,string userId) 
        {
            logger.LogWarning("Calling grpc protobuf service");
            var channel = GrpcChannel.ForAddress(configuration["GrpcGame"]);
            var client = new GrpcGame.GrpcGameClient(channel);
            var request = new GetGameRequest
            {
                Id = gameId,
                UserId = userId
            };

            try
            {
                var response = client.GetGame(request);
                Game game = new()
                {
                    Name = response.Game.GameName,
                    Price = Convert.ToDecimal(response.Game.Price),
                    VideoUrl = response.Game.VideoUrl,
                    Description = response.Game.GameDescription,
                    MinimumSystemRequirement = response.Game.MinimumSystemRequirement,
                    RecommendedSystemRequirement = response.Game.RecommendedSystemRequirement,
                    UserId = response.Game.UserId,
                    CategoryId = Guid.Parse(response.Game.CategoryId),
                };
                return game;
            }
            catch (Exception ex) {
                logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
