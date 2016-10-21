using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Programming.MatchMaking
{
    public class NearbyOpponentMatch
    {
        public string PlayerName {get; set;}
        
        public string FromAddress { get; set; }

        public float OpponentExpireTime { get; set; }

        public GameObject OpponentListItemInstance { get; set; }
    }
}
