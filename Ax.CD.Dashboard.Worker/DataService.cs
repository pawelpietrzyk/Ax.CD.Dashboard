using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ax.CD.Dashboard.Worker
{
    public class DataService
    {
        public static void Add(WmsShipmentData data)
        {
            using (DashboardDataEntities context = new DashboardDataEntities())
            {
                if (data != null)
                {
                    context.AddToWmsShipmentDatas(data);
                    context.SaveChanges();
                }
            }
        }
    }
}
