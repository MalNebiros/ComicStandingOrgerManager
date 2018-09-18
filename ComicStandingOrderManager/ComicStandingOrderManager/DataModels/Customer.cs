using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicStandingOrderManager.DataModels
{
    internal class Customer : ICustomer
    {
        private int _id;
        private string _firstName;
        private string _lastName;
        private string _email;
        private IList<IComicSeries> _standingOrders;

        internal Customer()
        {

        }
    }
}
