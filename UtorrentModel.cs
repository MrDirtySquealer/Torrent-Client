using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Utorrents
{
    class UtorrentModel : INotifyPropertyChanged
    {
        public delegate void StartDownloadTorrent(string _torrentPath, string _donwloadPath);

        AddTorrentWindow addTorrentWindow;

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                (addCommand = new RelayCommand(obj =>
                {
                    addTorrentWindow = new AddTorrentWindow();
                    addTorrentWindow.ShowDialog();


                    if (addTorrentWindow._torrentPath != null && addTorrentWindow._downloadPath != null)
                        if (addTorrentWindow._torrentPath != "" && addTorrentWindow._downloadPath != "")
                        {
                            TorrentDownl torrentDownl = new TorrentDownl();
                            StartDownloadTorrent startDownloadTorrent = new StartDownloadTorrent(torrentDownl.Start);

                            startDownloadTorrent.BeginInvoke(addTorrentWindow._torrentPath, addTorrentWindow._downloadPath, null, null);

                            TorrentsDownl.Insert(0, torrentDownl);
                        }
                        else
                            MessageBox.Show("Вы должны указать путь к .torrent файлу и указать папку загрузки.","Confirmation");

                }));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                (removeCommand = new RelayCommand(obj =>
                {
                    TorrentDownl torrentDownl = obj as TorrentDownl;
                    if (torrentDownl != null)
                    {
                        torrentDownl.exit();
                        TorrentsDownl.Remove(torrentDownl);
                    }
                },
                (obj) => TorrentsDownl.Count > 0));
            }
        }

        private RelayCommand continueCommand;
        public RelayCommand ContinueCommand
        {
            get
            {
                return continueCommand ??
                (continueCommand = new RelayCommand(obj =>
                {
                    TorrentDownl torrentDownl = obj as TorrentDownl;
                    if (torrentDownl != null)
                    {
                        torrentDownl.Continue();
                    }
                },
                (obj) => TorrentsDownl.Count > 0));
            }
        }

        private RelayCommand aboutProgrammCommand;
        public RelayCommand AboutProgrammCommand
        {
            get
            {
                return aboutProgrammCommand ??
                (aboutProgrammCommand = new RelayCommand(obj =>
                {
                    AboutProgramm aboutProgramm = new AboutProgramm();
                    aboutProgramm.ShowDialog();
                    

                }));
            }
        }

        private RelayCommand pauseCommand;
        public RelayCommand PauseCommand
        {
            get
            {
                return pauseCommand ??
                (pauseCommand = new RelayCommand(obj =>
                {
                    TorrentDownl torrentDownl = obj as TorrentDownl;
                    if (torrentDownl != null)
                    {
                        torrentDownl.Pause();
                    }
                },
                (obj) => TorrentsDownl.Count > 0));
            }
        }

        ObservableCollection<TorrentDownl> torrentsDownl;

        public ObservableCollection<TorrentDownl> TorrentsDownl
        {
            get { return torrentsDownl; }
            set { torrentsDownl = value; OnPropertyChanged("TorrentsDownl"); }
        }

        private TorrentDownl selectedTorrent;

        public TorrentDownl SelectedTorrent
        {
            get { return selectedTorrent; }

            set { selectedTorrent = value; OnPropertyChanged("SelectedTorrent"); }
        }

        public UtorrentModel()
        {
            TorrentsDownl = new ObservableCollection<TorrentDownl>();

           

        }

        ~UtorrentModel()
        {
            //for (int i = 0; i < TorrentsDownl.Count; i++)
            //{
            //    TorrentsDownl[i].exit();
            //    Thread.ResetAbort();
            //    TorrentsDownl.Remove(TorrentsDownl[i]);
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
