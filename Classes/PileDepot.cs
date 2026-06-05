using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1_1035.Classes
{
    internal class PileDepot
    {
        private List<Carte> Depot;
        private Observer Observateur; 
        private JeuxDePeche _jeu;
        public PileDepot(Observer observateur, JeuxDePeche jeu)
        {
            Depot = new List<Carte>();
            Observateur = observateur;
            _jeu = jeu;
        }

        // Obtenir en temps réel la dernière carte sur pile dépôt lors de la partie
        public JeuxDePeche GetJeu()
        {
            return _jeu;
        }

        // Retourne carte sur le dessus pour implémenter une méthode dans joueur.cs 
        public Carte GetCarteDuDessus()
        {
            //return Depot.Count > 0 ? Depot[Depot.Count - 1] : null;
            return Depot[Depot.Count - 1];
        }

        // Déposer une carte sur la pile de dépôt
        public bool DeposerCarte(Carte carte, Couleur couleurCourante)
        {
            // Si la pile est vide, on peut déposer n'importe quelle carte
            if (Depot.Count == 0)
            {
                Depot.Add(carte);
                NotifierObservateur($"La carte {carte} a été déposée sur la pile de dépôt.");
                return true;
            }

            Carte carteDuDessus = Depot[Depot.Count - 1];

            // Vérifier les règles pour savoir si la carte peut être déposée
            if (PeutDeposerCarte(carte, carteDuDessus, couleurCourante))
            {
                Depot.Add(carte);
                NotifierObservateur($"La carte {carte} a été déposée sur la pile de dépôt.");
                return true;
            }
            else
            {
                Console.WriteLine($"La carte {carte} ne peut pas être déposée sur {carteDuDessus}.");
                return false;
            }
        }

        // Vérifie si la carte peut être déposée sur la carte du dessus
        private bool PeutDeposerCarte(Carte carte, Carte carteDuDessus, Couleur couleurCourante)
        {
            // Règle : le Valet peut être joué sur n'importe quelle carte sauf un 7
            if (carte.GetValeur() == Valeur.Valet && carteDuDessus.GetValeur() != Valeur.Sept)
            {
                return true;
            }

            // Sinon, la carte doit correspondre par la couleur ou la valeur
            return carte.GetCouleur() == couleurCourante || carte.GetValeur() == carteDuDessus.GetValeur();
        }

        // Méthode pour mélanger le dépôt et transférer toutes les cartes sauf la dernière
        public List<Carte> MelangerEtTransférerCartes()
        {
            if (Depot.Count <= 1)
            {
                Console.WriteLine("Pas assez de cartes dans le dépôt pour transférer.");
                return new List<Carte>();
            }

            // Mélanger les cartes sauf la dernière
            Carte derniereCarte = Depot[Depot.Count - 1];
            Depot.RemoveAt(Depot.Count - 1);
            MelangerDepot();

            // Transférer les cartes mélangées
            List<Carte> cartesTransferees = new List<Carte>(Depot);
            Depot.Clear();

            // Remettre la dernière carte sur le dépôt
            Depot.Add(derniereCarte);

            NotifierObservateur("Les cartes ont été mélangées et transférées à la pile de pioche.");
            return cartesTransferees;
        }

        // Mélanger les cartes du dépôt
        private void MelangerDepot()
        {
            Random rng = new Random();
            int n = Depot.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Carte temp = Depot[k];
                Depot[k] = Depot[n];
                Depot[n] = temp;
            }
        }

        // Permet d'obtenir le nombre de cartes dans la pile de dépôt
        public int NombreDeCartes => Depot.Count;

        // Notifie l'observateur de l'action
        private void NotifierObservateur(string message)
        {
            Observateur?.NotifierObservateurs(message);
        }

        //Initialiser la pile de dépot
        public void InitialiserDepot(List<Carte> cartes)
        {
            Depot.AddRange(cartes);
        }
    }
}