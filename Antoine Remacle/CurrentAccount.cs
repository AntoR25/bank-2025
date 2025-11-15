namespace BanqueApp
{
    // =========================
    // CurrentAccount (Compte courant)
    // =========================

    public class CurrentAccount : Account
    {
        private decimal _creditLine;

        // Propriété CreditLine avec exception si < 0
        public decimal CreditLine
        {
            get => _creditLine;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "La ligne de crédit doit être supérieure ou égale à 0.");
                _creditLine = value;
            }
        }

        // Constructeur numéro + titulaire
        public CurrentAccount(string number, Person holder)
            : base(number, holder, "Courant")
        {
            CreditLine = 0m;
        }

        // Constructeur numéro + titulaire + solde
        public CurrentAccount(string number, Person holder, decimal balance)
            : base(number, holder, "Courant", balance)
        {
            CreditLine = 0m;
        }

        // 3% si solde positif, sinon 9,75%
        protected override double GetInterestRate()
        {
            return Balance > 0m ? 0.03 : 0.0975;
        }

        // la ligne de crédit :
        // décommente et remplace celle de la classe de base
        
        public new void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount),
                    "Le montant du retrait doit être strictement supérieur à zéro.");

            // Autorise le solde jusqu'à -CreditLine
            if (Balance - amount < -CreditLine)
                throw new InsufficientBalanceException(
                    "Fonds insuffisants (limite de crédit dépassée).",
                    Balance,
                    amount);

            // On modifie Balance via la propriété (setter privé mais accessible ici)
            typeof(Account)
                .GetProperty("Balance", System.Reflection.BindingFlags.Instance |
                                         System.Reflection.BindingFlags.NonPublic |
                                         System.Reflection.BindingFlags.Public)
                ?.SetValue(this, Balance - amount);

            Console.WriteLine($"Retrait de {amount}€ effectué (ligne de crédit utilisée). Nouveau solde : {Balance}€.");
        }
    }
}
