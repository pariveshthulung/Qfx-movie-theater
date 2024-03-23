using System.ComponentModel.DataAnnotations.Schema;

namespace QFX.Models;

public class Language
{
    public long ID { get; set; }
    public string? Name { get; set; }
}
