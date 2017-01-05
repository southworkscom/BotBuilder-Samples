namespace CardsBot
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class CardsDialog : IDialog<object>
    {
        private const string HeroCard = "Hero card";
        private const string ThumbnailCard = "Thumbnail card";
        private const string ReceiptCard = "Receipt card";
        private const string SigninCard = "Sign-in card";
        private const string AnimationCard = "Animation card";
        private const string VideoCard = "Video card";
        private const string AudioCard = "Audio card";

        private IEnumerable<string> options = new List<string> { HeroCard, ThumbnailCard, ReceiptCard, SigninCard, AnimationCard, VideoCard, AudioCard };

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            PromptDialog.Choice<string>(
                context,
                this.DisplaySelectedCard,
                this.options,
                "What card would like to test?",
                "Ooops, what you wrote is not a valid option, please try again",
                3,
                PromptStyle.PerLine);
        }

        public async Task DisplaySelectedCard(IDialogContext context, IAwaitable<string> result)
        {
            var selectedCard = await result;

            var message = context.MakeMessage();

            var attachment = GetSelectedCard(selectedCard);
            message.Attachments.Add(attachment);

            await context.PostAsync(message);

            context.Wait(this.MessageReceivedAsync);
        }

        private static Attachment GetSelectedCard(string selectedCard)
        {
            switch (selectedCard)
            {
                case HeroCard:
                    return GetHeroCard();
                case ThumbnailCard:
                    return GetThumbnailCard();
                case ReceiptCard:
                    return GetReceiptCard();
                case AnimationCard:
                    return GetAnimationCard();
                case VideoCard:
                    return GetVideoCard();
                case AudioCard:
                    return GetAudioCard();

                default:
                    return GetHeroCard();
            }
        }

        private static Attachment GetHeroCard()
        {
            var heroCard = new HeroCard
            {
                Title = "BotFramework Hero Card",
                Subtitle = "Your bots — wherever your users are talking",
                Text = "Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services.",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Get Started", value: "https://docs.botframework.com/en-us/") }
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetThumbnailCard()
        {
            var heroCard = new ThumbnailCard
            {
                Title = "BotFramework Thumbnail Card",
                Subtitle = "Your bots — wherever your users are talking",
                Text = "Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services.",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Get Started", value: "https://docs.botframework.com/en-us/") }
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetReceiptCard()
        {
            var receiptCard = new ReceiptCard
            {
                Title = "John Doe",
                Facts = new List<Fact> { new Fact("Order Number", "1234"), new Fact("Payment Method", "VISA 5555-****") },
                Items = new List<ReceiptItem>
                {
                    new ReceiptItem("Data Transfer", price: "$ 38.45", quantity: "368", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/traffic-manager.png")),
                    new ReceiptItem("App Service", price: "$ 45.00", quantity: "720", image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png")),
                },
                Tax = "$ 7.50",
                Total = "$ 90.95",
                Buttons = new List<CardAction>
                {
                    new CardAction(
                        ActionTypes.OpenUrl,
                        "More information",
                        "https://account.windowsazure.com/content/6.10.1.38-.8225.160809-1618/aux-pre/images/offer-icon-freetrial.png",
                        "https://azure.microsoft.com/en-us/pricing/")
                }
            };

            return receiptCard.ToAttachment();
        }

        private static Attachment GetSigninCard()
        {
            var signinCard = new SigninCard
            {
                Text = "BotFramework Sign-in Card",
                Buttons = new List<CardAction> { new CardAction(ActionTypes.Signin, "Sign-in", value: "https://login.microsoftonline.com/") }
            };

            return signinCard.ToAttachment();
        }

        private static Attachment GetAnimationCard()
        {
            var currentUrl = HttpContext.Current.Request.Url;

            var animationCard = new AnimationCard
            {
                Title = "Microsoft Bot Framework",
                Subtitle = "Animation Card",
                Media = new List<MediaUrl>()
                {
                    new MediaUrl()
                    {
                        Url = new UriBuilder(currentUrl.Scheme, currentUrl.Host, currentUrl.Port, "/images/botframework.gif").ToString()
                    }
                }
            };

            return animationCard.ToAttachment();
        }

        private static Attachment GetVideoCard()
        {
            var currentUrl = HttpContext.Current.Request.Url;

            var videoCard = new VideoCard
            {
                Title = "Microsoft Bot Framework and how we created the Azure Bot",
                Subtitle = "by Thiago Almeida",
                Text = "At Microsoft, we have first-hand experience writing bots and building artificial intelligence systems, so we’ve shared our services and tools so you can use them to add conversations to your own products. In this session we will cover the Microsoft Bot Framework and it's three components: the Microsoft Cognitive Services, the Bot Builder SDK, and the Bot Connector. We will also show the code and details about the Azure Bot and how it was built.",
                Media = new List<MediaUrl>()
                {
                    new MediaUrl()
                    {
                        Url = "http://video.ch9.ms/ch9/15ec/8933d06d-a6cd-460b-8a52-245ab52515ec/BotFramework_mid.mp4"
                    }
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Title = "See more in Channel9",
                        Type = ActionTypes.OpenUrl,
                        Value = "https://channel9.msdn.com/events/TechDays/Techdays-2016-The-Netherlands/Microsoft-Bot-Framework-and-how-we-created-the-Azure-Bot"
                    }
                }
            };

            return videoCard.ToAttachment();
        }

        private static Attachment GetAudioCard()
        {
            var currentUrl = HttpContext.Current.Request.Url;

            var audioCard = new AudioCard
            {
                Title = "Introduction to the Microsoft Bot Framework",
                Subtitle = "by Thiago Almeida",
                Text = "Bots are increasingly popular and useful in many scenarios. In this session you will learn what options you have and how to get started building Bots using the Microsoft Bot Framework and other technologies such as LUIS.",
                Media = new List<MediaUrl>()
                {
                    new MediaUrl()
                    {
                        Url = "http://video.ch9.ms/ch9/abcc/d1e3ab3f-2d06-4c62-92d1-56c36a9cabcc/IntroBot.mp3"
                    }
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Title = "See more in Channel9",
                        Type = ActionTypes.OpenUrl,
                        Value = "https://channel9.msdn.com/events/TechDays/Techdays-2016-The-Netherlands/Introduction-to-the-Microsoft-Bot-Framework"
                    }
                }
            };

            return audioCard.ToAttachment();
        }
    }
}