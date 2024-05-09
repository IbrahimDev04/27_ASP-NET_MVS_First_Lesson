using Pronia.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ProniaContext>();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute("admin", "{area:exists}/{controller=slider}/{action=index}/{id?}");
app.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");

app.Run();

