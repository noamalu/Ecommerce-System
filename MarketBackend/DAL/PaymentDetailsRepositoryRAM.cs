using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MarketBackend.Domain.Payment;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.DAL
{
    public class PaymentDetailsRepositoryRAM : IPaymentDetailsRepository
    {
        // Using a Tuple<int, string, string> as the composite key (paymentID, cardNumber, holderID)
        private readonly ConcurrentDictionary<(int paymentID, string cardNumber, string holderID), PaymentDetails> payments;

        private static PaymentDetailsRepositoryRAM _paymentRepository = null;

        public PaymentDetailsRepositoryRAM()
        {
            payments = new ConcurrentDictionary<(int, string, string), PaymentDetails>();
        }

        public static PaymentDetailsRepositoryRAM GetInstance()
        {
            _paymentRepository ??= new PaymentDetailsRepositoryRAM();
            return _paymentRepository;
        }

        public void Add(PaymentDetails entity)
        {
            var key = (entity.PaymentID, entity.CardNumber, entity.HolderID);
            if (payments.ContainsKey(key))
            {
                throw new ArgumentException($"Payment Details with the payment ID: {entity.PaymentID}, card number: {entity.CardNumber} and holder ID: {entity.HolderID} already exists.");
            }
            payments.TryAdd(key, entity);
        }

        public void Delete(PaymentDetails entity)
        {
            var key = (entity.PaymentID, entity.CardNumber, entity.HolderID);
            if (!payments.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Payment Details with the payment ID: {entity.PaymentID}, card number: {entity.CardNumber} and holder ID: {entity.HolderID} does not exist.");
            }

            payments.TryRemove(key, out _);
        }

        public IEnumerable<PaymentDetails> getAll()
        {
            return payments.Values.ToList();
        }

        public PaymentDetails GetById(int paymentID, string cardNumber, string holderID)
        {
            var key = (paymentID, cardNumber, holderID);
            if (payments.TryGetValue(key, out var paymentDetails))
            {
                return paymentDetails;
            }
            return null;
        }

        public void Update(PaymentDetails entity)
        {
            var key = (entity.PaymentID, entity.CardNumber, entity.HolderID);
            if (payments.ContainsKey(key))
            {
                payments[key] = entity;
            }
            else
            {
                throw new KeyNotFoundException($"Payment Details with the payment ID: {entity.PaymentID}, card number: {entity.CardNumber} and holder ID: {entity.HolderID} not found.");
            }
        }

        // public List<PaymentDetails> GetById(int paymentID)
        // {
        //     List<PaymentDetails> result = new List<PaymentDetails>();
        //     foreach (var payment in payments.Values)
        //     {
        //         if (payment.PaymentID == paymentID)
        //         {
        //             result.Add(payment);
        //         }
        //     }   
        //     return result;
        // }

        public PaymentDetails GetById(int paymentID)
        {
            throw new NotImplementedException();
        }
    }
}
