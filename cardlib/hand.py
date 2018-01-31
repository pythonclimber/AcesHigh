from card import Card

class Hand:

    def __len__(self):
        return len(self._cards)


    def __init__(self):
        self._cards = []


    def take_cards(self, cards):
        if type(cards) != type([]):
            raise ValueError('The cards for the hand must be passed in a list')

        for card in cards:
            self._cards.append(card)


    def  score_hand(self, card_scorer):
        score = card_scorer()
        print(score)



def test_hand_take_cards():
    hand = Hand()
    hand.take_cards([Card('Clubs', 'Ace', 1)])

    if len(hand) != 1:
        raise AssertionError('take_cards_test : Hand should contain one card')


def poker_card_scorer(cards):
    if type(cards) != type([]):
        raise ValueError('Cards to score must be a list')

    poker_hands = [None, 'High Card', 'Pair', 'Two Pair', 'Three of a Kind', 'Straight', 'Flush', 'Full House', 'Four of a Kind', 'Straight Flush']

    values = sorted([card.value for card in cards])
    is_flush = len({card.suit for card in cards}) == 1

    if max(values) - min(values) == 4:
        comparisons = {values[i] - 1 == values[i - 1] for i in range(1, len(values))}
    elif max(values) == 13 and min(values) == 1:
        comparisons = {values[i] - 1 == values[i - 1] for i in range(2, len(values))}

    is_straight = len(comparisons) == 1 and bool(comparisons.pop())





if __name__ == '__main__':
    test_hand_take_cards()