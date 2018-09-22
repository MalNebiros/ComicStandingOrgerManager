using ComicStandingOrderManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComicStandingOrderManager.Views
{
    /// <summary>
    /// Interaction logic for CustomersEntryView.xaml
    /// </summary>
    public partial class CustomersEntryView : Page
    {
        CustomersEntryViewModel _customerEntryViewModel;
        public CustomersEntryView()
        {
            InitializeComponent();
            _customerEntryViewModel = new CustomersEntryViewModel();
            DataContext = _customerEntryViewModel;
        }
    }
}
