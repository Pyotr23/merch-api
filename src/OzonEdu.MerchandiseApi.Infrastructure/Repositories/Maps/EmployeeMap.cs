using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Maps
{
    internal static class EmployeeMap
    {
        internal static Employee CreateEmployee(Models.Employee? employeeModel, Models.ClothingSize? sizeModel)
        {
            if (employeeModel is null)
                return null;

            var email = employeeModel.EmailAddress is null
                ? null
                : new EmailAddress(employeeModel.EmailAddress);

            var managerEmail = employeeModel.ManagerEmailAddress is null
                ? null
                : new EmailAddress(employeeModel.ManagerEmailAddress);

            var size = sizeModel is null
                ? null
                : new ClothingSize(sizeModel.Id.Value, sizeModel.Name);

            return new Employee(
                employeeModel.EmployeeId,
                new Name(employeeModel.Name),
                email,
                managerEmail,
                size);
        }
    }
}