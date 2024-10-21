global using SimpleExpenseTracker.Infra.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

// Database
if (builder.Environment.IsDevelopment())
    builder.Services.AddSqlite<SETContext>("Data Source=set.db", b => b.MigrationsAssembly("SimpleExpenseTracker.Infra"));
else
    builder.Services.AddSqlServer<SETContext>(Environment.GetEnvironmentVariable("ConnectionString"), b => b.MigrationsAssembly("SimpleExpenseTracker.Infra"));

// Swagger auth
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard authorization using the Bearer scheme (\"bearer {token}\") ",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// API JWT
string secretKey = "";
if (builder.Environment.IsDevelopment())
    secretKey = builder.Configuration.GetSection("AppSettings:ApiKey").Value;
else
    Environment.GetEnvironmentVariable("SecretKey");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
    options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
    

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Automatically Migrate Database on StartUp
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SETContext>();
    if (db.Database.EnsureCreated())
    {
        db.Database.Migrate();
    }
}
app.Run();
