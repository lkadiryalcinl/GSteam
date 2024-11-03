using GameService.Base;
using GameService.Data;
using GameService.Repositories.ForCategory;
using GameService.Repositories.ForGame;
using GameService.Services;
using Microsoft.EntityFrameworkCore;
using Global.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<GameDbContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped(typeof(BaseResponseModel));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IFileService, FileService>();

//var consumers = new Dictionary<string, Type>
//{
//    { "game-created-fault", typeof(GameCreatedFaultConsumer) }
//};

//builder.Services.AddMassTransitWithRabbitMq<GameCreatedFaultConsumer>(builder.Configuration, "game", consumers);

builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, "game");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
{
    opts.Authority = builder.Configuration["AuthorityServiceUrl"];
    opts.RequireHttpsMetadata = false;
    opts.TokenValidationParameters.ValidateAudience = false;
    opts.TokenValidationParameters.NameClaimType = "username";

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
