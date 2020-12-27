// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace netatmocore.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class NAUserResponse
    {
        /// <summary>
        /// Initializes a new instance of the NAUserResponse class.
        /// </summary>
        public NAUserResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the NAUserResponse class.
        /// </summary>
        public NAUserResponse(string status = default(string), NAUser body = default(NAUser), double? timeExec = default(double?), int? timeServer = default(int?))
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
        public NAUser Body { get; set; }

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
