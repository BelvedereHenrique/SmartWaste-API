using SmarteWaste_API.Contracts.OperationResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Contracts.Tests
{
    public static class OperationResultHelper
    {
        public static OperationResult GetSuccess(int messages)
        {
            var result = new OperationResult();

            for (int i = 0; i < messages; i++)
                result.AddWarning(Guid.NewGuid().ToString());

            return result;
        }

        public static OperationResult<T> GetSuccess<T>(T data, int messages)
        {
            var result = new OperationResult<T>() {
                Result = data
            };

            for (int i = 0; i < messages; i++)
                result.AddWarning(Guid.NewGuid().ToString());

            return result;
        }

        public static OperationResult<T> GetFail<T>(T data, int messages)
        {
            var result = new OperationResult<T>()
            {
                Result = data
            };

            for (int i = 0; i < messages; i++)
                result.AddError(Guid.NewGuid().ToString());

            return result;
        }
    }
}
