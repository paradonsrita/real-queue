using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using QMS.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Headers;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();


// à¾ÔèÁ QueueService áÅÐ HttpClient ÊÓËÃÑºàª×èÍÁµèÍ¡Ñº API
builder.Services.AddHttpClient("Queue", client =>
{
    client.BaseAddress = new Uri("https://192.168.1.15:44328/");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
    };
});

builder.Services.AddSignalR();

// ตั้งค่า URL สำหรับเชื่อมต่อกับ API ที่มี SignalR
builder.Services.AddScoped(service =>
{
    var connection = new HubConnectionBuilder()
        .WithUrl("https://192.168.1.15:44328/notificationHub", options =>
        {
            options.HttpMessageHandlerFactory = _ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            };
        })
        .WithAutomaticReconnect() // เพิ่มการเชื่อมต่อใหม่อัตโนมัติ
        .Build();

    // การจัดการสถานะการเชื่อมต่อ
    connection.Closed += async (error) =>
    {
        Console.WriteLine("Connection closed. Trying to reconnect...");
        await Task.Delay(new Random().Next(0, 5) * 1000);
        await connection.StartAsync(); // พยายามเชื่อมต่อใหม่
    };

    return connection;
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
