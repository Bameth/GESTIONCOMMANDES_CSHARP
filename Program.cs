using GESTIONCOMMANDES.data;
using GESTIONCOMMANDES.data.fixtures;
using GESTIONCOMMANDES.hub;
using GESTIONCOMMANDES.Models.Entities;
using GESTIONCOMMANDES.services;
using GESTIONCOMMANDES.services.Impl;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register the AppDbContext with PostgreSQL connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register Identity services
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Register the Fixtures service
builder.Services.AddScoped<Fixtures>();


builder.Services.AddSignalR(); // Ajouter SignalR aux services


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProduitService, ProduitService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<SmsService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Charger les fixtures
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        Fixtures.Initialize(context);
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de l'initialisation des fixtures : {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePages(context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 404)
    {
        response.Redirect("/Home/NotFoundPage");
    }

    return Task.CompletedTask;
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapHub<PanierHub>("/panierHub");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
