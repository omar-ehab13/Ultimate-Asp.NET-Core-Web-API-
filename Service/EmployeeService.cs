using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid employeeId)
        {
            var company = _repositoryManager.Companies
                .FindByCondition(c => c.Id == companyId, trackChanges: false)
                .FirstOrDefault();

            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employee = _repositoryManager.Employees
                .FindByCondition(e => e.CompanyId == companyId && e.Id == employeeId, trackChanges: false)
                .FirstOrDefault();

            if (employee is null)
                throw new EmployeeNotFoundException(employeeId);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public IEnumerable<EmployeeDto> GetEmployees(Guid companyId)
        {
            var company = _repositoryManager.Companies
                .FindByCondition(c => c.Id == companyId, trackChanges: false)
                .FirstOrDefault();

            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employees = _repositoryManager.Employees
                .FindByCondition(e => e.CompanyId == companyId, trackChanges: false)
                .OrderBy(e => e.Name)
                .ToList();

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;
        }
    }
}
