using CorporatePortal.Api.Data;
using Microsoft.EntityFrameworkCore;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "oidc";
})
.AddCookie("cookie", options =>
{
    options.Cookie.Name = "__Host-bff";
    options.Cookie.SameSite = SameSiteMode.Strict;
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = builder.Configuration["IdentityServer:Authority"];
    options.ClientId = builder.Configuration["IdentityServer:ClientId"];
    options.ClientSecret = builder.Configuration["IdentityServer:ClientSecret"];
    options.ResponseType = "code";
    options.ResponseMode = "query";
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.Scope.Add("api");
    options.Scope.Add("profile");
    options.Scope.Add("email");
});

// Add BFF
builder.Services.AddBff(options =>
{
    options.ManagementBasePath = "/bff";
})
.AddRemoteApis();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowVueApp");

app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapBffManagementEndpoints();

// Map API endpoints
app.MapControllers()
    .RequireAuthorization()
    .AsBffApiEndpoint();

app.Run();

