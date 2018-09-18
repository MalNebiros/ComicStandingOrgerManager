using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicStandingOrderManager.DataModels
{
    internal class Customer : ICustomer
    {
        private int Id;
        private string FirstName;
        private string LastName;
        private string Email;
        private IList<IComicSeries> StandingOrders;
    }
}
