using APICatalago.Context;
using APICatalago.DTOs.Mappings;
using APICatalago.Extensions;
using APICatalago.Filters;
using APICatalago.Models;
using APICatalago.RateLimitOptions;
using APICatalago.Repositories;
using APICatalago.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NuGet.Common;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// ignora o erro de ciclos (erro de repetição de um get entidade que faz referencia a outra entidade de forma ciclica)
// adiciona um filtro global de tratamento de exceção, assim podendo nao precisar utilizar try catch
builder.Services.AddControllers(options =>
    {
        options.Filters.Add(typeof(ApiExceptionFilter));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }).AddNewtonsoftJson();

// configurando CORS
var OrigensComAcessoPermitido = "_origensComAcessoPermitido";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: OrigensComAcessoPermitido,
    policy =>
    {
        policy.WithOrigins("https://localhost:7163")
            .WithMethods("GET", "POST")
            .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apiagenda", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer JWT"
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
                         new string[] {}
                    }
                });
});

builder.Services.AddAuthorization();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

var SQLConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => 
{
    options.UseSqlServer(SQLConnection);
});

// exemplos de leitura das configurações utilizando a instancia builder
var configuracaoTeste1 = builder.Configuration["chave1"];
var configuracaoTeste2 = builder.Configuration["secao1:chave2"];

//exemplo de filtro
//builder.Services.AddScoped<ApiLoggingFilter>();
//builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
//{
//    LogLevel = LogLevel.Information
//}));

// configurando autenticação
var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentException("Invalid secret key!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// configurando politicas de autorização
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("Admin").RequireClaim("id", "erik"));

    options.AddPolicy("UserOnly", policy => policy.RequireRole("usuario"));

    options.AddPolicy("ExclusivePolicyOnly", policy =>
    {
    policy.RequireAssertion(context => context.User.HasClaim(Claim =>
                            Claim.Type == "id" && Claim.Value == "erik") ||
                            context.User.IsInRole("SuperAdmin"));
    });
});

// configurando Rate Limiting

var MyOptions = new MyRateLimitOptions();

builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(MyOptions);

builder.Services.AddRateLimiter(ratelimiteroptions =>
{
    ratelimiteroptions.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
    {
        options.PermitLimit = MyOptions.PermitLimit;
        options.Window = TimeSpan.FromSeconds(MyOptions.Window);
        options.QueueLimit = MyOptions.QueueLimit;
    });
    ratelimiteroptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
} );

// configurando versionamento da API
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader(),
        new UrlSegmentApiVersionReader());
        
}).AddApiExplorer(apiExplorer =>
{
    apiExplorer.GroupNameFormat = "'v'VVV";
    apiExplorer.SubstituteApiVersionInUrl = true;
});

// adicionando a injeção de dependencia do repository
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnityOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

var app = builder.Build();

// middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseRateLimiter();

app.UseCors(OrigensComAcessoPermitido);

app.UseAuthorization();

app.MapControllers();

app.Run();
