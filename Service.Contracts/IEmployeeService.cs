using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        PagedList<EmployeeDto> GetEmployees(Guid companyId, EmployeeParameters employeeParameters);
        EmployeeDto GetEmployee(Guid companyId, Guid employeeId);
        EmployeeDto CreateEmployee(Guid companyId, EmployeeForCreationDto employee);
        void DeleteEmployeeForCompany(Guid companyId, Guid employeeId);
        void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate);
    }
}
