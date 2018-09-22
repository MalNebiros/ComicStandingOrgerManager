using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicStandingOrderManager.ViewModels
{
    internal class CustomersEntryViewModel
    {
        public int customerId;
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public int numberOfSeriesSubscribedTo { get; set; }
    }
}
