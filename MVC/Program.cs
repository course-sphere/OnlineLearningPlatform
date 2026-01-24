using Application;
using Application.IServices;
using Application.MyMapper;
using Domain;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration.Get<AppSettings>();
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(config!.ConnectionStrings.DefaultConnection);
    options.EnableSensitiveDataLogging()
           .EnableDetailedErrors()
           .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
    options.ConfigureWarnings(warning =>
        warning.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Auth/Login";
        opt.AccessDeniedPath = "/Auth/Login";
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzk2MTY5NjAwIiwiaWF0IjoiMTc2NDY1MzgwMSIsImFjY291bnRfaWQiOiIwMTlhZGQ4ODAxNmE3NDllOTNjNzRjOTE1MTcwM2I0YiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2JlczA3cDAyODN3c2I3aHNzY25ucmRoIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.FgAdXhDW2NOg4jdFCe0_ybRFo8GseXC6oDmzK1j9SUDDPmX10Dezd_4mItXx7WbaBUcItVN5FW5w-IN0tWFDhsNnC1hQ4ajkzN4Gj8WS2ZCAQNTxKVpCBDXfJbGXWKNc1aZDcbjpE_96_u1xPHEgOnZrDn-V_SNr4PRcQDNVjwP94GF_fJzbBvEsaPwtiOusZEQfEpE80ZdW2_5p9IxOiCirW1S0WYV71gJtq2KBc-O36wUBNPhLiVDmT4SEeRHRwftfLfcuawhK2Ru_hdaJvQocjglZ2YSMFWp67X1uxueRLeiLNV7u9so32QjcjJpg_eH6BAtPQGqKTH_mXTUkiQ", typeof(MapperConfigurationProfile));
builder.Services.AddSingleton(config!);
builder.Configuration.AddJsonFile("appsetting.json", optional: true, reloadOnChange: true);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<ILessonService, LessonService>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" }
);
app.Run();
