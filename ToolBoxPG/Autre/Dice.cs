using System;


namespace ToolBoxPG.Autre
{
    public class Dice
    {
        private const int _min = 1;
        private static int _seed = -1;
        private static Random _rng = new Random();

        public Dice(int Max)
        {
            if(Max>1)
            {
                this.Max = Max;
            }
            else
            {
                throw new ArgumentException("Error : La valeur max minimun pour un dé est de 2!", nameof(Max));
            }
        }

        public int Max { get; }

        public static int Roll(int Max)
        {
            if (_seed == -1)
            {
                _seed = _rng.Next();
            }
            else
            {
                _rng = new Random(_seed);
                _seed = _rng.Next();
            }

            return (_seed % Max) + 1;
        }

        public int Roll()
        {
            if(_seed == -1)
            {
                _seed = _rng.Next();
            }
            else
            {
                _rng = new Random(_seed);
                _seed = _rng.Next();
            }

            return (_seed % Max) + _min;
        }

        public static int[] MultiRollMax(Dice Dice, uint NbLancer, uint NbGarder)
        {
            if (NbGarder > NbLancer) throw new ArgumentException("Error : La valeur de nbLancer est suppérieur a nbGarder ce qui n'est pas possible", nameof(NbGarder));
            int[] Retour = new int[NbGarder];
            // ReSharper disable once InconsistentNaming
            for (int i = 0; i < NbLancer; i++)
            {
                int X = Dice.Roll();
                // ReSharper disable once InconsistentNaming
                for (int j = 0; j < NbGarder; j++)
                {
                    if (Retour[j] >= X) continue;
                    int Temp = Retour[j];
                    Retour[j] = X;
                    X = Temp;
                }
            }
            return Retour;
        }

        public static int[] MultiRollMin(Dice Dice, uint NbLancer, uint NbGarder)
        {
            if (NbGarder > NbLancer) throw new ArgumentException("Error : La valeur de nbLancer est supérieur a nbGarder ce qui n'est pas possible", nameof(NbGarder));
            int[] Retour = new int[NbGarder];
            // ReSharper disable once InconsistentNaming
            for (int i = 0; i < NbLancer; i++)
            {
                int X = Dice.Roll();
                // ReSharper disable once InconsistentNaming
                for (int j = 0; j < NbGarder; j++)
                {
                    if (Retour[j] <= X) continue;
                    int Temp = Retour[j];
                    Retour[j] = X;
                    X = Temp;
                }
            }
            return Retour;
        }

        public static int[] MultiRoll(Dice Dice, uint NbLancer)
        {

            int[] Retour = new int[NbLancer];
            // ReSharper disable once InconsistentNaming
            for (int i = 0; i < NbLancer; i++)
            {
                Retour[i] = Dice.Roll();
            }
            return Retour;
        }
    }
}
