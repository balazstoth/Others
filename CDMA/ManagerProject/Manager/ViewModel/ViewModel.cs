using CommonProject;
using System;
using System.Collections.ObjectModel;

namespace Manager
{
    class ViewModel : Binding
    {
        public ViewModel()
        {
            sortedFoods = new ObservableCollection<Food>();
        }

        #region Fields
        string[] foodCathergory = { "Szilárd ételek",
                                    "Táplálékkiegészítők" };

        string[] orderCathegory = { Order.Name.ToString(),
                                    Order.Kcal.ToString(),
                                    Order.Prot.ToString(),
                                    Order.CH.ToString(),
                                    Order.Fat.ToString(),
                                    Order.Fiber.ToString(),
                                    Order.GI.ToString()};

        string[] activity = { "Nincs edzés",
                               "1-3 nap/hét",
                               "3-5 nap/hét",
                               "6-7 nap/hét",
                               "Napi 2 edzés" };

        string[] comboBox_BasedOn = new string[] { DietBasedOn.Calorie.ToString(),
                                                   DietBasedOn.Weight.ToString() };

        string[] comboBox_GI = new string[] { GI.Unknown.ToString(),
                                              GI.Low.ToString(),
                                              GI.Medium.ToString(),
                                              GI.High.ToString() };

        ObservableCollection<Food> sortedFoods;
        #endregion

        #region Properties
        public ObservableCollection<Food> SortedFoods
        {
            get
            {
                return sortedFoods;
            }

            set
            {
                sortedFoods = value;
            }
        }
        public string[] FoodCathergory
        {
            get
            {
                return foodCathergory;
            }

            set
            {
                foodCathergory = value;
            }
        }
        public string[] OrderCathegory
        {
            get
            {
                return orderCathegory;
            }

            set
            {
                orderCathegory = value;
            }
        }
        public string[] Activity
        {
            get
            {
                return activity;
            }

            set
            {
                activity = value;
            }
        }
        public string[] ComboBox_BasedOn
        {
            get
            {
                return comboBox_BasedOn;
            }

            set
            {
                comboBox_BasedOn = value;
            }
        }
        public string[] ComboBox_GI
        {
            get
            {
                return comboBox_GI;
            }

            set
            {
                comboBox_GI = value;
            }
        }
        public string UserName
        {
            get { return Storage.User.Name; }
        }

        public string WeightCurrent
        {
            get
            {
                if (Storage.User.Diet != null)
                    return Storage.User.Diet.BasicInfo.WeightCurrent.ToString();
                else
                    return Communication.None;
            }
        }
        public string Completed
        {
            get
            {
                if (Storage.User.Diet != null)
                {
                    if (Storage.User.Diet.BasicInfo.BasedOn == DietBasedOn.Weight)
                    {
                        double goal = Storage.User.Diet.BasicInfo.WeightGoal;
                        double start = Storage.User.Diet.BasicInfo.WeightCurrent;
                        double diff = start - goal;
                        if (diff > 0) //diéta
                        {
                            return "-"; //(start - jelenlegi)/diff
                        }
                        else
                        {
                            if (diff < 0) //tömegelés
                            {
                                diff = Math.Abs(diff);
                                return "-"; //(Jelenlegi - start)/diff
                            }
                            else //holding
                            {
                                return "100";
                            }
                        }

                    }
                    else
                    {
                        double bookeddays = double.Parse(BookedDays);
                        double alldays = Storage.User.Diet.BasicInfo.Weeks * 7;
                        if ((Math.Round((bookeddays / alldays), 3) * 100) < 100)
                            return (Math.Round((bookeddays / alldays), 3) * 100).ToString();
                        else
                            return "100";
                    }
                }
                else
                    return Communication.None;
            }
        }
        public string Start
        {
            get
            {
                if (Storage.User.Diet != null)
                    return Storage.User.Diet.BasicInfo.Start.ToShortDateString();
                else
                    return Communication.None;
            }
        }
        public string BasedOn
        {
            get
            {
                if (Storage.User.Diet != null)
                {
                    if (Storage.User.Diet.BasicInfo.BasedOn == DietBasedOn.Weight)
                    {
                        //if (aktuálistestSúly = Céltestsúly)
                        //    return Communication.SuccessFullyCompleted;
                        //else
                            return String.Format("{0} kg elérése", Storage.User.Diet.BasicInfo.WeightGoal);
                    }
                    else
                    {
                        DateTime goal = Storage.User.Diet.BasicInfo.Start.AddDays(Storage.User.Diet.BasicInfo.Weeks * 7);
                        if (DateTime.Today >= goal)
                            return Communication.SuccessFullyCompleted;
                        else
                            return String.Format("{0}-ig folytatni", goal.ToShortDateString());
                    }
                }
                else
                    return Communication.None;
            }
        }
        public string DietType
        {
            get
            {
                if (Storage.User.Diet != null)
                    return Storage.User.Diet.BasicInfo.DietType.ToString();
                else
                    return Communication.None;
            }
        }
        public string BookedDays
        {
            get
            {
                if (Storage.User.Diet != null)
                {
                    int days = (DateTime.Today - Storage.User.Diet.BasicInfo.Start).Days;
                    //if (days < )
                    //{
                    return "";
                    //}
                }
                else
                    return Communication.None;
            }
        }
        public string RemainingDays
        {
            get
            {
                if (Storage.User.Diet != null)
                {
                    if (Storage.User.Diet.BasicInfo.BasedOn == DietBasedOn.Weight)
                        if (Finished())
                            return Communication.NoMoreDays;
                        else
                            return  Communication.Undefined;
                    else
                    {
                        double days = (Storage.User.Diet.BasicInfo.Weeks * 7 - int.Parse(BookedDays)) - 1;
                        if (days > 0)
                            return days.ToString();
                        else
                            return Communication.NoMoreDays;
                    }
                }
                else
                    return Communication.None;
            }
        }
        public bool Finished()
        {
            return false;
            //if (Storage.User.Diet.BasicInfo.Weeks == 0)
            //{
            //    if (Storage.User.Diet.BasicInfo.DietType == TypeOfDiet.Diet)
            //    {
            //        if (AktuálisSúly <= Storage.User.Diet.BasicInfo.WeightGoal)
            //        {
            //            return true;
            //        }
            //        return false;
            //    }

            //    if (Storage.User.Diet.BasicInfo.DietType == TypeOfDiet.MassGaining)
            //    {
            //        if (AktuálisSúly >= Storage.User.Diet.BasicInfo.WeightGoal)
            //        {
            //            return true;
            //        }
            //        return false;
            //    }
            //}
            //else
            //{
            //    return Completed == "100";
            //}
        }

