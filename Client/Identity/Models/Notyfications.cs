using Radzen;

namespace Client.Identity.Models
{
    public static class Notyfications
    {

        public static NotificationMessage ErrorNotyfication(string details) => new()
        {
            Severity = NotificationSeverity.Error,
            Summary = "Error",
            Detail = details,
            Duration = 4000
        };

        public static NotificationMessage SuccessNotyfication(string details) => new()
        {
            Severity = NotificationSeverity.Success,
            Summary = "Success",
            Detail = details,
            Duration = 4000
        };
    }
}
