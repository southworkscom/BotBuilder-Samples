namespace DirectLineSampleClient
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector.DirectLine;
    using Newtonsoft.Json;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Text;

    public class Program
    {
        private static string directLineSecret = ConfigurationManager.AppSettings["DirectLineSecret"];
        private static string botId = ConfigurationManager.AppSettings["BotId"];
        private static string fromUser = "DirectLineSampleClientUser";

        public static void Main(string[] args)
        {
            StartBotConversation().Wait();
        }

        private static async Task StartBotConversation()
        {
            var directLineClient = new DirectLineClient(directLineSecret);
            
            var conversation = await directLineClient.Conversations.StartConversationAsync();

            await Task.WhenAll(Receive(directLineClient, conversation.StreamUrl), Send(directLineClient, conversation.ConversationId));
        }

        static UTF8Encoding encoder = new UTF8Encoding();

        private static async Task Send(DirectLineClient client, string conversationId)
        {
            while (true)
            {
                string message = Console.ReadLine().Trim();

                var userMessage = new Activity
                {
                    From = new ChannelAccount(fromUser),
                    Text = message,
                    Type = ActivityTypes.Message
                };
                
                var input = JsonConvert.SerializeObject(userMessage);

                byte[] buffer = encoder.GetBytes(input);
                await client.Conversations.PostActivityAsync(conversationId, userMessage);
            }
        }
        
        private static async Task Receive(DirectLineClient client, string streamUrl)
        {
            var buffer = new byte[1024 * 4];
            var webSocket = new ClientWebSocket();
            await webSocket.ConnectAsync(new Uri(streamUrl), CancellationToken.None);
            
            while (true)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.Count == 0) break;

                var socketMessage = encoder.GetString(buffer, 0, result.Count);
                GetMessage(socketMessage);
            }
        }

        private static void GetMessage(string socketMessage)
        {
            var activitySet = JsonConvert.DeserializeObject<ActivitySet>(socketMessage);
            var activities = from x in activitySet.Activities
                             where x.From.Id == botId
                             select x;

            foreach (Activity activity in activities)
            {
                Console.WriteLine(activity.Text);

                if (activity.Attachments != null)
                {
                    foreach (Attachment attachment in activity.Attachments)
                    {
                        switch (attachment.ContentType)
                        {
                            case "application/vnd.microsoft.card.hero":
                                RenderHeroCard(attachment);
                                break;

                            case "image/png":
                                Console.WriteLine($"Opening the requested image '{attachment.ContentUrl}'");

                                Process.Start(attachment.ContentUrl);
                                break;
                        }
                    }
                }
                Console.Write("Command> ");
            }
        }
        
        private static void RenderHeroCard(Attachment attachment)
        {
            const int Width = 70;
            Func<string, string> contentLine = (content) => string.Format($"{{0, -{Width}}}", string.Format("{0," + ((Width + content.Length) / 2).ToString() + "}", content));

            var heroCard = JsonConvert.DeserializeObject<HeroCard>(attachment.Content.ToString());

            if (heroCard != null)
            {
                Console.WriteLine("/{0}", new string('*', Width + 1));
                Console.WriteLine("*{0}*", contentLine(heroCard.Title));
                Console.WriteLine("*{0}*", new string(' ', Width));
                Console.WriteLine("*{0}*", contentLine(heroCard.Text));
                Console.WriteLine("{0}/", new string('*', Width + 1));
            }
        }
    }
}
