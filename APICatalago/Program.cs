using APICatalago.Context;
using APICatalago.Extensions;
using APICatalago.Filters;
using APICatalago.Logging;
using APICatalago.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


// adicionando a injeção de dependencia do repository
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnityOfWork>();


var app = builder.Build();

// middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
