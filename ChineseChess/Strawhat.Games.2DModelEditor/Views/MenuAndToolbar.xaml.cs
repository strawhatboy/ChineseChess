using Strawhat.Games._2DGame.Infrastructure;
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

namespace Strawhat.Games._2DModelEditor.Views
{
    /// <summary>
    /// Interaction logic for MenuAndToolbar.xaml
    /// </summary>
    public partial class MenuAndToolbar : UserControl
    {
        public MenuAndToolbar()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        
    }
}
