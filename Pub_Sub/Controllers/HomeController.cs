using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Pub_Sub.Models;
using StackExchange.Redis;

namespace Pub_Sub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISubscriber _publisher;

        public static readonly ChatViewModel model = new() { };

        private static ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(
                new ConfigurationOptions
                {
                    EndPoints = { { "redis-15740.c325.us-east-1-4.ec2.redns.redis-cloud.com", 15740 } },
                    User = "default",
                    Password = "1AUGptjV12zSH8WoYjalgdb6MqmKzD5V"
                }
            );

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _publisher = muxer.GetSubscriber();

        }

        public IActionResult Index()
        {

            if (model.chat is null)
            {
                model.chat = new Chat();
            }
            return View(model);
        }

        public IActionResult ChannelClick(string name)
        {
            Console.WriteLine("Channel: " + name);
            model.SetChat(name);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddChannel(ChatViewModel Model)
        {
            string name = Model.addChannel;

            if (name is null)
            {
                name = "no_name";
            }

            _publisher.Subscribe(name, (channel, message) =>
            {
                var chat = model.Chats.FirstOrDefault(x => x.ChannelName == channel);
                chat.Messages.Add(message);
            });


            Console.WriteLine("Channel add: " + name);
            model.AddChannel(name);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SendMsg(ChatViewModel Model)
        {
            string msg = Model.sendMsg;

            if (msg is null)
            {
                msg = "no_msg";
            }

            _publisher.Publish(model.chat.ChannelName, msg);

            Console.WriteLine("Msg send: " + msg);
            return RedirectToAction("Index");
        }
    }
}
