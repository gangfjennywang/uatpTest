using Microsoft.AspNetCore.Mvc;
using NLog;


[ApiController]
[Route("api/Payment")]
public class PaymentsController : ControllerBase
{
    protected static Logger logger = LogManager.GetCurrentClassLogger();
    [HttpPost]
    public IActionResult CreatePayment([FromBody] PaymentRequest request)
    {
       
        if (request == null || request.Amount <= 0)
        {
            return BadRequest("Invalid payment amount.");
        }

        try
        {
            var feeMultiplier = UfeService.Instance.GetFeeMultiplier();
            var fee = request.Amount * feeMultiplier;

            var paymentResponse = new PaymentResponse
            {
                Amount = request.Amount,
                Fee = fee,
                Total = request.Amount + fee
            };
            return Ok(paymentResponse);
        }
        catch (Exception ex) 
        {
            logger.Error(ex.Message, "An error occurred during GetCardBalance process");
            throw;
        }
    }
}

public class PaymentRequest
{
    public decimal Amount { get; set; }
}

public class PaymentResponse
{
    public decimal Amount { get; set; }
    public decimal Fee { get; set; }
    public decimal Total { get; set; }
}
