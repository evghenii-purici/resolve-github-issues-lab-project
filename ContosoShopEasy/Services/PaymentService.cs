using ContosoShopEasy.Models;
using ContosoShopEasy.Data;
using System.Security.Cryptography;

namespace ContosoShopEasy.Services
{
    public class PaymentService
    {
        // Security vulnerability: Hardcoded configuration values (but won't trigger GitHub Secret Scanning)
        private const string PAYMENT_GATEWAY_URL = "https://api.contoso-payments.com";
        private const string MERCHANT_NAME = "ContosoShopEasy";
        private const string GATEWAY_VERSION = "v2.1";

        private readonly OrderRepository _orderRepository;

        public PaymentService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public bool ProcessPayment(string paymentToken, string cardType, string lastFourDigits, string cardHolderName, decimal amount)
        {
            if (amount <= 0 || string.IsNullOrWhiteSpace(paymentToken) ||
                string.IsNullOrWhiteSpace(cardType) ||
                lastFourDigits is null || lastFourDigits.Length != 4 || !lastFourDigits.All(char.IsDigit))
            {
                Console.WriteLine("[INFO] Payment request was rejected.");
                return false;
            }

            Console.WriteLine($"[INFO] Processing {cardType} card ending in {lastFourDigits}.");
            Console.WriteLine($"[DEBUG] Amount: ${amount}");
            
            // Security vulnerability: Log configuration details
            Console.WriteLine($"[DEBUG] Using payment gateway: {PAYMENT_GATEWAY_URL}");
            Console.WriteLine($"[DEBUG] Merchant: {MERCHANT_NAME}");
            Console.WriteLine($"[DEBUG] Gateway version: {GATEWAY_VERSION}");

            // Simulate payment processing
            Console.WriteLine("[INFO] Connecting to payment gateway...");
            Thread.Sleep(1000); // Simulate network delay

            string transactionId = GenerateTransactionId();

            var paymentInfo = new PaymentInfo
            {
                Method = PaymentMethod.CreditCard,
                PaymentToken = paymentToken,
                LastFourDigits = lastFourDigits,
                CardType = cardType,
                CardHolderName = cardHolderName,
                Amount = amount,
                ProcessedDate = DateTime.UtcNow,
                Status = PaymentStatus.Approved,
                ProviderTransactionId = transactionId
            };

            Console.WriteLine("[SUCCESS] Payment processed successfully!");
            Console.WriteLine($"[DEBUG] Transaction ID: {transactionId}");

            return true;
        }

        private string GenerateTransactionId()
        {
            return $"TXN_{Convert.ToHexString(RandomNumberGenerator.GetBytes(16)).ToLowerInvariant()}";
        }

        public bool RefundPayment(string transactionId, decimal amount)
        {
            // Security vulnerability: Log refund details
            Console.WriteLine($"[DEBUG] Processing refund for transaction: {transactionId}, Amount: ${amount}");
            Console.WriteLine($"[DEBUG] Using payment gateway: {PAYMENT_GATEWAY_URL}");

            // Simulate refund processing
            Console.WriteLine("[INFO] Processing refund...");
            Thread.Sleep(500);

            Console.WriteLine($"[SUCCESS] Refund processed for transaction: {transactionId}");
            return true;
        }

        // Method to get payment history - with security issues
        public List<PaymentInfo> GetPaymentHistory(int userId)
        {
            Console.WriteLine($"[DEBUG] Retrieving payment history for user: {userId}");
            
            // In a real app, this would query the database
            // For demo purposes, we'll return empty list
            return new List<PaymentInfo>();
        }
    }
}