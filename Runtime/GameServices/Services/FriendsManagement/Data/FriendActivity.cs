﻿using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace GameServices
{
    [Preserve]
    [DataContract]
    public class FriendActivity
    {
        [Preserve]
        [DataMember(Name = "venue", IsRequired = true, EmitDefaultValue = true)]
        public string Venue { get; set; }

        [Preserve]
        [DataMember(Name = "status", IsRequired = true, EmitDefaultValue = true)]
        public string Status { get; set; }
    }
}