using CSharpCourse.Core.Lib.Events;
using MediatR;

namespace OzonEdu.MerchandiseApi.Domain.Events
{
    public class EmployeeNotificationDomainEvent : INotification
    {
        public NotificationEvent NotificationEvent { get; }

        public EmployeeNotificationDomainEvent(NotificationEvent notificationEvent)
        {
            NotificationEvent = notificationEvent;
        }
    }
}