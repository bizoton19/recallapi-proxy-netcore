using Opendata.Recalls.Commands;
using Opendata.Recalls.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Opendata.Recalls.Repository
{
    public interface IRecallApiProxyRepository
    {
        Task<List<Recalls.Models.Recall>> RetrieveRecall(SearchCommand command);
    }
}