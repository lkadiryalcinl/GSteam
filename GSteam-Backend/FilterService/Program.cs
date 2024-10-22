using FilterService.Consumers;
using FilterService.Extensions;
using FilterService.Services;
using Global.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddElastic(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var consumers = new Dictionary<string, Type>
{
    { "game-created-filter", typeof(GameCreatedFilterConsumer) }
};

builder.Services.AddMassTransitWithRabbitMq<GameCreatedFilterConsumer>(builder.Configuration, "filter", consumers);

builder.Services.AddScoped<IFilterGameService,FilterGameService>();

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

app.Run();
