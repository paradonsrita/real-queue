using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using QMS.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using QMS.Services;
using Microsoft.AspNetCore.SignalR.Client;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();


// à¾ÔèÁ QueueService áÅÐ HttpClient ÊÓËÃÑºàª×èÍÁµèÍ¡Ñº API
builder.Services.AddHttpClient<QueueService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44328/"); // à»ÅÕèÂ¹à»ç¹ URL ¢Í§ API ¢Í§¤Ø³
});


builder.Services.AddSignalR();

// ตั้งค่า URL สำหรับเชื่อมต่อกับ API ที่มี SignalR
builder.Services.AddScoped(service => new HubConnectionBuilder()
    .WithUrl("https://localhost:44328/notificationHub") // URL ของ API
    .WithAutomaticReconnect() // เพิ่มการเชื่อมต่อใหม่อัตโนมัติ

    .Build()
);

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
