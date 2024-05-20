using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Payment
{
    public class PaymentDetails
    {
        private static int counterIDs = 0;
        private static readonly object _lock = new object();

        private int _paymentID;
        private String _cardNumber;
        private String _exprYear;
        private String _exprMonth;
        private String _cvv;
        private String _holderID; 
        private String _holderName; 


        public PaymentDetails (String cardNumber, String exprYear, String exprMonth, String cvv, String cardId, String name)
        {
            this._paymentID = GeneratePaymentID();
            this._cardNumber = cardNumber;
            this._exprMonth =  exprMonth;
            this._exprYear = exprYear;
            this._cvv = cvv;
            this._holderID = cardId;
            this._holderName = name;
        }

        private static int GeneratePaymentID()
        {
            lock (_lock)
            {
                return ++counterIDs;
            }
        }

        public int PaymentID {get => _paymentID; }
        public String CardNumber {get => _cardNumber; set => _cardNumber = value; }
        public String ExprMonth {get => _exprMonth; set => _exprMonth = value; }
        public String ExprYear {get => _exprYear; set => _exprYear = value; }
        public String Cvv {get => _cvv; set => _cvv = value; }
        public String HolderID {get => _holderID; set => _holderID = value; }
        public String HolderName {get => _holderName; set => _holderName = value; }


    }

}