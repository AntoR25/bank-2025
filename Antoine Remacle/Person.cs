namespace BanqueApp
{
    // =========================
    // Classe Person
    // =========================

    public class Person
    {
        public string Name { get; private set; }

        public Person(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override string ToString() => Name;
    }
}
