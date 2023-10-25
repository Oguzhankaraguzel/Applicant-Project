using CAWA.Application.Validations.FluentValidations;
using CAWA.Domain.Identity;
using CAWA.Infrastructure;
using CAWA.MVCUI.Filters;
using CAWA.Persistence;
using CAWA.Persistence.Context.SeedData;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<AplicantInformationCreateValidation>());

builder.Services.AddScoped<AdminInformationFilter>();
builder.Services.AddTransient<InviterNameSaveFilter>();

builder.Services.AddSession();
builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

            NameClaimType = ClaimTypes.Name
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await SeedData.Initialize(roleManager, userManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.Use(async (httpContext, next) =>
{
    var JWToken = httpContext.Session.GetString("JWToken");
    if (!string.IsNullOrEmpty(JWToken))
    {
        httpContext.Request.Headers.Add("Authorization", "Bearer " + JWToken);
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();


app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 401)
    {
        context.Response.Redirect("/Account/Login");
    }
    if (context.Response.StatusCode == 403)
    {
        context.Response.Redirect("/Account/AccessDenied");
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{invitingUserName?}");

app.Run();
