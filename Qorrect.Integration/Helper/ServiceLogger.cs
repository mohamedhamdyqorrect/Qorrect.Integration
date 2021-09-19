using Qorrect.Integration.Models;
using System;
using System.Collections.Generic;

namespace Qorrect.Integration.Helper
{
    public static class ServiceLogger
    {

        public static void LogRequestAndResponse(DTOClientRequest clientRequest)
        {
            try
            {
                var _hedear = new HashSet<DTOHeaderKey>();
                var _xForwardedFor = new HashSet<string>();

                clientRequest.Headers.ForEach(item =>
                {
                    if (item.Key.ToLower() == "x-forwarded-for")
                        _xForwardedFor = item.Value;

                    _hedear.Add(new DTOHeaderKey()
                    {
                        Key = item.Key,
                        Value = item.Value
                    });
                }
                );

                var RequestUri = clientRequest.RequestUri;
                var MethodType = clientRequest.MethodType.Length >= 10
                                ? clientRequest.MethodType.Substring(0, 10)
                                : clientRequest.MethodType;
                var Headers = _hedear.SerializeJson();
                var xForwardedFor = _xForwardedFor.SerializeJson();
                var RequestBody = clientRequest.RequestBody;
                var ResponseBody = clientRequest.ResponseBody;
                var Status = clientRequest.Status;
                var CourseId = clientRequest.CourseId;
                var Device = clientRequest.Device;

                LogRequestAndResponseToDb(RequestUri, MethodType, Headers, RequestBody, ResponseBody, Status, CourseId, Device);
            }
            catch (Exception ex)
            {

            }
        }

        public static void LogRequestAndResponseToDb(string requestUri, string methodType, string headers, string requestBody, string responseBody, string status, string courseId, string device)
        {
            //using (HyperPayContext _ctx = new HyperPayContext())
            //{
            //    string query = $"EXECUTE SP_CreateLogger '{userHostName}','{userHostAddress}','{origin}','{requestUri}','{pathAndQuery}','{methodType}','{headers}','{xForwardedFor}','{requestBody}','{responseBody.Replace("'", "\"")}','{status}',{userId}";
            //    _ctx.Database.SqlQuery<string>(query).FirstOrDefault();
            //}
        }
    }
}
