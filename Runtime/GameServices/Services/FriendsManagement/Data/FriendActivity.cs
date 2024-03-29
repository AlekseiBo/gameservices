﻿using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace GameServices
{
    [Preserve]
    [DataContract]
    public class FriendActivity
    {
        [Preserve]
        [DataMember(Name = "venue", IsRequired = false, EmitDefaultValue = true)]
        public string Venue { get; set; }

        [Preserve]
        [DataMember(Name = "code", IsRequired = false, EmitDefaultValue = true)]
        public string Code { get; set; }
    }
}