using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CardFramework.Decks;

namespace CardFramework
{
    public class CribPlayer : Player
    {
        #region Protected Members

        protected CribHand hand;
        protected Card turnCard;

        #endregion

        #region Properties

        public CribHand Hand
        {
            get { return hand; }
        }

        public Card TurnCard
        {
            get { return turnCard; }
            set { turnCard = value; }
        }

        public bool IsDealer { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor
        /// Should not be used as it does not differentiate between human and computer players (automatically creates non-computer player)
        /// </summary>
        public CribPlayer() : this(false)
        {
        }

        /// <summary>
        /// Constructor to instantiate a player with a bool determining whether player is computer or not
        /// </summary>
        /// <param name="newIsComp"></param>
        public CribPlayer(bool newIsComp)
        {
            hand = new CribHand(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Scores the player's CribHand attribute and the game's crib if he is the dealer.
        /// </summary>
        public int GetScore(CribHand crib)
        {
            Score += hand.ScoreHand(true, TurnCard);
            if (this.IsDealer && crib != null)
            {
                Score += crib.ScoreHand(true, TurnCard);
            }
            return Score;
        }

        /// <summary>
        /// Takes cards that are dealt to the player and passes them to his hand
        /// </summary>
        public void ReceiveCards(List<Card> cards)
        {
            hand.Cards.AddRange(cards);
        }

        /// <summary>
        /// Takes a single card given to the player and passes it to his hand
        /// </summary>
        public void ReceiveCard(List<Card> cards)
        {
            hand.Cards.AddRange(cards);
        }

        /// <summary>
        /// Removes cards from the users hand and passes them to this round's crib
        /// </summary>
        public void PassToCrib(int index1, int index2, CribHand crib)
        {
            List<Card> tempCards = new List<Card>();

            tempCards.Add(hand.Cards[index1]);
            tempCards.Add(hand.Cards[index2]);

            hand.Cards.RemoveAt(index1);
            hand.Cards.RemoveAt(index2);

            crib.Cards.AddRange(tempCards);
        }

        /// <summary>
        /// Calls CountTotalByCombo to determine which cards the computer player should pass to his crib and passes them
        /// </summary>
        /// <param name="crib"></param>
        public void CompPassToCrib(CribHand crib)
        {
            if (IsComputer)
            {
                Card[] cards = new Card[4];
                List<int> scores = new List<int>();
                List<List<Card>> combos = new List<List<Card>>();
                List<Card> tempHand = hand.Cards;
                int max = 0;
                CountTotalByCombo(4, 6, 0, cards, tempHand, scores, combos);

                if (combos.Count == scores.Count)
                {
                    for (int i = 1; i < scores.Count; i++)
                    {
                        if (scores[i] > scores[max])
                        {
                            max = i;
                        }
                    }

                    List<Card> tempCards = hand.GetCardsToPull(combos[max]);

                    crib.Cards.AddRange(tempCards);
                }
            }
        }

        public void DisplayCards()
        {
            foreach (Card card in hand.Cards)
            {
                Console.WriteLine(card.ToString());
            }
        }

        public static int CompCardToPlay(List<Card> playable, List<Card> played, int totalPlayed)
        {
            int play = -1;
            int indexFifteen;
            int indexThirtyOne;
            int pairSet;
            int indexStraight;
            int indexSet;

            if (played.Count < 2)
            {
                //If only one card has been played only a pair or a fifteen is possible as a run needs 3 cards and total can't be near 31 yet.
                if (played.Count > 0)
                {
                    indexFifteen = CanMakeFifteen(totalPlayed, playable);
                    indexSet = CanMakeSet(played, playable, totalPlayed);
                }
                else
                {
                    //If zero cards are played then both will be negative 1.
                    indexFifteen = -1;
                    indexSet = -1;
                }

                //Choose fifteen over pair because they score the same
                //and the pair gives the opponent the ability to score 6.
                if (indexFifteen > -1) 
                {
                    play = indexFifteen;
                }
                else if (indexSet > -1) 
                {
                    play = indexSet;
                }
                else
                {
                    int max = 0;
                    for (int i = 0; i < playable.Count; i++)
                    {
                        if (playable[i].FaceNum > playable[max].FaceNum)
                        {
                            max = i;
                        }
                    }

                    play = max;
                }
            }
            else
            {
                indexFifteen = CanMakeFifteen(totalPlayed, playable);
                indexThirtyOne = CanMakeThirtyOne(totalPlayed, playable);
                indexSet = CanMakeSet(played, playable, totalPlayed);
                indexStraight = CanMakeStraight(playable, played, totalPlayed);

                if (indexStraight > -1)
                {
                    play = indexStraight;
                }
                else if (indexSet > -1)
                {
                    play = indexSet;
                }
                else if (indexFifteen > -1)
                {
                    play = indexFifteen;
                }
                else if (indexThirtyOne > -1)
                {
                    play = indexThirtyOne;
                }
                else
                {
                    int max = 0;
                    for (int i = 0; i < playable.Count; i++)
                    {
                        if (playable[i].FaceNum > playable[max].FaceNum && playable[i].FaceNum + totalPlayed <= 31)
                        {
                            max = i;
                        }
                    }

                    if (max == 0 && playable[max].FaceNum + totalPlayed <= 31)
                    {
                        play = max;
                    }
                }
            }

            return play;
        }

        public void ClearLastHand()
        {
            hand.Cards.Clear();
        }

        #endregion

        #region Private and Protected Methods

        private static int CanMakeStraight(List<Card> playable, List<Card> played, int totalPlayed)
        {
            List<Card> cards = new List<Card>();

            for (int i = 0; i < playable.Count; i++)
            {
                foreach (Card card in played)
                {
                    cards.Add(card);
                }
                cards.Add(playable[i]);
                CribHand.Sort(cards);
                if (IsStraight(cards.ToArray()) && playable[i].FaceNum + totalPlayed <= 31)
                {
                    return i;
                }
                
                cards.Clear();
            }
            
            return -1;
        }

        private static bool IsStraight(Card[] cards)
        {
            for (int i = 0; i < cards.GetUpperBound(0); i++)
            {
                if (cards[i].FaceNum + 1 != cards[i + 1].FaceNum)
                {
                    return false;
                }
            }

            return true;
        }

        private static int CanMakeFifteen(int totalPlayed, List<Card> playable)
        {

            for (int i = 0; i < playable.Count; i++)
            {
                if (playable[i].FaceNum + totalPlayed == 15)
                {
                    return i;
                }
            }

            return -1;
        }

        private static int CanMakeThirtyOne(int totalPlayed, List<Card> playable)
        {

            for (int i = 0; i < playable.Count; i++)
            {
                if (playable[i].FaceNum + totalPlayed == 31)
                {
                    return i;
                }
            }

            return -1;
        }

        private static int CanMakeSet(List<Card> played, List<Card> playable, int totalPlayed)
        {
            for (int i = 0; i < playable.Count; i++)
            {
                if (playable[i].FaceNum == played[played.Count - 1].FaceNum && playable[i].FaceNum + totalPlayed <= 31)
                {
                    return i;
                }
            }


            return -1;
        }

        /// <summary>
        /// Scores each combination of four cards in the computer player's hand of six
        /// This is used to determine which cards to pass to the crib and should only be called from CompPassToCrib
        /// </summary>
        protected void CountTotalByCombo(int selected, int total, int start, Card[] cards, List<Card> tempHand, List<int> scores, List<List<Card>> combos)
        {
            selected--;
            for (int i = start; i < tempHand.Count; i++)
            {
                cards[selected] = tempHand[i];
                if (selected == 0)
                {

                    var scoreHand = new CribHand(cards.ToList());
                    int score = scoreHand.ScoreHand(false, TurnCard);
                    scores.Add(score);
                    combos.Add(new List<Card>(scoreHand.Cards));

                }
                else
                {
                    start++;
                    CountTotalByCombo(selected, total, start, cards, tempHand, scores, combos);
                }
            }
        }

        #endregion

        public override string DisplayHand()
        {
            throw new NotImplementedException();
        }

        public override void ScoreHand()
        {
            throw new NotImplementedException();
        }
    }
}
