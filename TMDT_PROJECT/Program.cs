using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TMDT_PROJECT.Data;
using TMDT_PROJECT.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<Game_DBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChuoiKetNoi"));
});


//Add authenservice
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.LoginPath = "/Customer/Login";
        options.AccessDeniedPath = "/Forbidden/";
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddSingleton(x => new PaypalClient
(
    builder.Configuration["PaypalOptions:ClientId"],
    builder.Configuration["PaypalOptions:ClientSecret"],
    builder.Configuration["PaypalOptions:Mode"]
    ));

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
