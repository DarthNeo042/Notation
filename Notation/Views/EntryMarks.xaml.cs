using Notation.ViewModels;
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
using System.Windows.Shapes;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour InputMarks.xaml
    /// </summary>
    public partial class EntryMarks : Window
    {
        public EntryMarks()
        {
            EntryMarksViewModel entryMarks = new EntryMarksViewModel();
            DataContext = entryMarks;
            InitializeComponent();
        }
    }
}
