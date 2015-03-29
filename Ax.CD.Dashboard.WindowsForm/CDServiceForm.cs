using Ax.CD.Dashboard.Service;
using Ax.CD.Dashboard.WindowsForm.Properties;
using Ax.CD.Dashboard.Worker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ax.CD.Dashboard.WindowsForm
{
    public partial class CDServiceForm : Form
    {
        DashboardWorker worker;
        public CDServiceForm()
        {
            InitializeComponent();
            this.initAxConf();
            worker = new DashboardWorker();
            worker.ApiUrl = Settings.Default.apiUrl;
            worker.TimeInterval = Settings.Default.TimeInterval;
            worker.HttpTimeout = Settings.Default.HttpTimeout;
            worker.PieChartShipments = Settings.Default.PieChartShipments;
            worker.FancyChartShipments = Settings.Default.FancyChartShipments;
            worker.Days = Settings.Default.Days;
            worker.ItemOwnerId = Settings.Default.ItemOwnerId;
            worker.MaxRefresh = Settings.Default.MaxRefresh;
            worker.Refreshed += worker_Refreshed;
            this.Bind();
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
            if (this.txtItemOwnerId.InvokeRequired)
            {
                SetCounterCallback callback = new SetCounterCallback(SetCounter);
                this.Invoke(callback);
            }
            else
            {
                worker.Counter++;
                //this.txtCounter.Text = count.ToString();
            }
        }
        void worker_Refreshed(object sender, EventArgs e)
        {
            this.SetCounter();
        }
        public void Bind()
        {
            this.txtUrl.DataBindings.Add("Text", worker, "ApiUrl");
            this.txtDays.DataBindings.Add("Text", worker, "Days");
            this.txtInterval.DataBindings.Add("Text", worker, "TimeInterval");
            this.txtItemOwnerId.DataBindings.Add("Text", worker, "ItemOwnerId");
            this.customDateTime.DataBindings.Add("Value", worker, "CustomDate");
            this.cbxIsCustomDate.DataBindings.Add("Checked", worker, "IsCustomDate");
            this.lblCounter.DataBindings.Add("Text", worker, "Counter");
            this.lblState.DataBindings.Add("Text", worker, "State");            
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            worker.Start();
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            worker.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            worker.Refresh();
        }
    }
}
