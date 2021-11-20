using System.ComponentModel.DataAnnotations;

namespace OrderService.Dtos;

public class OrderWriteDto
{
    [Required]
    public int OrderedBy { get; set; }
    [Required]
    [MinLength(1)]
    public IReadOnlyList<int> ProductIds { get; set; }
}