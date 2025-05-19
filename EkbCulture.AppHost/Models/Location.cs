using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkbCulture.AppHost.Models
{
    /// <summary>
    /// Класс элемента карты(объекта)
    /// Сейчас сюда добавлены простейшие свойства
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

        public int[] VisitedBy { get; set; }

        public byte[] FirstImage { get; set; }

        public byte[] SecondImage { get; set; }

        public byte[] ThirdImage { get; set; }

    }


}
