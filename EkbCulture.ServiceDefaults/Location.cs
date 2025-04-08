using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkbCulture.ServiceDefaults
{
    /// <summary>
    /// Класс элемента карты(объекта)
    /// Сейчас сюда добавлены простейшие свойства
    /// </summary>
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } //описание объекта
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float Rating { get; set; }
    }
}
