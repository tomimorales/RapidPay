using RapidPay.Data.DbContexts;
using RapidPay.Data.Models;
using RapidPay.Fees.Services.Fees;
using RapidPay.Fees.Services.UEF;
using System.Threading.Tasks;

namespace RapidPay.Data.Repositories
{
    public class CardRepository : ICardRepository
    {
        private IRapidPayDbContext _dbContext;
        private IFeeCalculatorService _feeCalculator;
        public CardRepository(RapidPayDbContext rapidPayDbContext, IUFEService ufeService, IFeeCalculatorService feeCalculator)
        {
            _dbContext = rapidPayDbContext;
            _feeCalculator = feeCalculator;
        }

        public async Task CreateCard(long cardNumber)
        {
            var newCard = new Card { Number = cardNumber, Balance = 0 };
            _dbContext.Cards.Add(newCard);

            await _dbContext.SaveChanges();
        }

        public async Task<Card> GetCard(long cardNumber)
        {
            var card = await _dbContext.Cards.FindAsync(cardNumber);

            return card;
        }

        public async Task<Card> Pay(PayModel payModel)
        {
            var card = await GetCard(payModel.CardNumber);

            var fee = _feeCalculator.GetFee();

            card.Balance += payModel.Amount + fee;

            await _dbContext.SaveChanges();

            return card;
        }
    }
}
