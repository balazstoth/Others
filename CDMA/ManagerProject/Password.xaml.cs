using CommonProject;
using Manager;
using System;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace ManagerProject
{
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = passwordBox_new_password.Password;
            try
            {
                TextBoxControll();
                PasswordModifier(newPassword);
                this.Close();
            }
            catch (TextBoxesAreEmtpy ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (PasswordWrong ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (PasswordsDontMatch ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button_back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void TextBoxControll()
        {
            string oldPw = passwordBox_old_password.Password;
            string newPw = passwordBox_new_password.Password;
            string newPwRepeat = passwordBox_new_password_repeat.Password;

            if (String.IsNullOrEmpty(oldPw) ||
                String.IsNullOrEmpty(newPw) ||
                String.IsNullOrEmpty(newPwRepeat))
                throw new TextBoxesAreEmtpy();


            string oldPasswordSelected = (from x in XmlData.XDocUser.Root.Elements(XmlData.XmlUserSecondRootElement)
                                          where x.Element(XmlData.XmlUserChildElementName).Value.Equals(Storage.User.Name)
                                          select x.Element(XmlData.XmlUserChildElementPassword).Value).FirstOrDefault();

            if (oldPasswordSelected != oldPw)
                throw new PasswordWrong();

            if (newPw != newPwRepeat)
                throw new PasswordsDontMatch();
        }
        private void PasswordModifier(string newPassword)
        {
            XElement oldPassword = (from x in XmlData.XDocUser.Root.Elements(XmlData.XmlUserSecondRootElement)
                                    where x.Element(XmlData.XmlUserChildElementName).Value.Equals(Storage.User.Name)
                                    select x.Element(XmlData.XmlUserChildElementPassword)).FirstOrDefault();

            oldPassword.Value = newPassword;
            XmlManager.XmlSave(XDocSaveOrLoad.User);
            Storage.User.Password = newPassword;
        }
    }
}
