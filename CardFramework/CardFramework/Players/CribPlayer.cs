using CardFramework.Decks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardFramework {
    public class CribPlayer : Player {
        protected List<Card> Hand;
        private bool _isComputer;
        private bool _isDealer;

        public int NumCards => Hand.Count;

        public bool IsDealer { get; internal set; }

        public CribPlayer() : this(false) {
        }

        public CribPlayer(bool isComputer) {
            Hand = new List<Card>();
            _isComputer = isComputer;
        }

        public int GetScore(List<Card> crib, Card turnCard) {
            Score += ScoreHand(true, turnCard);
            if (_isDealer && crib != null) {
                //todo: Score Crib
            }
            return Score;
        }

        public void ReceiveCards(List<Card> cards) {
            Hand.AddRange(cards);
        }

        public void PassToCrib(int index1, int index2, List<Card> crib) {
            var tempCards = new List<Card> { Hand[index1], Hand[index2] };

            Hand.RemoveAt(index1);
            Hand.RemoveAt(index2);

            crib.AddRange(tempCards);
        }

        public void CompPassToCrib(List<Card> crib) {
            if (IsComputer) {
                var cards = new Card[4];
                var scores = new List<int>();
                var combos = new List<List<Card>>();
                var tempHand = new List<Card>(Hand);
                var max = 0;
                CountTotalByCombo(4, 6, 0, cards, tempHand, scores, combos);

                if (combos.Count == scores.Count) {
                    for (int i = 1; i < scores.Count; i++) {
                        if (scores[i] > scores[max]) {
                            max = i;
                        }
                    }

                    var tempCards = Hand.Where(c => combos[max].IndexOf(c) == -1).ToList();

                    crib.AddRange(tempCards);
                }
            }
        }

        protected int ScoreHand(bool countNobs, Card turnCard) {
            return 0;
        }

        public void DisplayCards() {
            foreach (var card in Hand) {
                Console.WriteLine(card.ToString());
            }
        }

        public static int CompCardToPlay(List<Card> playable, List<Card> played, int totalPlayed) {
            int play = -1;
            int indexFifteen;
            int indexThirtyOne;
            int pairSet;
            int indexStraight;
            int indexSet;

            if (played.Count < 2) {
                //If only one card has been played only a pair or a fifteen is possible as a run needs 3 cards and total can't be near 31 yet.
                if (played.Count > 0) {
                    indexFifteen = CanMakeFifteen(totalPlayed, playable);
                    indexSet = CanMakeSet(played, playable, totalPlayed);
                } else {
                    //If zero cards are played then both will be negative 1.
                    indexFifteen = -1;
                    indexSet = -1;
                }

                //Choose fifteen over pair because they score the same
                //and the pair gives the opponent the ability to score 6.
                if (indexFifteen > -1) {
                    play = indexFifteen;
                } else if (indexSet > -1) {
                    play = indexSet;
                } else {
                    int max = 0;
                    for (int i = 0; i < playable.Count; i++) {
                        if (playable[i].FaceNum > playable[max].FaceNum) {
                            max = i;
                        }
                    }

                    play = max;
                }
            } else {
                indexFifteen = CanMakeFifteen(totalPlayed, playable);
                indexThirtyOne = CanMakeThirtyOne(totalPlayed, playable);
                indexSet = CanMakeSet(played, playable, totalPlayed);
                indexStraight = CanMakeStraight(playable, played, totalPlayed);

                if (indexStraight > -1) {
                    play = indexStraight;
                } else if (indexSet > -1) {
                    play = indexSet;
                } else if (indexFifteen > -1) {
                    play = indexFifteen;
                } else if (indexThirtyOne > -1) {
                    play = indexThirtyOne;
                } else {
                    int max = 0;
                    for (int i = 0; i < playable.Count; i++) {
                        if (playable[i].FaceNum > playable[max].FaceNum && playable[i].FaceNum + totalPlayed <= 31) {
                            max = i;
                        }
                    }

                    if (max == 0 && playable[max].FaceNum + totalPlayed <= 31) {
                        play = max;
                    }
                }
            }

            return play;
        }

        public void ClearLastHand() {
            Hand.Clear();
        }

        private static int CanMakeStraight(List<Card> playable, List<Card> played, int totalPlayed) {
            List<Card> cards = new List<Card>();

            for (int i = 0; i < playable.Count; i++) {
                foreach (Card card in played) {
                    cards.Add(card);
                }
                cards.Add(playable[i]);
                CribHand.Sort(cards);
                if (IsStraight(cards.ToArray()) && playable[i].FaceNum + totalPlayed <= 31) {
                    return i;
                }

                cards.Clear();
            }

            return -1;
        }

        private static bool IsStraight(Card[] cards) {
            for (int i = 0; i < cards.GetUpperBound(0); i++) {
                if (cards[i].FaceNum + 1 != cards[i + 1].FaceNum) {
                    return false;
                }
            }

            return true;
        }

        private static int CanMakeFifteen(int totalPlayed, List<Card> playable) {

            for (int i = 0; i < playable.Count; i++) {
                if (playable[i].FaceNum + totalPlayed == 15) {
                    return i;
                }
            }

            return -1;
        }

        private static int CanMakeThirtyOne(int totalPlayed, List<Card> playable) {

            for (int i = 0; i < playable.Count; i++) {
                if (playable[i].FaceNum + totalPlayed == 31) {
                    return i;
                }
            }

            return -1;
        }

        private static int CanMakeSet(List<Card> played, List<Card> playable, int totalPlayed) {
            for (int i = 0; i < playable.Count; i++) {
                if (playable[i].FaceNum == played[played.Count - 1].FaceNum && playable[i].FaceNum + totalPlayed <= 31) {
                    return i;
                }
            }


            return -1;
        }

        protected void CountTotalByCombo(int selected, int total, int start, Card[] cards, List<Card> tempHand, List<int> scores, List<List<Card>> combos) {
            selected--;
            for (int i = start; i < tempHand.Count; i++) {
                cards[selected] = tempHand[i];
                if (selected == 0) {

                    var scoreHand = new List<Card>(cards.ToList());
                    int score = 0;//scoreHand.ScoreHand(false, TurnCard);
                    scores.Add(score);
                    combos.Add(new List<Card>(scoreHand));

                } else {
                    start++;
                    CountTotalByCombo(selected, total, start, cards, tempHand, scores, combos);
                }
            }
        }

        public override string DisplayHand() {
            throw new NotImplementedException();
        }

        public override void ScoreHand() {
            throw new NotImplementedException();
        }
    }
}
