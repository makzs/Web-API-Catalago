namespace APICatalago.Repositories
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        Task commitAsync();
    }
}
