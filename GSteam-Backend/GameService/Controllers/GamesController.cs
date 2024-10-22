using GameService.DTOs;
using GameService.Repositories.ForGame;
using GameService.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController(IGameRepository gameRepository, IFileService fileService) : ControllerBase
    {
        private readonly IGameRepository _gameRepository = gameRepository;
        private readonly IFileService _fileService = fileService;

        [HttpPost]
        public async Task<ActionResult> CreateGame([FromForm] GameDTO game)
        {
            if (ModelState.IsValid)
            {
                if (game.GameFile == null || game.VideoFile == null)
                {
                    return BadRequest("inner");
                }
                var response = await _gameRepository.CreateGame(game);
                return Ok(response);
            }
            return BadRequest("outer");

        }

        [HttpDelete("{gameId}")]
        public async Task<ActionResult> RemoveGame([FromRoute] Guid gameId)
        {
            var response = await _gameRepository.RemoveGame(gameId);
            return Ok(response);
        }


        [HttpGet]
        public async Task<ActionResult> GetAllGames()
        {
            var response = await _gameRepository.GetAllGames();
            return Ok(response);
        }


        [HttpPost("Download")]
        public async Task<ActionResult> DownloadGame(string fileUrl)
        {
            var response = await _fileService.DownloadGame(fileUrl);
            return Ok(response);
        }


        [HttpGet("{categoryId}")]
        public async Task<ActionResult> GetGamesByCategoryId([FromRoute] Guid categoryId)
        {
            var response = await _gameRepository.GetGamesByCategory(categoryId);
            return Ok(response);
        }

        [HttpPut("{gameId}")]
        public async Task<ActionResult> UpdateGame(UpdateGameDTO model, [FromRoute] Guid gameId)
        {
            var response = await _gameRepository.UpdateGame(model, gameId);
            return Ok(response);
        }

        [HttpGet("game/{gameId}")]
        public async Task<ActionResult> GetGameById([FromRoute] Guid gameId)
        {
            var response = await _gameRepository.GetGameById(gameId);
            return Ok(response);
        }
    }
}
