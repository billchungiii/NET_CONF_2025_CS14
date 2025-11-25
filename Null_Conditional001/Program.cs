using System.Net.Sockets;

namespace Null_Conditional001
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Person person = new Person();

            // before 
            if (person.Address != null)
            {
                 person.Address.City = "New York";
            }

            // after
            person.Address?.City = "New York";  
        }
    }

    public class Person
    {
        public string Name { get; set; }

        public Address Address { get; set; }
    }
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
    }
}
