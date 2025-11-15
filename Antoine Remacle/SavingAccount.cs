namespace BanqueApp
{
    // =========================
    // SavingAccount (Compte épargne)
    // =========================

    public class SavingAccount : Account
    {
        // Constructeur numéro + titulaire
        public SavingAccount(string number, Person holder)
            : base(number, holder, "Épargne")
        {
        }

        // Constructeur numéro + titulaire + solde
        public SavingAccount(string number, Person holder, decimal balance)
            : base(number, holder, "Épargne", balance)
        {
        }

        // Taux fixe 4,5%
        protected override double GetInterestRate()
        {
            return 0.045;
        }
    }
}
