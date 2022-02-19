using System.ComponentModel.DataAnnotations;

namespace OrderService.Dtos;

public class OrderWriteDto
{
    [Required]
    [MinLength(1)]
    public IReadOnlyList<int> ProductIds { get; set; }
}