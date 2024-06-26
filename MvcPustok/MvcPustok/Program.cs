﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcPustok.Data;
using MvcPustok.Models;
using MvcPustok.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
{
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequiredLength = 8;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddScoped<LayoutService>();

builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromSeconds(5);
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
         );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

