using System.IO.Pipes;

namespace Biblio.Web.Data
{
    public class Categoria
    {
        public int CategoriaId { get;set; }
        public string? Nombre { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}
