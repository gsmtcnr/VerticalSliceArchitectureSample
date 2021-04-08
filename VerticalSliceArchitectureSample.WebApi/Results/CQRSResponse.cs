using Corex.Model.Infrastructure;
using System.Collections.Generic;
using System.Net;

namespace VerticalSliceArchitectureSample.WebApi.Results
{
    public record CQRSResponse
    {
        public CQRSResponse()
        {
            Messages = new List<MessageItem>();
            IsSuccess = true;
        }
        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;
        public bool IsSuccess { get; set; }
        public List<MessageItem> Messages { get; set; }
    }
    public record CQRSResponseData<T> : CQRSResponse
    {

        public T Data { get; set; }
      
    }
}
