using System;
using System.Collections.Generic;
using System.Text;

namespace IAFProject.BLL.Models.General
{
    public class ResponseWrapper<T>
    {
        public ResponseWrapper()
        {

        }

        public ResponseWrapper(bool hasError, string message = null)
        {
            HasError = hasError;
            Message = message;
        }

        public ResponseWrapper(bool hasError, T data, string message = null) : this(hasError, message)
        {
            Data = data;
        }

        public bool HasError { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
