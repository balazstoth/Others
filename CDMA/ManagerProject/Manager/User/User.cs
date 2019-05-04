using CommonProject;
using ManagerProject;

namespace Manager
{
    public class User
    {
        #region Fields
        string name;
        string password;
        Diet diet;
        #endregion

        #region Constructors
        public User(User user)
          : this(user.Name, user.Password)
        {
        }
        public User(string name, string password)
        {
            this.name = name;
            this.password = password;
        }
        public User(string name, string password, Diet diet)
            : this(name, password)
        {
            this.diet = diet;
        }
        public User()
        {
        }
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.UserName));
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
        public Diet Diet
        {
            get
            {
                return diet;
            }

            set
            {
                diet = value;
            }
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return string.Format("Name: {0}", Name);
        }
        #endregion
    }
}
