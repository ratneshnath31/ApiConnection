using APIInt;                    // bring in GarminOptions
using APIInt.Services;
using APIInt.Components;          // bring in IGarminAuthService & GarminAuthService
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// 1) bind your "Garmin" section of appsettings.json into GarminOptions
builder.Services.Configure<GarminOptions>(
    builder.Configuration.GetSection("Garmin"));

// 2) register your OAuth service
builder.Services.AddScoped<IGarminAuthService, GarminAuthService>();

// 3) add Razor Components / Blazor
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// 4) middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

// 5) static assets + component routing
app.MapStaticAssets();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
