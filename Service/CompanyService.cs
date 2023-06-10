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
    public sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        public CompanyDto CreateCompany(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);

            _repositoryManager.Companies.Create(companyEntity);
            _repositoryManager.Save();

            var companyDto = _mapper.Map<CompanyDto>(companyEntity);

            return companyDto;
        }

        public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();

            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);

            foreach (var company in companyEntities)
                _repositoryManager.Companies.Create(company);

            _repositoryManager.Save();

            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

            return (companies: companyCollectionToReturn, ids: ids);
        }

        public IEnumerable<CompanyDto> GetAllCompanies()
        {
            var companies = _repositoryManager.Companies.
                   FindAll(trackChanges: false)
                   .OrderBy(c => c.Name)
                   .ToList();

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return companiesDto;
        }

        public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var companyEntities = _repositoryManager.Companies
                .FindByCondition(c => ids.Contains(c.Id), trackChanges: false)
                .ToList();

            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            return companiesToReturn;
        }

        public CompanyDto GetCompany(Guid companyId)
        {
            var company = _repositoryManager.Companies
                .FindByCondition(c => c.Id.Equals(companyId), trackChanges: false)
                .FirstOrDefault();

            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var companyDto = _mapper.Map<CompanyDto>(company);

            return companyDto; 
        }
    }
}
