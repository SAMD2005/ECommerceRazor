using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class Categoria
    {
        [Key]//Indica que es la clave primaria
        public int Id { get; set; }

        [Required(ErrorMessage ="El campo Nombre es obligatorio")]
        [StringLength(100,ErrorMessage ="El Nombre no puede superar los 100 caracteres.")]
        [Display(Name="Nombre de la categoria")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El orden de visualizacion es obligatorio")]
        [Range(1,int.MaxValue,ErrorMessage = "El Orden debe ser mayor a 0.")]
        [Display(Name = "Orden de Visualizacion")]
        public int OrdenVisualizacion { get; set; }

        public DateTime FechaCreacion { get; set; }= DateTime.Now; //Fecha determinada a la

        //Relacion de uno a muchos: Una categoria puede tener muchos productos
        public  ICollection<Producto> Productos { get; set; }
    }
}
