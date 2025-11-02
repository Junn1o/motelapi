using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface ITierRepositories
    {
        TiersListResult GetlAllTier(int pageNumber = 1, int pageSize = 1000);
        TiersNoIdDTO GetTierById(int id);
        AddTiersDTO AddTier(AddTiersDTO addTiersDTO);
        AddTiersDTO UpdateTierById(int id, AddTiersDTO tiersNoIdDTO);
        Tiers? DeleteTierById(int id);

    }
}
