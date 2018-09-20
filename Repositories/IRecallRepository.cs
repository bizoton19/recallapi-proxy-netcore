using Opendata.Recalls.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Opendata.Recalls.Repository
{
    public interface IRecallRepository
    {
        Task<List<Recalls.Models.Recall>> RetrieveRecall(string searchfor, string productname, string manufacturername, string producttype, string productmodel, string recalldateend, string recalldatestart);

    }
}