using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Example10.Annotations;

namespace Example10.View
{
    /// <summary>
    /// Interaction logic for DataGridWindow.xaml
    /// </summary>
    public partial class DataGridWindow : Window
    {
        private BindingList<Human> people = new BindingList<Human>();

        public DataGridWindow()
        {
            InitializeComponent();

            using (var sr = new StreamReader("./Data.txt"))
            {
                while (!sr.EndOfStream)
                {
                    var h = people.AddNew();
                    h.ParseFromString(sr.ReadLine());
                }
            }
            
            
            people.ListChanged += (sender, args) =>
            {
                using (var sw = new StreamWriter("./Data.txt"))
                {
                    foreach (var human in people)
                    {
                        sw.WriteLine(human);      
                    }
                }
            };
            

            DataGrid.ItemsSource = people;

            //people.Add(new Human
            //{
            //    BirthDate = new DateTime(1990, 6, 13).ToShortDateString(),
            //    FirstName = "Alan",
            //    LastName = "Smith",
            //    Gender = Gender.Male
            //});
            //people.Add(new Human
            //{
            //    BirthDate = new DateTime(1960, 4, 23).ToShortDateString(),
            //    FirstName = "Megan",
            //    LastName = "Jons",
            //    Gender = Gender.Female
            //});
            //people.Add(new Human
            //{
            //    BirthDate = new DateTime(1985, 9, 12).ToShortDateString(),
            //    FirstName = "Emily",
            //    LastName = "Keller",
            //    Gender = Gender.Female
            //});
            //people.Add(new Human
            //{
            //    BirthDate = new DateTime(1991, 6, 11).ToShortDateString(),
            //    FirstName = "Jeremy",
            //    LastName = "Watson",
            //    Gender = Gender.Male
            //});
            //people.Add(new Human
            //{
            //    BirthDate = new DateTime(1996, 7, 3).ToShortDateString(),
            //    FirstName = "Christian",
            //    LastName = "Woods",
            //    Gender = Gender.Male
            //});
        }

        private void DataGridWindow_OnClosing(object sender, CancelEventArgs e)
        {
            people.ResetBindings();
        }
    }

    public enum Gender { Male, Female}

    public class Human : INotifyPropertyChanged
    {
        private DateTime _birthDate;
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string BirthDate
        {
            get { return _birthDate.ToShortDateString(); }
            set
            {
                
                _birthDate = DateTime.Parse(value);

                OnPropertyChanged("Age");
            }
        }

        public int Age
        {
            get
            {
                var age = DateTime.Now - _birthDate;
                return age.Days/365;
            }
        }

        public Gender Gender { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}\t{3}", FirstName, LastName, BirthDate, Gender);
        }

        public void ParseFromString(string line)
        {
            var items = line.Split('\t');
            FirstName = items[0];
            LastName = items[1];
            BirthDate = items[2];
            Gender = items[3].Contains("Female") ? Gender.Female : Gender.Male;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
