using System;
using Newtonsoft.Json;

namespace Project.Tesla.API.Model
{
    public class Behavior
    {
        public string BehaviorId { get; set; }
        public string BehaviorName { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
