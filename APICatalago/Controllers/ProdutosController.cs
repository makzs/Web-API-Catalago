using APICatalago.Context;
using APICatalago.Models;
using APICatalago.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _repository;

        public ProdutosController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        // sem utilizar padrao Repository
        //// exemplos de requisição async
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Produto>>> GetAsync() 
        //{
        //    var produtos = await _context.Produtos.AsNoTracking().Take(10).ToListAsync();

        //    if (produtos is null)
        //    {
        //        return NotFound("Produto não encontrado");
        //    }

        //    return produtos;

        //}

        // utilizando Padrao Repository
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _repository.GetProdutos();

            if (produtos is null)
            {
                return NotFound();
            }

            return Ok(produtos);
        }

        // sem utilizar padrao repository
        //// Exemplo de uso de Model Binding
        //[HttpGet("{id:int}", Name = "ObterProduto")]
        //public async Task<ActionResult<Produto>> GetAsync([FromQuery] int id)
        //{
        //    var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.ProdutoId == id);
        //    if (produto is null)
        //    {
        //        return NotFound("Produto não encontrado");
        //    }
        //    return produto;
        //}

        // utilizando padrao repository
        [HttpGet("{id:int}", Name="ObterProduto")]
        public ActionResult<Produto> GetAction(int id)
        {
            var produto = _repository.GetProduto(id);

            if (produto is null)
                return NotFound();

            return Ok(produto);
        }

        // sem utilizar o padrao repository
        //[HttpPost]
        //public ActionResult Post(Produto produto)
        //{
        //    if (produto is null)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Produtos.Add(produto);
        //    _context.SaveChanges();

        //    return new CreatedAtRouteResult("ObterProduto",
        //        new { id = produto.ProdutoId }, produto);
        //}

        // utilizando o padrao repository
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            var novoProduto = _repository.Create(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProduto.ProdutoId }, novoProduto);
        }

        // sem utilizar o padrao repository
        //[HttpPut("{id:int}")]
        //public ActionResult Put(int id, Produto produto)
        //{
        //    if (id != produto.ProdutoId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(produto).State = EntityState.Modified;
        //    _context.SaveChanges();

        //    return Ok(produto);
        //}

        // utilizando o padrao repository
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            bool atualizado = _repository.Update(produto);

            if (atualizado)
            {
                return Ok(produto);
            }
            else
            {
                return StatusCode(500, $"Falha em atualizar o produto de id = {id}");
            }

        }

        // sem utilizar o padrao repository
        //[HttpDelete("{id:int}")]
        //public ActionResult Delete(int id)
        //{
        //    var produto = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);

        //    if (produto is null)
        //    {
        //        return NotFound("Produto não localizado");
        //    }

        //    _context.Produtos.Remove(produto);
        //    _context.SaveChanges();
        //    return Ok(produto);
        //}

        // utilizando o padrao reopsitory
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var deletado = _repository.Delete(id);

            if (deletado)
            {
                return Ok($"Produto de id = {id} foi deletado com sucesso");
            }
            else
            {
                return StatusCode(500, $"Falha em deletar o produto de id = {id}");
            }
        }

    }
}
