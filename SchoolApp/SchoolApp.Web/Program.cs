using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Services.Data;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationDatabase(builder.Configuration);

builder.Services.AddSingleton(x =>
    new BlobServiceClient(builder.Configuration.GetConnectionString("AzureStorage") ??
        throw new InvalidOperationException("Connection string 'AzureStorage' not found.")));

builder.Services.AddApplicationIdentity(builder.Configuration);

builder.Services.RegisterRepositories();
builder.Services.RegisterUserDefinedServices(typeof(IDiaryService).Assembly);

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddApplicationAuthentication();

builder.Services.AddControllersWithViews(cfg =>
{
    cfg.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});

app.MapRazorPages();

await app.RunAsync();
