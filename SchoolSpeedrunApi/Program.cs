using Microsoft.EntityFrameworkCore;
using SchoolSpeedrunApi.Services;
using SchoolSpeedrunApi.Types.Database;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add configuration for storage path
builder.Configuration["PhotoStoragePath"] = Path.Combine(builder.Environment.ContentRootPath, "Photos");

// Register PhotoDatastore
builder.Services.AddSingleton<IPhotoDatastore, PhotoDatastore>();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// OpenAPI/Swagger
builder.Services.AddOpenApi();             // generates OpenAPI JSON
builder.Services.AddEndpointsApiExplorer(); // needed for SwaggerUI
builder.Services.AddSwaggerGen();           // adds Swagger UI

WebApplication app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();        // serves /swagger/v1/swagger.json
    app.UseSwaggerUI();      // serves interactive UI at /swagger
    app.MapOpenApi();        // still optional if you want /openapi.json
}

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();