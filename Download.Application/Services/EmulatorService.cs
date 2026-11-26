using AutoMapper;
using Download.Application.DTOs;
using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Application.Interfaces;

namespace Download.Application.Services
{
    public class EmulatorService(IEmulatorRepository emulatorRepository, IMapper mapper) : IEmulatorService
    {
        private readonly IEmulatorRepository _emulatorRepository = emulatorRepository;
        private readonly IMapper _mapper = mapper;


        public async Task<EmulatorsModelDTO> AddEmulatorAsync(EmulatorsModelDTO emulatorsModelDTO)
        {
            var emulatorsModel = _mapper.Map<EmulatorsModel>(emulatorsModelDTO);
            var result = await _emulatorRepository.AddEmulatorAsync(emulatorsModel);
            return _mapper.Map<EmulatorsModelDTO>(result);
        }


        public async Task<EmulatorsModelDTO> UpdateEmulatorAsync(EmulatorsModelDTO emulatorsModelDTO)
        {
            var emulatorsModel = _mapper.Map<EmulatorsModel>(emulatorsModelDTO);
             await _emulatorRepository.UpdateEmulatorAsync(emulatorsModel);
            return _mapper.Map<EmulatorsModelDTO>(emulatorsModel);
        }
        public async Task<EmulatorsModelDTO> DeleteEmulatorAsync(int id)
        {
            var result = await _emulatorRepository.DeleteEmulatorAsync(id);
            return _mapper.Map<EmulatorsModelDTO>(result);
        }
        public async Task<EmulatorsModelDTO?> GetEmulatorByIdAsync(int id)
        {
            var result = await _emulatorRepository.GetEmulatorByIdAsync(id);
            return _mapper.Map<EmulatorsModelDTO>(result);
        }
        public async Task<EmulatorsModelDTO?> GetEmulatorBySlugAsync(string slug)
        {
            var result = await _emulatorRepository.GetEmulatorBySlugAsync(slug);
            return _mapper.Map<EmulatorsModelDTO>(result);
        }
        public async Task<IEnumerable<EmulatorsModelDTO>> GetAllEmulatorsAsync()
        {
            var result = await _emulatorRepository.GetAllEmulatorsAsync();
            return _mapper.Map<IEnumerable<EmulatorsModelDTO>>(result);
        }
        public async Task<IEnumerable<EmulatorsModelDTO>> GetEmulatorsByPlataformIdAsync(int plataformId)
        {
            var result = await _emulatorRepository.GetEmulatorsByPlataformIdAsync(plataformId);
            return _mapper.Map<IEnumerable<EmulatorsModelDTO>>(result);
        }


    }
}
