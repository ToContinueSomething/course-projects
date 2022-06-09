using System;
using System.Collections.Generic;

namespace CardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            CardTable cardTable = new CardTable();
            cardTable.PlayGame();
        }
    }

    class CardTable
    {
        private List<Player> _players;

        private Deck _deck;
        private bool _isGame;

        public CardTable()
        {
            _players = new List<Player>();
            _deck = new Deck();
            CreateNewPlayers(4);
        }

        public void PlayGame()
        {
            int countCards = 16;
            _deck.AddCards(countCards);

            Console.WriteLine("Игра в (Двадцать одно).");
            int winningNumber = 21;
            _isGame = true;

            if (_deck.EnoughCardsForGame(_players.Count) == false)
            {
                _isGame = false;
                Console.WriteLine("Игра не может быть начата. Не хватает карт.");
            }

            while (_isGame)
            {
                Console.WriteLine("PickUp - взять карту|Stop - карт хватает.");

                switch (Console.ReadLine())
                {
                    case "PickUp":
                        GiveAwayCards();
                        break;
                    case "Stop":
                        _isGame = false;
                        break;
                    default:
                        Console.WriteLine("Такой команды нет.");
                        continue;
                }

                ViewPlayersCards();

                if (_deck.ContainsCards() == false)
                {
                    Console.WriteLine("\nКарты закончились.\n");
                    _isGame = false;
                }

                if (_isGame == false)
                {
                    if (CheckPlayersDraw() == false)
                    {
                        FindWinner(winningNumber);
                    }

                    Console.WriteLine("Игра закончена.");
                }
                else
                {
                    Console.WriteLine("\nДля продолжения нажмите клавишу...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private void GiveAwayCards()
        {
            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].AddCard(_deck.PickUpCard());
            }
        }

        private void CreateNewPlayers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _players.Add(new Player());
            }
        }

        private void ViewPlayersCards()
        {
            _players.Sort((onePlayer, twoPlayer) => twoPlayer.ValueCards.CompareTo(onePlayer.ValueCards));

            for (int i = 0; i < _players.Count; i++)
            {
                Console.WriteLine($"\nКарты {i + 1} игрока | Общее: { _players[i].ValueCards}");
                _players[i].ViewCards();
            }
        }

        private void FindWinner(int winningNumber)
        {
            for (int i = 0; i < _players.Count - 1; i++)
            {
                if (_players[i].ValueCards == winningNumber)
                {
                    Console.WriteLine("\nПобедил игрок с картами");
                    _players[i].ViewCards();
                }
                else
                {
                    if (_players[i].ValueCards > winningNumber)
                    {
                        if (_players[i].ValueCards > _players[i + 1].ValueCards)
                        {
                            Console.WriteLine("\nигрок с картами:");
                            _players[i + 1].ViewCards();
                            Console.WriteLine("Победил игрока с картами");
                            _players[i].ViewCards();
                        }
                    }
                    else
                    {
                        if (_players[i].ValueCards > _players[i + 1].ValueCards)
                        {
                            Console.WriteLine("\nигрок с картами:");
                            _players[i].ViewCards();
                            Console.WriteLine("Победил игрока с картами");
                            _players[i + 1].ViewCards();
                        }                      
                    }
                }
            }
        }

        private bool CheckPlayersDraw()
        {
            bool isDraw = false;

            for (int i = 0; i < _players.Count - 1; i++)
            {
                if (_players[i].ValueCards == _players[i + 1].ValueCards)
                {
                    Console.WriteLine("\nигрок с картами:");
                    _players[i].ViewCards();
                    Console.WriteLine("Ничья. Игрока с картами");
                    _players[i + 1].ViewCards();

                    isDraw = true;
                }
                else
                {
                    isDraw = false;
                }
            }

            return isDraw;
        }
    }
}

class Player
{
    private List<Card> _cards;

    public int ValueCards { get; private set; }

    public Player()
    {
        _cards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        ValueCards += card.Value;
        _cards.Add(card);
    }

    public void ViewCards()
    {
        foreach (var card in _cards)
        {
            card.ShowInfo();
        }
    }
}

class Deck
{
    private static Random _random;
    private Queue<Card> _cards;

    static Deck()
    {
        _random = new Random();
    }

    public Deck()
    {
        _cards = new Queue<Card>();
    }

    public void AddCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _cards.Enqueue(GetCorrectCard());
        }
    }

    public bool EnoughCardsForGame(int countPlayers)
    {
        return _cards.Count % countPlayers == 0;
    }

    public bool ContainsCards()
    {
        return _cards.Count > 0;
    }

    public Card PickUpCard()
    {
        return _cards.Dequeue();
    }

    private Card GetCorrectCard()
    {
        int minValue = 2;
        int maxValue = 11;

        Card tempCard = null;
        bool isWork = true;

        while (isWork)
        {
            isWork = false;
            tempCard = new Card(_random.Next(minValue, maxValue));

            if (_cards.Count <= 0)
            {
                foreach (var card in _cards)
                {
                    if (tempCard.Label == card.Label && tempCard.Value == card.Value)
                        isWork = true;
                }
            }
        }

        return tempCard;
    }
}

class Card
{
    private static Random _random;

    public string Label { get; private set; }
    public int Value { get; private set; }

    static Card()
    {
        _random = new Random();
    }

    public Card(int value)
    {
        Value = value;

        string[] labelCards = new string[] { "Черви", "Буби", "Пики", "Трефи" };
        Label = labelCards[_random.Next(0, labelCards.Length)];
    }

    public void ShowInfo()
    {
        Console.WriteLine($"{Label}|{Value}");
    }
}