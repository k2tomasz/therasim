using Microsoft.FluentUI.AspNetCore.Components;
using Auth0.AspNetCore.Authentication;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Therasim.Web.Components;
using Microsoft.SemanticKernel;
using Therasim.Infrastructure.Data;
using Therasim.Web.Services;
using Therasim.Web.Services.Interfaces;
using Services = Therasim.Web.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddAzureOpenAIChatCompletion(
    deploymentName: "gpt-4o",
    apiKey: builder.Configuration["AzureOpenAI:Key"]!,
    endpoint: builder.Configuration["AzureOpenAI:Endpoint"]!
);

builder.Services.AddTransient((serviceProvider) => new Kernel(serviceProvider));

//builder.Services.AddSingleton(new AssistantsClient(new Uri(builder.Configuration["AzureOpenAI:Endpoint"]!), new AzureKeyCredential(builder.Configuration["AzureOpenAI:Key"]!)));
//builder.Services.AddSingleton(new OpenAIClient(new Uri(builder.Configuration["AzureOpenAI:Endpoint"]!), new AzureKeyCredential(builder.Configuration["AzureOpenAI:Key"]!)));

builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<ISimulationService, SimulationService>();
builder.Services.AddScoped<IProblemService, ProblemService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<Services.Interfaces.IMessageService, Services.MessageService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();

builder.Services
    .AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:Domain"]!;
        options.ClientId = builder.Configuration["Auth0:ClientId"]!;
        options.Scope = "openid profile email";
    });

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();
builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

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

app.MapGet("/account/login", async (HttpContext httpContext, string redirectUri = "/") =>
{
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
        .WithRedirectUri(redirectUri)
        .Build();

    await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("/account/logout", async (HttpContext httpContext, string redirectUri = "/") =>
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
