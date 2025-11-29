using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Settings;
using Presentation.Endpoints;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddDbContext<BugDb>();

//Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IUserRepository, EfUserRepository>();

//Core Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISightingService, SightingService>();
builder.Services.AddScoped<IBugService, BugService>();

//Infrastructure Services 
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();

builder.Services.AddSwaggerWithJwt();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapUserEndpoints();
app.MapBugEndpoints();
app.MapSightingEndpoints();

app.Run();

public partial class Program { }