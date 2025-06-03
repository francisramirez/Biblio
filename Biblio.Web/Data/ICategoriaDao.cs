using Biblio.Web.Result;

namespace Biblio.Web.Data
{
    public interface ICategoriaDao
    {
        Task<OperationResult> GetAllAsync();
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(Categoria categoria);
        Task<OperationResult> UpdateAsync(Categoria categoria);
        
    }
}
