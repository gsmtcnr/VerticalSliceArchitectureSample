using Corex.Model.Infrastructure;
using System.Collections.Generic;

namespace VerticalSliceArchitectureSample.WebApi.Results
{
    public class ResultModel : BaseResultModel
    {
        public static ResultModel Ok()
        {
            return new ResultModel
            {
                IsSuccess = true
            };
        }
        public static ResultModel Error(List<MessageItem> messageItems)
        {

            return new ResultModel
            {
                IsSuccess = false,
                Messages = messageItems
            };
        }
        public static ResultModel Error(MessageItem messageItem)
        {
            return Error(new List<MessageItem> { messageItem });
        }
    }
}
