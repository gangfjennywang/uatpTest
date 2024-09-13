using NLog;
using NLog.Config;
using NLog.Targets;

namespace uatp.Models
{
        public class Card
        {
            public string? CardNumber { get; set; }
            public decimal Balance { get; set; }
        }

    public class CardRepository
        {
            private static readonly Dictionary<string, Card> Cards = new Dictionary<string, Card>();
            protected static Logger logger = LogManager.GetCurrentClassLogger();
        
        // Card CreateCard()
        public virtual Card CreateCard()
            {
                try
                {
                    var cardNumber = GenerateCardNumber();
                    var card = new Card { CardNumber = cardNumber, Balance = 0 };
                    Cards[cardNumber] = card;
                    return card;
                }
                catch (Exception ex) 
                {
                    logger.Error(ex, "An error occurred during CreateCard");
                    throw; 
                }
            }

            //public virtual bool Pay (string cardNumber, decimal amount)
            public bool Pay(string cardNumber, decimal amount)
            {
                if (Cards.ContainsKey(cardNumber))
                {
                    var card = Cards[cardNumber];
                    if (card.Balance >= amount)
                    {
                        card.Balance -= amount;
                        return true;
                    }
                }
                return false;
            }

            public decimal? GetCardBalance(string cardNumber)
            {
                return Cards.ContainsKey(cardNumber) ? Cards[cardNumber].Balance : (decimal?)null;
            }

            private static string GenerateCardNumber()
            {
                // Implement card number generation logic
                Random random = new Random();

                // Generate two random 7-digit numbers
                long part1 = (long)random.Next(1000000, 10000000); 
                long part2 = (long)random.Next(100000, 1000000);  

                // Combine them to form a 15-digit number
                long cardNumber = (part1 * 1000000) + part2;

                // Return the number formatted as a 15-digit string
                return cardNumber.ToString("D15");
            }
        }
    }

