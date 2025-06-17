using ForumSentiment.Data;
using ForumSentiment.Models;
using ForumSentiment.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// ЯСНО зададен connection string към ForumSentimentDB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=localhost,1433;Database=ForumSentimentDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<SentimentService>();

var app = builder.Build();

// Seed roles and admin
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Admin", "Moderator", "User" };

    foreach (var role in roles)
    {
        if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
        {
            roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
        }
    }

    string adminEmail = "admin@forum.com";
    string adminPassword = "Admin123!";

    var admin = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = userManager.CreateAsync(admin, adminPassword).GetAwaiter().GetResult();
        if (result.Succeeded)
        {
            userManager.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

if (args.Contains("--train"))
{
    Trainer.Train("comments.tsv", "SentimentModel.zip");
}
app.Run();
