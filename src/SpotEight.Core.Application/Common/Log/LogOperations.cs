using Newtonsoft.Json;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SpotEight.Core.Application.Common.Log;

[ExcludeFromCodeCoverage]
public static class LogOperations
{
    /// <summary>
    /// Register start process (method start)
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="objectLog"></param>
    public static void LogStart(this object instance, object? objectLog = null)
    {
        var stackTrace = new StackTrace();
        var callerFrame = stackTrace.GetFrame(1); // 0 é o frame atual, 1 é o chamador
        var callerMethod = callerFrame?.GetMethod();
        var callerMethodName = callerMethod?.Name;

        if (objectLog != null)
            objectLog = $"Tracked object: {JsonConvert.SerializeObject(objectLog, SerializerSettings())}.";

        var timeOfDay = $"Registered at: {DateTime.UtcNow.ToShortDateString()} - {DateTime.UtcNow.ToShortTimeString()}";
        Serilog.Log.Information(callerMethodName != null ? $"[{timeOfDay}] - Start method {callerMethodName} in class {instance.GetType().Name}. Object logged: {objectLog}" :
                                         $"[{timeOfDay}] - Start class {instance.GetType().Name}.");
    }

    /// <summary>
    /// Register end process (method finish)
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="processDescription">Description from process monitoring</param>
    /// <param name="logObject">Some object that you want to register</param>
    public static void LogRegisterProcess(this object instance, string processDescription, object? logObject = null)
    {
        var objectLog = string.Empty;

        var stackTrace = new StackTrace();
        var callerFrame = stackTrace.GetFrame(1); // 0 é o frame atual, 1 é o chamador
        var callerMethod = callerFrame?.GetMethod();
        var callerMethodName = callerMethod?.Name;

        var timeOfDay = $"Registered at: {DateTime.UtcNow.ToShortDateString()} - {DateTime.UtcNow.ToShortTimeString()}";

        if (logObject != null)
            objectLog = $"Tracked object: {JsonConvert.SerializeObject(logObject, SerializerSettings())}.";

        Serilog.Log.Information(callerMethodName != null ? $"[{timeOfDay}] - Register process in method {callerMethodName} in class {instance.GetType().Name}. Process description:{processDescription} {objectLog}" :
                                         $"[{timeOfDay}] - Register process in class {instance.GetType().Name}. Process description:{processDescription} {objectLog}");
    }

    /// <summary>
    /// Register end process (method finish)
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="logObject">Some object that you want to register</param>
    public static void LogEnd(this object instance, object? logObject = null)
    {
        var objectLog = string.Empty;

        var stackTrace = new StackTrace();
        var callerFrame = stackTrace.GetFrame(1); // 0 é o frame atual, 1 é o chamador
        var callerMethod = callerFrame?.GetMethod();
        var callerMethodName = callerMethod?.Name;

        var timeOfDay = $"Registered at: {DateTime.UtcNow.ToShortDateString()} - {DateTime.UtcNow.ToShortTimeString()}";

        if (logObject != null)
            objectLog = $"Tracked object: {JsonConvert.SerializeObject(logObject, SerializerSettings())}.";

        Serilog.Log.Information(callerMethodName != null ? $"[{timeOfDay}] - Finish method {callerMethodName} in class {instance.GetType().Name}. {objectLog}" :
                                         $"[{timeOfDay}] - Finish class {instance.GetType().Name}. {objectLog}");
    }

    /// <summary>
    /// Register error process
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="logObject">Object that you need to register. ex: Exception object</param>
    /// <param name="parameters"></param>
    public static void LogError(this object instance, object logObject, object? parameters = null)
    {
        var timeOfDay = $"Error registered at: {DateTime.UtcNow.ToShortDateString()} - {DateTime.UtcNow.ToShortTimeString()}";

        var completeStackTrace = string.Empty;

        if (logObject is Exception exception)
        {
            completeStackTrace = exception.StackTrace;
        }

        var request = string.Empty;

        if (parameters is not null)
        {
            request = JsonConvert.SerializeObject(parameters, SerializerSettings());
        }

        var stackTrace = new StackTrace();
        var callerFrame = stackTrace.GetFrame(1); // 0 é o frame atual, 1 é o chamador
        var callerMethod = callerFrame?.GetMethod();
        var callerMethodName = callerMethod?.Name;

        var objectLog = $"Tracked object: {JsonConvert.SerializeObject(logObject, SerializerSettings())}.";

        Serilog.Log.Error(callerMethodName != null ? $"[{timeOfDay}] - Error ocurred in method {callerMethodName} in class {instance.GetType().Name}. {objectLog} (complete stack trace - {completeStackTrace ?? "not present"}) - Request: {request} " :
                                         $"[{timeOfDay}] - Error ocurred in class {instance.GetType().Name}. {objectLog} (complete stack trace - {completeStackTrace ?? "not present"}) - Request: {request} ");
    }


    /// <summary>
    /// Responsável por logar requests
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="urlParams"></param>
    /// <param name="requestBody"></param>
    /// <param name="statusCode"></param>
    public static void LogRequest(this object instance, string urlParams, string requestBody, int statusCode)
    {
        var timeOfDay = $"Registered at: {DateTime.UtcNow.ToShortDateString()} - {DateTime.UtcNow.ToShortTimeString()}";

        if (string.IsNullOrWhiteSpace(requestBody))
            requestBody = "*request without body*";

        var objectLog = $"Request Status: {statusCode} - Request body: {requestBody} - URL and params: {urlParams}";

        var listErrorStatus = new List<int>() { 500, 400 };
        if (listErrorStatus.Contains(statusCode))
            Serilog.Log.Error($"[{timeOfDay}] - {objectLog}");
        else
            Serilog.Log.Information($"[{timeOfDay}] - {objectLog}");

    }

    #region Private methods

    private static JsonSerializerSettings SerializerSettings()
    {
        return new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }

    #endregion Private methods


}