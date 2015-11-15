﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Artist
{
    [DataContract]
    public class BiographyResponse : Response
    {
        [DataMember(Name = "start")]
        public int Start { get; set; }
        [DataMember(Name = "total")]
        public int Total { get; set; }
        [DataMember(Name = "biographies")]
        public List<BiographyItem> Biographies { get; set; }
    }
}