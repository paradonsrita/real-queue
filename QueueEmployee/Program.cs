using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using QMS.Services;
using QMS.Services.LocalStorage;
using Radzen;
using System.Net.Http;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddScoped<LocalStorageService>();

//calendar
builder.Services.AddScoped<DialogService>(); // Add this line


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//API
builder.Services.AddScoped<ApiProvider>();
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:44328/");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
