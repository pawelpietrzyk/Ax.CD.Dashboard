using ITPiAST.Axapta50Helper;
using ITPiAST.AxaptaHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ax.CD.Dashboard.Service
{
    public static class Ax
    {
        private static AxaptaConnector connector = null;        

        static Ax()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);            
            if (connector == null && AxConf.ConnectionType != "NAV")
            {
                CreateConnector();
                Logon();
            }
        }
        public static AxaptaConnector Connector
        {
            get { return Ax.connector; }
        }

        public static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            try
            {
                if (connector != null)
                {
                    connector.Logoff();
                    connector.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }
        private static void CreateConnector()
        {
            if (AxConf.Version == "50")
            {
                connector = new Axapta50Connector(AxConf.OperationTimeLimit, AxConf.BatchTimeLimit);
            }
        }
        private static void Logon()
        {
            connector.Logon(AxConf.Company, AxConf.Language, AxConf.ObjectServer, AxConf.ConfigFile);
        }
    }
    public class AxConf
    {
        #region Properties

        private static string connectionType;
        public static string ConnectionType
        {
            get { return connectionType; }
            set { connectionType = value; }
        }
        private static string company;

        public static string Company
        {
            get { return company; }
            set { company = value; }
        }
        private static string language;

        public static string Language
        {
            get { return language; }
            set { language = value; }
        }
        private static string objectServer;

        public static string ObjectServer
        {
            get { return objectServer; }
            set { objectServer = value; }
        }
        private static string configFile;

        public static string ConfigFile
        {
            get { return configFile; }
            set { configFile = value; }
        }
        private static string version;

        public static string Version
        {
            get { return version; }
            set { version = value; }
        }
        private static int operationTimeLimit;

        public static int OperationTimeLimit
        {
            get { return operationTimeLimit; }
            set { operationTimeLimit = value; }
        }
        private static int batchTimeLimit;

        public static int BatchTimeLimit
        {
            get { return batchTimeLimit; }
            set { batchTimeLimit = value; }
        }
        
        #endregion
    }
    
}
