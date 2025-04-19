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
        public string Name { get; set; } //название объекта
        public string Description { get; set; } //описание объекта

        public readonly double Latitude; //широта

        public readonly double Longitude; //долгота
        public float Rating { get; set; } //рейтинг локации
    }


}
