using Biblio.Web.Result;

namespace Biblio.Web.Data
{
    public class CategoryRepository : ICategoriaDao
    {
         
        public Task<OperationResult> AddAsync(Categoria categoria)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> UpdateAsync(Categoria categoria)
        {
            throw new NotImplementedException();
        }
    }
}
