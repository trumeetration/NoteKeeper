using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NoteKeeper
{
    class ViewModel : BaseViewModel
    {

        public ObservableCollection<TextNote> TextNoteCollection { get; set; }

        public ViewModel()
        {
            TextNoteCollection = new ObservableCollection<TextNote>();
        }

        private string _note;
        private string _hintPhrase;
        public string Note
        {
            get => _note;
            set
            {
                if (_note != value)
                {
                    _note = value;
                    OnPropertyChanged(nameof(Note));
                }
            }
        }
        public string HintPhrase
        {
            get => _hintPhrase;
            set
            {
                if (_hintPhrase != value)
                {
                    _hintPhrase = value;
                    OnPropertyChanged(nameof(HintPhrase));
                }
            }
        }

        public ICommand AddNote
        {
            get => new RelayCommand(() => 
            {
                TextNoteCollection.Add(new TextNote() {
                    Key = HintPhrase,
                    Value = Note
                });
                Note = HintPhrase = string.Empty;
            }, () => true);
        }

        public ICommand CloseApp
        {
            get => new RelayCommand(() =>
            {
                Application.Current.Shutdown();
            }, () => true);
        }
    }

    internal class TextNote
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
