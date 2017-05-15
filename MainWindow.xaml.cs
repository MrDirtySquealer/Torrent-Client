
using System.Windows;

namespace Utorrents
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    

    public partial class MainWindow : Window
    {
        UtorrentModel utorrentModel;

        public MainWindow()
        {
            utorrentModel = new UtorrentModel();
            InitializeComponent();
            DataContext = utorrentModel;
            
        }

        ~MainWindow()
        {
            utorrentModel = null;
        }

    }

}
