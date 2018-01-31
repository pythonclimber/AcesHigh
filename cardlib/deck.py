from card import Card
from random import randint

suits = ['Clubs', 'Hearts', 'Spades', 'Diamonds']

class Deck:

    def __iter__(self):
        self._index = -1
        return self


    def __next__(self):
        try:
            self._index += 1
            return self._cards[self._index]
        except IndexError:
            raise StopIteration('')


    def __init__(self):
        self.create_cards()


    def __len__(self):
        return len(self._cards)


    def shuffle(self):
        for x in range(0, 3):
            for i in range(0, len(self._cards)):
                idx = randint(0, len(self._cards)-1)
                self._cards[i], self._cards[idx] = self._cards[idx], self._cards[i]

        return self


    def deal_cards(self, count=1):
        if type(count) != type(0):
            raise ValueError('Count of cards to deal must be a number')

        cards = []

        for i in range(count):
            cards.append(self._cards.pop(0))

        return cards



class StandardDeck(Deck):

    def create_cards(self):
        faces = ['Ace', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine', 'Ten', 'Jack', 'Queen', 'King']

        self._cards = []

        for s in suits:
            val = 1
            for f in faces:
                self._cards.append(Card(s, f, val))
                val += 1



class EuchreDeck(Deck):

    def create_cards(self):
        faces = ['Nine', 'Ten', 'Jack', 'Queen', 'King', 'Ace']

        self._cards = []

        for s in suits:
            val = 1
            for f in faces:
                self._cards.append(Card(s, f, val))
                val += 1



class PinochleDeck(Deck):

    def create_cards(self):
        faces = ['Nine', 'Jack', 'Queen', 'King', 'Ten', 'Ace']

        self._cards = []

        for s in suits:
            val = 1
            for f in faces:
                self._cards.append(Card(s, f, val))
                self._cards.append(Card(s, f, val))
                val += 1
