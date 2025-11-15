using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BanqueApp
{
    // =========================
    // Interfaces
    // =========================

    // IAccount : lecture sur Balance + méthodes Deposit / Withdraw
    public interface IAccount
    {
        decimal Balance { get; }

        void Deposit(decimal amount);
        void Withdraw(decimal amount);
    }

    // IBankAccount : mêmes fonctionnalités que IAccount
    // + ApplyInterest, Owner (lecture), Number (lecture)
    public interface IBankAccount : IAccount
    {
        string Owner { get; }
        string Number { get; }

        void ApplyInterest();
    }

    // =========================
    // Programme Console
    // =========================

    class Program
    {
        static bool TryReadDecimal(out decimal value)
        {
            value = 0;
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return false;

            input = input.Trim().Replace(',', '.');
            return decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }

        static void Main()
        {
            var bank = new Bank();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n--- MENU BANQUE ---");
                Console.WriteLine("1. Créer un compte");
                Console.WriteLine("2. Afficher les comptes");
                Console.WriteLine("3. Dépôt");
                Console.WriteLine("4. Retrait");
                Console.WriteLine("5. Virement");
                Console.WriteLine("6. Quitter");
                Console.Write("Choix : ");
                string choice = Console.ReadLine()?.Trim() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        Console.Write("Numéro de compte : ");
                        string number = Console.ReadLine()?.Trim() ?? string.Empty;
                        Console.Write("Titulaire : ");
                        string owner = Console.ReadLine()?.Trim() ?? string.Empty;
                        Console.Write("Type (Courant/Épargne) : ");
                        string type = Console.ReadLine()?.Trim() ?? "Courant";
                        bank.CreateAccount(number, owner, type);
                        break;

                    case "2":
                        bank.ShowAccounts();
                        break;

                    case "3":
                        Console.Write("Numéro de compte : ");
                        var numDeposit = Console.ReadLine()?.Trim() ?? string.Empty;
                        var accDeposit = bank.FindAccount(numDeposit);
                        if (accDeposit != null)
                        {
                            Console.Write("Montant à déposer : ");
                            if (TryReadDecimal(out decimal amountDeposit))
                            {
                                try
                                {
                                    accDeposit.Deposit(amountDeposit);
                                }
                                catch (ArgumentOutOfRangeException ex)
                                {
                                    Console.WriteLine("Erreur : " + ex.Message);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Montant invalide.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Compte introuvable.");
                        }
                        break;

                    case "4":
                        Console.Write("Numéro de compte : ");
                        var numWithdraw = Console.ReadLine()?.Trim() ?? string.Empty;
                        var accWithdraw = bank.FindAccount(numWithdraw);
                        if (accWithdraw != null)
                        {
                            Console.Write("Montant à retirer : ");
                            if (TryReadDecimal(out decimal amountWithdraw))
                            {
                                try
                                {
                                    accWithdraw.Withdraw(amountWithdraw);
                                }
                                catch (ArgumentOutOfRangeException ex)
                                {
                                    Console.WriteLine("Erreur : " + ex.Message);
                                }
                                catch (InsufficientBalanceException ex)
                                {
                                    Console.WriteLine("Erreur : " + ex.Message);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Montant invalide.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Compte introuvable.");
                        }
                        break;

                    case "5":
                        Console.Write("Compte source : ");
                        string src = Console.ReadLine()?.Trim() ?? string.Empty;
                        Console.Write("Compte destination : ");
                        string dest = Console.ReadLine()?.Trim() ?? string.Empty;
                        Console.Write("Montant : ");
                        if (TryReadDecimal(out decimal amountTransfer))
                        {
                            bank.Transfer(src, dest, amountTransfer);
                        }
                        else
                        {
                            Console.WriteLine("Montant invalide.");
                        }
                        break;

                    case "6":
                        running = false;
                        Console.WriteLine("Merci d’avoir utilisé l’application bancaire.");
                        break;

                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
            }
        }
    }
}
