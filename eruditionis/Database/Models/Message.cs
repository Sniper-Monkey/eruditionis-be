namespace eruditionis.Database.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}
