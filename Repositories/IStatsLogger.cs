using Opendata.Recalls.Stats;

namespace Opendata.Recalls.Repository
{
    public interface IStatsLogger
    {
        void Log(BaseStats installInfo);
    }
}