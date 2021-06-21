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

        public bool IsSuccess { get; set; }
        public List<MessageItem> Messages { get; set; }
        public static CQRSResponse Ok()
        {
            return new CQRSResponse
            {
                IsSuccess = true
            };
        }
        public static CQRSResponse Error(List<MessageItem> messageItems)
        {

            return new CQRSResponse
            {
                IsSuccess = false,
                Messages = messageItems
            };
        }
        public static CQRSResponse Error(MessageItem messageItem)
        {
            return Error(new List<MessageItem> { messageItem });
        }
    }
    public record CQRSResponseData<T> : CQRSResponse
    {

        public T Data { get; set; }
     
    }
}
