using Microsoft.FluentUI.AspNetCore.Components;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Therasim.Web.Components;
using Azure.AI.OpenAI.Assistants;
using Azure;
using Azure.AI.OpenAI;
using Therasim.Infrastructure.Data;
using Therasim.Web.Services;
using Therasim.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddSingleton(new AssistantsClient(new Uri(builder.Configuration["AzureOpenAI:Endpoint"]!), new AzureKeyCredential(builder.Configuration["AzureOpenAI:Key"]!)));
builder.Services.AddSingleton(new OpenAIClient(new Uri(builder.Configuration["AzureOpenAI:Endpoint"]!), new AzureKeyCredential(builder.Configuration["AzureOpenAI:Key"]!)));

builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<ISimulationService, SimulationService>();
builder.Services.AddScoped<IPsychProblemsService, PsychProblemsService>();
builder.Services.AddScoped<ISkillService, SkillService>();


builder.Services
    .AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:Domain"]!;
        options.ClientId = builder.Configuration["Auth0:ClientId"]!;
        options.Scope = "openid profile email";
    });

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapGet("/Account/Login", async (HttpContext httpContext, string redirectUri = "/") =>
{
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
        .WithRedirectUri(redirectUri)
        .Build();

    await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("/Account/Logout", async (HttpContext httpContext, string redirectUri = "/") =>
{
    var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
        .WithRedirectUri(redirectUri)
        .Build();

    await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
