using Meadow;
using Meadow.Blazor;
using Meadow.Foundation.ICs.IOExpanders;
using MeadowBlazor.Components;

internal class MeadowApplication : App<Meadow.Desktop>
{
    public override Task Initialize()
    {
        FtdiExpanderCollection.Devices.Refresh();
        var ftdi = FtdiExpanderCollection.Devices[0];
        var output = ftdi.Pins.D7.CreateDigitalOutputPort(false);
        Resolver.Services.Add(output);
        return base.Initialize();
    }

    public override Task Run()
    {
        return base.Run();
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services
            .AddRazorComponents()
            .AddInteractiveServerComponents();

        var app = builder.Build();
        app.UseMeadow<MeadowApplication>();

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
    }
}