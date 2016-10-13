using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Example10.Annotations;

namespace Example10.ViewModel
{
    class DataGridViewModel : INotifyPropertyChanged
    {
        private BindingList<Human> people = new BindingList<Human>();
        private Func<Human, bool> _filter;


        public DataGridViewModel()
        {
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

//            people.Add(new Human
//            {
//                BirthDate = new DateTime(1990, 6, 13).ToShortDateString(),
//                FirstName = "Alan",
//                LastName = "Smith",
//                Gender = Gender.Male
//            });
//            people.Add(new Human
//            {
//                BirthDate = new DateTime(1960, 4, 23).ToShortDateString(),
//                FirstName = "Megan",
//                LastName = "Jons",
//                Gender = Gender.Female
//            });
//            people.Add(new Human
//            {
//                BirthDate = new DateTime(1985, 9, 12).ToShortDateString(),
//                FirstName = "Emily",
//                LastName = "Keller",
//                Gender = Gender.Female
//            });
//            people.Add(new Human
//            {
//                BirthDate = new DateTime(1991, 6, 11).ToShortDateString(),
//                FirstName = "Jeremy",
//                LastName = "Watson",
//                Gender = Gender.Male
//            });
//            people.Add(new Human
//            {
//                BirthDate = new DateTime(1996, 7, 3).ToShortDateString(),
//                FirstName = "Christian",
//                LastName = "Woods",
//                Gender = Gender.Male
//            });
        }

        public bool Filter
        {
            get { return _filter != null; }
            set
            {
                if (value) _filter = h => h.Gender == Gender.Male;
                else _filter = null;
                OnPropertyChanged("People");
            }
        }

        public BindingList<Human> People
        {
            get
            {
                if (_filter != null)
                {
                    var result = people.Where(_filter);
                    var list = new BindingList<Human>();
                    foreach (var h in result)
                    {
                        list.Add(h);
                    }
                    return list;
                }
                
                return people;
            }
        }

        public void SortByAge()
        {
            var sorted = people.OrderBy(x => x.Age).ToList();
            sorted.Reverse();
            people.Clear();
            foreach (var h in sorted)
            {
                people.Add(h);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public enum Gender { Male, Female }

    public class Human : INotifyPropertyChanged
    {
        private DateTime _birthDate;
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
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
                return age.Days / 365;
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
