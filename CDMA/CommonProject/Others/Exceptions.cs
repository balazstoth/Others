using System;

namespace CommonProject
{
    class ExceptionStrings
    {
        public static string UserNameCannotFound = "Nem létezik ilyen nevű felhasználó!";
        public static string TextBoxesAreEmtpy = "A beviteli mezők nem lehetnek üresek!";
        public static string PasswordWrong = "A megadott jelszó hibás!";
        public static string UserNameAlreadyExists = "Ilyen nevű felhasználó már létezik!";
        public static string PasswordsDontMatch = "A két jelszó nem egyezik meg!";
        public static string FoodAlreadyExists = "Már van ilyen név az adatbázisban!";
        public static string IncorrectFormat = "Nem megfelelő formátum!";
        public static string IncorrectSum = "Nem megfelelő tápérték összeg!";
    }

    public class UserNameCannotFound : Exception
    {
        public UserNameCannotFound()
            : base(ExceptionStrings.UserNameCannotFound)
        {
        }
    }

    public class TextBoxesAreEmtpy : Exception
    {
        public TextBoxesAreEmtpy()
            : base(ExceptionStrings.TextBoxesAreEmtpy)
        {
        }
    }

    public class PasswordWrong : Exception
    {
        public PasswordWrong()
            : base(ExceptionStrings.PasswordWrong)
        {
        }
    }

    public class UserNameAlreadyExists : Exception
    {
        public UserNameAlreadyExists()
            : base(ExceptionStrings.UserNameAlreadyExists)
        {
        }
    }

    public class PasswordsDontMatch : Exception
    {
        public PasswordsDontMatch()
            : base(ExceptionStrings.PasswordsDontMatch)
        {
        }
    }

    public class FoodAlreadyExists : Exception
    {
        public FoodAlreadyExists()
            : base(ExceptionStrings.FoodAlreadyExists)
        {
        }
    }

    public class IncorrectFormat : Exception
    {
        public IncorrectFormat()
            : base(ExceptionStrings.IncorrectFormat)
        {
        }
    }

    public class IncorrectSum : Exception
    {
        public IncorrectSum()
            : base(ExceptionStrings.IncorrectSum)
        {
        }
    }
}
