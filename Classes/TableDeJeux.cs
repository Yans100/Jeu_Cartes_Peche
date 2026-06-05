using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1_1035.Classes
{
    internal class TableDeJeux
    {
        public TableDeJeux()
        {
            Console.WriteLine("Paramètres de jeux - Ajouter les joueurs");
            List<Joueur> joueurs = AjouterJoueur();
            CommencerJeux(joueurs);
        }

        private List<Joueur> AjouterJoueur()
        {
            int QteJoueur;

            // Validation du nombre de joueurs
            while (true)
            {
                Console.WriteLine("Combien de joueurs? (2 - 4 joueurs)");
                string NombreJoueurs = Console.ReadLine();

                if (int.TryParse(NombreJoueurs, out QteJoueur) && QteJoueur >= 2 && QteJoueur <= 4)
                {
                    break; 
                }
                else
                {
                    Console.WriteLine("Veuillez entrer un nombre valide entre 2 et 4.");
                }
            }

            // Créer les joueurs
            List<Joueur> joueurs = new List<Joueur>();
            Random random = new Random();
            int joueurStrat = random.Next(0, QteJoueur); // Assigne le pouvoir de joueur stratégique aléatoirement

            for (int PlayerCount = 0; PlayerCount < QteJoueur; PlayerCount++)
            {
                string JoueurPrenom;
                string JoueurNom;

                // Validation du prénom
                while (true)
                {
                    Console.WriteLine($"Joueur {PlayerCount + 1} - Entrer le Prénom: ");
                    JoueurPrenom = Console.ReadLine();
                    if (EntreesValide(JoueurPrenom))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Le prénom doit contenir au moins 3 caractères et uniquement des lettres.");
                    }
                }

                // Validation du nom
                while (true)
                {
                    Console.WriteLine($"Joueur {PlayerCount + 1} - Entrer le Nom: ");
                    JoueurNom = Console.ReadLine();
                    if (EntreesValide(JoueurNom))
                    {
                        break; 
                    }
                    else
                    {
                        Console.WriteLine("Le nom doit contenir au moins 3 caractères et uniquement des lettres.");
                    }
                }

                int JoueurId = PlayerCount + 1; 
                Joueur joueur;

                // Déterminer si ce joueur doit être stratégique
                if (PlayerCount == joueurStrat)
                {
                    joueur = new JoueurStrategique(JoueurNom, JoueurPrenom, JoueurId);
                    Console.WriteLine($"Joueur {joueur.GetId()} ({JoueurPrenom} {JoueurNom}) ajouté comme joueur utilisant une stratégie.");
                }
                else
                {
                    joueur = new Joueur(JoueurNom, JoueurPrenom, JoueurId);
                    Console.WriteLine($"Joueur {joueur.GetId()} ({JoueurPrenom} {JoueurNom}) ajouté.");
                }

                joueurs.Add(joueur);
            }

            return joueurs;
        }

        // Vérifie la validité des inputs nom et prénom (pas d'espace, uniquement des lettres et    minimum 3 lettres)
        private bool EntreesValide(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length >= 3 && name.All(char.IsLetter);
        }

        private void CommencerJeux(List<Joueur> joueurs)
        {
            // Todo: set l'observer avec les joueurs avant de le passer au JeuxDePeche
            Observer observer = new Observer();
            observer.Attacher(observer);
            JeuxDePeche jeuxDePeche = new JeuxDePeche(joueurs, observer);
        }
    }
}
