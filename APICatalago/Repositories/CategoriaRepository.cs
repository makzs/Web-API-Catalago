using APICatalago.Context;
using APICatalago.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {

        public CategoriaRepository(AppDbContext context) : base(context)
        {

        }

        

    }
}
