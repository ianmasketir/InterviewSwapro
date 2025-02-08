using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using NLog.Targets;

namespace PORECT.Helper
{
    public class CommonLog
    {
        NLog.Logger Log = LogManager.GetCurrentClassLogger();

        public NLog.Logger Logging = NLog.LogManager.GetLogger("Logging");

        public void logExecutionTime(string Level, string Module, string Endpoint, string Position)//, params String[] additionalData)
        {
            StringBuilder sb = new StringBuilder();
            LogManager.Configuration.Variables["module"] = Module;
            LogManager.Configuration.Variables["Position"] = Position;
            LogManager.Configuration.Variables["Endpoint"] = Endpoint;
            CheckLevel(Level);
        }

        public void logExecutionTime(string Level, string Module, string Endpoint, string Position, string message)//, params String[] additionalData)
        {
            StringBuilder sb = new StringBuilder();
            LogManager.Configuration.Variables["module"] = Module;
            LogManager.Configuration.Variables["Position"] = Position;
            LogManager.Configuration.Variables["Endpoint"] = Endpoint;
            LogManager.Configuration.Variables["message"] = message;
            CheckLevel(Level);
        }

        public void CheckLevel(string Level)
        {
            if (Level == "Info")
            {
                this.Logging.Info("");
            }
            else if (Level == "Error")
            {
                this.Logging.Error("");
            }
        }
    }
}

