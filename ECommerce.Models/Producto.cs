using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "la Descipcion es obligatorio")]
        [StringLength(500, ErrorMessage = "La descipcion no puede superar los 500 caracteres.")]
        public string Descripcion { get; set; }

        //[Required(ErrorMessage = "la imagen es obligatorio")]
        //[StringLength(300, ErrorMessage = "La ruta de imagen no puede superar los 300 caracteres.")]
        public string? Imagen { get; set; }

        [NotMapped]
        public IFormFile? ImagenSubida { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue,ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Display(Name= "Cantidad disponible")]
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad no puede ser negativa")]
        public int CantidadDisponible { get; set; } //Campo para manejar la cantidad

        public DateTime FechaCreacion { get; set; } = DateTime.Now;// fecha predeterminada

        [Required(ErrorMessage ="La categoria es obligatoria")]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria {  get; set; }      
        
    }
}
