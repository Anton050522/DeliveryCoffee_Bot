using System;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DeliveryCoffeeBot.Doner
{
    public class WantDonerCommand : Command
    {
        public override string Name => "wantdoner";

        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;

            if (!ChatDonerParticipants.Participants.ContainsKey(chatId)
                || (ChatDonerParticipants.Participants[chatId].Date.Year != DateTime.Now.Year
                && ChatDonerParticipants.Participants[chatId].Date.Month != DateTime.Now.Month
                && ChatDonerParticipants.Participants[chatId].Date.Day != DateTime.Now.Day))
            {
                await client.SendTextMessageAsync(chatId, "Извините, для начала используйте команду '/donertime'");

                return;
            }
            if ((!string.IsNullOrEmpty(message.From.Username)
                && ChatDonerParticipants.Participants[chatId].Participants.Select(t => t.UserName).Contains(message.From.Username))
                || (!string.IsNullOrEmpty(message.From.Username)
                    && ChatDonerParticipants.Participants[chatId].Participants.Select(t => t.Id).Contains(message.From.Id)))
            {
                await client.SendTextMessageAsync(chatId, "Извините, вы уже в деле!");

                return;
            }
            ChatDonerParticipants.Participants[chatId].Participants.Add(new Participant(message.From.Username, message.From.Id));

            await client.SendTextMessageAsync(chatId, "Ты в деле! Мы вкусно покушаем!", replyToMessageId: message.MessageId);
        }
    }
}
