using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Ax.CD.Dashboard.Service.Model
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public string ItemOwnerId { get; set; }    
    }
    [DataContract]
    public class Response
    {
        [DataMember(Order = 1)]
        public int ErrorId { get; set; }
        [DataMember(Order = 2)]
        public string ErrorText { get; set; }
        public Response()
        {
            this.ErrorId = 0;
            this.ErrorText = String.Empty;
        }
    }
}