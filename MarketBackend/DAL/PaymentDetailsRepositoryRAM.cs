using System;
using System.Collections.Generic;
using System.Linq;
using MarketBackend.Domain.Payment;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.DAL
{
    public class PaymentDetailsRepositoryRAM //: IPaymentDetailsRepository TODO: need to think about because IPaymentDetailsRepository.getByID(***int***) needts to be implmemted but cant 
    {
        private readonly Dictionary<string, PaymentDetails> payments;

        public PaymentDetailsRepositoryRAM()
        {
            payments = new Dictionary<string, PaymentDetails>();
        }

        public void Add(PaymentDetails entity)
        {
            if(payments.ContainsKey(entity.CardNumber)){
                throw new ArgumentException($"Payment Details with the card number: {entity.CardNumber} already exists.");

            }
            payments.Add(entity.CardNumber, entity);
        }

        public void Delete(PaymentDetails entity)
        {
            if (!payments.ContainsKey(entity.CardNumber)){
                throw new KeyNotFoundException($"Payment Details with the card number: {entity.CardNumber} does not exist.");
            }

            payments.Remove(entity.CardNumber);
        }

        public IEnumerable<PaymentDetails> getAll()
        {
            return payments.Values.ToList();
        }

        public PaymentDetails getByID(string card)
        {
            bool exist = payments.ContainsKey(card);
            if (exist)
            {
                return payments[card];
            }
            return null;
        }


        public void Update(PaymentDetails entity)
        {
            if (payments.ContainsKey(entity.CardNumber))
            {
                payments[entity.CardNumber] = entity;
            }
            else
            {
                throw new KeyNotFoundException($"Payment Details with the card number: {entity.CardNumber} not found.");
            }
        }
    }
}

