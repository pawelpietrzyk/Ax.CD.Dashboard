using Ax.CD.Dashboard.Service;
using Ax.CD.Dashboard.WindowsService.Properties;
using Ax.CD.Dashboard.Worker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Ax.CD.Dashboard.WindowsService
{
    public partial class Service : ServiceBase
    {
        DashboardWorker worker;

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            worker = new DashboardWorker();
            if (args != null)
            {
                if (args.Length > 0)
                {
                    Log.Directory = args[0];
                }
                if (args.Length > 1)
                {
                    int max = 0;
                    if (int.TryParse(args[1], out max))
                    {
                        worker.MaxRefresh = max;
                    }
                }
                
            }
            this.initAxConf();
            
            worker.ApiUrl = Settings.Default.ApiURL;
            //worker.ApiUrl2 = Settings.Default.ApiURL2;
            worker.TimeInterval = Settings.Default.TimeInterval;            
            worker.Days = Settings.Default.Days;
            worker.ItemOwnerId = Settings.Default.ItemOwnerId;
            worker.HttpTimeout = Settings.Default.HttpTimeout;
            worker.PieChartShipments = Settings.Default.PieChartShipments;
            worker.FancyChartShipments = Settings.Default.FancyChartShipments;
            worker.Refreshed += worker_Refreshed;
            worker.Start();
            Log.AddMsg("Start worker");
            
        }

        protected override void OnStop()
        {
            worker.Stop();
            Log.AddMsg("Stopped worker");
        }
        public void initAxConf()
        {
            AxConf.Company = Settings.Default.Company;
            AxConf.Language = Settings.Default.Language;
            AxConf.ObjectServer = Settings.Default.ObjectServer;
            AxConf.ConfigFile = Settings.Default.ConfigFile;
            AxConf.Version = Settings.Default.Version;
            try
            {
                AxConf.OperationTimeLimit = int.Parse(Settings.Default.OperationTimeLimit);
                AxConf.BatchTimeLimit = int.Parse(Settings.Default.BatchTimeLimit);
            }
            catch (Exception)
            {
                AxConf.OperationTimeLimit = 60000;
                AxConf.BatchTimeLimit = 120000;
            }
        }
        delegate void SetCounterCallback();
        public void SetCounter()
        {
            worker.Counter++;            
        }
        void worker_Refreshed(object sender, EventArgs e)
        {
            this.SetCounter();
        }

    }
}
