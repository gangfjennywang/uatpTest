using Microsoft.AspNetCore.Mvc;
using uatp.Models;
using NLog;

namespace uatp.Controllers
{
    [ApiController]
    [Route("api/Cards")]
    public class CardsController : ControllerBase
    {
        
        private readonly CardRepository _cardRepository;
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        public CardsController(CardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        [HttpPost("create")]
        public IActionResult CreateCard()
        {
            var card = _cardRepository.CreateCard();
            return Ok(card);
        }

        public class PaymentRequest
        {
            public string? CardNumber { get; set; }
            public decimal Amount { get; set; }
        }

        [HttpPost("pay")]
        public IActionResult Pay([FromBody] PaymentRequest request)
        {
            if (string.IsNullOrEmpty(request.CardNumber))
            {
                throw new ArgumentException("Card number cannot be null or empty", nameof(request.CardNumber));
            }
            var success = _cardRepository.Pay(request.CardNumber, request.Amount);
            return success ? (IActionResult)Ok() : BadRequest("Payment failed.");
        }

        [HttpGet("balance/{cardNumber}")]
        public IActionResult GetCardBalance(string cardNumber)
        {
            try
            {
                var balance = _cardRepository.GetCardBalance(cardNumber);
                return balance.HasValue ? (IActionResult)Ok(new { Balance = balance.Value }) : NotFound("Card not found.");
            }
            catch (Exception ex) 
            {
                logger.Error(ex, "An error occurred during GetCardBalance");
                throw;
            }
         }
    }
}
