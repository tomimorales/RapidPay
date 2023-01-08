using RapidPay.Fees.Services.UEF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RapidPay.Fees.Services.Fees
{
    public class FeeCalculatorService: IFeeCalculatorService
    {
        private static double Fee = 1;
        private static readonly object Lock = new object();
        private readonly IUFEService _iUFEService;

        public FeeCalculatorService(IUFEService ufeService)
        {
            _iUFEService = ufeService;
        }

        public double GetFee()
        {
            //Thread locking
            lock (Lock)
            {
                var newFee = Fee * _iUFEService.GetSingletonRandomValue();
                Fee = newFee;
            }

            return Fee;
        }       
    }
}
