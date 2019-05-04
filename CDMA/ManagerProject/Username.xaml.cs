using CommonProject;
using Manager;
using System;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace ManagerProject
{
    public partial class UsernameWindow : Window
    {
        public UsernameWindow()
        {
            InitializeComponent();
        }

        private void UserNameHandler()
        {
            String username = textBox_username.Text;

            if (String.IsNullOrEmpty(username))
                throw new TextBoxesAreEmtpy();

            var selectedUser = (from user in XmlData.XDocUser.Root.Elements(XmlData.XmlUserSecondRootElement)
                                where user.Element(XmlData.XmlUserChildElementName).Value.Equals(username)
                                select user).FirstOrDefault();

            if (selectedUser != null)
                throw new UserNameAlreadyExists();

            XElement oldNameElement = (from x in XmlData.XDocUser.Root.Elements(XmlData.XmlUserSecondRootElement)
                                       where x.Element(XmlData.XmlUserChildElementName).Value.Equals(Storage.User.Name)
                                       select x.Element(XmlData.XmlUserChildElementName)).FirstOrDefault();

            oldNameElement.Value = username;
            XmlManager.XmlSave(XDocSaveOrLoad.User);
            Storage.User.Name = username;
        }
        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserNameHandler();
                this.Close();
            }
            catch (TextBoxesAreEmtpy ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (UserNameAlreadyExists ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button_back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
