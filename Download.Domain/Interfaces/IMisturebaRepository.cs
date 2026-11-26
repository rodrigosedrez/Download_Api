using Download.Domain.Entities;

namespace Download.Domain.Interfaces
{
    public interface IMisturebaRepository
    {
        Task<IEnumerable<MisturebaModel>> GetAllMisturebasAsync();
        Task<MisturebaModel> GetMisturebaByIdAsync(int id);

        Task<MisturebaModel> AddMisturebaAsync(MisturebaModel mistureba);
        Task<MisturebaModel> UpdateMisturebaAsync(MisturebaModel mistureba);
        Task<MisturebaModel> DeleteMisturebaAsync(int id);
    }
}
