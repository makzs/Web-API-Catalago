using APICatalago.Context;
using APICatalago.DTOs;
using APICatalago.DTOs.Mappings;
using APICatalago.Filters;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        // injeção de dependencia
        private readonly IUnitOfWork _uof;
        //private readonly IRepository<Categoria> _repository;
        //private readonly ICategoriaRepository _repository;
        //private readonly IConfiguration _configuration;
        //private readonly ILogger _logger;


        public CategoriasController(IUnitOfWork uof)
        {
            _uof = uof;
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
        //[HttpGet]
        //public ActionResult<IEnumerable<Categoria>> Get()
        //{
        //    var categorias = _repository.GetCategorias();
        //    return Ok(categorias);
        //}

        // Utilizando Repository Generico:
        //[HttpGet]
        //public ActionResult<IEnumerable<Categoria>> Get()
        //{
        //    var categorias = _repository.GetAll();
        //    return Ok(categorias);
        //}


        // Utilizando o padrao Unity Of Work e DTO
        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);

        }


        // implementação da paginação
        [HttpGet("Pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = _uof.CategoriaRepository.GetCategorias(categoriasParameters);

            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
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
        //[HttpGet("{id:int}", Name = "ObterCategoria")]
        //public ActionResult<Categoria> Get(int id)
        //{
        //    var categoria = _repository.GetCategoria(id);

        //    if (categoria is null)
        //        return NotFound();

        //    return Ok(categoria);
        //}

        // Utilizando repository generico:
        //[HttpGet("{id:int}", Name = "ObterCategoria")]
        //public ActionResult<Categoria> Get(int id)
        //{
        //    var categoria = _repository.Get(c=> c.CategoriaId == id);

        //    if (categoria is null)
        //        return NotFound();

        //    return Ok(categoria);
        //}


        // Utilizando o padrao Unity Of Work e DTO
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
                return NotFound();

            var CategoriaDTO = categoria.ToCategoriaDTO();

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
        //[HttpPost]
        //public ActionResult Post(Categoria categoria)
        //{
        //    if (categoria is null)
        //        return BadRequest();

        //    var categoriaCriada = _repository.Create(categoria);

        //    return new CreatedAtRouteResult("ObterCategoria", 
        //        new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        //}

        // utilizando repository generico:
        //[HttpPost]
        //public ActionResult Post(Categoria categoria)
        //{
        //    if (categoria is null)
        //        return BadRequest();

        //    var categoriaCriada = _repository.Create(categoria);

        //    return new CreatedAtRouteResult("ObterCategoria",
        //        new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        //}


        // Utilizando o padrao Unity Of Work e DTO
        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
                return BadRequest();

            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.commit();

            var NovaCategoriaDTO = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = NovaCategoriaDTO.CategoriaId }, NovaCategoriaDTO);
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
        //[HttpPut("{id:int}")]
        //public ActionResult Put(int id, Categoria categoria)
        //{
        //    if (categoria is null)
        //        return BadRequest();

        //    _repository.Update(categoria);
        //    return Ok();
        //}

        // utilizando repository generico
        //[HttpPut("{id:int}")]
        //public ActionResult Put(int id, Categoria categoria)
        //{
        //    if (categoria is null)
        //        return BadRequest();

        //    _repository.Update(categoria);
        //    return Ok();
        //}


        // utilizando o padrao Unity of Work e DTO
        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
                return BadRequest();

            var categoria = categoriaDto.ToCategoria();

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            _uof.commit();

            var CategoriaAtualizadaDTO = categoriaAtualizada.ToCategoriaDTO();

            return Ok(CategoriaAtualizadaDTO);
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
        //[HttpDelete("{id:int}")]
        //public ActionResult Delete(int id)
        //{
        //    var categoria = _repository.GetCategoria(id);

        //    if (categoria is null)
        //        return NotFound();

        //    var categoriaExcluida = _repository.Delete(id);
        //    return Ok(categoriaExcluida);
        //}

        // utilizando repository generico
        //[HttpDelete("{id:int}")]
        //public ActionResult Delete(int id)
        //{
        //    var categoria = _repository.Get(c => c.CategoriaId == id);

        //    if (categoria is null)
        //        return NotFound();

        //    var categoriaExcluida = _repository.Delete(categoria);
        //    return Ok(categoriaExcluida);
        //}


        // Utilizando o padrao Unity of Work e DTO
        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
                return NotFound();

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.commit();

            var categoriaExcluidaDTO = categoriaExcluida.ToCategoriaDTO();

            return Ok(categoriaExcluidaDTO);
        }

    }
}
