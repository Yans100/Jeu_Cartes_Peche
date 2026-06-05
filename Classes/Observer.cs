namespace TP1_1035.Classes
{
    public class Observer : IObservateur
    {
        private List<IObservateur> Observateurs; // Liste des observateurs

        public Observer()
        {
            Observateurs = new List<IObservateur>(); // Initialise la liste des observateurs
        }

        // affiche le message de notification dans la console
        public void MiseAJour(string message)
        {
            Console.WriteLine($"[Notification] : {message}");
        }

        // Ajoute un observateur
        public void Attacher(IObservateur observateur)
        {
            if (!Observateurs.Contains(observateur))
            {
                Observateurs.Add(observateur);
            }
        }

        // Détache un observateur
        public void Detacher(IObservateur observateur)
        {
            if (Observateurs.Contains(observateur))
            {
                Observateurs.Remove(observateur);
            }
        }

        // Notifier tous les observateurs
        public void NotifierObservateurs(string message)
        {
            foreach (var observateur in Observateurs)
            {
                observateur.MiseAJour(message);
            }
        }
    }
}