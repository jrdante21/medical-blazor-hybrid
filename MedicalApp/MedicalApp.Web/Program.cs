using MedicalApp.Shared.Data;
using MedicalApp.Shared.Hubs;
using MedicalApp.Shared.Services;
using MedicalApp.Web.Components;
using MedicalApp.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(
    connectionString,
    ServerVersion.AutoDetect(connectionString)
));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add Bootstrap Blazor
builder.Services.AddBlazorBootstrap();

// Add SignalR
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["application/octet-stream"]);
});
// Add Hub Connection
builder.Services.AddScoped(sp =>
{
    var navMan = sp.GetRequiredService<NavigationManager>();
    //var accessTokenProvider = sp.GetRequiredService<IAccessTokenProvider>();
    return new HubConnectionBuilder()
        .WithUrl(navMan.ToAbsoluteUri("/recordhub")
            //, options =>
            //{
            //    options.AccessTokenProvider = async () =>
            //    {
            //        var accessTokenResult = await accessTokenProvider.RequestAccessToken();
            //        accessTokenResult.TryGetToken(out var accessToken);
            //        return accessToken.Value;
            //    };
            //}
        )
        .WithAutomaticReconnect()
        .Build();
});

// Add device-specific services used by the MedicalApp.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

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
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(MedicalApp.Shared._Imports).Assembly);

app.UseResponseCompression();
app.MapHub<RecordHub>("/recordhub");

app.Run();
