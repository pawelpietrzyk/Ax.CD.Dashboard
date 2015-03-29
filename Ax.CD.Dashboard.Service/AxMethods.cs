using Ax.CD.Dashboard.Service.Model;
using Ax.Model;
using ITPiAST.AxaptaHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ax.CD.Dashboard.Service
{
    public static class AxMethods
    {
        public static WmsShipmentsResponse WmsShipmentGet(WmsShipmentsRequest request)
        {            
            return Ax.Connector.ExecuteBatch(request, new AxaptaBatchDelegate(batchWmsShipmentGet)) as WmsShipmentsResponse;            
        }
        private static object batchWmsShipmentGet(object data)
        {
            WmsShipmentsRequest request = data as WmsShipmentsRequest;
            WmsShipmentsResponse response = new WmsShipmentsResponse();
            if (request != null)
            {
                AxaptaObject axaptaQuery = Ax.Connector.CallStaticClassMethod("AxReportsService_alg", "GetShipments", request.DateFrom, request.DateTo) as AxaptaObject;
                AxaptaObject axaptaQueryRun = Ax.Connector.CreateAxaptaObject("QueryRun", axaptaQuery) as AxaptaObject;
                AxaptaRecord buffer;

                while (Convert.ToBoolean(axaptaQueryRun.Call("Next")))
                {
                    buffer = axaptaQueryRun.Call("GetNo", 1) as AxaptaRecord;

                    WmsShipment record = new WmsShipment();
                    record.RecId = (int)(long)buffer.get_Field("RecId");
                    record.Name = buffer.get_Field("name") as string;
                    record.Status = (WmsShipmentStatus)buffer.get_Field("status");
                    record.PickExpeditionStatus = (WmsExpeditionStatus)buffer.get_Field("pickExpeditionStatus");
                    record.LineQty = (double)buffer.get_Field("SumLineQty_itp");
                    record.setType();
                    record.setShipmentStatus();
                    response.Items.Add(record);                    
                }
            }
            return response;            
        }
        public static WmsShipmentSumResponse WmsShipmentSumGet(WmsShipmentSumRequest request)
        {
            return Ax.Connector.ExecuteBatch(request, new AxaptaBatchDelegate(batchWmsShipmentOrderStatusGet)) as WmsShipmentSumResponse;
        }
        private static object batchWmsShipmentOrderStatusGet(object data)
        {
            WmsShipmentSumRequest request = data as WmsShipmentSumRequest;
            WmsShipmentSumResponse response = new WmsShipmentSumResponse();
            if (request != null)
            {
                AxaptaRecord record = Ax.Connector.CallStaticClassMethod("AxReportsService_alg", "GetShipmentOrderStatus", request.Date, request.ItemOwnerId) as AxaptaRecord;
                WmsShipmentSum sum = new WmsShipmentSum();
                sum.All = (double)record.get_Field("Qty");
                sum.Wait = (double)record.get_Field("Qty1");
                sum.Activated = (double)record.get_Field("Qty2");
                sum.InProgress = (double)record.get_Field("Qty3");
                sum.Picked = (double)record.get_Field("Qty4");
                sum.Shipped = (double)record.get_Field("Qty5");
                sum.Stopped = (double)record.get_Field("Qty6");
                response.Sum = sum;                
            }
            return response;
        }

    }
}
