using RapidPay.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Data.Repositories
{
    public interface ICardRepository
    {
        Task<Card> GetCard(long cardNumber);
        Task CreateCard(long cardNumber);
        Task<Card> Pay(PayModel payModel);
    }
}
