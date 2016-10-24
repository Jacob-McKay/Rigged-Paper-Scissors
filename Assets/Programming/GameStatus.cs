using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Programming
{
    public static class GameState
    {
        public static bool ServerDidShoot { get; set; } 

        public static bool ClientDidShoot { get; set; }

        public static ValidHand ServerHandShot { get; set; }

        public static ValidHand ClientHandShot { get; set; }

        public static Outcome ServerOutcome
        {
            get
            {
                var serverOutcome = Hands.Against(ServerHandShot, ClientHandShot);
                return serverOutcome;
            }
        }

        public static Outcome ClientOutcome
        {
            get
            {
                var clientOutcome = Hands.Against(ClientHandShot, ServerHandShot);
                return clientOutcome;
            }
        }

        public static float TimeStarted { get; set; }

        public static void Reset()
        {
            ServerDidShoot = false;
            ClientDidShoot = false;
            ServerHandShot = ValidHand.Abstain;
            ClientHandShot = ValidHand.Abstain;
            TimeStarted = Time.time;
        }
    }
}
