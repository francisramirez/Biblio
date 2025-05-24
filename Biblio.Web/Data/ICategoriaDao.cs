namespace Biblio.Web.Data
{
    public interface ICategoriaDao
    {
        Task<List<Categoria>> GetAllAsync();
        Task<Categoria> GetByIdAsync(int id);
        Task AddAsync(Categoria categoria);
        Task UpdateAsync(Categoria categoria);
        
    }
}
