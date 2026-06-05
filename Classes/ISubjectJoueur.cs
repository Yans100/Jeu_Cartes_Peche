using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1_1035.Classes
{
    internal interface ISubjectJoueur
    {
        void AttachJoueur(IObservateurJoueur observer);
        void DetachJoueur(IObservateurJoueur observer);
        void NotifierJoueurs(int joueurID);
    }
}
