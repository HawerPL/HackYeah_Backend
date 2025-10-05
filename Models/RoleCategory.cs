using System.ComponentModel.DataAnnotations;

namespace HackYeah_Backend.Models
{
    public class RoleCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<RoleCharacteristic> Characteristics { get; set; } = new List<RoleCharacteristic>();
    }
}
