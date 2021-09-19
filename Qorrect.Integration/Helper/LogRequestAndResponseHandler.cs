using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Qorrect.Integration.Models;

namespace Qorrect.Integration.Helper
{
    public class LogRequestAndResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
           HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var requestHeaders = new HashSet<DTOHeaderKey>();

            request.Headers.ForEach(item =>
            {
                requestHeaders.Add(new DTOHeaderKey()
                {
                    Key = item.Key,
                    Value = item.Value?.ToHashSet()
                });
            });


            var method = request.Method.Method;

            var requestUri = request.RequestUri.ToString();
            var pathandQuery = request.RequestUri.PathAndQuery;

            var req = new DTOClientRequest()
            { 
                RequestUri = requestUri,
                MethodType = method,
                Device = "",
                CourseId = ""
            };

            var requestBody = await request.Content.ReadAsStringAsync();
            req.RequestBody = requestBody;


            HttpResponseMessage result;
            try
            {
                // let other handlers process the request
                result = await base.SendAsync(request, cancellationToken);

                if (result.Content != null)
                {
                    var lll = result.SerializeJson();
                    var resStatus = result.StatusCode.ToString();
                    req.Status = resStatus;
                    var responseBody = await result.Content.ReadAsStringAsync();
                    req.ResponseBody = responseBody;
                }
                else
                {
                    req.Status = "Content Null";
                }
            }
            catch (Exception e)
            {
                result = null;
                req.Status = "Null Response";
            }

            ServiceLogger.LogRequestAndResponse(req);

            return result;
        }
    }
}
