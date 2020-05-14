using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepBetter
{
    public static class Plan
    {
        public static Tuple<int,int> GetSleepTime(UInt32 days)
        {
            if (days < 3)
                return Tuple.Create(1, 0);
            else if (days < 6)
                return Tuple.Create(0, 50);
            else if (days < 9)
                return Tuple.Create(0, 40);
            else if (days < 12)
                return Tuple.Create(0, 30);
            else if (days < 15)
                return Tuple.Create(0, 20);
            else if (days < 18)
                return Tuple.Create(0, 10);
            else if (days < 21)
                return Tuple.Create(0, 0);
            else if (days < 24)
                return Tuple.Create(23, 55);
            else if (days < 27)
                return Tuple.Create(23, 50);
            else if (days < 30)
                return Tuple.Create(23, 45);
            else if (days < 33)
                return Tuple.Create(23, 40);
            else if (days < 36)
                return Tuple.Create(23, 35);
            else if (days < 39)
                return Tuple.Create(23, 30);
            else if (days < 45)
                return Tuple.Create(23, 25);
            else if (days < 50)
                return Tuple.Create(23, 20);
            else if (days < 55)
                return Tuple.Create(23, 15);
            else if (days < 60)
                return Tuple.Create(23, 10);
            else if (days < 70)
                return Tuple.Create(23, 5);
            else
                return Tuple.Create(23, 0);
        }
    }
}
