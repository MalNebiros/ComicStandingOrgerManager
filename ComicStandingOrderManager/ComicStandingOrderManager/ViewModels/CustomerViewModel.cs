using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicStandingOrderManager.ViewModels
{
    internal class CustomerViewModel : ICustomerViewModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public IList<string> namesOfSeriesSubscribedTo { get; set; }
    }
}
