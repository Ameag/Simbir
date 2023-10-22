using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Simbir.Data;
using Simbir.Repository;
using Simbir.Repository.AccountRepository;
using Simbir.Repository.AccountRepository.AccountRepository;
using Simbir.Repository.Interfaces;
using Simbir.Repository.Interfaces.AccountInterfaces;
using Simbir.Repository.Interfaces.TransportInterfaces;
using Simbir.Repository.TransportRepository;
using Simbir.Service.Implementations;
using Simbir.Service.Implementations.AccountService;
using Simbir.Service.Implementations.TransportService;
using Simbir.Service.Interfaces;
using Simbir.Service.Interfaces.AccountInterface;
using Simbir.Service.Interfaces.TransportInterface;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped<IAccountRepository, AccountsRepository>();
builder.Services.AddScoped<IAccountService, AccountsService>();
builder.Services.AddScoped<IAccountAdminRepository, AccountAdminRepository>();
builder.Services.AddScoped<IAccountAdminService, AccountAdminService>();

builder.Services.AddScoped<ITransportRepository, TransportsRepository>();
builder.Services.AddScoped<ITransportService, TransporstService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<IBlackListRepository, BlackListRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
