using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TP1_1035.Classes
{
    internal class JoueurStrategique : Joueur
    {
        private Regles _reglesDuJeu;

        public JoueurStrategique(string nom, string prenom, int id) 
            : base(nom, prenom, id)
        {
            _reglesDuJeu = new Regles();
        }

        public void JouerStrategiquement(JeuxDePeche jeu)
        {
            Carte carteDuDessus = jeu.ObtenirCarteDuDessus();
            
            // Trier les cartes dans la main par ordre décroissant de valeur
            _mains = _mains.OrderByDescending(c => Regles.ObtenirValeurCarte(c)).ToList();

            bool aJoue = false;
            if (JoueursDerniereCarte.Count > 0)
            {
                aJoue = JouerAttaque(_reglesDuJeu, jeu);

                //Vider la liste des joueurs à attaquer
                JoueursDerniereCarte.Clear();
            }

            // Essayer de jouer la carte la plus haute possible
            if (!aJoue)
            {
                for (int i = 0; i <= _mains.Count - 1; i++)
                {
                    if (_reglesDuJeu.PeutJouerCarte(_mains[i], carteDuDessus, jeu.GetCouleurCourante()))
                    {
                        if(jeu.JouerCarte(this, _mains[i]))
                        {
                            Console.WriteLine($"{GetNom()} a joué la carte {_mains[i]}");
                            _reglesDuJeu.AppliquerEffetCarteSpeciale(_mains[i], jeu);
                            _mains.RemoveAt(i);

                            if (_mains.Count == 1)
                            {
                                Console.WriteLine($"ATTENTION! Joueur {Nom} n'a plus qu'une seule carte!");
                                NotifierJoueurs(this.Id);
                            }

                            return;
                        }
                    }
                }
            }

            // Si aucune carte ne peut être jouée, piocher une carte
            PiocherCarte(jeu);
        }
    }
}