using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using MonoTorrent.BEncoding;
using MonoTorrent.Common;
using MonoTorrent.Client;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Utorrents
{
    class TorrentDownl : INotifyPropertyChanged
    {
        bool flag = false;

        string _programPath;
        string _downloadPath;
        string _fastResumeFile;
        string _torrentPath;
        ClientEngine _engine;
        TorrentManager _manager;

        string speed;
        string size;
        string name;
        string downloaded;
        double progress;

        public string Speed
        {
            get { return speed; }

            set { speed = value; OnPropertyChanged("Speed"); }
        }

        public string Size
        {
            get { return size; }

            set { size = value; OnPropertyChanged("Size"); }
        }

        public string Name
        {
            get { return name; }

            set { name = value; OnPropertyChanged("Name"); }
        }

        public string Downloaded
        {
            get { return downloaded; }

            set { downloaded = value; OnPropertyChanged("Downloaded"); }
        }

        public double Progress
        {
            get { return progress; }

            set { progress = value;  OnPropertyChanged("Progress"); }
        }

        public void Start(string torrentPath, string downloadPath)
        {
            _programPath = Environment.CurrentDirectory;
            _torrentPath = torrentPath;
            _downloadPath = downloadPath;
            _fastResumeFile = _programPath + "\\temp.data";

            DoDownload();
        }

        private void DoDownload()
        {
            int _port;
            _port = 31337;

            Torrent _torrent;
            EngineSettings _engineSettings = new EngineSettings();
            TorrentSettings _torrentDef = new TorrentSettings(5, 100, 0, 0);

            _engineSettings.SavePath = _downloadPath;
            _engineSettings.ListenPort = _port;

            _engine = new ClientEngine(_engineSettings);

            BEncodedDictionary _fastResume;

            try
            {
                _fastResume = BEncodedValue.Decode<BEncodedDictionary>(File.ReadAllBytes(_fastResumeFile));
            }
            catch
            {
                _fastResume = new BEncodedDictionary();
            }

            try
            {
                _torrent = Torrent.Load(_torrentPath);
            }
            catch
            {
                Console.Write("Decoding error");
                _engine.Dispose();
                return;
            }

            if (_fastResume.ContainsKey(_torrent.InfoHash))
                _manager = new TorrentManager(_torrent, _downloadPath, _torrentDef, new FastResume((BEncodedDictionary)_fastResume[_torrent.InfoHash]));
            else
                _manager = new TorrentManager(_torrent, _downloadPath, _torrentDef);

            _engine.Register(_manager);

            _manager.Start();

            int i = 0;
            bool _running = true;

            StringBuilder _stringBuilder = new StringBuilder(1024);

            Name = _manager.Torrent.Name;
            Size = (_torrent.Size / (1024.0 * 1024.0)).ToString("0.00")+ "MB";

            while (_running)
            {
                if ((i++) % 10 == 0)
                {
                    if (_manager.State == TorrentState.Stopped)
                    {
                        _running = false;
                        exit();
                    }

                    _stringBuilder.Remove(0, _stringBuilder.Length);

                    Speed = (_manager.Monitor.DownloadSpeed / 1024.0).ToString("0.00")+"KB/S";
                    Downloaded = (_manager.Monitor.DataBytesDownloaded / (1024.0 * 1024.0)).ToString("0.00")+"MB";
                    Progress = _manager.Progress;

                }

                if (flag)
                    return;

                System.Threading.Thread.Sleep(250);
            }
        }
        
        public void exit()
        {
            BEncodedDictionary fastResume = new BEncodedDictionary();

            WaitHandle handle = _manager.Stop(); ;

            fastResume.Add(_manager.Torrent.InfoHash, _manager.SaveFastResume().Encode());

            File.WriteAllBytes(_fastResumeFile, fastResume.Encode());

            _engine.Dispose();

            foreach (TraceListener lst in Debug.Listeners)
            {
                lst.Flush();
                lst.Close();
            }

            flag = true;
        }

        public void Pause()
        {
            _manager.Pause();
        }

        public void Continue()
        {
            _manager.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
