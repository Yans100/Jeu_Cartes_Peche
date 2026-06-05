using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1_1035.Classes
{
    internal class Regles
    {
        private Random random = new Random();

        // Choisit un joueur aléatoire pour commencer
        public int ChoisirPremierJoueur(int nombreJoueurs)
        {
            int index = random.Next(nombreJoueurs);
            return index;
        }

        // Vérifie si une carte peut être jouée selon les règles
        public bool PeutJouerCarte(Carte carte, Carte carteDuDessus, Couleur couleurCourante)
        {
            return carte.GetCouleur() == couleurCourante ||
                   carte.GetValeur() == carteDuDessus.GetValeur() ||
                   carte.GetValeur() == Valeur.Valet; // Le valet peut être joué à tout moment sauf sur un 7
        }

        // Méthode pour changer la couleur quand un Valet est joué
        public Couleur ChangerCouleur(Joueur joueur)
        {
            Console.WriteLine($"{joueur.GetNom()}, choisissez une nouvelle couleur :");

            return joueur.ChoisirCouleur();
        }

        // Gère les effets des cartes spéciales
        public void AppliquerEffetCarteSpeciale(Carte carte, JeuxDePeche jeu)
        {
            jeu.DefinirCouleurCourante(carte.GetCouleur());

            switch (carte.GetValeur())
            {
                case Valeur.As:
                    Console.WriteLine("Le tour du joueur suivant est sauté !");
                    jeu.PasserAuJoueurSuivant(); // Le joueur suivant voit son tour sauté
                    break;
                case Valeur.Dix:
                    Console.WriteLine("La direction du jeu est inversée !");
                    jeu.ChangerDirection();
                    break;
                case Valeur.Sept:
                    Console.WriteLine("Le joueur suivant doit piocher 2 cartes !");
                    Joueur joueurSuivant = jeu.ObtenirJoueurSuivant();
                    jeu.DistribuerCartes(joueurSuivant, 2);
                    jeu.PasserAuJoueurSuivant(); // Le joueur suivant voit son tour sauté
                    break;
                case Valeur.Valet:
                    Console.WriteLine("Le joueur peut changer la couleur !");
                    Couleur nouvelleCouleur = ChangerCouleur(jeu.GetJoueurActuel());
                    jeu.DefinirCouleurCourante(nouvelleCouleur);
                    Console.WriteLine($"La nouvelle couleur est {nouvelleCouleur}.");
                    break;
                    
            }
        }

        // Détermine si la partie est terminée
        public bool EstPartieTerminee(List<Joueur> joueurs)
        {
            // Exemple : vérifier si l'un des joueurs a vidé sa main
            foreach (var joueur in joueurs)
            {
                if (joueur.NombreDeCartes() == 0)
                {
                    Console.WriteLine($"{joueur.GetNom()} a gagné la partie !");
                    return true; // La partie est terminée
                }
            }
            return false; // La partie continue
        }

        // Détermine la valeur des cartes pour le calcul des points
        public static int ObtenirValeurCarte(Carte carte)
        {
            // Les valeurs numériques des cartes sont directement leurs points
            switch (carte.GetValeur())
            {
                case Valeur.As:
                    return 1;
                case Valeur.Deux:
                    return 2;
                case Valeur.Trois:
                    return 3;
                case Valeur.Quatre:
                    return 4;
                case Valeur.Cinq:
                    return 5;
                case Valeur.Six:
                    return 6;
                case Valeur.Sept:
                    return 7;
                case Valeur.Huit:
                    return 8;
                case Valeur.Neuf:
                    return 9;
                case Valeur.Dix:
                case Valeur.Valet:
                case Valeur.Reine:
                case Valeur.Roi:
                    return 10;
                default:
                    return 0;
            }
        }
    }
}