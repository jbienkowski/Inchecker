using System.ComponentModel.DataAnnotations;

namespace Inchecker.Entities
{
    public class Setting
    {
        [Key]
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
