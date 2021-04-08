using Corex.ExceptionHandling.Manager;
using System;

namespace VerticalSliceArchitectureSample.WebApi.ExceptionHandler
{
    public class ExceptionManager : BaseExceptionManager
    {
        public ExceptionManager(Exception ex) : base(ex)
        {
        }
    }
}
