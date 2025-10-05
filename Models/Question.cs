using System.Text.Json.Serialization;

namespace HackYeah_Backend.Models
{
    public class Question
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }


        public int Order {  get; set; }

        public int Form { get; set; }

    }

}
