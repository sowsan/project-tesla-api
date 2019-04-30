using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Project.Tesla.API.Model
{
    public class Event {
        public string EventID { get; set; }
        public string Type { get; set; }
        public string SessionID { get; set; }
        public string BehaviorID { get; set; }
        public string BehaviorName { get; set; }
        public int Point { get; set; }
        public string NotifcationType { get; set; }
        public string TherapistID { get; set; }
        public string ChildID { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}