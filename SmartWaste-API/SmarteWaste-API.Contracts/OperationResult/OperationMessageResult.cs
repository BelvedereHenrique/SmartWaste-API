using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.OperationResult
{
    public class OperationMessageResult
    {
        public OperationMessageResult(bool isError, String message) {
            this.IsError = isError;
            this.Message = message;
        }
                
        public bool IsError { get; set; }
        public String Message { get; set; }
    }
}
