using System;
using System.Collections.Generic;
using System.Text;

namespace IAFProject.BLL.Models.General
{
    public class ResponseModel<T>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public static T Data { get; set; }
        
        public ResponseModel()
        {

        }

        public ResponseModel(bool hasError, string message = null)
        {
            HasError = hasError;
            Message = message;
        }

        public ResponseModel(bool hasError, T data, string message = null) : this(hasError, message)
        {
            Data = data;
        }
    }
}
