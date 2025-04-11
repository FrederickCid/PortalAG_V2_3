using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using PortalAG_V2;
using PortalAG_V2.Auth;
using PortalAG_V2.Client.Auth;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Preferences;
using Radzen;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using Syncfusion.Licensing;
using DialogService = Radzen.DialogService;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        #region Synfusion license

        if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/SyncfusionLicense.txt"))
        {
            string licenseKey = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/SyncfusionLicense.txt");
            SyncfusionLicenseProvider.RegisterLicense(licenseKey);
        }

        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("OTk4OTIyQDMyMzAyZTM0MmUzMG0wZFFub1hsaEFrcHlBQy9nVkpUQVAycVZNeWxCNnh5a3prSlBQZkZwN0k9;Mgo+DSMBaFt/QHRqVVhjVFpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jSHxXd0BnXn9ZdX1WRg==;Mgo+DSMBPh8sVXJ0S0J+XE9HflRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31Td0dnWHpad3RSQGBbVw==;Mgo+DSMBMAY9C3t2VVhkQlFadVdJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkRhXX5bdHNVQWZeVUI=;OTk4OTI2QDMyMzAyZTM0MmUzMFJOcVhFM2N2T2FHRjJRRlVnUit3dmRRNGpLOXNiazNhdGgzWjNwa0NEUW89;OTk4OTI3QDMyMzAyZTM0MmUzMFg5U2tDd3Jva2xSZEFhaCtoQklhd0J2c2kyNVFtc1QwUlFjR2VQK3NXQ009");
        #endregion

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        #region Syncfusion Services

        builder.Services.AddSyncfusionBlazor();
        builder.Services.AddScoped<SfDialogService>();

        #endregion

        #region Radzen

        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<TooltipService>();
        builder.Services.AddScoped<ContextMenuService>();

        #endregion

        //Servicio PDF
        builder.Services.AddSingleton<ExportService>();
        builder.Services.AddMudServices();
        ConfigureServices(builder.Services);
        await builder.Build().RunAsync();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthorizationCore();
        services.AddBlazoredLocalStorage();
        services.AddScoped<ClientPreferenceManager>();
        services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacionJWT>();
        services.AddScoped<ProveedorAutenticacionJWT>();
        services.AddScoped<AuthenticationManager>();

        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
            config.SnackbarConfiguration.HideTransitionDuration = 800;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
        });
    }
}