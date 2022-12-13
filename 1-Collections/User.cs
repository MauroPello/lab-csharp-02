using System;

namespace Collections
{
    public class User : IUser
    {
        public User(string fullName, string username, uint? age)
        {
            if(username == null)
            {
                throw new ArgumentNullException("Username cannot be null");
            }

            this.Age = age;
            this.Username = username;
            this.FullName = fullName;
        }
        
        public uint? Age { get; }
        
        public string FullName { get; }
        
        public string Username { get; }

        public bool IsAgeDefined => Age != null;
        
        protected bool Equals(User other) => Age == other.Age && FullName == other.FullName && Username == other.Username;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode() => HashCode.Combine(Age, FullName, Username);

        public override string ToString() => $"Username: {Username}, Full name: {FullName}, Age: {Age}.";
    }
}
