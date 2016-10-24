using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace Assets.Programming
{
    public class Hands : MonoBehaviour
    {
        public Sprite rockImage;
        public Sprite paperImage;
        public Sprite scissorsImage;

        private List<Hand> _hands;

        public void Awake()
        {
            _hands = new List<Hand> { Rock, Paper, Scissors };
        }

        public Hand GetRandomHand()
        {
            if(_hands == null)
            {
                _hands = new List<Hand> { Rock, Paper, Scissors };
            }
            return _hands[UnityEngine.Random.Range(0, _hands.Count)];
        }

        public static Outcome Against(ValidHand yourHand, ValidHand theirHand)
        {
            //everyone starts off a loser, because I'm a mean guy
            Outcome outcome = Outcome.Loss;

            switch (yourHand)
            {
                case ValidHand.Rock:
                    if (theirHand == ValidHand.Rock)
                        outcome = Outcome.Stalemate;

                    if (theirHand == ValidHand.Paper)
                        outcome = Outcome.Loss;

                    if (theirHand == ValidHand.Scissors)
                        outcome = Outcome.Win;

                    if (theirHand == ValidHand.Abstain)
                        outcome = Outcome.Win;
                    break;

                case ValidHand.Paper:
                    if (theirHand == ValidHand.Rock)
                        outcome = Outcome.Win;

                    if (theirHand == ValidHand.Paper)
                        outcome = Outcome.Stalemate;

                    if (theirHand == ValidHand.Scissors)
                        outcome = Outcome.Loss;

                    if (theirHand == ValidHand.Abstain)
                        outcome = Outcome.Win;
                    break;

                case ValidHand.Scissors:
                    if (theirHand == ValidHand.Rock)
                        outcome = Outcome.Loss;

                    if (theirHand == ValidHand.Paper)
                        outcome = Outcome.Win;

                    if (theirHand == ValidHand.Scissors)
                        outcome = Outcome.Stalemate;

                    if (theirHand == ValidHand.Abstain)
                        outcome = Outcome.Win;

                    break;

                case ValidHand.Abstain:
                    if (theirHand == ValidHand.Rock)
                        outcome = Outcome.Loss;

                    if (theirHand == ValidHand.Paper)
                        outcome = Outcome.Loss;

                    if (theirHand == ValidHand.Scissors)
                        outcome = Outcome.Loss;

                    if (theirHand == ValidHand.Abstain)
                        outcome = Outcome.Stalemate;

                    break;
                default:
                    throw new Exception("Wtf did you shoot?");
            }

            return outcome;
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
