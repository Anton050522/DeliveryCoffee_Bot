using System;
using System.Collections.Generic;

namespace DeliveryCoffeeBot.Doner
{
    public static class ChatDonerParticipants
    {
        public static Dictionary<long, DonerParticipants> Participants { get; set; }
            = new Dictionary<long, DonerParticipants>();
    }

    public class DonerParticipants
    {
        public List<Participant> Participants { get; set; }
        public DateTime Date { get; set; }
        public bool IsUsed { get; set; }

        public DonerParticipants()
        {
            Participants = new List<Participant>();
            Date = DateTime.Now;
        }
    }

    public class Participant
    {
        public string UserName { get; set; }
        public long Id { get; set; }

        public Participant(string userName, long id)
        {
            UserName = userName;
            Id = id;
        }
    }
}
