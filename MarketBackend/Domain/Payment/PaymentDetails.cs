using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Payment
{
    public class PaymentDetails
    {
        String cardNumber;
        String exprYear;
        String exprMonth;
        String cvv;
        String holderID; 
        String holderName; 


        public PaymentDetails (String cardNumber, String exprYear, String exprMonth, String cvv, String cardId, String name)
        {
            this.cardNumber = cardNumber;
            this.exprMonth =  exprMonth;
            this.exprYear = exprYear;
            this. cvv = cvv;
            this.holderID = cardId;
            this.holderName = name;
        }

        public String CardNumber {get => cardNumber; set => cardNumber = value; }
        public String ExprMonth {get => exprMonth; set => exprMonth = value; }
        public String ExprYear {get => exprYear; set => exprYear = value; }
        public String Cvv {get => cvv; set => cvv = value; }
        public String HolderID {get => holderID; set => holderID = value; }
        public String HolderName {get => holderName; set => holderName = value; }


    }

}