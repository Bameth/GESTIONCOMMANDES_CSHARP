using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace GESTIONCOMMANDES.services
{
    public class SmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendSmsAsync(string toPhoneNumber, string message)
        {
            var accountSid = _configuration["TwilioSettings:AccountSid"];
            var authToken = _configuration["TwilioSettings:AuthToken"];
            var fromPhoneNumber = _configuration["TwilioSettings:FromPhoneNumber"];

            TwilioClient.Init(accountSid, authToken);

            await MessageResource.CreateAsync(
                to: new Twilio.Types.PhoneNumber(toPhoneNumber),
                from: new Twilio.Types.PhoneNumber(fromPhoneNumber),
                body: message
            );
        }
    }
}
