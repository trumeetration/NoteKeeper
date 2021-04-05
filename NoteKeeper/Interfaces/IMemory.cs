using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteKeeper.Interfaces
{
    interface IMemory
    {
        ObservableCollection<TextNote> Memory { get; }
        int Count { get; }
        void Add(TextNote textNote);
        void Remove(int index);

    }
}
