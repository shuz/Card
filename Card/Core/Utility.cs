using System;
using System.Collections.Generic;
using System.Text;

namespace Card.Core
{
    public static class Utility
    {
        public static void Swap<T>(ref T x, ref T y)
        {
            T temp = x;
            x = y;
            y = temp;
        }

        public static int GCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            do
            {
                if (a < b)
                {
                    Swap<int>(ref a, ref b);
                }
                a %= b;
            } while (a != 0);

            return b;
        }
    }

}
