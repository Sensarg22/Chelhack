using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ChelHackApi
{
    public class ErrorModel
    {
        public ErrorModel()
        {
        }

        public ErrorModel(ModelStateDictionary modelState)
        {
            this.Code = ErrorCode.ValidationError.ToString();
            this.Message = "Fields validation is failed";
            this.PropertyErrors = (object)new SerializableError(modelState);
            this.MultipleErrors = new List<ErrorModel>();
        }


        public ErrorModel(ErrorCode code, string message)
        {
            this.Code = code.ToString();
            this.Message = message;
        }

        public ErrorModel(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public string Code { get; set; }

        public string Message { get; set; }

        public object PropertyErrors { get; set; }

        public List<ErrorModel> MultipleErrors { get; set; }
    }
}