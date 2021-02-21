using System.Collections.Generic;
using System.Threading.Tasks;
using Angular_ASPNET_Core.Data.Entities;

namespace Angular_ASPNET_Core.Data
{
    public interface IStatesRepository
    {
        Task<List<State>> GetStatesAsync();
    }
}