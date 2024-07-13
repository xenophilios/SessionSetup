using D.Data;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<DC>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("DC"))
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
    .EnableDetailedErrors());

builder.Services
    .AddDistributedSqlServerCache(options =>
    { // for Session State

        options.ConnectionString = builder.Configuration.GetConnectionString("DC");

        options.SchemaName = "dbo";
        options.TableName = "SS";

        /* From Performance/Caching/Distributed Caching
         * in ASP.NET 3.1 documentation (dated 02/07/20).
         */
    });

builder.Services.AddCookiePolicy(options =>
{
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services
    .AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(20);
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;        
        options.Cookie.Name = "Alexander";
    });

builder.Services
    .AddAntiforgery(options =>
    {
        options.FormFieldName = "RTF";
        options.HeaderName = "RTH";

        //options.Cookie.HttpOnly = true; // default
        //options.Cookie.IsEssential = true; // default
        //options.Cookie.SameSite = SameSiteMode.Strict; // default

        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.Name = "RTC";

    });

builder.Services
    .AddHsts(options =>
    {
        options.MaxAge = TimeSpan.FromHours(1.0);
        options.IncludeSubDomains = true;
    });

builder.Services
    .AddControllersWithViews(options =>
    {
        options.Filters.Add(

            new ResponseCacheAttribute
            {
                NoStore = true,
                Location = ResponseCacheLocation.None
            });

        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    });

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error/generalerror");
}

app.UseHttpsRedirection();
app.UseFileServer(false);
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();
app.UseRequestLocalization();
app.UseSession();

if (!builder.Environment.IsDevelopment())
{
    app.UseHsts();
}

_ = app.MapControllerRoute(
    string.Empty, "{controller}/{action}/{id}");

_ = app.MapControllerRoute(
    string.Empty, "{controller=Open}/{action=PrepareLogin}");

app.Run();