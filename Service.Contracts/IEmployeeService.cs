using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetEmployees(Guid companyId);
        EmployeeDto GetEmployee(Guid companyId, Guid employeeId);
        EmployeeDto CreateEmployee(Guid companyId, EmployeeForCreationDto employee);
        void DeleteEmployeeForCompany(Guid companyId, Guid employeeId);
    }
}
