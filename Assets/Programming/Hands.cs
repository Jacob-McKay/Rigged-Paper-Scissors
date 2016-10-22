using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Programming
{
    public class Hands : MonoBehaviour
    {
        public Sprite rockImage;
        public Sprite paperImage;
        public Sprite scissorsImage;

        private List<Hand> _hands;

   
        public void Start()
        {
            _hands = new List<Hand> { Rock, Paper, Scissors };
        }

        public Hand GetRandomHand()
        {
            return _hands[Random.Range(0, _hands.Count)];
        }

        public Hand Rock
        {
            get {
                return new Hand { hand = ValidHand.Rock, name = "Rock", image = rockImage };
            }
        }

        public Hand Paper
        {
            get
            {
                return new Hand { hand = ValidHand.Paper, name = "Paper", image = paperImage };
            }
        }

        public Hand Scissors
        {
            get
            {
                return new Hand { hand = ValidHand.Scissors, name = "Scissors", image = scissorsImage };
            }
        }
    }
}
