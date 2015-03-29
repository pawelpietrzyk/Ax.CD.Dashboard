using Ax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Ax.CD.Dashboard.Service.Model
{
    [DataContract]
    public class WmsShipmentSumRequest
    {
        [DataMember]
        public string ItemOwnerId { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
    }
    [DataContract]
    public class WmsShipmentSumResponse
    {
        [DataMember]
        public WmsShipmentSum Sum { get; set; }
    }

    [DataContract]
    public class WmsShipmentsRequest
    {
        [DataMember]
        public DateTime DateFrom { get; set; }

        [DataMember]
        public DateTime DateTo { get; set; }


    }
    [DataContract]
    public class WmsShipmentsResponse
    {
        [DataMember]
        public List<WmsShipment> Items { get; set; }

        public WmsShipmentsResponse()
        {
            this.Items = new List<WmsShipment>();
        }
    }
}
