using System.Reflection;
using System.Xml.Linq;

namespace Pub_Sub.Models
{
    public class ChatViewModel
    {
        public List<Chat> Chats { get; set; }
        public Chat chat { get; set; }
        public string addChannel { get; set; }
        public string sendMsg { get; set; }

        public ChatViewModel()
        {
            chat= new Chat();
            Chats = new List<Chat>();
        }

        public void AddChannel(string channelName)
        {
            chat = new Chat(channelName);
            Chats.Add(chat);
        }

        public void SendMsg(string msg)
        {
            chat.Messages.Add(msg);
        }

        public void SetChat(string channelName)
        {
            chat = Chats.FirstOrDefault(x => x.ChannelName == channelName);
        }

    }
}
