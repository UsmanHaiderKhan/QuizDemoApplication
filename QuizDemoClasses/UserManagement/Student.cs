using System;

namespace QuizDemoClasses.UserManagement
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccessLevel { get; set; }
        public DateTime EntryDate { get; set; }
        public string Email { get; set; }
        public long PhonNo { get; set; }
        public int PassHash { get; set; }

    }
}
