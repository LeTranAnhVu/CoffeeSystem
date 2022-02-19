using System.ComponentModel.DataAnnotations;

namespace ProductService.Dtos
{
    public class ProductReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class ProductWriteDto
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public double Price { get; set; }
    }
}