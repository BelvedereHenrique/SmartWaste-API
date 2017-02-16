using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartWaste_API.Models
{
    public class JsonModel<T>
    {
        private const string GENERIC_ERROR_MESSAGE = "An Error occurred in your request!";

        public JsonModel()
        {
            this.Messages = new List<JsonResultMessageModel>();
        }
        public JsonModel(T result) : this()
        {
            this.Result = result;
        }

        public JsonModel(Exception exception) : this()
        {
            this.AddError(exception);
        }
        public List<JsonResultMessageModel> Messages { get; set; }
        public T Result { get; set; }
        public bool Success
        {
            get
            {
                return this.Messages.All(x => !x.IsError);
            }
        }
        public void AddError(Exception ex)
        {
            this.AddError(ex != null ? ex.Message : GENERIC_ERROR_MESSAGE);
        }

        public void AddError(string error)
        {
            this.Messages.Add(new JsonResultMessageModel()
            {
                IsError = true,
                Message = error
            });
        }
        public void AddWarning(string warning)
        {
            this.Messages.Add(new JsonResultMessageModel()
            {
                IsError = false,
                Message = warning
            });
        }
    }
    public class JsonResultMessageModel
    {
        public string Message { get; set; }
        public bool IsError { get; set; }
    }
}