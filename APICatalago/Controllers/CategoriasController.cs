using APICatalago.Context;
using APICatalago.Filters;
using APICatalago.Models;
using APICatalago.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        // injeção de dependencia
        private readonly ICategoriaRepository _repository;
        //private readonly IConfiguration _configuration;
        //private readonly ILogger _logger;

        public CategoriasController(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        //Exemplo de como ler dados que estão na configuração do projeto (appsettings.json)
        //[HttpGet("LerArquivosDeConfiguracao")]
        //public string GetValores()
        //{
        //    var chave1 = _configuration["chave1"];
        //    var chave2 = _configuration["chave2"];

        //    var secao1 = _configuration["secao1:chave2"];

        //    return $"chave 1 = {chave1} \nchave 2 = {chave2} \nSeção 1 -> Chave 2 = {secao1}";
        //}

        // retorna categoria junto com os produtos
        //[HttpGet("produtos")]
        //public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        //{
        //    _logger.LogInformation("------------ GET/CATEGORIAS/PRODUTOS ------------");
        //    //return _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToList();
        //    return _context.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaId <= 10).AsNoTracking().ToList();
        //}

        // exemplo de utilização de filtros
        //[HttpGet]
        //[ServiceFilter(typeof(ApiLoggingFilter))]
        //public ActionResult<IEnumerable<Categoria>> Get()
        //{
        //    _logger.LogInformation("------------ GET/CATEGORIAS ------------");

        //    var categorias = _context.Categorias.AsNoTracking().Take(10).ToList();

        //    if (categorias is null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(categorias);
        //}

        //Utilizando Repository
        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _repository.GetCategorias();
            return Ok(categorias);
        }

        // Get por Id sem utilizar repository
        //[HttpGet("{id:int}", Name = "ObterCategoria")]
        //public ActionResult<Categoria> Get(int id)
        //{
        //    _logger.LogInformation($"------------ GET/CATEGORIAS/ID = {id} ------------");

        //    //testando middleware de tratamento de excecoes
        //    //throw new Exception("Exceção ao retornar categoria por id");


        //    var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(x => x.CategoriaId == id);

        //    if (categoria is null)
        //    {
        //        return NotFound("Categoria não encontrada");
        //    }

        //    return Ok(categoria);
        //}

        //Utilizando Repository
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _repository.GetCategoria(id);

            if (categoria is null)
                return NotFound();

            return Ok(categoria);
        }

        // Sem utilizar Repository
        //[HttpPost]
        //public ActionResult Post(Categoria categoria)
        //{
        //    _logger.LogInformation("------------ POST/CATEGORIAS ------------");

        //    if (categoria is null)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Categorias.Add(categoria);
        //    _context.SaveChanges();

        //    return new CreatedAtRouteResult("ObterCategoria",
        //        new { id = categoria.CategoriaId }, categoria);
        //}

        // Utilizando Repository
        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            var categoriaCriada = _repository.Create(categoria);

            return new CreatedAtRouteResult("ObterCategoria", 
                new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }

        // sem utilizar repository
        //[HttpPut("{id:int}")]
        //public ActionResult Put(int id, Categoria categoria)
        //{

        //    _logger.LogInformation($"------------ PUT/CATEGORIAS/ID = {id} ------------");

        //    if (id != categoria.CategoriaId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(categoria).State = EntityState.Modified;
        //    _context.SaveChanges();

        //    return Ok(categoria);
        //}

        // utilizando repository
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _repository.Update(categoria);
            return Ok();
        }

        // sem utilizar repository
        //[HttpDelete("{id:int}")]
        //public ActionResult Delete(int id)
        //{

        //    _logger.LogInformation($"------------ DELETE/CATEGORIAS/ID = {id} ------------");

        //    var categoria = _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);

        //    if (categoria is null)
        //    {
        //        return NotFound("Categoria não localizada ");
        //    }

        //    _context.Categorias.Remove(categoria);
        //    _context.SaveChanges();

        //    return Ok(categoria);
        //}


        // utilizando repository
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _repository.GetCategoria(id);

            if (categoria is null)
                return NotFound();
            
            var categoriaExcluida = _repository.Delete(id);
            return Ok(categoriaExcluida);
        }
    }
}
