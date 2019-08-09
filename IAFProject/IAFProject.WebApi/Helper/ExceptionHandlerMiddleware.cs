using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IAFProject.BLL.Models.General;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace IAFProject.WebApi.Helper
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ExceptionHandlerMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate?.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }
        Task HandleException(HttpContext context, Exception ex)
        {
            string message = ex.Message;

            ResponseWrapper<object> responseModel = new ResponseWrapper<object>(true, null, message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            string result = JsonConvert.SerializeObject(responseModel);
            return context.Response.WriteAsync(result);
        }
    }
}
