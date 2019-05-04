using System.Windows;
using CommonProject;
using Login;
using System.Windows.Input;

namespace ManagerProject
{

    public partial class LoginWindow : Window
    {
        LoginLogic loginLogic;
        CreateLogic createLogic;

        public LoginWindow()
        {
            InitializeComponent();
            CallXMLControlMethods();
            loginLogic = new LoginLogic();
            createLogic = new CreateLogic();

            textBox_login_username.Text = "Balázs";
            passwordBox_login_password.Password = "teszt";
        }

        #region Events
        private void expander_newfelhasználó_Expanded(object sender, RoutedEventArgs e)
        {
            ExpanderExpanded();
        }
        private void expander_newfelhasználó_Collapsed(object sender, RoutedEventArgs e)
        {
            ExpanderCollapsed();
        }
        private void button_login_bejelentkezés_Click(object sender, RoutedEventArgs e)
        {
            loginLogic.LoginManager(this, textBox_login_username.Text, passwordBox_login_password.Password);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                loginLogic.LoginManager(this, textBox_login_username.Text, passwordBox_login_password.Password);
        }
        private void button_new_create_Click(object sender, RoutedEventArgs e)
        {
            createLogic.CreateNewUserManager(this, textBox_new_username.Text, passwordBox_new_password.Password, passwordBox_new_password_repeat.Password);
        }
        #endregion

        #region Methods
        void ExpanderCollapsed()
        {
            Application.Current.MainWindow.Height = Property.HeigthCollapsed;
        }
        void ExpanderExpanded()
        {
            Application.Current.MainWindow.Height = Property.HeightExpanded;
        }
        public void TextBoxClearMethod()
        {
            textBox_login_username.Clear();
            textBox_new_username.Clear();

            passwordBox_login_password.Clear();
            passwordBox_new_password.Clear();
            passwordBox_new_password_repeat.Clear();
        }
        void CallXMLControlMethods()
        {
            XmlManager.XmlUserControl();
            XmlManager.XmlFoodControl();
            XmlManager.XmlSupplementControl();
        }
        #endregion
    }
}
