using Ax.Model;
using Dashboard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ax.CD.Dashboard.Board
{
    
    public class BoardPanel
    {
        public List<ShipmentData> PrepareShipmentDataList(List<WmsShipment> data)
        {
            IEnumerable<ShipmentData> items = (from item in data                                              
                                              group item by new ShipmentDataGroup { Status = item.ShipmentStatus, Type = item.Type }                                              
                                                  into grc
                                                  select new ShipmentData
                                                  {
                                                      Status = grc.Key.Status,
                                                      Type = grc.Key.Type,
                                                      Count = grc.Count()
                                                  }).OrderBy(p=>p.Type);
            return items.ToList();            
        }
        public ShipmentDataList PrepareShipmentData(List<WmsShipment> data)
        {
            ShipmentDataList shipData = new ShipmentDataList();
            IEnumerable<ShipmentData> items = (from item in data
                                               group item by new ShipmentStatusGroup { Status = item.ShipmentStatus}
                                                   into grc
                                                   let temp = new ShipmentData
                                                   {
                                                       Status = grc.Key.Status,                                                       
                                                       Count = grc.Count()
                                                   }
                                                   orderby temp.Status
                                                   select temp);
            shipData.AddRangeMerge(items);
            return shipData;            
        }
        //public static ExpenseCategoryResponse GetExpenseCategorySum(ExpenseCategoryRequest request)
        //{
        //    ExpenseCategoryResponse response = new ExpenseCategoryResponse();
        //    if (request != null)
        //    {
        //        using (ExpensesEntities context = new ExpensesEntities())
        //        {
        //            IEnumerable<ExpenseCategoryGroupSum> items =
        //                from item in context.ExpenseCategories
        //                where
        //                (
        //                    ((item.Date >= request.BeginDate) || (request.BeginDate == null)) &&
        //                    ((item.Date <= request.EndDate) || (request.EndDate == null))
        //                )
        //                group item by
        //                    new ExpenseCategoryGroup { CategoryName = item.CategoryName, Year = item.Date.Year, Month = item.Date.Month }
        //                    into grc
        //                    select new ExpenseCategoryGroupSum
        //                    {
        //                        CategoryName = grc.Key.CategoryName,
        //                        Year = grc.Key.Year,
        //                        Month = grc.Key.Month,
        //                        Value = grc.Sum(p => p.Value),
        //                        ValuePlanned = grc.Sum(p => p.ValuePlan)
        //                    };
        //            response.Items = items.ToList();
        //        }
        //    }
        //    return response;
        //}

        public ChartSeriesList PrepareChartSeries(WmsShipmentSum data)
        {
            ChartSeriesList series = new ChartSeriesList();
            if (data != null)
            {                
                series.Add(new ChartSeries() { X = data.WaitLabel, Y = data.WaitPercent });
                series.Add(new ChartSeries() { X = data.ActivatedLabel, Y = data.ActivatedPercent });
                series.Add(new ChartSeries() { X = data.InProgressLabel, Y = data.InProgressPercent });
                series.Add(new ChartSeries() { X = data.PickedLabel, Y = data.PickedPercent });
                series.Add(new ChartSeries() { X = data.ShippedLabel, Y = data.ShippedPercent });
                series.Add(new ChartSeries() { X = data.StoppedLabel, Y = data.StoppedPercent });                
            }
            return series;
        }

        public ChartSeriesList PrepareChartSeries(ShipmentDataList data)
        {
            ChartSeriesList series = new ChartSeriesList();
            foreach (ShipmentData item in data)
            {
                ChartSeries serie = new ChartSeries();
                serie.X = String.Format("{0}", item.StatusLabel);
                serie.Y = item.Percent;
                series.Add(serie);
            }
            return series;
        }

        public FancyListingItems PrepareListingSeries(WmsShipmentSum data)
        {
            FancyListingItems series = new FancyListingItems();
            if (data != null)
            {
                series.Add(new FancyListingItem()
                {
                    Text = data.WaitLabel,
                    Label = String.Format("{0} ({1} %)", data.Wait, data.WaitPercent),
                    Position = new FancyListingPosition() { Position = 1, Label = new FancyListingLabel() { Center = false, LabelColor = "blue" } }
                });
                series.Add(new FancyListingItem()
                {
                    Text = data.ActivatedLabel,
                    Label = String.Format("{0} ({1} %)", data.Activated, data.ActivatedPercent),
                    Position = new FancyListingPosition() { Position = 2, Label = new FancyListingLabel() { Center = false, LabelColor = "yellow" } }
                });
                series.Add(new FancyListingItem()
                {
                    Text = data.InProgressLabel,
                    Label = String.Format("{0} ({1} %)", data.InProgress, data.InProgressPercent),
                    Position = new FancyListingPosition() { Position = 3, Label = new FancyListingLabel() { Center = false, LabelColor = "violet" } }
                });
                series.Add(new FancyListingItem()
                {
                    Text = data.PickedLabel,
                    Label = String.Format("{0} ({1} %)", data.Picked, data.PickedPercent),
                    Position = new FancyListingPosition() { Position = 4, Label = new FancyListingLabel() { Center = false, LabelColor = "orange" } }
                });
                series.Add(new FancyListingItem()
                {
                    Text = data.ShippedLabel,
                    Label = String.Format("{0} ({1} %)", data.Shipped, data.ShippedPercent),
                    Position = new FancyListingPosition() { Position = 5, Label = new FancyListingLabel() { Center = false, LabelColor = "green" } }
                });
                series.Add(new FancyListingItem()
                {
                    Text = data.StoppedLabel,
                    Label = String.Format("{0} ({1} %)", data.Stopped, data.StoppedPercent),
                    Position = new FancyListingPosition() { Position = 6, Label = new FancyListingLabel() { Center = false, LabelColor = "red" } }
                });
            }
            return series;
        }
        public FancyListingItems PrepareListingSeries(ShipmentDataList data)
        {
            FancyListingItems series = new FancyListingItems();
            foreach (ShipmentData item in data)
            {                
                FancyListingItem serie = new FancyListingItem();                
                serie.Text = String.Format("{0}", item.StatusLabel);
                serie.Label = String.Format("{0} ({1} %)", item.Count, item.Percent);
                series.Add(serie);
            }
            return series;
        }
        public FancyListing CreateFancyListing(string key, List<WmsShipment> data)
        {
            ShipmentDataList datalist = this.PrepareShipmentData(data);
            return this.CreateFancyListing(key, datalist);
        }
        public FancyListing CreateFancyListing(string key, WmsShipmentSum data)
        {
            FancyListingItems list = this.PrepareListingSeries(data);
            FancyListing listing = new FancyListing();
            listing.Key = key;
            listing.Data = list;
            listing.Config.VerticalCenter = false;            
            return listing;
        }
        public FancyListing CreateFancyListing(string key, ShipmentDataList data)
        {
            FancyListingItems list = this.PrepareListingSeries(data);
            FancyListing listing = new FancyListing();
            listing.Key = key;
            listing.Data = list;
            listing.Config.VerticalCenter = false;
            listing.Config.Items.Clear();
            listing.Config.Items.Add(new FancyListingPosition() { Position = 1, Label = new FancyListingLabel() { Center = false, LabelColor = "blue" } });
            listing.Config.Items.Add(new FancyListingPosition() { Position = 2, Label = new FancyListingLabel() { Center = false, LabelColor = "yellow" } });
            listing.Config.Items.Add(new FancyListingPosition() { Position = 3, Label = new FancyListingLabel() { Center = false, LabelColor = "violet" } });
            listing.Config.Items.Add(new FancyListingPosition() { Position = 4, Label = new FancyListingLabel() { Center = false, LabelColor = "orange" } });
            listing.Config.Items.Add(new FancyListingPosition() { Position = 5, Label = new FancyListingLabel() { Center = false, LabelColor = "green" } });
            return listing;
        }
        public FancyListingConfig CreateFancyListingConfig()
        {
            FancyListingConfig conf = new FancyListingConfig();
            conf.VerticalCenter = true;
            conf.Items.Add(new FancyListingPosition() { Position = 1, Label = new FancyListingLabel() { Center = true, LabelColor = "red" } });
            conf.Items.Add(new FancyListingPosition() { Position = 2, Label = new FancyListingLabel() { Center = true, LabelColor = "green" } });
            conf.Items.Add(new FancyListingPosition() { Position = 3, Label = new FancyListingLabel() { Center = true, LabelColor = "yelow" } });
            conf.Items.Add(new FancyListingPosition() { Position = 4, Label = new FancyListingLabel() { Center = true, LabelColor = "gray" } });
            return conf;
        }
        public PieChart CreatePieChart(string key, string tile, string title, ChartSeriesList data)
        {
            PieChart chart = new PieChart();
            chart.Key = key;
            chart.Tile = tile;
            chart.Data = new PieChartData() { Title = title, Series = data };
            return chart;
        }
        public PieChart CreatePieChart(string key, string tile, string title, WmsShipmentSum data)
        {
            ChartSeriesList chartSeriesList = this.PrepareChartSeries(data);
            return this.CreatePieChart(key, tile, title, chartSeriesList);
        }
        public PieChart CreatePieChart(string key, string tile, string title, List<WmsShipment> data)
        {
            ShipmentDataList shipmentDataList = this.PrepareShipmentData(data);
            return this.CreatePieChart(key, tile, title, shipmentDataList);            
        }
        public PieChart CreatePieChart(string key, string tile, string title, ShipmentDataList data)
        {            
            ChartSeriesList chartSeriesList = this.PrepareChartSeries(data);
            return this.CreatePieChart(key, tile, title, chartSeriesList);
        }
    
    }
    public class ShipmentDataGroup
    {
        public ShipmentType Type { get; set; }
        public ShipmentStatus Status { get; set; }

        public override bool Equals(object obj)
        {
            ShipmentDataGroup tmp = obj as ShipmentDataGroup;
            if (tmp != null)
            {
                if (tmp.Status == this.Status && tmp.Type == this.Type)
                {
                    return true;
                }
            }
            return false;
        }
        public override int GetHashCode()
        {
            return (int)this.Status ^ (int)this.Type;
        }

    }
    public class ShipmentStatusGroup
    {
        public ShipmentStatus Status { get; set; }

        public override bool Equals(object obj)
        {
            ShipmentDataGroup tmp = obj as ShipmentDataGroup;
            if (tmp != null)
            {
                if (tmp.Status == this.Status)
                {
                    return true;
                }
            }
            return false;
        }
        public override int GetHashCode()
        {
            return (int)this.Status;
        }
    }
}
