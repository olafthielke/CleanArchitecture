using Notification.SMS.Models;

namespace Notification.SMS.Twilio
{
    public class TwilioWhatsAppSender(TwilioConfiguration config) 
        : TwilioSmsSender(config)
    {
        public override async Task Send(SmsMessage message)
        {
            var whatsAppMsg = new WhatsAppMessage(message);

            await base.Send(whatsAppMsg);
        }


        private class WhatsAppMessage(SmsMessage message) 
            : SmsMessage(
                Format(message.From), 
                Format(message.To), 
                message.Body)
        {
            private static string Format(string mobileNumber) => $"whatsapp:{mobileNumber}";
        }
    }
}
