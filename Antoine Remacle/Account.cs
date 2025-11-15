namespace BanqueApp
{
    // =========================
    // Classe de base Account
    // =========================

    public abstract class Account : IBankAccount
    {
        public string Number { get; private set; }
        public Person Holder { get; private set; }
        public decimal Balance { get; private set; }
        public string Type { get; private set; }

        // Propriété Owner (lecture seule) pour IBankAccount
        public string Owner => Holder.ToString();

        // Constructeur : numéro + titulaire
        protected Account(string number, Person holder, string type)
        {
            Number = number ?? throw new ArgumentNullException(nameof(number));
            Holder = holder ?? throw new ArgumentNullException(nameof(holder));
            Type = type ?? "Inconnu";
            Balance = 0m;
        }

        // Constructeur : numéro + titulaire + solde (cas base de données)
        protected Account(string number, Person holder, string type, decimal balance)
        {
            Number = number ?? throw new ArgumentNullException(nameof(number));
            Holder = holder ?? throw new ArgumentNullException(nameof(holder));
            Type = type ?? "Inconnu";
            Balance = balance;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount),
                    "Le montant du dépôt doit être strictement supérieur à zéro.");

            Balance += amount;
            Console.WriteLine($"Dépôt de {amount}€ effectué. Nouveau solde : {Balance}€.");
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount),
                    "Le montant du retrait doit être strictement supérieur à zéro.");

            if (amount > Balance)
                throw new InsufficientBalanceException(
                    "Fonds insuffisants pour effectuer le retrait.",
                    Balance,
                    amount);

            Balance -= amount;
            Console.WriteLine($"Retrait de {amount}€ effectué. Nouveau solde : {Balance}€.");
        }

        // Méthode abstraite : taux d'intérêt à appliquer
        protected abstract double GetInterestRate();

        // Méthode ApplyInterest de IBankAccount
        public void ApplyInterest()
        {
            double rate = GetInterestRate();
            if (rate <= 0) return;

            decimal interest = Balance * (decimal)rate;
            Balance += interest;
            Console.WriteLine($"Intérêts appliqués ({rate:P}) : +{interest}€. Nouveau solde : {Balance}€.");
        }

        public override string ToString()
        {
            return $"[{Type}] {Number} - {Owner} : {Balance}€";
        }
    }
}