        public string DailyCalories
        {
            get
            {
                if (Storage.User.Diet != null)
                    return Storage.User.Diet.BasicInfo.calorieSum.ToString();
                else
                    return Communication.None;
            }
        }
        public string DailyProteinG
        {
            get
            {
                if (Storage.User.Diet != null)
                {
                    return (Math.Round((double.Parse(DailyProteinKcal) / Formula.ProteinGrammToKcal),1)).ToString();
                }
                else
                    return Communication.None;
            }
        }
        public string DailyProteinKcal
        {
            get
            {
                if (Storage.User.Diet != null)
                {
                    switch (Storage.User.Diet.BasicInfo.DietType)
                    {
                        case TypeOfDiet.Diet:
                            return ((int)(Storage.User.Diet.BasicInfo.calorieSum * Formula.FatLossProteinPercent)).ToString();
                        case TypeOfDiet.MassGaining:
                            return ((int)(Storage.User.Diet.BasicInfo.calorieSum * Formula.MassGainProteinPercent)).ToString();
                        case TypeOfDiet.Maintenance:
                            return ((int)(Storage.User.Diet.BasicInfo.calorieSum * Formula.MaintenanceProteinPercent)).ToString();
                    }
                    return Communication.None;
                }
                else    
                    return Communication.None;
            }
        }
        public string DailyCarbonG
        {
            get
            {
                if (Storage.User.Diet != null)
                {
                    return (Math.Round((double.Parse(DailyCarbonKcal) / Formula.CarbonGrammToKcal), 1)).ToString();
                }
                else
                    return Communication.None;
            }
        }
        public string DailyCarbonKcal
        {
            get
            {
                if (Storage.User.Diet != null)
                {
                    switch (Storage.User.Diet.BasicInfo.DietType)
                    {
                        case TypeOfDiet.Diet:
                            return ((int)(Storage.User.Diet.BasicInfo.calorieSum * Formula.FatLossCarbonPercent)).ToString();
                        case TypeOfDiet.MassGaining:
                            return ((int)(Storage.User.Diet.BasicInfo.calorieSum * Formula.MassGainCarbonPercent)).ToString();
                        case TypeOfDiet.Maintenance:
                            return ((int)(Storage.User.Diet.BasicInfo.calorieSum * Formula.MaintenanceCarbonPercent)).ToString();
                    }
                    return Communication.None;
                }
                else
                    return Communication.None;
            }
        }
        public string DailyFatG
        {
            get
            {
                if (Storage.User.Diet != null)
                {
                    return (Math.Round((double.Parse(DailyFatKcal) / Formula.FatGrammToKcal), 1)).ToString();
                }
                else
                    return Communication.None;
            }
        }
        public string DailyFatKcal
        {
            get
            {
                if (Storage.User.Diet != null)
                {
                    switch (Storage.User.Diet.BasicInfo.DietType)
                    {
                        case TypeOfDiet.Diet:
                            return ((int)(Storage.User.Diet.BasicInfo.calorieSum * Formula.FatLossFatPercent)).ToString();
                        case TypeOfDiet.MassGaining:
                            return ((int)(Storage.User.Diet.BasicInfo.calorieSum * Formula.MassGainFatPercent)).ToString();
                        case TypeOfDiet.Maintenance:
                            return ((int)(Storage.User.Diet.BasicInfo.calorieSum * Formula.MaintenanceFatPercent)).ToString();
                    }
                    return Communication.None;
                }
                else
                    return Communication.None;
            }
        }
        #endregion

        #region Methods
        public void OnPropertyChangedCallMethod(string Name)
        {
            OnPropertyChanged(Name);
        } 
        #endregion
    }
}
