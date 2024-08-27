﻿using APICatalago.Context;
using APICatalago.Filters;
using APICatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        // injeção de dependencia
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriasController(AppDbContext context, IConfiguration configuration, ILogger logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        //Exemplo de como ler dados que estão na configuração do projeto (appsettings.json)
        [HttpGet("LerArquivosDeConfiguracao")]
        public string GetValores()
        {
            var chave1 = _configuration["chave1"];
            var chave2 = _configuration["chave2"];

            var secao1 = _configuration["secao1:chave2"];

            return $"chave 1 = {chave1} \nchave 2 = {chave2} \nSeção 1 -> Chave 2 = {secao1}";
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("------------ GET/CATEGORIAS/PRODUTOS ------------");
            //return _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToList();
            return _context.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaId <= 10).AsNoTracking().ToList();
        }

        // exemplo de utilização de filtros
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            _logger.LogInformation("------------ GET/CATEGORIAS ------------");

            var categorias = _context.Categorias.AsNoTracking().Take(10).ToList();

            if (categorias is null)
            {
                return NotFound();
            }

            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            _logger.LogInformation($"------------ GET/CATEGORIAS/ID = {id} ------------");

            //testando middleware de tratamento de excecoes
            //throw new Exception("Exceção ao retornar categoria por id");


            var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(x => x.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não encontrada");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            _logger.LogInformation("------------ POST/CATEGORIAS ------------");

            if (categoria is null)
            {
                return BadRequest();
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {

            _logger.LogInformation($"------------ PUT/CATEGORIAS/ID = {id} ------------");

            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {

            _logger.LogInformation($"------------ DELETE/CATEGORIAS/ID = {id} ------------");

            var categoria = _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não localizada ");
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
