using System;
using System.Linq;
using CommonProject;
using System.Windows;
using System.Xml.Linq;
using ManagerProject;
using Manager;

namespace Login
{
    class LoginLogic
    {
        string warningHintMessage;

        public LoginLogic()
        {
            warningHintMessage = "Figyelem!";
        }

        public void LoginManager(LoginWindow window, string username, string password)
        {
            try
            {
                LogIn(window, username, password);
            }
            catch (TextBoxesAreEmtpy ex)
            {
                MessageBox.Show(ex.Message, warningHintMessage);
            }
            catch (UserNameCannotFound ex)
            {
                MessageBox.Show(ex.Message, warningHintMessage);
            }
            catch (PasswordWrong ex)
            {
                MessageBox.Show(ex.Message, warningHintMessage);
            }
        }
        void LogIn(LoginWindow window, string username, string password)
        {
            TextBoxLoginControllMethod(username, password);
            if (XmlReaderGetUser(username, password))
            {
                ManagerWindow manager = new ManagerWindow();
                manager.Show();
                window.Close();
            }
        }
        void TextBoxLoginControllMethod(string username, string password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                throw new TextBoxesAreEmtpy();
        }
        bool XmlReaderGetUser(string userNameInTextBox, string passwordInTextBox)
        {
            XDocument document = XmlData.XDocUser;
            User selectedUser = null;

            if (UsersDietExists(userNameInTextBox))
            {
                selectedUser = (from user in document.Root.Elements(XmlData.XmlUserSecondRootElement)
                                where user.Element(XmlData.XmlUserChildElementName).Value.Equals(userNameInTextBox)
                                select new User(user.Element(XmlData.XmlUserChildElementName).Value,
                                                user.Element(XmlData.XmlUserChildElementPassword).Value,
                                                new Diet(user.Element(XmlData.XmlUserChildElementDiet)))).FirstOrDefault();
            }
            else
            {
                selectedUser = (from user in document.Root.Elements(XmlData.XmlUserSecondRootElement)
                                where user.Element(XmlData.XmlUserChildElementName).Value.Equals(userNameInTextBox)
                                select new User(user.Element(XmlData.XmlUserChildElementName).Value,
                                                user.Element(XmlData.XmlUserChildElementPassword).Value)).FirstOrDefault();
            }

            if (selectedUser != null)
            {
                Storage.User = selectedUser;
                if (!Storage.User.Password.Equals(passwordInTextBox))
                    throw new PasswordWrong();
            }
            else
                throw new UserNameCannotFound();

            return true;
        }
        bool UsersDietExists(string UserName)
        {
            XDocument document = XmlData.XDocUser;
            XElement selectedUser = (from x in document.Root.Elements(XmlData.XmlUserSecondRootElement)
                                     where x.Element(XmlData.XmlUserChildElementName).Value.Equals(UserName)
                                     select x.Element(XmlData.XmlUserChildElementDiet)).FirstOrDefault();

            return selectedUser == null ? false : true;
        }
    }
}
