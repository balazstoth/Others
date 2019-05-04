using System;
using System.Linq;
using System.Xml.Linq;
using CommonProject;
using System.Windows;
using ManagerProject;

namespace Login
{
    class CreateLogic
    {
        string warningMessage;

        public CreateLogic()
        {
            warningMessage = "Figyelem!";
        }

        public void CreateNewUserManager(LoginWindow window, string usernameNewUser, string passwordFirst, string passwordRepeat)
        {
            XDocument document = XmlData.XDocUser;
            bool controlled = false;

            try
            {
                controlled = TextBoxCreateControllMethod(passwordFirst, passwordRepeat, usernameNewUser);
            }
            catch (TextBoxesAreEmtpy ex)
            {
                MessageBox.Show(ex.Message, warningMessage);
            }
            catch (UserNameAlreadyExists ex)
            {
                MessageBox.Show(ex.Message, warningMessage);
            }
            catch (PasswordsDontMatch ex)
            {
                MessageBox.Show(ex.Message, warningMessage);
            }

            if (controlled)
            {
                document.Root.Add(new XElement(XmlData.XmlUserSecondRootElement,
                                                new XElement(XmlData.XmlUserChildElementName, usernameNewUser),
                                                new XElement(XmlData.XmlUserChildElementPassword, passwordFirst)));

                XmlManager.XmlSave(XDocSaveOrLoad.User);
                window.TextBoxClearMethod();
                Communication.UserCreatedSuccessfully();
            }
        }
        bool TextBoxCreateControllMethod(string passwordFirst, string passwordRepeat, string usernameNewUser)
        {
            XDocument document = XmlData.XDocUser;

            if (String.IsNullOrEmpty(usernameNewUser) ||
                String.IsNullOrEmpty(passwordFirst) ||
                String.IsNullOrEmpty(passwordRepeat))
                throw new TextBoxesAreEmtpy();

            var userInDataBase = (from user in document.Root.Elements(XmlData.XmlUserSecondRootElement)
                                  where user.Element(XmlData.XmlUserChildElementName).Value.Equals(usernameNewUser)
                                  select user).FirstOrDefault();

            if (userInDataBase != null) throw new UserNameAlreadyExists();

            if (!passwordFirst.Equals(passwordRepeat))
                throw new PasswordsDontMatch();

            return true;
        }
    }
}
