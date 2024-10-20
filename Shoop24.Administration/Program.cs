using MudBlazor;
using MudBlazor.Services;
using Shoop24.Administration.Components;
using ShoopCommunication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddMudBlazorDialog();
builder.Services.AddSignalR();
builder.Services.AddScoped<ShoopClient>(sp => new ShoopClient(null));
builder.Services.AddMudBlazorSnackbar(o =>
{
    o.NewestOnTop = true;
    o.HideTransitionDuration = 200;
    o.ShowTransitionDuration = 300;
    o.SnackbarVariant = Variant.Filled;
    o.MaxDisplayedSnackbars = 5;
    o.PositionClass = Defaults.Classes.Position.TopCenter;
});

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
