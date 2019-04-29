using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Project.Tesla.API.Model
{
    public class Session
    {
        public string Id { get; set; }
        public string SessionID { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int TherapistId { get; set; }
        public string ActivityName { get; set; }
        public int ChildID { get; set; }
        public int RewardGoal { get; set; }
        public int ActivityDuration { get; set; }
        public List<Behavior> Behaviors { get; set; }
        public int StartingReward { get; set; }
        public string NotificationType { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
