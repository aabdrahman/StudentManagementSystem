using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentManagementApplication.Configurations;
using StudentManagementApplication.Context;
using StudentManagementApplication.Interfaces;
using StudentManagementApplication.Middleware;
using StudentManagementApplication.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.ConfigureAuth();
builder.Services.AddScoped<TokenProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["JwT:Issuer"],
        ValidAudience = builder.Configuration["JwT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwT:Secrets"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };

});

builder.Services.AddAuthorization();

builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DatabaseConnection"));
//Add Application DbContext
builder.Services.AddDbContext<StudentManagementDbContext>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IStudentManagementService, StudentManagementService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "StudentManagement/swagger/{documentname}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/StudentManagement/swagger/v1/swagger.json", "StudentManagement");
        c.RoutePrefix = "StudentManagement";
    });
}


await app.UpdateMigration();

app.UseExceptionHandler();

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();





app.MapControllers();


app.Run();
