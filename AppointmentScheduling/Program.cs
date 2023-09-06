using AppointementScheduling.Models;
using AppointementScheduling.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using AppointementScheduling.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDBcontext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Home/AccessDenied");
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBcontext>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDbInitializer,DbInitializer>();
builder.Services.AddTransient<IAppointementService,AppointementService>();
builder.Services.AddIdentityCore<ApplicationUser>(opt =>
{
    opt.Password.RequireLowercase= false;
    opt.Password.RequireUppercase= false;
    opt.Password.RequireNonAlphanumeric=false;
});
builder.Services.AddControllers()
       .AddJsonOptions(options =>
       {
           options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
           options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
           options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
           options.JsonSerializerOptions.IgnoreNullValues = true;
           options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
          
       });
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
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider.GetRequiredService<IDbInitializer>;
    services.Invoke().Initialize();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
