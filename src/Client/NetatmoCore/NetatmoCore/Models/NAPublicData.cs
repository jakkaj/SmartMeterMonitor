// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace netatmocore.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class NAPublicData
    {
        /// <summary>
        /// Initializes a new instance of the NAPublicData class.
        /// </summary>
        public NAPublicData()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the NAPublicData class.
        /// </summary>
        /// <param name="_id">id of the station</param>
        /// <param name="place">Information about the station location
        /// (latitude/longitude of the station, altitude (meters),
        /// timezone)</param>
        /// <param name="moduleTypes">Latest measurements of the station,
        /// organized by module</param>
        public NAPublicData(string _id = default(string), NAPlace place = default(NAPlace), int? mark = default(int?), IList<string> modules = default(IList<string>), IDictionary<string, string> moduleTypes = default(IDictionary<string, string>), IDictionary<string, NAMeasure> measures = default(IDictionary<string, NAMeasure>))
        {
            this._id = _id;
            Place = place;
            Mark = mark;
            Modules = modules;
            ModuleTypes = moduleTypes;
            Measures = measures;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets id of the station
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string _id { get; set; }

        /// <summary>
        /// Gets or sets information about the station location
        /// (latitude/longitude of the station, altitude (meters), timezone)
        /// </summary>
        [JsonProperty(PropertyName = "place")]
        public NAPlace Place { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "mark")]
        public int? Mark { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "modules")]
        public IList<string> Modules { get; set; }

        /// <summary>
        /// Gets or sets latest measurements of the station, organized by
        /// module
        /// </summary>
        [JsonProperty(PropertyName = "module_types")]
        public IDictionary<string, string> ModuleTypes { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "measures")]
        public IDictionary<string, NAMeasure> Measures { get; set; }

    }
}