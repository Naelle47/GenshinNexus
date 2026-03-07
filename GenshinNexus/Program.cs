using System.Data;
using GenshinNexus.Data.Repositories.CharacterRepo;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);

// Configure PostgreSQL connection
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var cs = builder.Configuration.GetConnectionString("GenshinNexus");
    return new NpgsqlConnection(cs);
});

builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles(); // Permet de servir les fichiers statiques (CSS, JS, images) depuis wwwroot

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
