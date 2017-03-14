using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.OperationResult
{
    public class OperationResult
    {
        public OperationResult()
        {
            Messages = new List<OperationMessageResult>();
        }

        public bool Success
        {
            get
            {
                return Messages.All(message => !message.IsError);
            }
        }

        public string GetMessage(bool? errors = null)
        {
            if (Messages.Count == 0)
                return String.Empty;

            var builder = new StringBuilder();

            Messages.Where(x => x.IsError == errors || errors == null)
                .Aggregate(builder, (b, message) => b.Append(message.Message));

            return builder.ToString();
        }

        public List<OperationMessageResult> Messages { get; set; }

        public void AddError(String message)
        {
            AddMessage(true, message);
        }

        public void AddWarning(String message)
        {
            AddMessage(false, message);
        }

        public void AddMessage(bool isError, String message)
        {
            Messages.Add(new OperationMessageResult(isError, message));
        }

        public void Merge(OperationResult operationResult)
        {
            this.Messages.AddRange(operationResult.Messages);
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public OperationResult() : base() { }
        public OperationResult(T result) : base()
        {
            this.Result = result;
        }

        public T Result { get; set; }
    }
}
