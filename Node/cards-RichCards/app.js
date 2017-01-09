var builder = require('botbuilder');
var restify = require('restify');

// Setup Restify Server
var server = restify.createServer();
server.listen(process.env.port || process.env.PORT || 3978, function () {
    console.log('%s listening to %s', server.name, server.url);
});

// Create chat bot and listen to messages
var connector = new builder.ChatConnector({
    appId: process.env.MICROSOFT_APP_ID,
    appPassword: process.env.MICROSOFT_APP_PASSWORD
});
server.post('/api/messages', connector.listen());

var bot = new builder.UniversalBot(connector, [
    function (session) {
        builder.Prompts.choice(session, 'What card would like to test?', CardNames, {
            maxRetries: 3,
            retryPrompt: 'Ooops, what you wrote is not a valid option, please try again'
        });
    },
    function (session, results) {

        // create the card based on selection
        var selectedCardName = results.response.entity;
        var card = createCard(selectedCardName, session);

        // attach the card to the reply message
        var msg = new builder.Message(session).addAttachment(card);
        session.send(msg);
    }
]);

const HeroCardName = 'Hero card';
const ThumbnailCardName = 'Thumbnail card';
const ReceiptCardName = 'Receipt card';
const SigninCardName = 'Sign-in card';
const AnimationCardName = "Animation card";
const VideoCardName = "Video card";
const AudioCardName = "Audio card";
const CardNames = [HeroCardName, ThumbnailCardName, ReceiptCardName, SigninCardName, AnimationCardName, VideoCardName, AudioCardName];

function createCard(selectedCardName, session) {
    switch (selectedCardName) {
        case HeroCardName:
            return createHeroCard(session);
        case ThumbnailCardName:
            return createThumbnailCard(session);
        case ReceiptCardName:
            return createReceiptCard(session);
        case SigninCardName:
            return createSigninCard(session);
        case AnimationCardName:
            return createAnimationCard(session);
        case VideoCardName:
            return createVideoCard(session);
        case AudioCardName:
            return createAudioCard(session);
        default:
            return createHeroCard(session);
    }
}

function createHeroCard(session) {
    return new builder.HeroCard(session)
        .title('BotFramework Hero Card')
        .subtitle('Your bots — wherever your users are talking')
        .text('Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services.')
        .images(getSampleCardImages(session))
        .buttons(getSampleCardActions(session));
}

function createThumbnailCard(session) {
    return new builder.ThumbnailCard(session)
        .title('BotFramework Thumbnail Card')
        .subtitle('Your bots — wherever your users are talking')
        .text('Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services.')
        .images(getSampleCardImages(session))
        .buttons(getSampleCardActions(session));
}

var order = 1234;
function createReceiptCard(session) {
    return new builder.ReceiptCard(session)
        .title('John Doe')
        .facts([
            builder.Fact.create(session, order++, 'Order Number'),
            builder.Fact.create(session, 'VISA 5555-****', 'Payment Method')
        ])
        .items([
            builder.ReceiptItem.create(session, '$ 38.45', 'Data Transfer')
                .quantity(368)
                .image(builder.CardImage.create(session, 'https://github.com/amido/azure-vector-icons/raw/master/renders/traffic-manager.png')),
            builder.ReceiptItem.create(session, '$ 45.00', 'App Service')
                .quantity(720)
                .image(builder.CardImage.create(session, 'https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png'))
        ])
        .tax('$ 7.50')
        .total('$ 90.95')
        .buttons([
            builder.CardAction.openUrl(session, 'https://azure.microsoft.com/en-us/pricing/', 'More Information')
                .image('https://raw.githubusercontent.com/amido/azure-vector-icons/master/renders/microsoft-azure.png')
        ]);
}

function createSigninCard(session) {
    return new builder.SigninCard(session)
        .text('BotFramework Sign-in Card')
        .button('Sign-in', 'https://login.microsoftonline.com');
}

function createAnimationCard(session) {
    return new builder.AnimationCard(session)
        .title('Microsoft Bot Framework')
        .subtitle('Animation Card')
        .media([
            { url: 'https://bot-framework.azureedge.net/videos/skype-hero-sm.mp4' }
        ]);
}

function createVideoCard(session) {
    return new builder.VideoCard(session)
        .title('Microsoft Bot Framework and how we created the Azure Bot')
        .subtitle('by Thiago Almeida')
        .text('At Microsoft, we have first-hand experience writing bots and building artificial intelligence systems, so we’ve shared our services and tools so you can use them to add conversations to your own products. In this session we will cover the Microsoft Bot Framework and it\'s three components: the Microsoft Cognitive Services, the Bot Builder SDK, and the Bot Connector. We will also show the code and details about the Azure Bot and how it was built.')
        .media([
            { url: 'http://video.ch9.ms/ch9/15ec/8933d06d-a6cd-460b-8a52-245ab52515ec/BotFramework_mid.mp4' }
        ])
        .autoloop(true)
        .autostart(true)
        .buttons([
            builder.CardAction.openUrl(session, 'https://channel9.msdn.com/events/TechDays/Techdays-2016-The-Netherlands/Microsoft-Bot-Framework-and-how-we-created-the-Azure-Bot', 'See more in Channel9')
        ]);
}

function createAudioCard(session) {
    return new builder.AudioCard(session)
        .title('Introduction to the Microsoft Bot Framework')
        .subtitle('by Thiago Almeida')
        .text('Bots are increasingly popular and useful in many scenarios. In this session you will learn what options you have and how to get started building Bots using the Microsoft Bot Framework and other technologies such as LUIS.')
        .media([
            { url: 'http://video.ch9.ms/ch9/abcc/d1e3ab3f-2d06-4c62-92d1-56c36a9cabcc/IntroBot.mp3' }
        ])
        .buttons([
            builder.CardAction.openUrl(session, 'https://channel9.msdn.com/events/TechDays/Techdays-2016-The-Netherlands/Introduction-to-the-Microsoft-Bot-Framework', 'See more in Channel9')
        ]);
}

function getSampleCardImages(session) {
    return [
        builder.CardImage.create(session, 'https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg')
    ];
}

function getSampleCardActions(session) {
    return [
        builder.CardAction.openUrl(session, 'https://docs.botframework.com/en-us/', 'Get Started')
    ];
}