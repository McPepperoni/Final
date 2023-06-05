using System.ComponentModel.DataAnnotations;

namespace MVC.DTOs;

public class DisplayListDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime ModifiedAt { get; set; }
}