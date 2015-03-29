using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ax.Model
{
    public class WmsShipmentSum
    {
        public DateTime Generated { get; set; }
        public string ItemOwnerId { get; set; }
        public DateTime Date { get; set; }
        public string AllLabel { get; set; }
        public double All { get; set; }
        public string WaitLabel { get; set; }
        public double Wait { get; set; }
        public string ActivatedLabel { get; set; }
        public double Activated { get; set; }
        public string InProgressLabel { get; set; }
        public double InProgress { get; set; }
        public string PickedLabel { get; set; }
        public double Picked { get; set; }
        public string ShippedLabel { get; set; }
        public double Shipped { get; set; }
        public string StoppedLabel { get; set; }
        public double Stopped { get; set; }
        public double WaitPercent
        {
            get
            {
                return (this.All != 0 ? Math.Round(((this.Wait / this.All) * 100), 2) : 0);
            }
        }
        public double ActivatedPercent
        {
            get
            {
                return (this.All != 0 ?  Math.Round(((this.Activated / this.All) * 100), 2) : 0);
            }
        }
        public double InProgressPercent
        {
            get
            {
                return (this.All != 0 ?  Math.Round(((this.InProgress / this.All) * 100), 2) : 0);
            }
        }
        public double PickedPercent
        {
            get
            {
                return (this.All != 0 ?  Math.Round(((this.Picked / this.All) * 100), 2) : 0);
            }
        }
        public double ShippedPercent
        {
            get
            {
                return (this.All != 0 ?  Math.Round(((this.Shipped / this.All) * 100), 2) : 0);
            }
        }
        public double StoppedPercent
        {
            get
            {
                return (this.All != 0 ?  Math.Round(((this.Stopped / this.All) * 100), 2) : 0);
            }
        }
    }

    public class WmsShipment
    {
        public string Name { get; set; }        
        public WmsShipmentStatus Status { get; set; }
        public WmsExpeditionStatus PickExpeditionStatus { get; set; }
        public double LineQty { get; set; }
        public ShipmentType Type { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public int RecId { get; set; }

        public void setType()
        {
            if (this.Name == "Wielkogabarytowe")
            {
                this.Type = ShipmentType.BigItem;
            }
            else if (this.Name == "Zwykłe1")
            {
                if (this.LineQty == 1)
                {
                    this.Type = ShipmentType.OneItem;
                }
                else
                {
                    this.Type = ShipmentType.MultiItem;
                }                
            }
        }
        public void setShipmentStatus()
        {
            if (this.Status == WmsShipmentStatus.Activated && this.PickExpeditionStatus == WmsExpeditionStatus.Activated)
            {
                this.ShipmentStatus = ShipmentStatus.ToPick;
            }
            else if (this.Status == WmsShipmentStatus.Activated && this.PickExpeditionStatus >= WmsExpeditionStatus.Registered)
            {
                this.ShipmentStatus = ShipmentStatus.Picking;
            }
            else if (this.Status == WmsShipmentStatus.Picked && this.PickExpeditionStatus == WmsExpeditionStatus.Complete)
            {
                this.ShipmentStatus = ShipmentStatus.ToPack;
            }
            else if (this.Status == WmsShipmentStatus.Shipped && this.PickExpeditionStatus == WmsExpeditionStatus.Complete)
            {
                this.ShipmentStatus = ShipmentStatus.Packed;
            }
        }
    }
    public class WmsShipmentList : List<WmsShipment>
    {

    }
    public class ShipmentData
    {
        private ShipmentDataList parent;

        public ShipmentDataList Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        public ShipmentType Type { get; set; }
        public string TypeLabel
        {
            get
            {
                return ShipmentTypeTranslate.Translate(this.Type);
            }
        }
        public ShipmentStatus Status { get; set; }
        public string StatusLabel
        {
            get
            {
                return ShipmentStatusTranslate.Translate(this.Status);
            }
        }
        public int Count { get; set; }
        public double Percent
        {
            get { return this.countPercent(); }
        }

        private double countPercent()
        {
            if (parent != null)
            {
                long total = parent.Total;
                if (total > 0)
                {
                    return Math.Round(((double)this.Count / (double)total) * 100, 0);
                }
            }
            return 0;
        }

        public override bool Equals(object obj)
        {
            ShipmentData tmp = obj as ShipmentData;
            if (tmp != null)
            {
                if (tmp.Type == this.Type &&
                    tmp.Status == this.Status)
                {
                    return true;
                }
            }
            return false;
        }
        public override int GetHashCode()
        {
            return (int)this.Type ^ (int)this.Status;
        }
    }
    public class ShipmentDataList : List<ShipmentData>
    {
        private int total;

        public int Total
        {
            get { return total; }
            set { total = value; }
        }
        public void AddRangeMerge(IEnumerable<ShipmentData> list)
        {
            foreach (ShipmentData item in list)
            {
                this.AddMerge(item);
            }
        }
        public void AddMerge(ShipmentData data)
        {
            int idx = this.IndexOf(data);
            if (idx > -1)
            {
                ShipmentData tmp = this[idx];
                if (tmp != null)
                {
                    tmp.Count += data.Count;
                }
            }
            else
            {
                data.Parent = this;
                this.Add(data);
            }
            total += data.Count;
        }
    }
    public enum ShipmentType
    {
        Standard,
        OneItem,
        MultiItem,
        BigItem       
    }
    public class ShipmentTypeTranslate
    {        
        public static string Translate(ShipmentType type)
        {
            string label = String.Empty;
            switch (type)
            {
                case ShipmentType.Standard: label = "Normalne"; break;
                case ShipmentType.OneItem: label = "Jednosztukowe"; break;
                case ShipmentType.MultiItem: label = "Wielosztukowe"; break;
                case ShipmentType.BigItem: label = "Wielkogarytowe"; break;
            }
            return label;
        }
    }    

    public enum ShipmentStatus
    {
        ToActive,        
        ToPick,
        Picking,        
        ToPack,
        Packed,
        Shipped
    }
    public class ShipmentStatusTranslate
    {        
        public static string Translate(ShipmentStatus status)
        {
            string label = String.Empty;
            switch (status)
            {
                case ShipmentStatus.ToActive: label = "Do aktywacji"; break;
                case ShipmentStatus.ToPick: label = "Do zebrania"; break;
                case ShipmentStatus.ToPack: label = "Do spakowania"; break;
                case ShipmentStatus.Shipped: label = "Wysłane"; break;
                case ShipmentStatus.Packed: label = "Spakowane"; break;
                case ShipmentStatus.Picking: label = "Zbierane"; break;
            }
            return label;
        }
    }
    public enum WmsExpeditionStatus
    {
        None = 0, //Zarezerwowane
        Registered = 1,
        Activated = 3,
        Started = 4,
        Picked = 7,
        Staged = 8,
        Loaded = 9,
        Complete = 10,
        Cancelled = 20
    }
    public enum WmsShipmentStatus
    {
        Registered = 0,
        Reserved = 1,        
        Activated = 4,
        Picked = 5,
        Staged = 7,
        Loaded = 8,
        Shipped = 9,
        Canceled = 15
    }
}
