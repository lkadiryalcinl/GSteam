using SearchService.Consumers;
using SearchService.Data;
using Global.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var consumers = new Dictionary<string, Type>
{
    { "game-created", typeof(GameCreatedConsumer) }
};

builder.Services.AddMassTransitWithRabbitMq<GameCreatedConsumer>(builder.Configuration, "search", consumers);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () => {
    try
    {
        await DbInitializer.InitializeDb(app);
    }
    catch (System.Exception)
    {

        throw;
    }
});

app.Run();
