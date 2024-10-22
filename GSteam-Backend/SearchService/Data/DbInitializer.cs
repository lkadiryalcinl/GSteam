using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitializeDb(WebApplication app)
        {
            await DB.InitAsync("GameSearchDb", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

            await DB.Index<GameItem>()
                .Key(x => x.Name, KeyType.Text)
                .Key(x => x.Description, KeyType.Text)
                .Key(x => x.Author, KeyType.Text).CreateAsync();


        }
    }
}
