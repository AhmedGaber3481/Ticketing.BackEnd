using System.Globalization;
using LinkDev.Ticketing.Core.Models;

namespace LinkDev.Ticketing.Resources
{
    public class MessagesHelper
    {
        public static string? GetString(string resourceKey, CultureInfo culture)
        {
            return Messages.ResourceManager.GetString(resourceKey, culture);
        }

        public static ResponseErrorMessage[] GetErrorMessages(ResponseErrorMessage[] messages, CultureInfo culture)
        {
            if (messages != null)
            {
                foreach (ResponseErrorMessage message in messages) 
                { 
                    if(message != null && message.ErrorCode != null)
                    {
                        message.ErrorMessage = Messages.ResourceManager.GetString(message.ErrorCode.ToString(), culture);
                    }
                }
            }
            return messages;
        }
    }
}
