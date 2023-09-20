using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using motel.Data;
using motel.Repositories;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<AdminDbContext>(options =>

options.UseSqlServer(builder.Configuration.GetConnectionString("AdminDbContext")));
builder.Services.AddScoped<IUserRepositories, UserRepositories>();
builder.Services.AddScoped<ITierRepositories, TierRepositories>();
builder.Services.AddScoped<IRoleRepositories, RoleRepositories>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
 option.TokenValidationParameters = new TokenValidationParameters
 {
     ValidateIssuer = true,
     ValidateAudience = true,
     ValidateLifetime = true,
     ValidateIssuerSigningKey = true,
     ValidIssuer = builder.Configuration["Jwt:Issuer"],
     ValidAudience = builder.Configuration["Jwt:Audience"],
     ClockSkew = TimeSpan.Zero,
     IssuerSigningKey = new SymmetricSecurityKey(
 Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
 });
builder.Services.AddIdentityCore<IdentityUser>()
 .AddRoles<IdentityRole>()
 .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Admin")
 .AddEntityFrameworkStores<AdminDbContext>()
 .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequireDigit = false;// Yêu c?u v? password ch?a ký s?: không
    option.Password.RequireLowercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequiredLength = 6;
    option.Password.RequiredUniqueChars = 1;
});

IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json")
            .Build();
builder.Services.Configure<JwtSecurityToken>(configuration.GetSection("JwtSettings"));

var app = builder.Build();
app.UseAuthorization();
app.UseAuthentication();
//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
