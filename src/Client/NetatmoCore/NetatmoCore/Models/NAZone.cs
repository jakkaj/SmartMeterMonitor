// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace netatmocore.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class NAZone
    {
        /// <summary>
        /// Initializes a new instance of the NAZone class.
        /// </summary>
        public NAZone()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the NAZone class.
        /// </summary>
        public NAZone(int? id = default(int?), int? type = default(int?), string name = default(string), double? temp = default(double?))
        {
            Id = id;
            Type = type;
            Name = name;
            Temp = temp;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public int? Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "temp")]
        public double? Temp { get; set; }

    }
}
