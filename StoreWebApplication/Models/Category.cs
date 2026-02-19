using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreWebApplication.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [MaxLength(50)]
    [Required]
    [DisplayName("Category Name")]
    public string Name { get; set; }
    [DisplayName("Display Order")]
    [Range(1, 1000, ErrorMessage = "Display Order must be between 1 and 1000")]
    public int DisplayOrder { get; set; }
}