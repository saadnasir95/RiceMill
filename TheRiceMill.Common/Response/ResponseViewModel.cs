using System.Collections.Generic;

namespace TheRiceMill.Common.Response
{
    public class ResponseViewModel
    {
        public Dictionary<string,object> Response { get; set; }
        public int Status { get; set; }

        public ResponseViewModel CreateOk()
        {
            Status = 200;
            return this;
        }
        public ResponseViewModel CreateOk(object data)
        {
            Status = 200;
            Response = new Dictionary<string, object>()
            {
                { "data",data }
            };
            return this;
        }
        public ResponseViewModel CreateOk(object data, int count)
        {
            Status = 200;
            Response = new Dictionary<string, object>()
            {
                { "data",data },
                { "count",count },
            };
            return this;
        }
    }

    public class ResponseBadRequestViewModel
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }

    public class ResponseExceptionViewModel
    {
        public string Exception { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }

    public class CustomResponseExceptionViewModel
    {
        public string Message { get; set; }

    }
}