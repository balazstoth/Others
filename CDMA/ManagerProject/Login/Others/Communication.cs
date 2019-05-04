using System.Windows;

namespace Login
{
    class Communication
    {
        public static void UserCreatedSuccessfully()
        {
            MessageBox.Show("Felhasználó hozzáadva, most már be tud jelentkezni!", "Sikeresen hozzáadva!");
        }
    }
}
