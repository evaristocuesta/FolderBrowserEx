using FolderBrowserEx;
using System.Windows;

namespace NetFrameworkSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(new FolderBrowserDialog());
        }
    }
}
