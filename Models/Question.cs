using System.Text.Json.Serialization;

namespace HackYeah_Backend.Models
{
    public class Question
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<QuestionCategory> Categories { get; set; } = new List<QuestionCategory>();
        public ICollection<QuestionAnswer> Answers { get; set; } = new List<QuestionAnswer>();

        public int Order {  get; set; }

    }
    public class QuestionAnswer
    {
        public int Id { get; set; }

        public string Answer { get; set; }

        public string Value { get; set; }

        [JsonIgnore]
        public int QuestionId { get; set; }

        [JsonIgnore]
        public Question Question { get; set; }
    }

    public class QuestionCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
