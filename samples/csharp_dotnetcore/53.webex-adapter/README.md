﻿# Webex Adapter

Bot Framework v4 echo bot using Webex Adapter sample.

This bot has been created using [Bot Framework](https://dev.botframework.com), it shows how to create a simple echo bot that connects with Webex to respond messages.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) version 2.1

  ```bash
  # determine dotnet version
  dotnet --version
  ```

## To try this sample

- Clone the repository

    ```bash
    git clone https://github.com/Microsoft/botbuilder-samples.git
    ```

- In a terminal, navigate to `BotBuilder-Samples/samples/csharp_dotnetcore/53. WebexAdapterBot`

- Connect the bot with Webex by following the instructions below.

- Run the bot from a terminal or from Visual Studio, choose option A or B.

  A) From a terminal

  ```bash
  # run the bot
  dotnet run
  ```

  B) Or from Visual Studio

  - Launch Visual Studio
  - File -> Open -> Project/Solution
  - Navigate to `samples/csharp_dotnetcore/53. WebexAdapterBot` folder
  - Select `WebexAdapterBot.csproj` file
  - Press <kbd>F5</kbd> to run the project

- Using Webex Teams, search your bot by the _Bot Username_ in the contacts barTest and send a message.

### Connect the bot with Webex

1 - Create an Account in [Webex](https://www.webex.com/).

2 - Create a new app (bot) in [Webex Team] (https://developer.webex.com/my-apps).

_**Note**: When clicking in the `Add bot` button, the bot's credentials will be generated._

3 - Copy the _Access Token_ from the Dashboard. This credential will be needed to connect the bot to Webex.

4 - Using a tunneling tool like [Ngrok](https://ngrok.com/download), expose the bot's endpoint.

5 - Create/Update the webhooks for messages and attachments for the bot using https://developer.webex.com/docs/api/v1/webhooks 

    Complete the form with the following data:
        - Authorization: Complete with the WebexAccessToken (the one obtained in step 3)
        - Name: Choose a name for the Webhook
        - TargetUrl: Complete with the https url generated in step 4 adding '/api/messages' to it
        - Resource: Complete with 'messages' or 'attachmentActions'
        - Event: Complete with 'all'
        - Secret: Complete with any random secret.
    Click the Run button

6 - Back in the bot project, set the credentials in _appsettings.json_.

    WebexAccessToken (the one obtained in step 3)
    WebexPublicAddress (the url obtained in step 4)
    WebexSecret (the one set for the Webhook)
    WebexWebhookName (The one created in step 5)


## Deploy the bot to Azure

To learn more about deploying a bot to Azure, see [Deploy your bot to Azure](https://aka.ms/azuredeployment) for a complete list of deployment instructions.

## Further reading

- [Bot Framework Documentation](https://docs.botframework.com)
- [Bot Basics](https://docs.microsoft.com/azure/bot-service/bot-builder-basics?view=azure-bot-service-4.0)
- [Activity processing](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-concept-activity-processing?view=azure-bot-service-4.0)
- [Azure Bot Service Introduction](https://docs.microsoft.com/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0)
- [Azure Bot Service Documentation](https://docs.microsoft.com/azure/bot-service/?view=azure-bot-service-4.0)
- [.NET Core CLI tools](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x)
- [Azure CLI](https://docs.microsoft.com/cli/azure/?view=azure-cli-latest)
- [Azure Portal](https://portal.azure.com)
- [Language Understanding using LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/)
- [Channels and Bot Connector Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-concepts?view=azure-bot-service-4.0)
- [Restify](https://www.npmjs.com/package/restify)
- [dotenv](https://www.npmjs.com/package/dotenv)