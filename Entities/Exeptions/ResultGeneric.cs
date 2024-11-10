using Serilog;

namespace Entities.Exeptions
  

{
    public  class ResultGeneric<T>
    {
        public bool IsSuccess { get; private set; }
        public string Error { get; private set; }
        public string UserMessage { get; private set; }
        public T Data { get; private set; }

        private ResultGeneric(bool isSuccess, T data, string error, string userMessage)
        {
            IsSuccess = isSuccess;
            Data = data; 
            Error = error;
            UserMessage = userMessage;
        }

        public static ResultGeneric<T> Ok(T data) => new ResultGeneric<T>(true, data, null, null);
        public static ResultGeneric<T> Fail(string error, string userMessage = "Bir şeyler ters gitti.")
        {
            LogError(error);  
            return new ResultGeneric<T>(false,default, error, userMessage);
        }

        private static void LogError(string error)
        {
            Log.Error($"Hata: {error}");  
        }
    }
}
