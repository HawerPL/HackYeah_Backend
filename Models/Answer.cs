namespace HackYeah_Backend.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public List<RoleCharacteristic> characteristics { get; set; }
    }
}
