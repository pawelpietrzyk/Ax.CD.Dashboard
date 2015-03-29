using Ax.CD.Dashboard.Service;
using Ax.CD.Dashboard.Service.Model;
using Ax.CD.Dashboard.Board;
using Ax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Dashboard.Model;
using Web.Client.Rest;
using System.ComponentModel;
using System.IO;

namespace Ax.CD.Dashboard.Worker
{
    public class DashboardWorker : INotifyPropertyChanged
    {
        private Timer timer;
        private DateTime dateStart;
        private DateTime dateEnd;        
        private FancyListing fancyListing;
        private string apiUrl;        
        private double timeInterval;            
        private int days;
        public event EventHandler Refreshed;
        private WmsShipmentSum wmsShipmentSum;
        private List<string> messages;

        public DashboardWorker()
        {
            this.customDate = DateTime.Now;
            //this.SetStartTime(new TimeSpan(23, 59, 59));
            this.timer = new Timer();
            this.timer.Elapsed += TimerElapsed;
            this.timer.Disposed += TimerDisposed;                
        }
        public void SetStartTime(TimeSpan timeSpan)
        {
            DateTime tmp = DateTime.Now;
            this.StartTime = new DateTime(tmp.Year, tmp.Month, tmp.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        public void SetDates()
        {
            DateTime tmp = DateTime.Now;            
            this.dateEnd = new DateTime(tmp.Year, tmp.Month, tmp.Day, startTime.Hour, startTime.Minute, startTime.Second);
            this.dateStart = this.dateEnd.AddDays(-this.Days);
        }        
        public void SetInterval()
        {
            this.timer.Interval = this.TimeInterval;
        }
        public void Stop()
        {
            this.timer.Stop();
            this.State = DashboardWorkerState.Stopped;
        }
        public void Start()
        {
            this.State = DashboardWorkerState.Started;
            this.timer.Start();            
        }
        public void Refresh()
        {            
            DateTime tmp = DateTime.Now;
            if (this.isCustomDate)
            {
                tmp = this.customDate;
            }
            else
            {
                this.customDate = tmp;
            }
            wmsShipmentSum = this.CallWmsShipmentSumGet(tmp, this.itemOwnerId);
            if (wmsShipmentSum != null)
            {
                this.PushPieShipmentsChart(wmsShipmentSum);                
            }            
        }
        public void Refresh_old()
        {
            List<WmsShipment> shipments = this.CallWmsShipmentGet(this.dateStart, this.dateEnd);
            if (shipments != null)
            {
                this.PushPieShipmentsChart(shipments);
                this.PushFancyShipmentList(shipments);
                this.OnRefreshed();
            }
        }
        protected void OnRefreshed()
        {
            if (Refreshed != null)
            {
                Refreshed(this, new EventArgs());
            }
        }
        private void PushPieShipmentsChart(WmsShipmentSum data)
        {
            if (data != null)
            {
                BoardPanel panel = new BoardPanel();
                string title = String.Format("{0:d MMM yyyy} {1:t} Razem {2}", this.customDate, DateTime.Now, data.All);
                PieChart pieChart = panel.CreatePieChart(this.pieChartShipments, "pie_chart", title, data);
                pieChart.Url = this.apiUrl;
                Log.AddMsg(String.Format("Send to {0}. {1}", pieChart.Url, pieChart.Data));
                Http.post(pieChart.UrlPush, pieChart.DataChart, new HttpParams(), Http.DefaultContentType, this.httpTimeout).then(pushPieComplete).Async();
            }
        }
        private void PushFancyShipmentList(WmsShipmentSum data)
        {
            if (data != null)
            {
                BoardPanel panel = new BoardPanel();
                string title = String.Format("{0} {1:d} {1:t}", "Shipments", this.customDate);
                fancyListing = panel.CreateFancyListing(this.fancyChartShipments, data);
                fancyListing.Url = this.apiUrl;
                Log.AddMsg(String.Format("Send to {0}. {1}", fancyListing.Url, fancyListing.Data));
                Http.post(fancyListing.UrlConfig, fancyListing.DataConfig, new HttpParams(), Http.DefaultContentType, this.httpTimeout).then(fancyConfigComplete).Async();

            }
        }
        private void PushPieShipmentsChart(List<WmsShipment> data)
        {
            if (data != null)
            {
                BoardPanel panel = new BoardPanel();
                string title = String.Format("{0:d MMM yyyy} {1:t} Razem {2}", this.dateEnd, DateTime.Now, data.Count);
                PieChart pieChart = panel.CreatePieChart("pie1", "pie_chart", title, data);
                pieChart.Url = this.ApiUrl;
                Http.post(pieChart.UrlPush, pieChart.DataChart, new HttpParams(), Http.DefaultContentType, 10000).then(pushPieComplete).Async();
            }
        }
        private void PushFancyShipmentList(List<WmsShipment> data)
        {
            if (data != null)
            {
                BoardPanel panel = new BoardPanel();
                string title = String.Format("{0} {1:d} {1:t}", "Shipments", this.dateEnd);
                fancyListing = panel.CreateFancyListing("fancy1", data);
                fancyListing.Url = this.ApiUrl;
                Http.post(fancyListing.UrlConfig, fancyListing.DataConfig, new HttpParams()).then(fancyConfigComplete).Async();            
                
            }
        }
        private WmsShipmentSum CallWmsShipmentSumGet(DateTime date, string itemOwnerId)
        {
            WmsShipmentSumRequest request = new WmsShipmentSumRequest();
            request.Date = date;
            request.ItemOwnerId = itemOwnerId;
            WmsShipmentSumResponse response = AxMethods.WmsShipmentSumGet(request);            
            if (response != null)
            {
                if (response.Sum != null)
                {
                    response.Sum.WaitLabel = String.Format("{0}", WorkerSettings.Default.WaitLabel);
                    response.Sum.ActivatedLabel = String.Format("{0}", WorkerSettings.Default.ActivatedLabel);
                    response.Sum.InProgressLabel = String.Format("{0}", WorkerSettings.Default.InProgressLabel);
                    response.Sum.PickedLabel = String.Format("{0}", WorkerSettings.Default.PickedLabel);
                    response.Sum.ShippedLabel = String.Format("{0}", WorkerSettings.Default.ShippedLabel);
                    response.Sum.StoppedLabel = String.Format("{0}", WorkerSettings.Default.StoppedLabel);
                }
                return response.Sum;
            }
            return null;
        }
        private List<WmsShipment> CallWmsShipmentGet(DateTime dateFrom, DateTime dateTo)
        {
            WmsShipmentsRequest request = new WmsShipmentsRequest();
            request.DateFrom = dateFrom;
            request.DateTo = dateTo;
            WmsShipmentsResponse response = AxMethods.WmsShipmentGet(request);
            if (response != null)
            {
                return response.Items;
            }
            return null;
        }
        private void fancyConfigComplete(object sender, HttpResponseEventArgs e)
        {
            Log.AddMsg(String.Format("FancyConfigComplete: {0} {1}", e.StatusCode, e.Content));
            if (e.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Http.post(fancyListing.UrlPush, fancyListing.DataChart, new HttpParams(), Http.DefaultContentType, this.httpTimeout).then(pushFancyComplete).Async();
            }

            
        }
        private void pushPieComplete(object sender, HttpResponseEventArgs e)
        {
            Http http = sender as Http;
            if (http != null)
            {
                if (e != null)
                {
                    Log.AddMsg(String.Format("PushPieComplete: {0} {1}", e.StatusCode, e.Content));
                }
                this.PushFancyShipmentList(wmsShipmentSum);                   
            }
            
        }
        private void pushFancyComplete(object sender, HttpResponseEventArgs e)
        {
            if (e != null)
            {
                Log.AddMsg(String.Format("PushFancyComplete: {0} {1}", e.StatusCode, e.Content));
            }
            OnRefreshed();
            
        }
        bool CheckContinue()
        {
            if (maxRefresh != 0 && counter >= maxRefresh)
            {
                return false; 
            }
            return true;
        }
        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (this.CheckContinue())
            {
                this.Refresh();
            }
            else
            {

                Timer timer = sender as Timer;
                if (timer != null)
                {
                    timer.Stop();
                }
            }
            
        }

        void TimerDisposed(object sender, EventArgs e)
        {
            this.timer.Stop();
        }

        #region Properties

        public List<string> Messages
        {
            get
            {
                return this.messages;
            }
        }
        private DashboardWorkerState state;
        public DashboardWorkerState State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }
        private int counter;

