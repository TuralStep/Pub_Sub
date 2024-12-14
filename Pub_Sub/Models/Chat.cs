namespace Pub_Sub.Models
{
    public class Chat
    {
        public string ChannelName { get; set; }
        public List<string> Messages { get; set; }

        public Chat(string name = "empty")
        {
            ChannelName = name;
            Messages = new List<string> { };
        }
    }
}
