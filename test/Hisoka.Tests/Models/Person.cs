using System;

namespace Hisoka.Tests.Models
{
    class Address
    {
        public string Street { get; set; }
    }

    class Contacts
    {
        public string Name { get; set; }
        public Address[] Addresses { get; set; }
    }

    class Person
    {
        public string Name { get; set; }
        public string[] Hobbies { get; set; }
        public Contacts[] Contacts { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
