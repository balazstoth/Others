using System.Collections.Generic;

namespace DBPermission
{
    public class User
    {
        public string Name { get; set; }

        public User(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object obj)
        {
            if(obj is User)
            {
                if ((obj as User).Name == this.Name)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
