using System;

namespace Smart.Helpers
{
    public class KWHelper
    {
        private const int impKwh = 800;
        
        public static double CalcKWH(int val)
        {
            
            Console.WriteLine(val);
            double msToKwh = val * impKwh;
            double secToKwh = msToKwh / 1000;
            return 3600 / secToKwh;
        }
    }
}
