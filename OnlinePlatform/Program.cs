using Application.IServices;
using Domain;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var config = builder.Configuration.Get<AppSettings>();
builder.Services.Configure<AppSettings>(builder.Configuration);


builder.Services.AddSingleton(config!);
builder.Configuration.AddJsonFile("appsetting.json", optional: true, reloadOnChange: true);
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddScoped<IOllamaService, OllamaService>();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Ollama}/{action=GetAIResponse}/{id?}");

app.Run();
