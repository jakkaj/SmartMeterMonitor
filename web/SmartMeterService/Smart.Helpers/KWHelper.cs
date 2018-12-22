using System;

namespace Smart.Helpers
{
    public class KWHelper
    {
        private const int impKwh = 800;
        public const decimal CostPerKwh = 28.52M;
        public static double CalcKWH(int imp, int time)
        {
            if (imp < 1 || time < 1)
            {
                return -1;
            }

            //if i get 4 in 15 seconds, how many would i get an hour?

            //how many seconds would it take to get to 800imp
            var impSegments = impKwh / imp; //this is per 15s



            //how many 15s per hour
            var segments = 3600 / time;

            double kwh = (double)segments / (double)impSegments;

            // var impSegments = impKwh / imp;

            //var perHour = impSegments * imp;

            //how many times per hour?
            //var segments = 3600 / time;

            return kwh;

        }
    }
}
