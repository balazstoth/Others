using System.Text.RegularExpressions;

namespace Manager
{
    class Regexs
    {
        public static Regex weight1 = new Regex("^[4-9]{1}[0-9]{1},[0-9]{1}$");
        public static Regex weight2 = new Regex("^1[0-4]{1}[0-9]{1},[0-9]{1}$");

        public static Regex food1 = new Regex("^100,0$");
        public static Regex food2 = new Regex("^[1-9]{1}[0-9]{1},[0-9]{1}$");
        public static Regex food3 = new Regex("^[0-9]{1},[0-9]{1}$");

        public static Regex food4 = new Regex("^[1-9]{1}[0-9]{3}$");
        public static Regex food5 = new Regex("^[1-9]{1}[0-9]{2}$");
        public static Regex food6 = new Regex("^[1-9]{1}[0-9]{1}$");
        public static Regex food7 = new Regex("^[0-9]{1}$");
    }
}
