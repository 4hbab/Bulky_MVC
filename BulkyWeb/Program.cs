using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb;

public class Program
{
	public static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);

		// --------------------------------------------------
		// Add services to the container.
		builder.Services.AddControllersWithViews();

		// Zelf toegevoegd
		// DbContext
		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

		// # Security
		// # Scaffolding Identity - V.+/-8u15
		// > Default user toevoegen
		// > Optioneel confirmatieMail bij SignIn
		// > Mapping van IdentityTables met ApplicationDbContext
		// > Zonder mail-confirmatie
		// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
		// 	.AddEntityFrameworkStores<ApplicationDbContext>();

		// builder.Services.AddDefaultIdentity<IdentityUser>()
		// 	.AddEntityFrameworkStores<ApplicationDbContext>();

		// # IdentityRoles toevoegen > Bovenstaand statement wijzigen naar onderstaand
		// > Geen Default Identity
		// > IdentityUser + IdentityRole
		builder.Services.AddIdentity<IdentityUser, IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			// Nodig omdat we geen DefaultIdentity gebruiken
			.AddDefaultTokenProviders();

		// # Default Routes van IdentityPages instellen (overschrijven)
		// > Om redirects juist in te stellen
		// > V.+/- 9u06m
		// > Deze Configuratie werkt alleen "na" het toevoegen van de Identity
		builder.Services.ConfigureApplicationCookie(options => {
			options.LoginPath = $"/Identity/Account/Login";
			options.LogoutPath = $"/Identity/Account/Logout";
			options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
		});

		// # EmailSender implementeren > Zie Bulky.Utility.EmailSender voor meer info
		// > Om error te vermijden bij activeren van RegistratiePagina
		// > V.+/- 8u45
		builder.Services.AddScoped<IEmailSender, EmailSender>();

		// > Toevoegen na Scaffolding Identity
		builder.Services.AddRazorPages();

		// # _categoryRepo
		// builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

		// # Unit of Work
		builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


		// --------------------------------------------------
		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment()) {
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		// # Scaffolding Identity - V.+/-8u15
		// > Manueel toevoegen van onderstaande
		// > Authentication komt altijd voor Authorization
		app.UseAuthentication();

		app.UseAuthorization();

		// > Toevoegen na Scaffolding Identity
		app.MapRazorPages();

		app.MapControllerRoute(
			name: "default",
			pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

		app.Run();
	}
}