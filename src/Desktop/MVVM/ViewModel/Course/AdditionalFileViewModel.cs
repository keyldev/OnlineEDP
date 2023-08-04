using OnlineEDP.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineEDP.MVVM.ViewModel.Course
{
    internal class AdditionalFileViewModel : ObservableObject
    {
        private string _headerText = "Edit me()";

        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; NotifyPropertyChanged(); }
        }
        private string _fileName = "Седер Наоми - Python. Экспресс-курс. 3-е изд. 2019";

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; NotifyPropertyChanged(); }
        }
        private string _fileURL = "vk.com";

        public string FileURL
        {
            get { return _fileURL; }
            set { _fileURL = value; NotifyPropertyChanged(); }
        }
        private bool _isReadOnly = true;

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; NotifyPropertyChanged(); }
        }
        public RelayCommand OpenBrowserLink { get; set; }
        public AdditionalFileViewModel()
        {
            OpenBrowserLink = new RelayCommand(o =>
            {
                try
                {
                    // добавить команды: скачать, копировать и т.п.
                    Process.Start(FileURL);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message.ToString());
                }
            });
        }

    }
}
