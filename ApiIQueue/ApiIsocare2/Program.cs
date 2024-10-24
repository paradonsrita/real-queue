using Newtonsoft.Json.Serialization;
using System.Text;
using ApiIsocare2.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using ApiIsocare2.Utilities.Interface;
using ApiIsocare2.Utilities;
using ApiIsocare2.Utilities.SignalR;
using Microsoft.AspNetCore.Cors.Infrastructure;



namespace ApiIsocare2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //ngrok
            // กำหนดค่า Kestrel ให้รองรับ HTTPS
            builder.WebHost.UseIISIntegration();



            //forget password
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });



            builder.Services.AddScoped<IEmailService, EmailService>();
            // Add services to the container.
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //JSON Serializer
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(options =>
            options.SerializerSettings.ContractResolver = new DefaultContractResolver());


            // เพิ่มการตั้งค่า CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:7062")
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials(); // ต้องอนุญาต Credentials สำหรับ WebSocket

                    });
            });

            builder.Services.AddSignalR();  // ต้องใส่บรรทัดนี้



            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // Ensure authentication is used

            app.UseAuthorization();
            app.UseStaticFiles(); //มีประโยชน์ในการจัดการไฟล์ที่ผู้ใช้สามารถเข้าถึงได้โดยตรง

            app.UseCors("AllowSpecificOrigin"); // เพิ่มส่วนนี้ใน pipeline

            app.MapHub<NotificationHub>("/notificationHub");

            app.MapControllers();





            app.Run();
        }
    }
}
