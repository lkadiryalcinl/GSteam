using AutoMapper;
using Contracts;
using GameService.Base;
using GameService.Data;
using GameService.DTOs;
using GameService.Entities;
using GameService.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace GameService.Repositories.ForGame
{
    public class GameRepository(GameDbContext gameDbContext, IMapper mapper, IFileService fileService, BaseResponseModel baseResponseModel, IHttpContextAccessor httpContextAccessor, IPublishEndpoint publishEndpoint) : IGameRepository
    {
        private readonly GameDbContext _context = gameDbContext;
        private IMapper _mapper = mapper;
        private IFileService _fileService = fileService;
        private BaseResponseModel _responseModel = baseResponseModel;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IHttpContextAccessor _httpContextAccesor = httpContextAccessor;

        public async Task<BaseResponseModel> CreateGame(GameDTO game)
        {
            if (game.VideoFile.Length > 0 && game.GameFile.Length > 0)
            {
                string videoUrl = await _fileService.UploadVideo(game.VideoFile);
                string gameUrl = await _fileService.UploadZip(game.GameFile);
                var objDTO = _mapper.Map<Game>(game);
                objDTO.VideoUrl = videoUrl;
                objDTO.GameImages.Add(new GameImage
                {
                    GameId = objDTO.Id,
                    URL = gameUrl
                });
                await _context.Games.AddAsync(objDTO);
                await _publishEndpoint.Publish(_mapper.Map<GameCreated>(objDTO));

                if (await _context.SaveChangesAsync() > 0)
                {

                    _responseModel.IsSuccess = true;
                    _responseModel.Message = "Created Game Successfully";
                    _responseModel.Data = objDTO;
                    return _responseModel;
                }
            }

            _responseModel.IsSuccess = false;
            return _responseModel;
        }

        public async Task<BaseResponseModel> GetAllGames()
        {
            List<Game> games = await _context.Games.Include(x => x.Category).Include(x => x.GameImages).ToListAsync();
            if (games is not null)
            {
                _responseModel.Data = games;
                _responseModel.IsSuccess = true;
                return _responseModel;
            }

            _responseModel.IsSuccess = false;
            return _responseModel;
        }


        public async Task<BaseResponseModel> RemoveGame(Guid gameId)
        {
            Game game = await _context.Games.FindAsync(gameId);
            if (game is not null)
            {
                _context.Games.Remove(game);
                await _publishEndpoint.Publish<GameDeleted>(new { Id = gameId.ToString() });
                if (await _context.SaveChangesAsync() > 0)
                {
                    _responseModel.IsSuccess = true;
                    _responseModel.Data = game;
                    return _responseModel;
                }

            }
            _responseModel.IsSuccess = false;
            return _responseModel;
        }

        public async Task<BaseResponseModel> UpdateGame(UpdateGameDTO game, Guid gameId)
        {
            Game gameObj = await _context.Games.FindAsync(gameId);
            if (gameObj is not null)
            {
                //gameObj.Price = game.Price;
                //gameObj.RecommendedSystemRequirement = game.RecommendedSystemRequirement;
                //gameObj.MinimumSystemRequirement = game.MinimumSystemRequirement;
                //gameObj.Author = game.Author;
                //gameObj.Name = game.Name;
                //gameObj.Description = game.Description;
                _mapper.Map<UpdateGameDTO, Game>(game, gameObj);
                await _publishEndpoint.Publish(_mapper.Map<GameUpdated>(gameObj));
                if (await _context.SaveChangesAsync() > 0)
                {
                    _responseModel.IsSuccess = true;
                    _responseModel.Data = gameObj;
                    return _responseModel;
                }
            }

            _responseModel.IsSuccess = false;
            return _responseModel;
        }

        public async Task<BaseResponseModel> GetGamesByCategory(Guid categoryId)
        {
            List<Game> games = await _context.Games.Include(x => x.GameImages).Where(x => x.CategoryId == categoryId).ToListAsync();
            if (games is not null)
            {
                _responseModel.Data = games;
                _responseModel.IsSuccess = true;
                return _responseModel;
            }

            _responseModel.IsSuccess = false;
            return _responseModel;
        }

        public async Task<BaseResponseModel> GetGameById(Guid gameId)
        {
            var result = await _context.Games.Include(x => x.GameImages).FirstOrDefaultAsync(x => x.Id == gameId);
            if (result is not null)
            {
                _responseModel.Data = result;
                _responseModel.IsSuccess = true;
                return _responseModel;
            }
            _responseModel.IsSuccess = false;
            return _responseModel;

        }
    }
}
