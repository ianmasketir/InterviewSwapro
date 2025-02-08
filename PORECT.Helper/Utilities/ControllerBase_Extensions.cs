using PORECT.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public static class ControllerBase_Extensions
{
    public static void Try(this ControllerBase extBase, PORECTLog logger, string endpoint, Action action)
    {
        logger.logExecutionTime(NLog.LogLevel.Debug, extBase.GetType().Name, endpoint, "START", "-");
        try
        {
            action.Invoke();
        }
        catch (Exception ex)
        {
            //logger.Logging.Error( ex, ex.Message );
            ExceptionMessage exMsg = new ExceptionMessage(ex);
            logger.logExecutionTime(NLog.LogLevel.Error, extBase.GetType().Name, endpoint, string.Empty, JsonConvert.SerializeObject(exMsg));
            throw;
        }
        logger.logExecutionTime(NLog.LogLevel.Debug, extBase.GetType().Name, endpoint, "END", "-");
    }

    public static T Try<T>(this ControllerBase extBase, PORECTLog logger, string endpoint, Func<T> func)
    {
        logger.logExecutionTime(NLog.LogLevel.Debug, extBase.GetType().Name, endpoint, "START", "-");
        T result;
        try
        {
            result = func.Invoke();
        }
        catch (Exception ex)
        {
            ExceptionMessage exMsg = new ExceptionMessage(ex);
            logger.logExecutionTime(NLog.LogLevel.Error, extBase.GetType().Name, endpoint, string.Empty, JsonConvert.SerializeObject(exMsg));

            throw; //new Exception(ex.Message.ToString());
        }
        logger.logExecutionTime(NLog.LogLevel.Debug, extBase.GetType().Name, endpoint, "END", "-");
        return result;
    }

    public static T Try<T>(this ControllerBase extBase, BaseJsonResponse response, PORECTLog logger, string endpoint, Func<T> func)
    {
        logger.logExecutionTime(NLog.LogLevel.Debug, extBase.GetType().Name, endpoint, "START", "-");
        T result;
        try
        {
            result = func.Invoke();
        }
        catch (Exception ex)
        {
            ExceptionMessage exMsg = new ExceptionMessage(ex);
            logger.logExecutionTime(NLog.LogLevel.Error, extBase.GetType().Name, endpoint, string.Empty, JsonConvert.SerializeObject(exMsg));

            GenerateErrorResponse(ex, response, HttpCodeConstants.BadRequest);
            return default(T);
            //throw new Exception( ex.Message.ToString() ) ;
        }
        logger.logExecutionTime(NLog.LogLevel.Debug, extBase.GetType().Name, endpoint, "END", "-");
        return result;
    }

    public static IActionResult ToActionResult(this ControllerBase extBase, BaseJsonResponse response)
    {
        //return response.Header.Errors.Count == 0 ? extBase.Ok( response ) : (IActionResult)extBase.BadRequest( response );
        if (response.Header.Errors.Count == 0)
        {
            return extBase.Ok(response);
        }
        else
        {
            response.Data = response.Header.Errors[0];
            return extBase.BadRequest(response);
        }
    }

    private static void GenerateErrorResponse(Exception ex, BaseJsonResponse response, HttpCodeConstants errorCode)
    {
        //if (response.Header == null)
        //{
        //	response.Header = new BaseJsonResponseHeader();
        //}
        string errorMessage = string.Empty;
        string innerException = (ex.InnerException != null) ? ex.InnerException.ToString() : ex.StackTrace;
        //PORECTLog logger = new PORECTLog();
        //logger.WriteErrorToLog(ex, "ProcurementContractReminding.API", "ListAgreement");

        switch (errorCode)
        {
            case HttpCodeConstants.BadRequest:
                errorMessage = (ex.Message != null) ? "Bad Request: " + ex.Message : "Bad Request";
                break;
            case HttpCodeConstants.InternalServerError:
                errorMessage = "Internal Server Error";
                break;
            default:
                break;
        }
        BaseJsonResponseError errorResponse = new BaseJsonResponseError(errorMessage, innerException, ((int)errorCode).ToString());
        response.Header.Errors.Add(errorResponse);
        //return response;
    }
}
