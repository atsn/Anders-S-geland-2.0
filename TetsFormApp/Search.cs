using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TetsFormApp
{
    public class Search : INotifyPropertyChanged
    {
        public string Output
        {
            get { return _output; }
            set { _output = value; OnPropertyChanged("Output"); }
        }

        private string _output;
        public bool casesensitive = false;
        private int index = 0;


        private List<string> UsedDirect = new List<string>();
        public BindingList<string> NotSerached { get; set; }
        public BindingList<string> Found { get; set; }
        private DateTime Lastchange = DateTime.Now;
        public DateTime Begintime = DateTime.Now;
        private char[] loading = new char[] { '-', '\\', '|', '/' };


        public event PropertyChangedEventHandler PropertyChanged;

        public Search()
        {
            Found = new BindingList<string>();
            NotSerached = new BindingList<string>();
        }

        public void SearchNow(string directory, string searchKriteria)
        {
            clear();
            serach(directory, searchKriteria);
            Output = "";

        }

        private void serach(string directory, string searchKriteria)
        {
            try
            {
                if (DateTime.Now - Lastchange > TimeSpan.FromMilliseconds(100))
                {
                    if (index > 3) index = 0;

                    var s = "searching[" + loading[index] + "]" + " Current Directory: " + directory;

                    Output = s;
                    index++;
                    Lastchange = DateTime.Now;
                }

                UsedDirect.Add(directory);
                var files = Directory.GetFiles(directory);

                foreach (var file in files)
                {
                    if (casesensitive)
                    {
                        if (file.Split('\\')[file.Split('\\').Length - 1].Contains(searchKriteria))
                        {

                            Found.Add(file);
                        }
                    }
                    else
                    {
                        if (file.Split('\\')[file.Split('\\').Length - 1].ToLower().Contains(searchKriteria))
                        {

                            Found.Add(file);
                        }
                    }

                }

                var directories = Directory.GetDirectories(directory);

                foreach (var directori in directories)
                {
                    if (!UsedDirect.Contains(directori))
                    {
                        serach(directori, searchKriteria);
                    }

                }
            }
            catch (System.UnauthorizedAccessException e)
            {
                NotSerached.Add(e.Message.Split('\'')[1]);
            }

        }

        private void clear()
        {
            UsedDirect.Clear();
            NotSerached.Clear();
            Found.Clear();
            Begintime = DateTime.Now;
            index = 0;
        }

        protected void OnPropertyChanged(string name)
        {

            var handler = PropertyChanged;
            if (handler != null)
            {
                if (Application.OpenForms.Count == 0) return;
                var mainForm = Application.OpenForms[0];
                if (mainForm == null) return; // No main form - no calls

                if (mainForm.InvokeRequired)
                {
                    // We are not in UI Thread now
                    mainForm.Invoke(handler, new object[] {
                        this, new PropertyChangedEventArgs(name)});
                }
                else
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }

            // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
