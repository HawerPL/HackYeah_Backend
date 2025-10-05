using System.ComponentModel.DataAnnotations.Schema;

namespace HackYeah_Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
     
        public int FormId { get; set; }
        public Form Form { get; set; }

    }
}
