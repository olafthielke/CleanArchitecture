﻿using Amazon;
using Amazon.SimpleEmail;

namespace Notification.Email.AWS.Interfaces
{
    public interface IAmazonSimpleEmailServiceClientFactory
    {
        IAmazonSimpleEmailService Create(RegionEndpoint region);
    }
}
