using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicStandingOrderManager.DataModels
{
    internal class ComicSeries : IComicSeries
    {
        internal int Id { get; private set; }
        internal string Name { get; private set; }
        internal string Publisher { get; private set; }

        internal ComicSeries(int id, string name, string publisher)
        {
            Id = id;
            Name = name;
            Publisher = publisher;
        }
    }
}
