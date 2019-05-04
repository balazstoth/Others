using CommonProject;
using ManagerProject;
using System;
using System.Xml.Linq;

namespace Manager
{
    public class Diet
    {
        BasicInfo basicInfo;

        public BasicInfo BasicInfo
        {
            get
            {
                return basicInfo;
            }

            set
            {
                basicInfo = value;
            }
        }

        #region Constructors
        public Diet(XElement dietElement)
        {
            this.BasicInfo = SetElements(dietElement);
        }

        public Diet(BasicInfo basicInfo)
        {
            this.BasicInfo = basicInfo;
        }

        public Diet()
        {
        } 
        #endregion

        BasicInfo SetElements(XElement dietElement)
        {
            return new BasicInfo(
                                int.Parse(dietElement.Element(XmlData.XmlUserChildElementDietAge).Value),
                                double.Parse(dietElement.Element(XmlData.XmlUserChildElementDietWeightCurrent).Value),
                                double.Parse(dietElement.Element(XmlData.XmlUserChildElementDietWeightGoal).Value),
                                (Gender)Enum.Parse(typeof(Gender), dietElement.Element(XmlData.XmlUserChildElementDietGender).Value),
                                int.Parse(dietElement.Element(XmlData.XmlUserChildElementDietHeight).Value),
                                DateTime.Parse(dietElement.Element(XmlData.XmlUserChildElementDietStart).Value),
                                (DietBasedOn)Enum.Parse(typeof(DietBasedOn), dietElement.Element(XmlData.XmlUserChildElementDietBase).Value),
                                int.Parse(dietElement.Element(XmlData.XmlUserChildElementDietWeeks).Value),
                                (ActivityPerWeek)Enum.Parse(typeof(ActivityPerWeek), dietElement.Element(XmlData.XmlUserChildElementDietActivity).Value),
                                int.Parse(dietElement.Element(XmlData.XmlUserChildElementDietCalorieBase).Value),
                                int.Parse(dietElement.Element(XmlData.XmlUserChildElementDietCalorieModify).Value),
                                bool.Parse(dietElement.Element(XmlData.XmlUserChildElementDietDescCalorie).Value));
        }
        public static void CallOnPropertyChanged()
        {
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.DailyCalories));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.DailyProteinKcal));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.DailyCarbonKcal));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.DailyFatKcal));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.DailyProteinG));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.DailyCarbonG));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.DailyFatG));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.WeightCurrent));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.Completed));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.Start));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.BasedOn));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.DietType));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.BookedDays));
            ManagerWindow.Viewmodel.OnPropertyChangedCallMethod(nameof(ManagerWindow.Viewmodel.RemainingDays));
        }
    }
}
