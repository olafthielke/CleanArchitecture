using Notification.SMS.Interfaces;
using Notification.SMS.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using TwilioTypes = Twilio.Types;

namespace Notification.SMS.Twilio
{
    public class TwilioSmsSender(TwilioConfiguration config) : ISmsSender
    {
        private TwilioConfiguration Config { get; } = config;

        public async Task Send(SmsMessage message)
        {
            TwilioClient.Init(Config.AccountSID, Config.AuthToken);

            var _ = await MessageResource.CreateAsync(
                body: message.Body,
                from: new TwilioTypes.PhoneNumber(message.From),
                to: new TwilioTypes.PhoneNumber(message.To));
        }
    }
}
