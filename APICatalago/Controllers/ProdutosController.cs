using APICatalago.Context;
using APICatalago.DTOs;
using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;

namespace APICatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        //private readonly IProdutoRepository _produtoRepository;
        //private readonly IRepository<Produto> _repository;

        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        //// novo metodo especifico do repository generico
        //[HttpGet("Produtos/{id}")]
        //public ActionResult <IEnumerable<Produto>> GetProdutosCategoria(int id)
        //{
        //    var produtos = _produtoRepository.GetProdutosPorCategoria(id);

        //    if (produtos is null)
        //        return BadRequest();

        //    return Ok(produtos);
        //}


        // novo metodo especifico do repository generico no padrao Unity of Work e utilizando DTO
        [HttpGet("Produtos/{id}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if (produtos is null)
                return BadRequest();

            // var destino = _mapper.Map<Destino>(origem);
            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
        }


        // Get produtos utilizando Paginação
        [HttpGet("pagination")]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = _uof.ProdutoRepository.GetProdutos(produtosParameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            // incluir a variavel anonima no response
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        // implementando paginação com filtro de dados
        [HttpGet("filter/preco/pagination")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosFiltroPreco([FromQuery] ProdutosFiltroPreco produtosFiltroPreco)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosFiltroPreco(produtosFiltroPreco);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            // incluir a variavel anonima no response
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
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
        //[HttpGet]
        //public ActionResult<IEnumerable<Produto>> Get()
        //{
        //    var produtos = _repository.GetProdutos();

        //    if (produtos is null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(produtos);
        //}


        // Utilizando repository generico
        //[HttpGet]
        //public ActionResult<IEnumerable<Produto>> Get()
        //{
        //    var produtos = _repository.GetAll();

        //    if (produtos is null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(produtos);
        //}


        // utilizando o padrao Unity of Work e DTO
        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _uof.ProdutoRepository.GetAll();

            if (produtos is null)
            {
                return NotFound();
            }

            var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDTO);
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
        //[HttpGet("{id:int}", Name="ObterProduto")]
        //public ActionResult<Produto> GetAction(int id)
        //{
        //    var produto = _repository.GetProduto(id);

        //    if (produto is null)
        //        return NotFound();

        //    return Ok(produto);
        //}


        // Utilizando repository generico:
        //[HttpGet("{id:int}", Name = "ObterProduto")]
        //public ActionResult<Produto> GetAction(int id)
        //{
        //    var produto = _repository.Get(p=> p.ProdutoId == id);

        //    if (produto is null)
        //        return NotFound();

        //    return Ok(produto);
        //}


        // utilizando o padrao Unity of Work e DTO
        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> GetAction(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
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
        //[HttpPost]
        //public ActionResult Post(Produto produto)
        //{
        //    if (produto is null)
        //        return BadRequest();

        //    var novoProduto = _repository.Create(produto);

        //    return new CreatedAtRouteResult("ObterProduto",
        //        new { id = novoProduto.ProdutoId }, novoProduto);
        //}

        // utilizando repository generico
        //[HttpPost]
        //public ActionResult Post(Produto produto)
        //{
        //    if (produto is null)
        //        return BadRequest();

        //    var novoProduto = _repository.Create(produto);

        //    return new CreatedAtRouteResult("ObterProduto",
        //        new { id = novoProduto.ProdutoId }, novoProduto);
        //}


        // utilizando o padrao Unity of Work
        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
        {
            if (produtoDto is null)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDto);

            var novoProduto = _uof.ProdutoRepository.Create(produto);
            _uof.commit();

            var novoProdutoDTO = _mapper.Map<ProdutoDTO>(novoProduto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProdutoDTO.ProdutoId }, novoProdutoDTO);
        }

        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
        {
            if(patchProdutoDto is null || id <= 0)
                return BadRequest();

            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if(produto is null)
                return NotFound();


            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);

            if (!ModelState.IsValid || TryValidateModel(produtoUpdateRequest))
                return BadRequest(ModelState);

            _mapper.Map(produtoUpdateRequest, produto);

            _uof.ProdutoRepository.Update(produto);
            _uof.commit();

            return Ok(_mapper.Map<ProdutoDTOUpdateRequest>(produto));
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
        //[HttpPut("{id:int}")]
        //public ActionResult Put(int id, Produto produto)
        //{
        //    if (id != produto.ProdutoId)
        //    {
        //        return BadRequest();
        //    }

        //    bool atualizado = _repository.Update(produto);

        //    if (atualizado)
        //    {
        //        return Ok(produto);
        //    }
        //    else
        //    {
        //        return StatusCode(500, $"Falha em atualizar o produto de id = {id}");
        //    }

        //}

        // utilizando repository generico:
        //[HttpPut("{id:int}")]
        //public ActionResult Put(int id, Produto produto)
        //{
        //    if (id != produto.ProdutoId)
        //    {
        //        return BadRequest();
        //    }

        //    var produtoAtualizado = _repository.Update(produto);

        //    return Ok(produtoAtualizado);

        //}


        // utilizando o padrao Unity of Work
        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            _uof.commit();

            var produtoAtualizadoDTO = _mapper.Map<ProdutoDTO>(produtoAtualizado);

            return Ok(produtoAtualizadoDTO);

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
        //[HttpDelete("{id:int}")]
        //public ActionResult Delete(int id)
        //{
        //    var produto = _repository.Get(p => p.ProdutoId == id);

        //    if (produto is null)
        //        return NotFound();

        //    var produtoDeletado = _repository.Delete(produto);

        //    return Ok(produtoDeletado);
        //}


        // utilizando o padrao Unity of Work
        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound();

            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            _uof.commit();

            var produtoDeletadoDTO = _mapper.Map<ProdutoDTO>(produtoDeletado);

            return Ok(produtoDeletadoDTO);
        }

    }
}
