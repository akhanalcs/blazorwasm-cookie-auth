using BlazorServer.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Stuffs I added 👇
// Gotten from BlazorWASM.Backend project's Program.cs
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {/*
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 302;
            context.Response.Headers["location"] = "https://localhost:7115/Account/Login";
            //context.Response.Redirect(context.RedirectUri);
            //context.RedirectUri = "https://localhost:7115/Account/Login";
            return Task.CompletedTask;
        };*/
        //options.LoginPath = "https://localhost:7115/Account/Login";
    });

builder.Services.AddDataProtection()
    .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect("127.0.0.1:6379"))
    .SetApplicationName("my-sso-apps");
// Stuffs I added 👆

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();