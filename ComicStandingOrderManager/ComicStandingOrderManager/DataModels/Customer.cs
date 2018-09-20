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

        internal Customer(string firstName, string _lastName, string email)
        {
            _email = email;
        }

        internal Customer(string firstName, string _lastName, string email, IList<IComicSeries> standingOrders) : this(firstName, lastName, email)
        {
            _standingOrders = standingOrders;
        }
    }
}
