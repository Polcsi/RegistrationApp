using System;

namespace RegistrationApp
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Residence { get; set; }
        public string Gender { get; set; }
        public User(string userName, string password, string fullName, DateTime birthDate, string residence, string gender)
        {
            UserName = userName;
            Password = password;
            FullName = fullName;
            BirthDate = birthDate;
            Residence = residence;
            Gender = gender;
        }
        public override string ToString()
        {
            return String.Format($"Felhasználónév: {UserName} Jelszó: {Password}");
        }
    }
}
