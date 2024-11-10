using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exeptions
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string Error { get; private set; }
        public string UserMessage { get; private set; }

        protected Result(bool isSuccess, string error, string userMessage)
        {
            IsSuccess = isSuccess;
            Error = error;
            UserMessage = userMessage;
        }
        public static Result Ok() => new Result(true, null, null);
        public static Result Fail(string error, string userMessage = "Bir şeyler ters gitti.")
        {
            LogError(error);
            return new Result(false, error, userMessage);
        }

        private static void LogError(string error)
        {
            Log.Error($"Hata: {error}");
        }
    }
}