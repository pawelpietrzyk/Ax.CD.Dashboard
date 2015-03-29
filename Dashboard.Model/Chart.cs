using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Dashboard.Model
{
    public class Chart
    {
        public string Key { get; set; }
        public string Tile { get; set; }
        public ChartData Data { get; set; }
        public string Url { get; set; }
        public string UrlPush
        {
            get
            {
                return String.Format("{0}//{1}", this.Url, "push");
            }
        }
        public string UrlConfig
        {
            get
            {
                return String.Format("{0}//{1}//{2}", this.Url, "tileconfig", this.Key);
            }
        }
        public Chart()
        {
            this.Data = this.InitChartData();
            this.Tile = this.InitTile();
        }
        public string DataChart
        {
            get
            {
                return this.ConstructQueryString(this.values());
            }
        }
        
        public string ConstructQueryString(NameValueCollection parameters)
        {
            List<string> items = new List<string>();

            foreach (String name in parameters)
                items.Add(String.Concat(name, "=", HttpUtility.UrlEncode(parameters[name])));

            return String.Join("&", items.ToArray());
        }
        private NameValueCollection values()
        {            
            NameValueCollection values = new NameValueCollection();
            values.Add("key", this.Key);
            values.Add("tile", this.Tile);
            values.Add("data", this.Data.ToString());
            return values;
        }
        public virtual string InitTile()
        {
            return String.Empty;
        }
        public virtual ChartData InitChartData()
        {
            return new ChartData();
        }
    }    
    
    public class PieChart : Chart
    {
        public override string InitTile()
        {
            return "pie_chart";
        }
        public override ChartData InitChartData()
        {
            return new PieChartData();
        }
    }
    public class ChartData
    {
        public string Title { get; set; }
        public ChartSeriesList Series { get; set; }
        public virtual string ChartDataType
        {
            get { return "chart_data"; }
        }
        public override string ToString()
        {
            return "{" + String.Format("\"title\": \"{0}\", \"{1}\": {2}", new object[] { this.Title, this.ChartDataType, this.Series.ToString() }) + "}";
        }
    }    
    public class PieChartData : ChartData
    {
        public override string ChartDataType
        {
            get { return "pie_data"; }
        }        
    }
    public class ChartSeriesList : List<ChartSeries>
    {
        public List<string> ToStringList()
        {
            List<string> list = new List<string>();
            foreach (ChartSeries series in this)
            {
                list.Add(series.ToString());
            }
            return list;
        }
        public override string ToString()
        {
            string[] array = this.ToStringList().ToArray();
            string ret = String.Join(",", array);
            return String.Format("[{0}]", ret);
        }
    }
    public class ChartSeries
    {
        public string X { get; set; }
        public double Y { get; set; }

        public override string ToString()
        {
            double num = Math.Round(this.Y, 0, MidpointRounding.ToEven);
            num = (num == 0.0 ? 1.0 : num);
            return String.Format(CultureInfo.InvariantCulture, "[\"{0}\",{1:0.00}]", this.X, num);
        }
    }
    #region Listing    
    
    #endregion

    #region Fancy Listing

    public class FancyListing : Listing
    {
        public FancyListing()
        {
            this.Config = new FancyListingConfig();
        }
        public FancyListingConfig Config { get; set; }        
        public override string InitTile()
        {
            return "fancy_listing";
        }
        public string DataConfig
        {
            get
            {
                return this.ConstructQueryString(this.configValues());
            }
        }
        public override void SetConfig(ListingItems items)
        {
            FancyListingItems fancyItems = items as FancyListingItems;
            if (fancyItems != null)            
            {
                this.Config.Items.Clear();
                foreach (FancyListingItem item in items)
                {
                    this.Config.Items.Add(item.Position);
                }
            }
        }
        private NameValueCollection configValues()
        {
            NameValueCollection values = new NameValueCollection();
            values.Add("value", this.Config.ToString());
            return values;
        } 
    }
    public class Listing
    {
        public string Key { get; set; }
        public string Tile { get; set; }
        private ListingItems data;
        public ListingItems Data
        {
            get { return data; }
            set
            {
                this.data = value;
                this.SetConfig(data);
            }
        }
        public string Url { get; set; }
        public string UrlPush
        {
            get
            {
                return String.Format("{0}/{1}", this.Url, "push");
            }
        }
        public virtual void SetConfig(ListingItems items)
        {

        }
        public string UrlConfig
        {
            get
            {
                return String.Format("{0}/{1}/{2}", this.Url, "tileconfig", this.Key);
            }
        }
        public Listing()
        {
            this.Data = this.InitData();
            this.Tile = this.InitTile();            
        }
        public Listing(string url) : this()
        {
            this.Url = url;
        }
        public string DataChart
        {
            get
            {
                return this.ConstructQueryString(this.values());
            }
        }

        public string ConstructQueryString(NameValueCollection parameters)
        {
            List<string> items = new List<string>();

            foreach (String name in parameters)
                items.Add(String.Concat(name, "=", System.Web.HttpUtility.UrlEncode(parameters[name])));

            return String.Join("&", items.ToArray());
        }
        private NameValueCollection values()
        {
            NameValueCollection values = new NameValueCollection();
            values.Add("key", this.Key);
            values.Add("tile", this.Tile);
            values.Add("data", this.Data.ToString());
            return values;
        }
        public virtual string InitTile()
        {
            return String.Empty;
        }
        public virtual ListingItems InitData()
        {
            return new ListingItems();
        }
    }
    public class ListingItem
    {
        public string Text { get; set; }
        public override string ToString()
        {
            return String.Format("\"{0}\"", this.Text);
        }
    }
    public class FancyListingItem : ListingItem
    {
        public string Label { get; set; }        
        public string Description { get; set; }
        public FancyListingPosition Position { get; set; }
        public override string ToString()
        {
            return "{" + String.Format("\"label\": \"{0}\", \"text\": \"{1}\", \"description\": \"{2}\"", new object[] { this.Label, this.Text, this.Description }) + "}";
        }
    }
    public class ListingItems : List<ListingItem>
    {
        public virtual List<string> ToStringList()
        {
            List<string> list = new List<string>();
            foreach (ListingItem series in this)
            {
                list.Add(series.ToString());
            }
            return list;
        }
        public override string ToString()
        {
            string[] array = this.ToStringList().ToArray();
            string ret = String.Join(",", array);
            return String.Format("[{0}]", ret);
        }
    }
    public class FancyListingItems : ListingItems
    {
        public override List<string> ToStringList()
        {
            List<string> list = new List<string>();
            foreach (FancyListingItem series in this)
            {
                list.Add(series.ToString());
            }
            return list;
        }
    }
    public class FancyListingConfig
    {
        public FancyListingConfig()
        {
            this.Items = new FancyListingPositions();
        }
        public bool VerticalCenter { get; set; }
        public FancyListingPositions Items { get; set; }
        public override string ToString()
        {
            return "{" + String.Format("\"vertical_center\": {0}, {1}", (this.VerticalCenter ? "true" : "false"), this.Items) + "}";
        }        
    }
    public class FancyListingLabel
    {
        public bool Center { get; set; }
        public string LabelColor { get; set; }
        public override string ToString()
        {
            return "{" + String.Format("\"label_color\": \"{0}\", \"center\": {1}", this.LabelColor, (this.Center ? "true" : "false")) + "}";
        }
    }
    public class FancyListingPositions : List<FancyListingPosition>
    {
        public virtual List<string> ToStringList()
        {
            List<string> list = new List<string>();
            foreach (FancyListingPosition position in this)
            {
                list.Add(position.ToString());
            }
            return list;
        }
        public override string ToString()
        {
            string[] array = this.ToStringList().ToArray();
            string ret = String.Join(",", array);
            return String.Format("{0}", ret);
        }
    }
    public class FancyListingPosition
    {
        public int Position { get; set; }
        public FancyListingLabel Label { get; set; }

        public override string ToString()
        {
            return String.Format("\"{0}\": {1}", this.Position, this.Label);
        }
    }
    #endregion
}
