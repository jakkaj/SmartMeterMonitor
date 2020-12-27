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

    public partial class NAPlug
    {
        /// <summary>
        /// Initializes a new instance of the NAPlug class.
        /// </summary>
        public NAPlug()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the NAPlug class.
        /// </summary>
        /// <param name="type">Included in every device or module. It defines
        /// the type of the device/module. Its values are among :
        /// NAMain : for the base station
        /// NAModule1 : for the outdoor module
        /// NAModule4 : for the additionnal indoor module
        /// NAModule3 : for the rain gauge module
        /// NAPlug : for the thermostat relay/plug
        /// NATherm1 : for the thermostat module
        /// </param>
        /// <param name="wifiStatus">It contains the current wifi status. The
        /// different thresholds to take into account are
        /// RSSI_THRESHOLD_0 = 86 bad signal
        /// RSSI_THRESHOLD_1 = 71 middle quality signal
        /// RSSI_THRESHOLD_2 = 56 good signal
        /// </param>
        public NAPlug(string _id = default(string), int? firmware = default(int?), int? lastStatusStore = default(int?), NAPlace place = default(NAPlace), string stationName = default(string), string type = default(string), int? wifiStatus = default(int?), int? plugConnectedBoiler = default(int?), bool? udpConn = default(bool?), int? lastPlugSeen = default(int?), NAYearMonth lastBilan = default(NAYearMonth), IList<NAThermostat> modules = default(IList<NAThermostat>), bool? syncing = default(bool?))
        {
            this._id = _id;
            Firmware = firmware;
            LastStatusStore = lastStatusStore;
            Place = place;
            StationName = stationName;
            Type = type;
            WifiStatus = wifiStatus;
            PlugConnectedBoiler = plugConnectedBoiler;
            UdpConn = udpConn;
            LastPlugSeen = lastPlugSeen;
            LastBilan = lastBilan;
            Modules = modules;
            Syncing = syncing;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string _id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "firmware")]
        public int? Firmware { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "last_status_store")]
        public int? LastStatusStore { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "place")]
        public NAPlace Place { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "station_name")]
        public string StationName { get; set; }

        /// <summary>
        /// Gets or sets included in every device or module. It defines the
        /// type of the device/module. Its values are among :
        /// NAMain : for the base station
        /// NAModule1 : for the outdoor module
        /// NAModule4 : for the additionnal indoor module
        /// NAModule3 : for the rain gauge module
        /// NAPlug : for the thermostat relay/plug
        /// NATherm1 : for the thermostat module
        ///
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets it contains the current wifi status. The different
        /// thresholds to take into account are
        /// RSSI_THRESHOLD_0 = 86 bad signal
        /// RSSI_THRESHOLD_1 = 71 middle quality signal
        /// RSSI_THRESHOLD_2 = 56 good signal
        ///
        /// </summary>
        [JsonProperty(PropertyName = "wifi_status")]
        public int? WifiStatus { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "plug_connected_boiler")]
        public int? PlugConnectedBoiler { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "udp_conn")]
        public bool? UdpConn { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "last_plug_seen")]
        public int? LastPlugSeen { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "last_bilan")]
        public NAYearMonth LastBilan { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "modules")]
        public IList<NAThermostat> Modules { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "syncing")]
        public bool? Syncing { get; set; }

    }
}