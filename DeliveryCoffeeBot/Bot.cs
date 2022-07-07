using DeliveryCoffeeBot.Coffee;
using DeliveryCoffeeBot.Doner;
using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace DeliveryCoffeeBot
{
    public static class Bot
    {
        private static TelegramBotClient client;

        public static List<Command> commandList;
        public static IReadOnlyList<Command> Commands { get => commandList.AsReadOnly(); }

        [System.Obsolete]
        public static TelegramBotClient Get()
        {
            if (client != null)
            {
                return client;
            }
            commandList = new List<Command>()
            {
                new CoffeeTimeCommand(),
                new WantCoffeeCommand(),
                new ShowCoffeeResultCommand(),
                new DonerTimeCommand(),
                new ShowDonerResult(),
                new WantDonerCommand()
            };
            client = new TelegramBotClient(AppSettings.Key);
            client.OnMessage += BotOnMessageReceived;
            client.OnMessageEdited += BotOnMessageReceived;
            client.StartReceiving();

            return client;
        }

        [System.Obsolete]
        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            foreach (var command in Commands)
            {
                if (command.Contains(message.Text))
                {
                    command.Execute(message, client);
                }
            }
        }
    }
}
