using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeatherMap.Model
{
    public partial class SimpleWeather
    {
        public long Id { get; set; }
        public MainEnum Main { get; set; }
        public Description Description { get; set; }
        public string Icon { get; set; }
    }
}
