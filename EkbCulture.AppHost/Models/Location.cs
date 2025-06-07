using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkbCulture.AppHost.Models
{
    /// <summary>
    /// Локация на карте    
    /// </summary>
    /// 

    
    public class Location
    {
        public int Id { get; set; } 

        public string Name { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Rating { get; set; }

        //public Dictionary<User, int> RateByDict { get; set; }

        public int[] VisitedBy { get; set; }

        public string FirstImage { get; set; }

        public string SecondImage { get; set; }

        public string ThirdImage { get; set; }

        /*
        public void UpdateRating()
        {
            var sum = 0;
            var rateByCount = 0;
            foreach (var item in RateByDict)
            {
                sum += item.Value;
                rateByCount++;
            }
            Rating = sum / rateByCount;
        }
        */

    }


}
