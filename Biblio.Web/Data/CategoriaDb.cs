
using Biblio.Web.Exceptions;
using Microsoft.Data.SqlClient;
namespace Biblio.Web.Data
{
    public class CategoriaDao : ICategoriaDao
    {
        private readonly string _connString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoriaDao> _logger;

        public CategoriaDao(string connString,
                            IConfiguration configuration,
                            ILogger<CategoriaDao> logger)
        {
            _connString = connString;
            _configuration = configuration;
            _logger = logger;
        }
        public Task<List<Categoria>> GetAllAsync()
        {
            // Simulate async data retrieval
            return Task.FromResult(new List<Categoria>());
        }
        public Task<Categoria> GetByIdAsync(int id)
        {
            // Simulate async data retrieval
            return Task.FromResult(new Categoria { CategoriaId = id, Nombre = "Sample Category", Estado = true, FechaCreacion = DateTime.Now });
        }
        public async Task AddAsync(Categoria categoria)
        {

            try
            {
                using (var connection = new SqlConnection(this._connString))
                {

                    using (var command = new SqlCommand("dbo.GuardandoCategoria", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                        command.Parameters.AddWithValue("@Estado", categoria.Estado);
                        command.Parameters.AddWithValue("@FechaCreacion", categoria.FechaCreacion);

                        SqlParameter p_result = new SqlParameter("@p_Result", System.Data.SqlDbType.Int)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_result);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        var result = (string)p_result.Value;

                        if (result != "Ok")
                            throw new CategoriaException(result);


                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding category  {ex.Message}", ex.ToString());
            }


        }
        public Task UpdateAsync(Categoria categoria)
        {
            // Simulate async data update
            return Task.CompletedTask;
        }
    }
}