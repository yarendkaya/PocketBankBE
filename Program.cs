using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PocketBankBE.Data;
using PocketBankBE.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- CORS Politikas�n� Ekleme ---
// Frontend (React) uygulamas�n�n API'ye eri�ebilmesi i�in gerekli izin.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
                      policy =>
                      {
                          // Frontend'in �al��t��� adresler
                          policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});


// --- Servisleri Ekleme (Dependency Injection) ---

// 1. Veritaban� Ba�lant�s� (DbContext) SQLite olarak ayarland�.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Yazd���n�z t�m servisler buraya ekleniyor.
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<BillService>();
builder.Services.AddScoped<BudgetService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<SavingGoalService>();
builder.Services.AddScoped<TransactionService>();


// --- JWT Kimlik Do�rulama (Authentication) Yap�land�rmas� ---
// API'nin gelen token'lar� nas�l do�rulayaca��n� belirler.
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
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    logging.RequestHeaders.Add("X-Real-IP");
    logging.RequestHeaders.Add("X-Forwarded-For");
});

// Swagger'a JWT deste�i ekleme (Authorize butonu i�in)
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

// Geli�tirme ortam�nda Swagger'� etkinle�tir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// HTTP logging'i etkinle�tir
app.UseHttpLogging();

// CORS politikas�n� etkinle�tir
app.UseCors("AllowReactApp");

// Kimlik do�rulama ve yetkilendirme middleware'lerini etkinle�tir
// S�ralama �nemlidir: �nce Authentication, sonra Authorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();