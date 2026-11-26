

using AutoMapper;
using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Application.DTOs;
using Download.Application.Interfaces;

namespace Download.Application.Services
{
   
    public class MisturebaService(IMisturebaRepository misturebaRepository, IMapper mapper) : IMisturebaService
    {  
    private readonly IMisturebaRepository _misturebaRepository = misturebaRepository;
    private readonly IMapper _mapper = mapper;
    
        public async Task<MisturebaModelDTO> AddMisturebaAsync(MisturebaModelDTO misturebaModelDTO)
        {
            var misturebaModel = _mapper.Map<MisturebaModel>(misturebaModelDTO);
            var addedMistureba = await _misturebaRepository.AddMisturebaAsync(misturebaModel);
            return _mapper.Map<MisturebaModelDTO>(addedMistureba);
        }
        public async Task<MisturebaModelDTO> UpdateMisturebaAsync(MisturebaModelDTO misturebaModelDTO)
        {
            var misturebaModel2 = _mapper.Map<MisturebaModel>(misturebaModelDTO);
             await _misturebaRepository.UpdateMisturebaAsync(misturebaModel2);
            return _mapper.Map<MisturebaModelDTO>(misturebaModel2);
        }
        public async Task<MisturebaModelDTO> DeleteMisturebaAsync(int id)
        {
            var deletedMistureba = await _misturebaRepository.DeleteMisturebaAsync(id);
            return _mapper.Map<MisturebaModelDTO>(deletedMistureba);
        }
        public async Task<MisturebaModelDTO?> GetMisturebaByIdAsync(int id)
        {
            var misturebaModel = await _misturebaRepository.GetMisturebaByIdAsync(id);
            return _mapper.Map<MisturebaModelDTO>(misturebaModel);
        }
        public async Task<IEnumerable<MisturebaModelDTO>> GetAllMisturebasAsync()
        {
            var misturebas = await _misturebaRepository.GetAllMisturebasAsync();
            return _mapper.Map<IEnumerable<MisturebaModelDTO>>(misturebas);
        }
    }

}
