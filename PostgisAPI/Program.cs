using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PostgisAPI;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApiDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PostGIS Backend APIs", Version = "v1.29.24", Description = "Current CORS (Cross-Origin Resource Sharing) configuration remove all default security restrictions for the request when connecting to the backend server via these APIs. Therefore, please make sure that the CORS policies in the 'Program.cs' file matched to the production usages, otherwise, they may cause some security problems." });

    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin() // Allow access from all origins
                                                      .AllowAnyMethod() // Allow all API methods
                                                      .AllowAnyHeader()); // All all API request headers
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PostGIS APIs");

    });
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
