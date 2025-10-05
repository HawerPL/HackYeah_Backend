namespace HackYeah_Backend.Models
{
    public class Form
    {
        public int Id { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
