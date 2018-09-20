using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicStandingOrderManager.DataModels
{
    internal class Customer : ICustomer
    {
        internal int Id { get; private set; }
        internal string FirstName { get; private set; }
        internal string LastName { get; private set; }
        internal string Email { get; private set; }
        internal IList<IComicSeries> StandingOrders { get; private set; }

        internal Customer(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        internal Customer(string firstName, string lastName, string email, IList<IComicSeries> standingOrders) : this(firstName, lastName, email)
        {
            StandingOrders = standingOrders;
        }
    }
}
