// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace netatmocore.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class NAHouseModelCa
    {
        /// <summary>
        /// Initializes a new instance of the NAHouseModelCa class.
        /// </summary>
        public NAHouseModelCa()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the NAHouseModelCa class.
        /// </summary>
        public NAHouseModelCa(double? te = default(double?), int? ti = default(int?), string so = default(string))
        {
            Te = te;
            Ti = ti;
            So = so;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "te")]
        public double? Te { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ti")]
        public int? Ti { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "so")]
        public string So { get; set; }

    }
}
