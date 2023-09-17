using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface ITierRepositories
    {
        List<TiersDTO> GetlAllTier();
        TiersNoIdDTO GetTierById(int id);
        AddTiersDTO AddTier(AddTiersDTO addTiersDTO);
        TiersNoIdDTO UpdateTierById(int id, TiersNoIdDTO tiersNoIdDTO);
        Tiers? DeleteTierById(int id);

    }
}
