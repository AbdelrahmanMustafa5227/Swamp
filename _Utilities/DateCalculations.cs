using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Utilities
{
    public static class DateCalculations
    {

        public static string CalculateDifference(DateTime date)
        {
            string difference;
            TimeSpan diff = DateTime.Now - date;

            if (diff.TotalDays >= 1)
            {
                difference = ((int)diff.TotalDays).ToString() + " days ago";
            }
            else
            {
                if (diff.TotalHours >= 1)
                {
                    difference = ((int)diff.TotalHours).ToString() + " hours ago";
                }
                else
                {
                    if (diff.TotalMinutes >= 1)
                    {
                        difference = ((int)diff.TotalMinutes).ToString() + " minutes ago";
                    }
                    else 
                    {
                        difference = "Just now";
                    }
                    
                }
            }


            return difference;
        }
    }
}
