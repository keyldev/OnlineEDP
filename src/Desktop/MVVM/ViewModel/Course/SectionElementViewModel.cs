using OnlineEDP.Core;
using OnlineEDP.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEDP.MVVM.ViewModel.Course
{
    internal class SectionElementViewModel : ObservableObject
    {
        private string _sectionHeaderText = "change me()";

        public string SectionHeaderText
        {
            get { return _sectionHeaderText; }
            set { _sectionHeaderText = value; NotifyPropertyChanged(); Debug.WriteLine(_sectionHeaderText); }
        }

        private bool _isReadOnly = true;

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<object> _courseElementsList;

        public ObservableCollection<object> CourseElementsList
        {
            get { return _courseElementsList; }
            set { _courseElementsList = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<string> _controlsComboBox = new ObservableCollection<string>()
        {
            "Подраздел",
            "Заголовок&описание",
            "Файл(название/ссылка)"
        };

        public ObservableCollection<string> ControlsComboBox
        {
            get { return _controlsComboBox; }
            set { _controlsComboBox = value; NotifyPropertyChanged(); }
        }
        private int _selectedControlIndex;
        //
        public int SelectedControlIndex
        {
            get { return _selectedControlIndex; }
            set { _selectedControlIndex = value; NotifyPropertyChanged(); }
        }
        public RelayCommand AddControlCommand { get; set; }
        public SectionElementViewModel()
        {

            CourseElementsList = new ObservableCollection<object>();

            AddControlCommand = new RelayCommand(o =>
            {
                // создаем CourseUIModel и при нажатии сохранить вбиваем инфу и отсюда и с описания и т.п.
                switch (SelectedControlIndex)
                {
                    case 0: // подраздел
                        {
                            CourseElementsList.Add(new SectionElementViewModel() { IsReadOnly = false });
                            break;
                        }
                    case 1: // заголовок и описание
                        {
                            CourseElementsList.Add(new SimpleTextViewModel() { IsReadOnly = false });
                            break;
                        }
                    case 2: // файл
                        {
                            CourseElementsList.Add(new AdditionalFileViewModel() { IsReadOnly = false });
                            break;
                        }
                    default: // Заголовок и описание
                        {
                            CourseElementsList.Add(new SimpleTextViewModel() { IsReadOnly = false });
                            break;
                        }
                }
            });
        }
    }
}
