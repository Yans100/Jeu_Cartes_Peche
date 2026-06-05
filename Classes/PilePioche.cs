using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1_1035.Classes
{
    internal class PilePioche
    {
        private List<Carte> Pioche;
        private Observer Observateur;

        public PilePioche(Observer observateur)
        {
            Pioche = new List<Carte>();
            Observateur = observateur;
        }

        public List<Carte> GetPioche()
        {
            return Pioche;
        }

        // Piger une ou plusieurs cartes de la pile de pioche
        public List<Carte> PigerCartes(int quantite, PileDepot pileDepot)
        {
            List<Carte> cartesPiochees = new List<Carte>();

            for (int i = 0; i < quantite; i++)
            {
                if (Pioche.Count == 0)
                {
                    // Si la pile de pioche est vide, la pile de dépôt mélangée devient la nouvelle pile de pioche
                    Console.WriteLine("La pile de pioche est vide. Mélange de la pile de dépôt.");
                    TransfertDepotVersPioche(pileDepot);
                }

                if (Pioche.Count > 0)
                {
                    Carte carte = Pioche[0];
                    Pioche.RemoveAt(0);
                    cartesPiochees.Add(carte);
                }
                else
                {
                    Console.WriteLine("Il n'y a plus de cartes disponibles.");
                    break;
                }
            }

            // Notifier l'observateur du nombre de cartes piochées
            NotifierObservateur($"{cartesPiochees.Count} carte(s) ont été piochée(s).");
            return cartesPiochees;
        }

        // Transférer les cartes de la pile de dépôt à la pile de pioche
        public void TransfertDepotVersPioche(PileDepot pileDepot)
        {
            if (pileDepot.NombreDeCartes <= 1)
            {
                Console.WriteLine("Pas assez de cartes dans la pile de dépôt pour transférer.");
                return;
            }

            // Utiliser la méthode de la classe PileDepot pour transférer les cartes
            List<Carte> cartesDuDepot = pileDepot.MelangerEtTransférerCartes();

            // Ajouter toutes les cartes transférées à la pile de pioche
            Pioche.AddRange(cartesDuDepot);

            // Notifier tous les observateurs que les cartes ont été transférées
            NotifierObservateur("Les cartes ont été transférées de la pile de dépôt à la pile de pioche.");
        }

        // Obtenir le nombre de cartes restantes dans la pile de pioche
        public int NombreDeCartes => Pioche.Count;

        // Notifier tous les observateurs
        private void NotifierObservateur(string message)
        {
            Observateur.NotifierObservateurs(message);
        }
    }
}