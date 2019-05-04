using System.IO;
using System.Xml.Linq;

namespace CommonProject
{
    public class XmlData
    {
        #region Fields
        #region FolderPath
        private static string folderName = "XmlFiles";
        public static string XmlSavePath = string.Format(@"{0}\{1}\", Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, folderName);
        #endregion

        #region User
        public static string XmlUserRootElement = "Users";
        public static string XmlUserSecondRootElement = "User";
        public static string XmlUserChildElementName = "Name";
        public static string XmlUserChildElementPassword = "Password";

        public static string XmlUserChildElementDiet = "Diet";
        public static string XmlUserChildElementDietAge = "Age";
        public static string XmlUserChildElementDietWeightCurrent = "CurrentWeight";
        public static string XmlUserChildElementDietWeightGoal = "GoalWeight";
        public static string XmlUserChildElementDietGender = "Gender";
        public static string XmlUserChildElementDietHeight = "Height";
        public static string XmlUserChildElementDietStart = "Start";
        public static string XmlUserChildElementDietBase = "Base";
        public static string XmlUserChildElementDietType = "Type";
        public static string XmlUserChildElementDietWeeks = "Weeks";
        public static string XmlUserChildElementDietActivity = "Activity";
        public static string XmlUserChildElementDietCalorieBase = "CalorieBase";
        public static string XmlUserChildElementDietCalorieModify = "CalorieModify";
        public static string XmlUserChildElementDietDescCalorie = "DescCalorie";
        #endregion

        #region Supplement
        public static string XmlSupplementRootElement = "Supplements";
        public static string XmlSupplementSecondRootElement = "Supplement";
        public static string XmlSupplementChildElementName = "Name";
        public static string XmlSupplementChildElementProt = "Protein";
        public static string XmlSupplementChildElementCH = "CarbonHydrate";
        public static string XmlSupplementChildElementFat = "Fat";
        public static string XmlSupplementChildElementCalorie = "Calorie";
        public static string XmlSupplementChildElementFiber = "Fiber";
        public static string XmlSupplementChildElementGI = "GI";
        #endregion

        #region Food
        public static string XmlFoodRootElement = "Foods";
        public static string XmlFoodSecondRootElement = "Food";
        public static string XmlFoodChildElementName = "Name";
        public static string XmlFoodChildElementProt = "Protein";
        public static string XmlFoodChildElementCH = "CarbonHydrate";
        public static string XmlFoodChildElementFat = "Fat";
        public static string XmlFoodChildElementCalorie = "Calorie";
        public static string XmlFoodChildElementFiber = "Fiber";
        public static string XmlFoodChildElementGI = "GI";
        #endregion

        #region File
        public static string xmlFileNameUser = "Users.xml";
        public static string xmlFileNameSupplement = "Supplements.xml";
        public static string xmlFileNameFood = "Foods.xml";
        #endregion

        #region XDoc
        static XDocument xDocUser;
        static XDocument xDocFood;
        static XDocument xDocSupplement;
        #endregion
        #endregion

        #region Properties
        public static XDocument XDocUser
        {
            get
            {
                return xDocUser;
            }

            set
            {
                xDocUser = value;
            }
        }
        public static XDocument XDocFood
        {
            get
            {
                return xDocFood;
            }

            set
            {
                xDocFood = value;
            }
        }
        public static XDocument XDocSupplement
        {
            get
            {
                return xDocSupplement;
            }

            set
            {
                xDocSupplement = value;
            }
        }
        #endregion
    }
}
