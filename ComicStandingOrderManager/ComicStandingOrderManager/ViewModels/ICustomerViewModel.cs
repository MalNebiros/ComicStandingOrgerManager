using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicStandingOrderManager.ViewModels
{
    public interface ICustomerViewModel
    {
        string firstName { get; set; }
        string lastName { get; set; }
        string emailAddress { get; set; }
        IList<string> namesOfSeriesSubscribedTo { get; set; }
    }
}
