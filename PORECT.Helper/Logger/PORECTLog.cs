using System.Text;
using System.Text.Json;
using NLog;

namespace PORECT.Helper
{
	public class PORECTLog : CommonLog
	{
		new public void logExecutionTime( string level, string module, string endpoint, string position )//, params String[] additionalData)
		{
			logExecutionTime( level, module, endpoint, position, string.Empty );
		}

		new public void logExecutionTime( string level, string module, string endpoint, string position, string message )//, params String[] additionalData)
		{
			StringBuilder sb = new StringBuilder();
			LogManager.Configuration.Variables["module"] = module;
			LogManager.Configuration.Variables["Position"] = position;
			LogManager.Configuration.Variables["Endpoint"] = endpoint;
			LogManager.Configuration.Variables["message"] = message;
			CheckLevel( level ); //write the log to text file
		}

		public virtual void logExecutionTime( LogLevel level, string module, string endpoint, string position, string message = "" )//, params String[] additionalData)
		{
			logExecutionTime( level.Name, module, endpoint, position, message );
		}

		public virtual void LogDebug( string module, string endpoint, string position, string message = "" )
		{
			logExecutionTime( LogLevel.Debug, module, endpoint, position, message );
		}

		public virtual void LogInfo( string module, string endpoint, string position, string message = "" )
		{
			logExecutionTime( LogLevel.Info, module, endpoint, position, message );
		}

		public virtual void LogError( string module, string endpoint, string position, string message = "" )
		{
			logExecutionTime( LogLevel.Error, module, endpoint, position, message );
		}

		new public void CheckLevel( string Level )
		{
			if (Level.ToUpper() == LogLevel.Info.Name.ToUpper())
			{
				this.Logging.Info( "" );
			}
			else if (Level.ToUpper() == LogLevel.Debug.Name.ToUpper())
			{
				this.Logging.Debug( "" );
			}
			else if (Level.ToUpper() == LogLevel.Error.Name.ToUpper())
			{
				this.Logging.Error( "" );
			}
		}

		public void WriteErrorToLog(Exception ex, string module, string endpoint)
		{
			ExceptionMessage exMsg = new ExceptionMessage(ex);
			//logExecutionTime(NLog.LogLevel.Error, module, endpoint, string.Empty, JsonSerializer.Serialize(exMsg));
			logExecutionTime(NLog.LogLevel.Error, module, endpoint, string.Empty, Newtonsoft.Json.JsonConvert.SerializeObject(exMsg));
		}
	}
}