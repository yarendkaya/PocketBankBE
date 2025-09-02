using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PocketBankBE.Data;
using PocketBankBE.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- CORS Politikasýný Ekleme ---
// Frontend (React) uygulamasýnýn API'ye eriþebilmesi için gerekli izin.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
                      policy =>
                      {
                          // Frontend'in çalýþtýðý adresler
                          policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});


// --- Servisleri Ekleme (Dependency Injection) ---

// 1. Veritabaný Baðlantýsý (DbContext) SQLite olarak ayarlandý.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Yazdýðýnýz tüm servisler buraya ekleniyor.
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<BillService>();
builder.Services.AddScoped<BudgetService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<SavingGoalService>();
builder.Services.AddScoped<TransactionService>();


// --- JWT Kimlik Doðrulama (Authentication) Yapýlandýrmasý ---
// API'nin gelen token'larý nasýl doðrulayacaðýný belirler.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };
    });


// --- Standart Servisler ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger'a JWT desteði ekleme (Authorize butonu için)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PocketBank API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


// --- UYGULAMA KURULUMU (MIDDLEWARE) ---
var app = builder.Build();

// Geliþtirme ortamýnda Swagger'ý etkinleþtir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS politikasýný etkinleþtir
app.UseCors("AllowReactApp");

// Kimlik doðrulama ve yetkilendirme middleware'lerini etkinleþtir
// Sýralama önemlidir: Önce Authentication, sonra Authorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();