        public int Counter
        {
            get { return counter; }
            set
            {
                counter = value;
                OnPropertyChanged("Counter");
            }
        }
        public int Days
        {
            get { return days; }
            set
            {
                days = value;
                this.SetDates();
                OnPropertyChanged("Days");
            }
        }
        public double TimeInterval
        {
            get { return timeInterval; }
            set
            {
                timeInterval = value;
                this.SetInterval();
                OnPropertyChanged("TimeInterval");
            }
        }
        public string ApiUrl
        {
            get { return apiUrl; }
            set
            {
                apiUrl = value;
                OnPropertyChanged("ApiUrl");
            }
        }
        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        private string itemOwnerId;
        public string ItemOwnerId
        {
            get { return itemOwnerId; }
            set
            {
                itemOwnerId = value;
                OnPropertyChanged("ItemOwnerId");
            }
        }

        private string apiUrl2;
        public string ApiUrl2
        {
            get { return apiUrl2; }
            set
            {
                apiUrl2 = value;                
            }
        }

        private DateTime customDate;
        public DateTime CustomDate
        {
            get { return customDate; }
            set
            {
                customDate = value;
                OnPropertyChanged("CustomDate");
            }
        }
        private string logFile;
        public string LogFile
        {
            get { return logFile; }
            set { logFile = value; }
        }
        private bool isCustomDate;
        public bool IsCustomDate
        {
            get { return isCustomDate; }
            set
            {
                isCustomDate = value;
                OnPropertyChanged("IsCustomDate");
            }
        }
        private int httpTimeout;
        public int HttpTimeout
        {
            get { return httpTimeout; }
            set { httpTimeout = value; }
        }
        private string pieChartShipments;
        public string PieChartShipments
        {
            get { return pieChartShipments; }
            set { pieChartShipments = value; }
        }
        private string fancyChartShipments;
        public string FancyChartShipments
        {
            get { return fancyChartShipments; }
            set { fancyChartShipments = value; }
        }
        private int maxRefresh;
        public int MaxRefresh
        {
            get { return maxRefresh; }
            set
            {
                maxRefresh = value;
                OnPropertyChanged("MaxRefresh");
            }
        }
        #endregion

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class Log
    {
        static Log()
        {
            
        }
        private static string directory;
        public static string Directory
        {
            get { return Log.directory; }
            set { Log.directory = value; }
        }

        private static string filePath;

        public static string FilePath
        {
            get { return Log.filePath; }
            set { Log.filePath = value; }
        }
        private static string createFileName(string prefix)
        {
            string f = String.Format("{0}_{1:ddMMyyyy}.log", prefix, DateTime.Now); 
            if (!String.IsNullOrEmpty(directory))
            {
                DirectoryInfo di = new DirectoryInfo(directory);
                if (di.Exists)
                {
                    f = String.Format("{0}\\{1}", directory, f);
                }                
            }
            return f;
        }
        public static void AddMsg(string msg)
        {
            string s = String.Format("{0:d MMM yyyy HH:mm:ss}; {1}", DateTime.Now, msg);
            Add(s);
        }
        public static void Add(string msg)
        {
            try
            {
                filePath = createFileName("dash");
                StreamWriter writer = File.AppendText(filePath);
                if (writer != null)
                {
                    writer.WriteLine(msg);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        public static void AddData(WmsShipmentSum sum)
        {
            if (sum != null)
            {
                try
                {
                    WmsShipmentData data = new WmsShipmentData();
                    data.DateTime = DateTime.Now;
                    data.Total = sum.All;
                    data.Activated = sum.Activated;
                    data.InProgress = sum.InProgress;
                    data.Picked = sum.Picked;
                    data.Shipped = sum.Shipped;
                    data.Stopped = sum.Stopped;
                    DataService.Add(data);
                }
                catch (Exception ex)
                {
                    Log.AddMsg(ex.Message);
                }
            }
        }

    }
    public enum DashboardWorkerState
    {
        Stopped,
        Started
    }
}
