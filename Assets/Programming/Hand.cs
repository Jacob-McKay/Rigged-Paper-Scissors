using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Programming
{
    public enum ValidHand { Rock, Paper, Scissors };

    public enum Outcome { Win, Loss, Stalemate }

    public class Hand
    {
        public ValidHand hand;

        public string name;

        public Sprite image;

        public Outcome Against(Hand otherHand)
        {
            //everyone starts off a loser, because I'm a mean guy
            Outcome outcome = Outcome.Loss;

            switch (hand)
            {
                case ValidHand.Rock:
                    if (otherHand.hand == ValidHand.Rock)
                        outcome = Outcome.Stalemate;

                    if (otherHand.hand == ValidHand.Paper)
                        outcome = Outcome.Loss;

                    if (otherHand.hand == ValidHand.Scissors)
                        outcome = Outcome.Win;
                    break;

                case ValidHand.Paper:
                    if (otherHand.hand == ValidHand.Rock)
                        outcome = Outcome.Win;

                    if (otherHand.hand == ValidHand.Paper)
                        outcome = Outcome.Stalemate;

                    if (otherHand.hand == ValidHand.Scissors)
                        outcome = Outcome.Loss;
                    break;

                case ValidHand.Scissors:
                    if (otherHand.hand == ValidHand.Rock)
                        outcome = Outcome.Loss;

                    if (otherHand.hand == ValidHand.Paper)
                        outcome = Outcome.Win;

                    if (otherHand.hand == ValidHand.Scissors)
                        outcome = Outcome.Stalemate;
                    break;
                default:
                    throw new Exception("Wtf did you shoot?");
            }

            return outcome;
        }
    }
}
