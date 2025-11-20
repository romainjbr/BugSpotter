

using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


builder.Services.AddDbContext<BugDb>();

//Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IUserRepository, IUserRepository>();

//Core Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISightingService, SightingService>();
builder.Services.AddScoped<IBugService, BugService>();

//Infrastructure Services 
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
