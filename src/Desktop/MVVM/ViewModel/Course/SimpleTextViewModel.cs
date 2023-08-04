using OnlineEDP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEDP.MVVM.ViewModel.Course
{
    internal class SimpleTextViewModel : ObservableObject
    {
        private string _headerBlockText = "Простой заголовок";

        public string HeaderBlockText
        {
            get { return _headerBlockText; }
            set { _headerBlockText = value; NotifyPropertyChanged(); }
        }
        private string _descriptionBlockText = "Простое описание, привет";

        public string DescriptionBlockText
        {
            get { return _descriptionBlockText; }
            set { _descriptionBlockText = value; NotifyPropertyChanged(); }
        }
        /// <summary>
        /// Если роль пользователя - учитель, тогда readonly - false, и он может редачить инфу
        /// </summary>
        private bool _isReadOnly = true;

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; NotifyPropertyChanged(); }
        }
        public SimpleTextViewModel()
        {

        }
    }
}
