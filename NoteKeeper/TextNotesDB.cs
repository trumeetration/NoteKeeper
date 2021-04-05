using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeper.Interfaces;

namespace NoteKeeper
{
    class TextNotesDB : IMemory
    {
        public ObservableCollection<TextNote> Memory { get; }
        private string _dbName;

        public TextNotesDB()
        {
            _dbName = "data.db";
            Memory = Memory ?? new ObservableCollection<TextNote>();
            if (File.Exists(_dbName) == false)
            {
                SQLiteConnection.CreateFile(_dbName);
                using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + _dbName))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText =
                            @"CREATE TABLE text_notes (
                                hint VARCHAR NOT NULL,
                                value VARCHAR NOT NULL)";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + _dbName))
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand("SELECT * FROM text_notes", connection);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Memory.Add(new TextNote()
                        {
                            Key = dt.Rows[i][0].ToString(),
                            Value = dt.Rows[i][1].ToString()
                        });
                    }
                }
            }
        }

        public int Count => Memory.Count;

        public void Add(TextNote textNote)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + _dbName))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = @"INSERT INTO text_notes
                                        VALUES (@key, @note)";
                command.Parameters.AddWithValue("@key", textNote.Key);
                command.Parameters.AddWithValue("@note", textNote.Value);
                command.ExecuteNonQuery();
            }
            Memory.Add(textNote);
        }

        public void Remove(int index)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = " + _dbName))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = @"DELETE FROM text_notes
                                        WHERE rowid=@noteid";
                command.Parameters.AddWithValue("@noteid", (index+1).ToString());
                command.ExecuteNonQuery();
            }
            Memory.RemoveAt(index);
        }
    }
}
