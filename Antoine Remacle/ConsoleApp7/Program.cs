using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace BanqueApp
{
    // Classe de base abstraite : contient maintenant la méthode protégée 
    public abstract class CompteBancaire
    {
        public string Numero { get; }
        public string Titulaire { get; }
        public decimal Solde { get; private set; }
        public string Type { get; }

        protected CompteBancaire(string numero, string titulaire, string type)
        {
            Numero = numero;
            Titulaire = titulaire;
            Type = type;
            Solde = 0;
        }

        public void Depot(decimal montant)
        {
            Solde += montant;
            Console.WriteLine($"Dépôt de {montant}€ effectué.");
        }

        public bool Retrait(decimal montant)
        {
            if (montant <= Solde)
            {
                Solde -= montant;
                Console.WriteLine($"Retrait de {montant}€ effectué.");
                return true;
            }
            Console.WriteLine("Fonds insuffisants.");
            return false;
        }

        // Méthode abstraite protégée demandée
        protected abstract double CalculInterets();

        public override string ToString()
        {
            return $"[{Type}] {Numero} - {Titulaire} : {Solde}€";
        }
    }

    // Livret d'épargne : taux fixe 4.5%
    public class CompteEpargne : CompteBancaire
    {
        public CompteEpargne(string numero, string titulaire)
            : base(numero, titulaire, "Épargne")
        {
        }

        protected override double CalculInterets()
        {
            return 0.045;
        }
    }

    // Compte courant : 3% si solde positif, sinon 9.75%
    public class CompteCourant : CompteBancaire
    {
        public CompteCourant(string numero, string titulaire)
            : base(numero, titulaire, "Courant")
        {
        }

        protected override double CalculInterets()
        {
            return Solde > 0m ? 0.03 : 0.0975;
        }
    }

    public class Banque
    {
        private List<CompteBancaire> comptes = new List<CompteBancaire>();

        public void CreerCompte(string numero, string titulaire, string type)
        {
            if (string.IsNullOrWhiteSpace(numero) || string.IsNullOrWhiteSpace(titulaire))
            {
                Console.WriteLine("Numéro et titulaire obligatoires.");
                return;
            }

            if (RechercherCompte(numero) != null)
            {
                Console.WriteLine("Ce numéro de compte existe déjà.");
                return;
            }

            // Instancier la classe concrète appropriée selon le type
            CompteBancaire nouveau;
            if (!string.IsNullOrWhiteSpace(type) &&
                (type.Trim().Equals("épargne", StringComparison.OrdinalIgnoreCase) ||
                 type.Trim().Equals("epargne", StringComparison.OrdinalIgnoreCase)))
            {
                nouveau = new CompteEpargne(numero, titulaire);
            }
            else
            {
                // Par défaut compte courant 
                nouveau = new CompteCourant(numero, titulaire);
            }

            comptes.Add(nouveau);
            Console.WriteLine("Compte créé avec succès.");
        }

        public CompteBancaire? RechercherCompte(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero)) return null;
            return comptes.FirstOrDefault(c => c.Numero == numero);
        }

        public void AfficherComptes()
        {
            if (comptes.Count == 0)
            {
                Console.WriteLine("Aucun compte enregistré.");
                return;
            }
            Console.WriteLine("Liste des comptes :");
            foreach (var compte in comptes)
            {
                Console.WriteLine(compte);
            }
        }

        public bool Virement(string source, string destination, decimal montant)
        {
            var compteSource = RechercherCompte(source);
            var compteDest = RechercherCompte(destination);

            if (compteSource != null && compteDest != null && compteSource.Retrait(montant))
            {
                compteDest.Depot(montant);
                Console.WriteLine("Virement effectué.");
                return true;
            }
            Console.WriteLine("Virement échoué.");
            return false;
        }
    }

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
            Banque banque = new Banque();
            bool continuer = true;

            while (continuer)
            {
                Console.WriteLine("\n--- MENU BANQUE ---");
                Console.WriteLine("1. Créer un compte");
                Console.WriteLine("2. Afficher les comptes");
                Console.WriteLine("3. Dépôt");
                Console.WriteLine("4. Retrait");
                Console.WriteLine("5. Virement");
                Console.WriteLine("6. Quitter");
                Console.Write("Choix : ");
                string choix = Console.ReadLine()?.Trim() ?? string.Empty;

                switch (choix)
                {
                    case "1":
                        Console.Write("Numéro de compte : ");
                        string numero = Console.ReadLine()?.Trim() ?? string.Empty;
                        Console.Write("Titulaire : ");
                        string titulaire = Console.ReadLine()?.Trim() ?? string.Empty;
                        Console.Write("Type (Courant/Épargne) : ");
                        string type = Console.ReadLine()?.Trim() ?? "Courant";
                        banque.CreerCompte(numero, titulaire, type);
                        break;
                    case "2":
                        banque.AfficherComptes();
                        break;

                    case "3":
                        Console.Write("Numéro de compte : ");
                        var numDepot = Console.ReadLine()?.Trim() ?? string.Empty;
                        var cDepot = banque.RechercherCompte(numDepot);
                        if (cDepot != null)
                        {
                            Console.Write("Montant à déposer : ");
                            if (TryReadDecimal(out decimal montantDepot))
                                cDepot.Depot(montantDepot);
                            else
                                Console.WriteLine("Montant invalide.");
                        }
                        else
                        {
                            Console.WriteLine("Compte introuvable.");
                        }
                        break;

                    case "4":
                        Console.Write("Numéro de compte : ");
                        var cRetrait = banque.RechercherCompte(Console.ReadLine()?.Trim() ?? string.Empty);
                        if (cRetrait != null)
                        {
                            Console.Write("Montant à retirer : ");
                            if (TryReadDecimal(out decimal montantRetrait))
                                cRetrait.Retrait(montantRetrait);
                            else
                                Console.WriteLine("Montant invalide.");
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
                        if (TryReadDecimal(out decimal montantVirement))
                            banque.Virement(src, dest, montantVirement);
                        else
                            Console.WriteLine("Montant invalide.");
                        break;

                    case "6":
                        continuer = false;
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



