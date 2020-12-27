// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace netatmocore.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class NAPidAlgo
    {
        /// <summary>
        /// Initializes a new instance of the NAPidAlgo class.
        /// </summary>
        public NAPidAlgo()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the NAPidAlgo class.
        /// </summary>
        public NAPidAlgo(int? gain = default(int?), int? period = default(int?), int? td = default(int?), int? ti = default(int?))
        {
            Gain = gain;
            Period = period;
            Td = td;
            Ti = ti;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "gain")]
        public int? Gain { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "period")]
        public int? Period { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "td")]
        public int? Td { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ti")]
        public int? Ti { get; set; }

    }
}