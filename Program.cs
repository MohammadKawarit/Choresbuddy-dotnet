using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using Choresbuddy_dotnet.Data;
using Choresbuddy_dotnet.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddScoped<IRewardService, RewardService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IRewardCartService, RewardCartService>();
builder.Services.AddScoped<ITrophyService, TrophyService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

var app = builder.Build(); 

// Configure Middleware
    app.UseSwagger();
    app.UseSwaggerUI();
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");
app.MapGet("/", () => "Welcome to ChoresBuddy API!");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
