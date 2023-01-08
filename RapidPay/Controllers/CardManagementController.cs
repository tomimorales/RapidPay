using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Data.Models;
using RapidPay.Data.Repositories;
using System.Threading.Tasks;

namespace RapidPay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardManagementController : ControllerBase
    {
        private readonly ICardRepository _cardRepository;
        public CardManagementController(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }


        [HttpPost]
        [Route("CreateCardAsync")]
        public async Task<IActionResult> CreateCardAsync([FromBody] Card card )
        {
            if (card != null && !IsValidCardNumber(card.Number))
            {
                return BadRequest("Card Number is not valid. It must be a positive 15 digit number.");
            }

            //Check if card exists before creating

            if (await _cardRepository.GetCard(card.Number) != null)
            {
                return BadRequest("Card Number already exists.");
            }

            //Create card
            await _cardRepository.CreateCard(card.Number);

            return new ObjectResult(card) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Added Authorize Policy for Admins so only Admin users can Pay. For testing purposes.
        /// </summary>
        /// <param name="payModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = JwtPolicies.Admin)]
        [Route("PayAsync")]
        public async Task<IActionResult> PayAsync([FromBody] PayModel payModel)
        {
            if (payModel != null && !IsValidCardNumber(payModel.CardNumber))
            {
                return BadRequest("Card Number is not valid. It must be a positive 15 digit number.");
            }

            if (payModel != null && payModel.Amount <= 0)
            {
                return BadRequest("Pay Amount is not valid. It must be a positive value.");
            }

            //Check if card exists
            var card = await _cardRepository.GetCard(payModel.CardNumber);

            if (card == null)
            {
                return BadRequest("Card Number does not exist.");
            }

            //Make Payment
            var updatedCard = await _cardRepository.Pay(payModel);

            return Ok(updatedCard);
        }

        [HttpGet]
        [Route("GetBalanceAsync")]
        public async Task<ActionResult> GetBalanceAsync(long cardNumber)
        {
            if (!IsValidCardNumber(cardNumber))
            {
                return BadRequest("Card Number is not valid. It must be a positive 15 digit number.");
            }

            //Check if card exists
            var card = await _cardRepository.GetCard(cardNumber);

            if (card == null)
            {
                return BadRequest("Card Number does not exist.");
            }

            return Ok(card.Balance);
        }

        private bool IsValidCardNumber(long number)
        {
            if (number <= 0 ||
                number.ToString().Length != 15)
            {
                return false;
            }

            return true;
        }
    }
}
