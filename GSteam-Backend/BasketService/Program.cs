using BasketService.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Global.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
{
    opts.Authority = builder.Configuration["AuthorityServiceUrl"];
    opts.RequireHttpsMetadata = false;
    opts.TokenValidationParameters.ValidateAudience = false;
    opts.TokenValidationParameters.NameClaimType = "username";

});

builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, "basket");

builder.Services.AddScoped<IBasketRepository,BasketRepository>();

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
