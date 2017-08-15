

using System;
using System.Collections.Generic;

namespace Domain.Services.Validates
{
    public class RnException: Exception
    {
        public override string Message { get; }
        
        public RnException(string message)
        {
            Message = message;
        }


    }
}
