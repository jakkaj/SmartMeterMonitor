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

    public partial class NAPartnerDevicesResponse
    {
        /// <summary>
        /// Initializes a new instance of the NAPartnerDevicesResponse class.
        /// </summary>
        public NAPartnerDevicesResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the NAPartnerDevicesResponse class.
        /// </summary>
        public NAPartnerDevicesResponse(string status = default(string), IList<string> body = default(IList<string>), double? timeExec = default(double?), int? timeServer = default(int?))
        {
            Status = status;
            Body = body;
            TimeExec = timeExec;
            TimeServer = timeServer;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "body")]
        public IList<string> Body { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "time_exec")]
        public double? TimeExec { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "time_server")]
        public int? TimeServer { get; set; }

    }
}
