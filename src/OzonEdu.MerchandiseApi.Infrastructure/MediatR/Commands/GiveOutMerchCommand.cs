using MediatR;

namespace OzonEdu.MerchandiseApi.Infrastructure.MediatR.Commands
{
    public class GiveOutMerchCommand : IRequest
    {
        public int EmployeeId { get; set; }
        public int MerchPackTypeId { get; set; }

        /// <summary>
        ///     Приходил ли самостоятельно сотрудник за мерчем.
        /// </summary>
        public bool IsManual { get; set; }
    }
}