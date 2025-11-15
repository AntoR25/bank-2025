namespace BanqueApp
{
    // =========================
    // Banque
    // =========================

    public class Bank
    {
        private readonly List<IBankAccount> _accounts = new List<IBankAccount>();

        public void CreateAccount(string number, string ownerName, string type)
        {
            if (string.IsNullOrWhiteSpace(number) || string.IsNullOrWhiteSpace(ownerName))
            {
                Console.WriteLine("Numéro et titulaire obligatoires.");
                return;
            }

            if (FindAccount(number) != null)
            {
                Console.WriteLine("Ce numéro de compte existe déjà.");
                return;
            }

            var person = new Person(ownerName.Trim());
            IBankAccount account;

            if (!string.IsNullOrWhiteSpace(type) &&
                (type.Trim().Equals("épargne", StringComparison.OrdinalIgnoreCase) ||
                 type.Trim().Equals("epargne", StringComparison.OrdinalIgnoreCase)))
            {
                account = new SavingAccount(number.Trim(), person);
            }
            else
            {
                account = new CurrentAccount(number.Trim(), person);
            }

            _accounts.Add(account);
            Console.WriteLine("Compte créé avec succès.");
        }

        public IBankAccount? FindAccount(string number)
        {
            if (string.IsNullOrWhiteSpace(number)) return null;
            return _accounts.FirstOrDefault(c => c.Number == number.Trim());
        }

        public void ShowAccounts()
        {
            if (_accounts.Count == 0)
            {
                Console.WriteLine("Aucun compte enregistré.");
                return;
            }

            Console.WriteLine("Liste des comptes :");
            foreach (var acc in _accounts)
            {
                Console.WriteLine(acc);
            }
        }

        public bool Transfer(string sourceNumber, string destNumber, decimal amount)
        {
            var src = FindAccount(sourceNumber);
            var dest = FindAccount(destNumber);

            if (src == null || dest == null)
            {
                Console.WriteLine("Compte source ou destination introuvable.");
                return false;
            }

            try
            {
                src.Withdraw(amount);
                dest.Deposit(amount);
                Console.WriteLine("Virement effectué.");
                return true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Erreur de montant : " + ex.Message);
            }
            catch (InsufficientBalanceException ex)
            {
                Console.WriteLine("Erreur de solde : " + ex.Message);
            }

            Console.WriteLine("Virement échoué.");
            return false;
        }
    }
}
