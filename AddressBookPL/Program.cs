﻿using AddressBookDL;
using AddressBookEL.IdentityModels;
using AddressBookEL.Mapping;
using AddressBookPL.DefaultData;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});

var lockoutOptions = new LockoutOptions()
{
    AllowedForNewUsers = true,
    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1),
    MaxFailedAccessAttempts = 2
};

//identity ayarları
builder.Services.AddIdentity<AppUser,AppRole>(options =>
{
    //ayarlar eklenecek
    options.Password.RequiredLength = 4;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false; //@ / () [] {} ? : ; karakterler
    options.Password.RequireDigit = false;  
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz-_0123456789";
    options.Lockout = lockoutOptions;


}).AddDefaultTokenProviders().AddEntityFrameworkStores<MyContext>();

//auto mapper ayarları

builder.Services.AddAutoMapper(x =>
{
    x.AddExpressionMapping();
    x.AddProfile(typeof(Maps));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

//interfacelerin DI için yaşam döngüleri (AddScoped)

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles(); //wwwroot

app.UseRouting();

app.UseAuthentication(); ///login logout
app.UseAuthorization(); //yetki

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//Proje ilk çalışacağı zaman default olarak eklenmesini istediğiniz verileri ya da başka işlemleri yazdığınız class'ı burada çağırmalısınız

//buraya geri döneceğiz

//app.Data(); //extension metot olarak çağırmak
//DataDefault().Data(app); //harici çağırmak

//Xihan Shen ablanın yönteminden yapalım böylece Erdener'in static olmasın isteğini yerine getirelim!


using (var scope = app.Services.CreateScope())
{
    //Resolve ASP .NET Core Identity with DI help
    var userManager = (UserManager<AppUser>?)scope.ServiceProvider.GetService(typeof(UserManager<AppUser>));
    var roleManager = (RoleManager<AppRole>?)scope.ServiceProvider.GetService(typeof(RoleManager<AppRole>));
    // do you things here

    DataDefaultXihan d = new DataDefaultXihan();
    d.CheckAndCreateRoles(roleManager);

}


app.Run(); //uygulamayı çalıştırır
