using Miles.Sample.Domain.Command.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Domain.Read.Leagues
{
    public interface ILeagueReader
    {
        Task<List<League>> GetLeaguesAsync();
    }
}
