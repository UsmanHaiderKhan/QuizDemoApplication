using System;
using System.Collections.Generic;

namespace QuizDemoClasses.UserManagement
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AccessLevel { get; set; }
        public DateTime EntryDate { get; set; }
        public string Email { get; set; }
        public string PhonNo { get; set; }
        public List<Registration> Registrations { get; set; }
        public int PassHash { get; set; }

        public Student()
        {
            Registrations = new List<Registration>();
        }
    }
}
