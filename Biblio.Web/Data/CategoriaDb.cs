
using Biblio.Web.Exceptions;
using Biblio.Web.Result;
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
        public async Task<OperationResult> GetAllAsync()
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                _logger.LogInformation("Retrieving all categories from the database.");

                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("dbo.ObtenerCategorias", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        await connection.OpenAsync();
                        var reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            List<Categoria> categorias = new List<Categoria>();
                            while (reader.Read())
                            {
                                Categoria categoria = new Categoria
                                {
                                    CategoriaId = reader.GetInt32(0),
                                    Nombre = reader.GetString(1),
                                    Estado = reader.GetBoolean(2),
                                    FechaCreacion = reader.GetDateTime(3)
                                };

                                categorias.Add(categoria);
                            }
                            Opresult = OperationResult.Success("Categories retrieved successfully.", categorias);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure("No categories found.");
                        }
                    }
                }


            }
            catch (Exception)
            {
                _logger.LogError("Error retrieving categories from the database.");
                Opresult = OperationResult.Failure("Error retrieving categories from the database.");
            }
            return Opresult;
        }
        public Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                _logger.LogInformation($"Retrieving category with ID {id} from the database.");
            }
            catch (Exception)
            {

                throw;
            }
            return Task.FromResult(Opresult);
        }
        public async Task<OperationResult> AddAsync(Categoria categoria)
        {
            OperationResult Opresult = new OperationResult();

            try
            {

                if (categoria == null)
                    Opresult = OperationResult.Failure("Category cannot be null.");

                if (string.IsNullOrWhiteSpace(categoria!.Nombre))
                    Opresult = OperationResult.Failure("Category name cannot be empty.");


                using (var connection = new SqlConnection(this._connString))
                {

                    using (var command = new SqlCommand("dbo.GuardandoCategoria", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Nombre", categoria!.Nombre);
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
                            Opresult = OperationResult.Failure($"Error adding category: {result}");
                        else
                            Opresult = OperationResult.Success(result);



                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding category  {ex.Message}", ex.ToString());
            }

            return Opresult;


        }
        public async Task<OperationResult> UpdateAsync(Categoria categoria)
        {
            OperationResult Opresult = new OperationResult();
            try
            {
                if (categoria == null)
                    Opresult = OperationResult.Failure("Category cannot be null.");

                if (string.IsNullOrWhiteSpace(categoria!.Nombre))
                    Opresult = OperationResult.Failure("Category name cannot be empty.");


                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("dbo.ActualizandoCategoria", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CategoriaId", categoria.CategoriaId);
                        command.Parameters.AddWithValue("@p_Descripcion", categoria.Nombre);
                        command.Parameters.AddWithValue("@Estado", categoria.Estado);


                        SqlParameter p_result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_result);
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        var result = (string)p_result.Value;
                        if (result != "Ok")
                            Opresult = OperationResult.Failure($"Error updating category: {result}");
                        else
                            Opresult = OperationResult.Success(result);
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error updating category {ex.Message}", ex.ToString());
                Opresult = OperationResult.Failure($"Error updating category {ex.Message}");
            }
            return Opresult;
        }
    }
}