﻿using System;

namespace ESOC.CLTPull.WebApi
{
    /// <summary>
    /// Base class used by API responses
    /// </summary>
    public abstract class BaseResponse : BaseMessage
    {
        public BaseResponse(Guid correlationId) : base()
        {
            base._correlationId = correlationId;
        }

        public BaseResponse()
        {
        }
    }
}