
using Biblio.Web.Exceptions;
using Biblio.Web.Result;
using Microsoft.Data.SqlClient;
namespace Biblio.Web.Data
{
    public class CategoriaDao : ICategoriaDao
    {
        private string _connString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CategoriaDao> _logger;

        public CategoriaDao(
                            IConfiguration configuration,
                            ILogger<CategoriaDao> logger)
        {
            _connString = configuration.GetConnectionString("biblioConn");
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
                        var reader = await command.ExecuteReaderAsync();
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
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving categories from the database.",ex.ToString());
                Opresult = OperationResult.Failure("Error retrieving categories from the database.");
            }
            return Opresult;
        }
        public async Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                _logger.LogInformation($"Retrieving category with ID {id} from the database.");

                using (var connection = new SqlConnection(this._connString))
                {
                    using (var command = new SqlCommand("dbo.ObtenerCategoriaPorId", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_CategoriaId", id);
                        await connection.OpenAsync();
                        var reader = await command.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Categoria categoria = new Categoria
                            {
                                CategoriaId = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Estado = reader.GetBoolean(2),
                                FechaCreacion = reader.GetDateTime(3)
                            };
                            Opresult = OperationResult.Success("Category retrieved successfully.", categoria);
                        }
                        else
                        {
                            Opresult = OperationResult.Failure($"No category found with ID {id}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving category with ID {id} from the database.", ex.ToString());
                OperationResult.Failure($"Error retrieving category with ID {id} from the database.");
            }
            return Opresult;
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
                        command.Parameters.AddWithValue("@p_Descripcion", categoria!.Nombre);
                        command.Parameters.AddWithValue("@p_Estado", categoria.Estado);
                        command.Parameters.AddWithValue("@p_FechaCreacion", categoria.FechaCreacion);

                        SqlParameter p_result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
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
                        command.Parameters.AddWithValue("@p_IdCategoria", categoria.CategoriaId);
                        command.Parameters.AddWithValue("@p_Descripcion", categoria.Nombre);
                        command.Parameters.AddWithValue("@p_Estado", categoria.Estado);


                        SqlParameter p_result = new SqlParameter("@p_Result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 4000,
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