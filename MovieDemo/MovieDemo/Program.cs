using Microsoft.EntityFrameworkCore;
using MovieDemo.Models;
using MovieDemo.Models.Repository;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.
    GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException
    ("Connection string 'DefaultConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IMovieDemoRepo<Movie>, MovieRepo>();
builder.Services.AddScoped<IMovieDemoRepo<Genre>, GenreRepo>();
builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions
{
    CloseButton = true,
    ProgressBar = true,
    PositionClass = ToastPositions.TopCenter,
    PreventDuplicates = true
});

builder.Services.AddAutoMapper(typeof(Program));

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
