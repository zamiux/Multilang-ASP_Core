using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Multilang.Context;
using Multilang.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


#region Multilang
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
#endregion

// Add services to the container with multilang.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
    options => {
        options.ResourcesPath = "Resources";
})
    .AddDataAnnotationsLocalization(options => {
        options.DataAnnotationLocalizerProvider =
        (type, factory) => factory.Create(typeof(ShareResource));
    });

#region Service To DB Connection
// Start Service To DB Connection
    builder.Services.AddDbContext<MultilangDBContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

// End Service To DB Connection
#endregion



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
#region MultiLang
app.UseCookiePolicy();
#endregion

app.UseRouting();

app.UseAuthorization();

#region MultiLage
var supportCulture = new List<CultureInfo>()
{
    new CultureInfo("fa-IR"),
    new CultureInfo("en-US")
};

var options = new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture("fa-IR"),
    SupportedCultures = supportCulture,
    SupportedUICultures = supportCulture,
    RequestCultureProviders = new List<IRequestCultureProvider>()
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    }
};

app.UseRequestLocalization(options);
#endregion

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
