using DataAccess.Contexts;
using DataAccess.Services;
using DataAccess.Services.Bases;
using ETradeCoreBilgeAdam.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region Localization
List<CultureInfo> cultures = new List<CultureInfo>()
{
    new CultureInfo("en-US")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name);
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});
#endregion

// bu session'un kullan�lmas� i�in a�a��da use kullanmak gerekiyor
builder.Services.AddSession(options =>  // eklenen �r�nlerin sepette kalma s�resi
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // default 20 mins
});

var section = builder.Configuration.GetSection(nameof(AppSettings));    // AppSettings'i buraya tan�ml�yoruz
section.Bind(new AppSettings());

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); // country ve cities aras�nda cycle oldu�u i�in addjson methodu yapt�k

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(config =>   // cookie taraf�
{
    config.LoginPath = "/Accounts/Home/Login";  // giri�i i�in path verdik
    config.AccessDeniedPath = "/Accounts/Home/AccessDenied";    // hatal� giri�
    config.ExpireTimeSpan = TimeSpan.FromMinutes(30);   // 30 dk cookie kapanacak
    config.SlidingExpiration = true;        // e�er i�lem yap�yorsa 30dk s�rekli uzat�lacak
});

#region IoC Container (Inversion of Control Container)
// Autofac, Ninject
var connectionString = builder.Configuration.GetConnectionString("ETiracretDb");
builder.Services.AddDbContext<Db>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ProductServiceBase, ProductService>();   // dependecy inj. productservicebase g�rd��� yerde protuctservice'i newler
builder.Services.AddScoped<CategoryServiceBase, CategoryService>();
/*builder.Services.AddSingleton<CategoryServiceBase, CategoryService>();*/  // AddSingleton uygulama kapanana kadar a��k kal�yor tek sefer new'leme yapar
builder.Services.AddScoped<ShopServiceBase, ShopService>();// AddScoped istek �a��r�ld��� zaman new'ler sonra biter 
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<UserServiceBase, UserService>();
builder.Services.AddScoped<CountryServiceBase, CountryService>();
builder.Services.AddScoped<CityServiceBase, CityService>();
builder.Services.AddScoped<IReportService, ReportService>();
#endregion

var app = builder.Build();

#region Localization
app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name),
    SupportedCultures = cultures,
    SupportedUICultures = cultures,
});
#endregion

AppCore.App.Environment.IsDevelopment = app.Environment.IsDevelopment();

// Configure the HTTP request pipeline.
if (!AppCore.App.Environment.IsDevelopment)
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();    // Authentication ekledikten sonra burda kullanmam�z laz�m ----- sen kimsin ?
                            // S�ralama �nemli �nce authentication sonra authorization gelmeli !!!!!!!!
app.UseAuthorization();     // sen neler yapabilirsin?

app.UseEndpoints(endpoints =>   // MVC Area'da Accounts olu�turduk ��kan yolu buraya ekledik
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.UseEndpoints(endpoints =>   // �sttekinin farkl� bir y�ntemle denemesi
{
    endpoints.MapControllerRoute(
      name: "welcome",
      pattern: "Home/Index"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
