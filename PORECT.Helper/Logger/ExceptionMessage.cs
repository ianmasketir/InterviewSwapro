using System;

namespace PORECT.Helper
{
	public class ExceptionMessage
	{
		public ExceptionMessage(Exception ex)
		{
			StackTrace = ex.StackTrace;
			Message = ex.Message;
			if(ex.InnerException!= null)
			{
				InnerException = new ExceptionMessage( ex.InnerException );
			}
		}

		public string StackTrace { get; private set; }
		public string Message { get; private set; }
		public ExceptionMessage InnerException { get; private set; }
	}
}