from time import time

from deck import StandardDeck

from cardlib.card import Card

start = (time() * 1000)

deck = StandardDeck()
deck.shuffle()

for card in deck:
    print(card)

print()
deck._cards = sorted(deck, key=Card.value)

for card in deck:
    print(card)

stop = (time() * 1000)

print((stop - start))