class Card:

    def __str__(self):
        return "The {} of {}".format(self._face, self._suit)


    def __init__(self, suit, face, value):
        self._suit = suit
        self._face = face
        self._value = value


    def value(self):
        return self._value


    def face(self):
        return self._face


    def suit(self):
        return self._suit


    def faceValue(self):
        return 10 if self._value > 10 else self._value
