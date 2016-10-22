using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Programming
{
    public class Hands : MonoBehaviour
    {
        public static Image rockImage;
        public static Image paperImage;
        public static Image scissorsImage;

        public static Hand rock = new Hand { hand = ValidHand.Rock, name = "Rock", image = rockImage};
        public static Hand paper = new Hand { hand = ValidHand.Paper, name = "Paper", image = paperImage};
        public static Hand scissors = new Hand { hand = ValidHand.Scissors, name = "Scissors", image = scissorsImage};

        public static List<Hand> hands = new List<Hand> { rock, paper, scissors };

        public static Hand RandomHand()
        {
            return hands[Random.Range(0, (hands.Count - 1))];
        }
    }
}
