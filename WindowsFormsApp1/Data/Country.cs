using System;
using System.ComponentModel;

namespace WindowsFormsApp1.Data.Models
{
    [Serializable]
    class Country
    {
        [DisplayName("Название"), Category("Сводка")]
        public string Name { get; set; }

        [DisplayName("Столица"), Category("Сводка")]
        public string Capital { get; set; }

        [DisplayName("Численность"), Category("Демография")]
        [Description("Общая численность в млн. чел. ")]
        public float Population { get; set; }

        [DisplayName("Площадь"), Category("Сводка")]
        [Description("Общая площадь в кв.км ")]
        public uint Area { get; set; }
        public string ImageFile { get; set; }
    }
}
