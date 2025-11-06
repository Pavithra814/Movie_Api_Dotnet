using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MovieDbApi.Data;
using MovieDbApi.Repositories;
using MovieDbApi.Services;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Registering Movie Module
//builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

builder.Services.AddScoped<IFavouriteRepository, FavouriteRepository>();
//builder.Services.AddScoped<IFavouriteService, FavouriteService>();

// Registering User Module
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Optional: serve from a custom folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/images"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

// Enable CORS BEFORE Authorization
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
