using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TP1_1035.Classes
{
    internal class Joueur : Personne, ISubjectJoueur, IObservateurJoueur
    {
        internal List<Carte> _mains = new List<Carte>();
        private List<IObservateurJoueur> ObservateursJoueurs = new List<IObservateurJoueur>();
        internal List<int> JoueursDerniereCarte = new List<int>();

        public Joueur() {
            this.Nom = "";
            this.Prenom = "";
            this.Id = -1;
        }

        public Joueur(string pNom, string pPrenom, int pId)
        {
            this.Nom = pNom;
            this.Prenom = pPrenom;
            this.Id = pId;
        }

        public string GetNom()
        {
            return this.Nom;
        }

        public string GetPrenom()
        {
            return this.Prenom;
        }

        public int GetId()
        {
            return this.Id;
        }

        public void AjouterCarte(Carte carte)
        {
            _mains.Add(carte);
            Console.WriteLine($"{Nom} a ajouté la carte {carte} dans sa main.");
        }

        //Jouer une carte
        public void JouerCarte(Regles regles, JeuxDePeche jeu)
        {
            if (_mains.Count == 0)
            {
                return;
            }

            bool aJoue = false;

            //Attaque lorsqu'un joueur a seulement une carte
            if (JoueursDerniereCarte.Count > 0)
            {
                aJoue = JouerAttaque(regles, jeu);

                //Vider la liste des joueurs à attaquer
                JoueursDerniereCarte.Clear();
            }


            if (!aJoue)
            {
                Carte carteDuDessus = jeu.ObtenirCarteDuDessus();

                // Essaie toutes les cartes pour voir si le joueur peut jouer
                for (int i = _mains.Count - 1; i >= 0; i--)
                {
                    Carte carteAJouer = _mains[i];

                    // Application des règles avant de jouer une carte
                    if (regles.PeutJouerCarte(carteAJouer, carteDuDessus, jeu.GetCouleurCourante()))
                    {
                        if (jeu.JouerCarte(this, carteAJouer))
                        {
                            _mains.RemoveAt(i);
                            Console.WriteLine($"{Nom} a joué {carteAJouer}.");
                            regles.AppliquerEffetCarteSpeciale(carteAJouer, jeu); // Applique l'effet de la carte
                            aJoue = true;
                            if (_mains.Count == 0)
                            {
                                return;
                            }
                            else if(_mains.Count == 1)
                            {
                                Console.WriteLine($"ATTENTION! Joueur {Nom} n'a plus qu'une seule carte!");
                                NotifierJoueurs(this.Id);
                            }

                            break;
                        }
                    }
                }
            }

            // Pioche si le joueur ne peut jouer aucune carte 
            if (!aJoue)
            {
                //Console.WriteLine($"{Nom} doit piocher une carte.");
                PiocherCarte(jeu);
            }
        }

        //Jouer une carte attaquante
        internal bool JouerAttaque(Regles regles, JeuxDePeche jeu)
        {
            Carte carteDuDessus = jeu.ObtenirCarteDuDessus();

            //Vérifier si le joueur est en position pour attaquer le joueur gagnant
            int ProchainJoueur = jeu.GetNextPlayerIndex();
            bool PeutAttaquer = JoueursDerniereCarte.Any(p => p - 1 == ProchainJoueur);

            if (PeutAttaquer)
            {
                //Rechercher si le joueur possède une carte pour attaquer
                for (int i = _mains.Count - 1; i >= 0; i--)
                {
                    Carte carte = _mains[i];

                    Valeur ValeurCarte = carte.GetValeur();
                    if (ValeurCarte == Valeur.As || ValeurCarte == Valeur.Dix || ValeurCarte == Valeur.Sept || ValeurCarte == Valeur.Valet)
                    {
                        //Attaquer le joueur avec cette carte si elle peut être jouée
                        if (regles.PeutJouerCarte(carte, carteDuDessus, jeu.GetCouleurCourante()))
                        {
                            if (jeu.JouerCarte(this, carte))
                            {
                                _mains.RemoveAt(i);
                                Console.WriteLine($"{Nom} a joué {carte}.");
                                regles.AppliquerEffetCarteSpeciale(carte, jeu); // Applique l'effet de la carte
                                if (_mains.Count == 0)
                                {
                                    return true;
                                }
                                else if (_mains.Count == 1)
                                {
                                    Console.WriteLine($"ATTENTION! Joueur {Nom} n'a plus qu'une seule carte!");
                                    NotifierJoueurs(this.Id);
                                }

                                //Vider la liste des joueurs à attaquer
                                JoueursDerniereCarte.Clear();
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        //Piocher une carte
        public void PiocherCarte(JeuxDePeche jeu)
        {
            Console.WriteLine($"{Nom} doit piocher une carte.");
            List<Carte> cartesPigees = jeu.PiocherCarte(1);
            if (cartesPigees.Count > 0)
            {
                _mains.Add(cartesPigees[0]);
                Console.WriteLine($"{Nom} a pigé {cartesPigees[0]}.");
            }
            else
            {
                Console.WriteLine("La pile de pioche est vide...");
            }
        }

        // Implémentation de la méthode CalculerPoints() appelé dans JeuxDePeche
        public int CalculerPoints()
        {
            int points = 0;

            foreach (var carte in _mains)
            {
                points += Regles.ObtenirValeurCarte(carte);
            }
            return points;
        }

        public int NombreDeCartes()
        {
            int NombreCarte = _mains.Count;
            return NombreCarte;
        }

        public List<Carte> ObtenirMain()
        {
            return _mains.ToList();
        }

        //Joueur choisit la couleur selon les types de carte en mains
        public Couleur ChoisirCouleur()
        {
            List<int> qteCouleur = new List<int>();
            qteCouleur.Add(0);
            qteCouleur.Add(0);
            qteCouleur.Add(0);
            qteCouleur.Add(0);

            foreach (Carte carte in _mains)
            {
                switch (carte.GetCouleur())
                {
                    case Couleur.Coeur:
                        qteCouleur[0] = qteCouleur[0] + 1;
                        break;
                    case Couleur.Carreau:
                        qteCouleur[1] = qteCouleur[1] + 1;
                        break;
                    case Couleur.Trefle:
                        qteCouleur[2] = qteCouleur[2] + 1;
                        break;
                    case Couleur.Pique:
                        qteCouleur[3] = qteCouleur[3] + 1;
                        break;
                }
            }

            int indexCouleurMax = qteCouleur.IndexOf(qteCouleur.Max());

            return (Couleur)indexCouleurMax;
        }


        //OBSERVER

        //Implémenter observateur
        public void MiseAJourDerniereCarte(int joueurID)
        {
            JoueursDerniereCarte.Add(joueurID);
            Console.WriteLine($"Joueur {Nom} à été notifié");
        }

        //Implémenter sujet
        public void AttachJoueur(IObservateurJoueur observer)
        {
            if (!ObservateursJoueurs.Contains(observer))
            {
                ObservateursJoueurs.Add(observer);
            }
        }
        public void DetachJoueur(IObservateurJoueur observer)
        {
            if (ObservateursJoueurs.Contains(observer))
            {
                ObservateursJoueurs.Remove(observer);
            }
        }

        //Notifier les joueurs
        public void NotifierJoueurs(int joueurID)
        {
            foreach (var observer in ObservateursJoueurs)
            {
                observer.MiseAJourDerniereCarte(joueurID);
            }
        }
    }
}
