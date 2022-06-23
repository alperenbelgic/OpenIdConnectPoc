using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenIdConnectPoC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseInMemoryDatabase("dbname", builder => { });
});

builder.Services
    .AddDefaultIdentity<IdentityUser>(options => { })
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services
    .AddAuthentication()
    .AddGoogle(options =>
{
    options.ClientId = "332239365544-aplvo6vo18pquqrja7tk6a1veoclqmj7.apps.googleusercontent.com";
    options.ClientSecret = "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";

    options.CallbackPath = new PathString("/signin");

    //options.Scope.Add("https://www.googleapis.com/auth/calendar.readonly");
    //options.Scope.Add("https://www.googleapis.com/auth/gmail.labels");

    options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
    {
        OnAccessDenied = (context) =>
        {
            return Task.CompletedTask;
        },
        OnCreatingTicket = (context) =>
        {
            return Task.CompletedTask;
        },
        OnRemoteFailure = (context) =>
        {
            return Task.CompletedTask;
        },
        OnTicketReceived = (context) =>
        {
            return Task.CompletedTask;
        },
    };
});

builder.Services.AddTransient<AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
