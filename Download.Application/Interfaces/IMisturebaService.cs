

using Download.Application.DTOs;

namespace Download.Application.Interfaces
{
    public interface IMisturebaService
    {
        Task<MisturebaModelDTO> AddMisturebaAsync(MisturebaModelDTO misturebaModelDTO);
        Task<MisturebaModelDTO> UpdateMisturebaAsync(MisturebaModelDTO misturebaModelDTO);
        Task<MisturebaModelDTO> DeleteMisturebaAsync(int id);
        Task<MisturebaModelDTO?> GetMisturebaByIdAsync(int id);
        Task<IEnumerable<MisturebaModelDTO>> GetAllMisturebasAsync();
    }
}
                    