using APICatalago.Context;

namespace APICatalago.Repositories
{
    public class UnityOfWork : IUnitOfWork
    {
        private IProdutoRepository _ProdutoRepo;

        private ICategoriaRepository _CategoriaRepo;

        public AppDbContext _context;

        public UnityOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _ProdutoRepo = _ProdutoRepo ?? new ProdutoRepository(_context);
            }
        }
        
        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _CategoriaRepo = _CategoriaRepo ?? new CategoriaRepository(_context);
            }
        }

        public void commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
