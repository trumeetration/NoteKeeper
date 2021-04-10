using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NoteKeeper.Interfaces;
using Notifications.Wpf;

namespace NoteKeeper
{
    class ViewModel : BaseViewModel
    {
        public IMemory TextNoteStorage { get; }
        public NotificationManager NotifyManager;

        public ViewModel()
        {
            TextNoteStorage = new TextNotesDB();
            NotifyManager = new NotificationManager();
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
                TextNoteStorage.Add(new TextNote() {
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

        public ICommand MinimizeApp
        {
            get => new RelayCommand(() =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            }, () => true);
        }

        public ICommand DeleteNote
        {
            get => new RelayCommand<TextBox>(x =>
            {
                TextNoteStorage.Remove(x.Text);
            }, x => true);
        }

        public ICommand CopyToClipBoard
        {
            get => new RelayCommand<TextBox>(x =>
            {
                Clipboard.SetText(x.Text);
                NotifyManager.Show(new NotificationContent
                {
                    Title = "Copied",
                    Message = "Note has been copied to clipboard",
                    Type = NotificationType.Information
                });
            }, x => true);
        }
    }

    internal class TextNote
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
