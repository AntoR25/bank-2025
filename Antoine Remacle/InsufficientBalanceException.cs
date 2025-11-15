namespace BanqueApp
{
    // =========================
    // Exceptions
    // =========================

    public class InsufficientBalanceException : Exception
    {
        public decimal CurrentBalance { get; }
        public decimal AttemptedAmount { get; }

        public InsufficientBalanceException(string message, decimal currentBalance, decimal attemptedAmount)
            : base(message)
        {
            CurrentBalance = currentBalance;
            AttemptedAmount = attemptedAmount;
        }
    }
}
