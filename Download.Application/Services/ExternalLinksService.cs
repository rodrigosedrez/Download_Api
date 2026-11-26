using AutoMapper;
using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Application.DTOs;
using Download.Application.Interfaces;

namespace Download.Application.Services
{
    public class ExternalLinksService(IExternalLinksRepository externalLinksRepository, IMapper mapper) : IExternalLinksService
    {
        private readonly IExternalLinksRepository _externalLinksRepository = externalLinksRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ExternalLinksModelDTO> AddExternalLinkAsync(ExternalLinksModelDTO externalLinksModelDTO)
        {
            var externalLink = _mapper.Map<ExternalLinksModel>(externalLinksModelDTO);
            var addedLink = await _externalLinksRepository.AddExternalLinkAsync(externalLink);
            return _mapper.Map<ExternalLinksModelDTO>(addedLink);
        }

        public async Task<ExternalLinksModelDTO> UpdateExternalLinkAsync(ExternalLinksModelDTO externalLinksModelDTO)
        {
            var externalLink = _mapper.Map<ExternalLinksModel>(externalLinksModelDTO);
            var test = await _externalLinksRepository.UpdateExternalLinkAsync(externalLink);
            return _mapper.Map<ExternalLinksModelDTO>(test);
        }

        public async Task<ExternalLinksModelDTO> DeleteExternalLinkAsync(int id)
        {
            var deletedLink = await _externalLinksRepository.DeleteExternalLinkAsync(id);
            return _mapper.Map<ExternalLinksModelDTO>(deletedLink);
        }

        public async Task<ExternalLinksModelDTO?> GetExternalLinkByIdAsync(int id)
        {
            var externalLink = await _externalLinksRepository.GetExternalLinkByIdAsync(id);
            return _mapper.Map<ExternalLinksModelDTO>(externalLink);
        }

        public async Task<IEnumerable<ExternalLinksModelDTO>> GetAllExternalLinksAsync()
        {
            var externalLinks = await _externalLinksRepository.GetAllExternalLinksAsync();
            return _mapper.Map<IEnumerable<ExternalLinksModelDTO>>(externalLinks);
        }
    }
}