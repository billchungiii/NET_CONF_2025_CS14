using System.ComponentModel;
using System.Runtime.Intrinsics.Arm;
using static System.Net.WebRequestMethods;

namespace Partial001
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var p = new Person() { FirstName = "Bill", LastName = "Chung" };
            AddEventMethod(p);
            AddEventMethod(p);
            RemoveEventMethod(p);
        }

        private static void AddEventMethod(Person p)
        {
            p.OnFirstNameChanged +=  OnChanged;
            Console.WriteLine($"Event count: {p.EventCount}");
        }

        private static void RemoveEventMethod(Person p)
        {
            p.OnFirstNameChanged -= OnChanged;
            Console.WriteLine($"Event count: {p.EventCount}");
        }

        private static void OnChanged(object sender, string newFirstName)
        {
            Console.WriteLine($"First name changed to: {newFirstName}");
        }
    }

    internal partial class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int EventCount { get; private set; }

        public partial Person();

        public partial event EventHandler<string> OnFirstNameChanged;
    }

    internal partial class Person
    {
        public partial Person() { }

        private EventHandler<string> _firstNameChanged;
        public partial event EventHandler<string> OnFirstNameChanged
        {
            add
            {
                _firstNameChanged += value;
                EventCount++;
            }
            remove
            {
                _firstNameChanged -= value;
                EventCount--;
            }
        }

        protected void RaiseFirstNameChanged(string newFirstName)
        {
            _firstNameChanged?.Invoke(this, newFirstName);
        }
    }





}
