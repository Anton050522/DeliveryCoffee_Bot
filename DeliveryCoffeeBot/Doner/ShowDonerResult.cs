using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DeliveryCoffeeBot.Doner
{
    public class ShowDonerResult : Command
    {
        public override string Name => "showdonerresult";

        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;

            if (!ChatDonerParticipants.Participants.ContainsKey(chatId)
                || (ChatDonerParticipants.Participants[chatId].Date.Year != DateTime.Now.Year
                && ChatDonerParticipants.Participants[chatId].Date.Month != DateTime.Now.Month
                && ChatDonerParticipants.Participants[chatId].Date.Day != DateTime.Now.Day))
            {
                await client.SendTextMessageAsync(chatId, "Извините, для начала используйте команду '/donertime'");
            }
            else if (ChatDonerParticipants.Participants[chatId].Participants.Count <= 1)
            {
                await client.SendTextMessageAsync(chatId, "Извините, не набралось необходимое количество участников!");
            }
            else if (ChatDonerParticipants.Participants[chatId].IsUsed)
            {
                await client.SendTextMessageAsync(chatId, "Извините, сегодня команда уже использовалась, попробуйте завтра!");
            }
            else
            {
                ChatDonerParticipants.Participants[chatId].IsUsed = true;
                var shuffleParticipants = ChatDonerParticipants.Participants[chatId].Participants.OrderBy(x => Guid.NewGuid().ToString()).ToList();
                var pairCount = shuffleParticipants.Count / 2;
                var winnerCount = new Random().Next(1, pairCount + 1);
                var resultMessage = new StringBuilder();

                for (var i = 0; i < winnerCount; i++)
                {
                    var winner = !string.IsNullOrWhiteSpace(shuffleParticipants[i * 2].UserName)
                        ? shuffleParticipants[i * 2].UserName
                        : $"tg://user?id={shuffleParticipants[i * 2].Id}";

                    resultMessage.Append($"Сегодня {DateTime.Now.Day} за шаурмой идет {winner}");
                }
                await client.SendTextMessageAsync(chatId, resultMessage.ToString());
            }
        }
    }
}
