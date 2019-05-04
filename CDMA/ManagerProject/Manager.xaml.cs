using System;
using System.Windows;
using CommonProject;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Controls;
using Manager;

namespace ManagerProject
{

    public partial class ManagerWindow : Window
    {
        #region Fields
        static ViewModel viewmodel;
        public static BasicInfo basicInfo;
        ControlView controlObject;
        IEnumerable<Food> unOrderedList;
        #endregion

        #region Properties
        internal static ViewModel Viewmodel
        {
            get
            {
                return viewmodel;
            }

            set
            {
                viewmodel = value;
            }
        }
        public int Weeks
        {
            get
            {
                return slider_newdiet_weeks.IsEnabled == true ? int.Parse(slider_newdiet_weeks.Value.ToString()) : 0;
            }
        }
        #endregion

        public ManagerWindow()
        {
            InitializeComponent();
            Initialize();
        }

        #region Methods
        void Initialize()
        {
            Viewmodel = new ViewModel();
            controlObject = new ControlView(this);
            datePicker_newdiet.SelectedDate = DateTime.Today;
            DataContext = Viewmodel;
            TextBoxHintSetter();
        }
        void DeleteProfile()
        {
            MessageBoxResult result = MessageBox.Show(Communication.profileDelete, Communication.profileDeleteHint, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                MessageBoxResult decision = MessageBox.Show(Communication.profileDeleteFinally, Communication.profileDeleteFinallyHint, MessageBoxButton.OKCancel);
                if (decision == MessageBoxResult.OK)
                {
                    var selectedUser = (from user in XmlData.XDocUser.Root.Elements(XmlData.XmlUserSecondRootElement)
                                        where user.Element(XmlData.XmlUserChildElementName).Value.Equals(Storage.User.Name)
                                        select user).FirstOrDefault();

                    selectedUser.Remove();
                    XmlManager.XmlSave(XDocSaveOrLoad.User);
                    Logout();
                }
            }
        }
        void Logout()
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
        void AddFood()
        {
            if (radioButton_food_food.IsChecked == true)
            {
                XmlData.XDocFood.Root.Add(new XElement(XmlData.XmlFoodSecondRootElement,
                                                        new XElement(XmlData.XmlFoodChildElementName, textBox_food_name.Text),
                                                        new XElement(XmlData.XmlFoodChildElementProt, textBox_food_protein.Text),
                                                        new XElement(XmlData.XmlFoodChildElementCH, textBox_food_CH.Text),
                                                        new XElement(XmlData.XmlFoodChildElementFat, textBox_food_fat.Text),
                                                        new XElement(XmlData.XmlFoodChildElementCalorie, textBox_food_energy.Text),
                                                        new XElement(XmlData.XmlFoodChildElementFiber, textBox_food_fiber.Text),
                                                        new XElement(XmlData.XmlFoodChildElementGI, comboBox_food_GI.SelectedValue.ToString())));
                XmlManager.XmlSave(XDocSaveOrLoad.Food);
            }
            else
            {
                XmlData.XDocSupplement.Root.Add(new XElement(XmlData.XmlSupplementSecondRootElement,
                                                        new XElement(XmlData.XmlSupplementChildElementName, textBox_food_name.Text),
                                                        new XElement(XmlData.XmlSupplementChildElementProt, textBox_food_protein.Text),
                                                        new XElement(XmlData.XmlSupplementChildElementCH, textBox_food_CH.Text),
                                                        new XElement(XmlData.XmlSupplementChildElementFat, textBox_food_fat.Text),
                                                        new XElement(XmlData.XmlSupplementChildElementCalorie, textBox_food_energy.Text),
                                                        new XElement(XmlData.XmlSupplementChildElementFiber, textBox_food_fiber.Text),
                                                        new XElement(XmlData.XmlSupplementChildElementGI, comboBox_food_GI.SelectedValue.ToString())));
                XmlManager.XmlSave(XDocSaveOrLoad.Supplement);
            }

            controlObject.ClearAll();
            TextBoxHintSetter();
            ListFoods();
        }
        void ListFoods()
        {
            XmlManager.XmlFoodControl();
            XmlManager.XmlSupplementControl();
            Viewmodel.SortedFoods.Clear();

            if (comboBox_food_modify_cathegory.SelectedValue.ToString() == Viewmodel.FoodCathergory[0])
            {
                IEnumerable<Food> food = from x in XmlData.XDocFood.Root.Elements(XmlData.XmlFoodSecondRootElement)
                                         select new Food(x.Element(XmlData.XmlFoodChildElementName).Value,
                                                        int.Parse(x.Element(XmlData.XmlFoodChildElementCalorie).Value),
                                                        Convert.ToDouble(x.Element(XmlData.XmlFoodChildElementCalorie).Value),
                                                        Convert.ToDouble(x.Element(XmlData.XmlFoodChildElementProt).Value),
                                                        Convert.ToDouble(x.Element(XmlData.XmlFoodChildElementCH).Value),
                                                        Convert.ToDouble(x.Element(XmlData.XmlFoodChildElementFat).Value),
                                                        Convert.ToDouble(x.Element(XmlData.XmlFoodChildElementFiber).Value),
                                                        (GI)Enum.Parse(typeof(GI), x.Element(XmlData.XmlFoodChildElementGI).Value),
                                                        NutritionType.Food);
                unOrderedList = food;
            }
            else
            {
                IEnumerable<Food> supplement = from x in XmlData.XDocSupplement.Root.Elements(XmlData.XmlSupplementSecondRootElement)
                                               select new Food(x.Element(XmlData.XmlSupplementChildElementName).Value,
                                                         int.Parse(x.Element(XmlData.XmlSupplementChildElementCalorie).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlSupplementChildElementCalorie).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlSupplementChildElementProt).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlSupplementChildElementCH).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlSupplementChildElementFat).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlSupplementChildElementFiber).Value),
                                                         (GI)Enum.Parse(typeof(GI), x.Element(XmlData.XmlSupplementChildElementGI).Value),
                                                         NutritionType.Supplement);
                unOrderedList = supplement;
            }

            Ordering();
        }
        bool DeleteFood()
        {
            string foodName = textBox_food_name.Text;
            bool? foodCheckBox = radioButton_food_food.IsChecked;

            if (String.IsNullOrWhiteSpace(foodName))
            {
                MessageBox.Show(Communication.selectOneFood);
                return false;
            }
            else
            {
                if (foodCheckBox == true)
                {
                    var food = from x in XmlData.XDocFood.Root.Elements(XmlData.XmlFoodSecondRootElement)
                               where x.Element(XmlData.XmlFoodChildElementName).Value.Equals(foodName)
                               select x;
                    food.Remove();
                    XmlManager.XmlSave(XDocSaveOrLoad.Food);
                }
                else
                {
                    var supplement = from x in XmlData.XDocFood.Root.Elements(XmlData.XmlSupplementSecondRootElement)
                                     where x.Element(XmlData.XmlSupplementChildElementName).Value.Equals(foodName)
                                     select x;

                    supplement.Remove();
                    XmlManager.XmlSave(XDocSaveOrLoad.Supplement);
                }
            }
            ListFoods();
            return true;
        }
        void TextBoxHintSetter()
        {
            textBox_food_protein.Foreground = Brushes.Gray;
            textBox_food_protein.Text = Communication.formatUsual;

            textBox_food_CH.Foreground = Brushes.Gray;
            textBox_food_CH.Text = Communication.formatUsual;

            textBox_food_fat.Foreground = Brushes.Gray;
            textBox_food_fat.Text = Communication.formatUsual;

            textBox_food_fiber.Text = "0,0";

            textBox_food_energy.Foreground = Brushes.Gray;
            textBox_food_energy.Text = Communication.formatUsual;
        }
        ActivityPerWeek activity(string activity)
        {
            if (activity == Viewmodel.Activity[0])
            {
                return ActivityPerWeek.None;
            }
            else
            {
                if (activity == Viewmodel.Activity[1])
                {
                    return ActivityPerWeek.Low;
                }
                else
                {
                    if (activity == Viewmodel.Activity[2])
                    {
                        return ActivityPerWeek.Medium;
                    }
                    else
                    {
                        if (activity == Viewmodel.Activity[3])
                        {
                            return ActivityPerWeek.High;
                        }
                        else
                        {
                            return ActivityPerWeek.Extreme;
                        }
                    }
                }
            }
        }
        bool NumpadControl(KeyEventArgs e)
        {
            if (e.Key == Key.NumPad0 ||
                e.Key == Key.NumPad1 ||
                e.Key == Key.NumPad2 ||
                e.Key == Key.NumPad3 ||
                e.Key == Key.NumPad4 ||
                e.Key == Key.NumPad5 ||
                e.Key == Key.NumPad6 ||
                e.Key == Key.NumPad7 ||
                e.Key == Key.NumPad8 ||
                e.Key == Key.NumPad9 ||
                e.Key == Key.Tab)
            {
                return true;
            }
            return false;
        }
        void Ordering()
        {
            Order order;
            var selectedOrderValue = comboBox_food_modify_order.SelectedValue;
            bool? descendant = radioButton_food_modify_order_descendant.IsChecked;

            if (selectedOrderValue != null)
                order = (Order)Enum.Parse(typeof(Order), selectedOrderValue.ToString());
            else
                order = Order.Name;

            if (descendant == false)
            {
                switch (order)
                {
                    case Order.Name:
                        unOrderedList = unOrderedList.OrderBy(x => x.Name);
                        break;
                    case Order.Kcal:
                        unOrderedList = unOrderedList.OrderBy(x => x.Calorie);
                        break;
                    case Order.Prot:
                        unOrderedList = unOrderedList.OrderBy(x => x.Protein);
                        break;
                    case Order.CH:
                        unOrderedList = unOrderedList.OrderBy(x => x.CH);
                        break;
                    case Order.Fat:
                        unOrderedList = unOrderedList.OrderBy(x => x.Fat);
                        break;
                    case Order.Fiber:
                        unOrderedList = unOrderedList.OrderBy(x => x.Fiber);
                        break;
                    case Order.GI:
                        unOrderedList = unOrderedList.OrderBy(x => x.GI);
                        break;
                }
            }
            else
            {
                switch (order)
                {
                    case Order.Name:
                        unOrderedList = unOrderedList.OrderByDescending(x => x.Name);
                        break;
                    case Order.Kcal:
                        unOrderedList = unOrderedList.OrderByDescending(x => x.Calorie);
                        break;
                    case Order.Prot:
                        unOrderedList = unOrderedList.OrderByDescending(x => x.Protein);
                        break;
                    case Order.CH:
                        unOrderedList = unOrderedList.OrderByDescending(x => x.CH);
                        break;
                    case Order.Fat:
                        unOrderedList = unOrderedList.OrderByDescending(x => x.Fat);
                        break;
                    case Order.Fiber:
                        unOrderedList = unOrderedList.OrderByDescending(x => x.Fiber);
                        break;
                    case Order.GI:
                        unOrderedList = unOrderedList.OrderByDescending(x => x.GI);
                        break;
                }
            }

            Viewmodel.SortedFoods.Clear();
            foreach (var i in unOrderedList)
            {
                Viewmodel.SortedFoods.Add(i);
            }
        }
        bool BodyWeightControl()
        {
            bool initialbodyweight = Regexs.weight1.IsMatch(textBox_newdiet_bodyweight.Text) ||
                                     Regexs.weight2.IsMatch(textBox_newdiet_bodyweight.Text);

            bool goalbodyweight = Regexs.weight1.IsMatch(textBox_newdiet_goalbodyweight.Text) ||
                                   (Regexs.weight1.IsMatch(textBox_newdiet_goalbodyweight.Text) ||
                                   textBox_newdiet_goalbodyweight.IsEnabled == false);

            return initialbodyweight && goalbodyweight;
        }
        void DateSelector()
        {
            datePicker_newdiet.DisplayDateStart = DateTime.Today;
        }
        void Undo()
        {
            grid_newdiet_first.IsEnabled = true;
            grid_newdiet_second.IsEnabled = false;
            slider_newdiet_calorie_modify.Value = 0;
            textBox_newdiet_calorie.Text = String.Empty;
            textBox_newdiet_modify.Text = String.Empty;
            textBox_newdiet_all.Text = String.Empty;
        }
        void Correction(object sender)
        {
            TextBox senderBox = (TextBox)sender;
            string text = senderBox.Text;

            if (text.Contains(".") || text.Contains(","))
                (senderBox).Text.Replace('.', ',');
            else
                (senderBox).Text = text + ",0";
        }
        void SaveDiet()
        {
            var user = (from x in XmlData.XDocUser.Root.Elements(XmlData.XmlUserSecondRootElement)
                        where x.Element(XmlData.XmlUserChildElementName).Value.Equals(Storage.User.Name)
                        select x).FirstOrDefault();

            if (user != null)
            {
                var diet = user.Element(XmlData.XmlUserChildElementDiet);
                if (diet != null)
                    diet.Remove();

                user.Add(new XElement(XmlData.XmlUserChildElementDiet,
                                            new XElement(XmlData.XmlUserChildElementDietAge, basicInfo.Age.ToString()),
                                            new XElement(XmlData.XmlUserChildElementDietWeightCurrent, basicInfo.WeightCurrent.ToString()),
                                            new XElement(XmlData.XmlUserChildElementDietWeightGoal, basicInfo.WeightGoal.ToString()),
                                            new XElement(XmlData.XmlUserChildElementDietGender, basicInfo.Gender.ToString()),
                                            new XElement(XmlData.XmlUserChildElementDietHeight, basicInfo.Height.ToString()),
                                            new XElement(XmlData.XmlUserChildElementDietStart, new string(basicInfo.Start.ToString().Where(x => x != ' ').ToArray()).Substring(0,10)),
                                            new XElement(XmlData.XmlUserChildElementDietBase, basicInfo.BasedOn.ToString()),
                                            new XElement(XmlData.XmlUserChildElementDietWeeks, basicInfo.Weeks.ToString()),
                                            new XElement(XmlData.XmlUserChildElementDietActivity, basicInfo.Activity),
                                            new XElement(XmlData.XmlUserChildElementDietCalorieBase, basicInfo.CalorieBase.ToString()),
                                            new XElement(XmlData.XmlUserChildElementDietCalorieModify, basicInfo.CalorieModified.ToString()),
                                            new XElement(XmlData.XmlUserChildElementDietDescCalorie, basicInfo.DescreasingCalorie)));
                

                XmlManager.XmlSave(XDocSaveOrLoad.User);
                Storage.User.Diet = new Diet(basicInfo);
                Diet.CallOnPropertyChanged();
            }
        }
        #endregion

        #region Events
        void button_logout_Click(object sender, RoutedEventArgs e)
        {
            Logout();
        }
        void button_profile_delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteProfile();
        }
        void button_new_password_Click(object sender, RoutedEventArgs e)
        {
            PasswordWindow passwordWindow = new PasswordWindow();
            passwordWindow.ShowDialog();
        }
        void button_new_name_Click(object sender, RoutedEventArgs e)
        {
            UsernameWindow usernameWindow = new UsernameWindow();
            usernameWindow.ShowDialog();
        }
        void button_food_record_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                controlObject.IsEmpty();
                controlObject.CorrectFormat();
                controlObject.AlreadyExists();
                controlObject.SumControl(100);
                AddFood();
            }
            catch (TextBoxesAreEmtpy ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (IncorrectFormat ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (FoodAlreadyExists ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (IncorrectSum ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void button_food_modify_delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteFood();
            controlObject.ClearAll();
            TextBoxHintSetter();
        }
        void button_food_modify_modify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                controlObject.IsEmpty();
                controlObject.CorrectFormat();
                controlObject.SumControl(100);

                if (DeleteFood())
                    AddFood();
            }
            catch (TextBoxesAreEmtpy ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (IncorrectFormat ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (FoodAlreadyExists ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (IncorrectSum ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void button_Click_start(object sender, RoutedEventArgs e)
        {
            basicInfo.CalorieBase = int.Parse(textBox_newdiet_calorie.Text);
            basicInfo.CalorieModified = int.Parse(textBox_newdiet_modify.Text);
            basicInfo.DescreasingCalorie = checkBox_newdiet_descendantcalorie.IsChecked == true ? true : false;

            int all_tb = int.Parse(textBox_newdiet_all.Text);
            if (all_tb != basicInfo.calorieSum)
                basicInfo.CalorieModified = all_tb - basicInfo.CalorieBase;

            Undo();
            SaveDiet();
        }
        void button_newdiet_Next_Click(object sender, RoutedEventArgs e)
        {
            if (!BodyWeightControl())
                MessageBox.Show(Communication.IncorrectBodyWeight);
            else
            {
                
                textBox_newdiet_modify.Text = "0";

                basicInfo = new BasicInfo(
                                int.Parse(slider_newdiet_age.Value.ToString()),
                                double.Parse(textBox_newdiet_bodyweight.Text.ToString()),
                                String.IsNullOrEmpty(textBox_newdiet_goalbodyweight.Text) ? 0 : double.Parse(textBox_newdiet_goalbodyweight.Text.ToString()),
                                radioButton_newdiet_male.IsChecked == true ? Gender.Male : Gender.Female,
                                int.Parse(slider_newdiet_height.Value.ToString()),
                                datePicker_newdiet.SelectedDate.Value,
                                (DietBasedOn)Enum.Parse(typeof(DietBasedOn), comboBox_newdiet_base.SelectedValue.ToString()),
                                Weeks,
                                activity(comboBox_newdiet_activity.SelectedValue.ToString()),
                                0,
                                0,
                                false);

                grid_newdiet_first.IsEnabled = false;
                grid_newdiet_second.IsEnabled = true;

                Formula.BmrCount(basicInfo.Gender, basicInfo.WeightCurrent, basicInfo.Height, basicInfo.Age);
                Formula.DailyCalorie(basicInfo.Activity);
                textBox_newdiet_calorie.Text = Formula.dailyCalorie.ToString();
                textBox_newdiet_all.Text = Formula.dailyCalorie.ToString();
            }
        }
        void button_newdiet_Reset_Click(object sender, RoutedEventArgs e)
        {
            Undo();
        }

        void textBox_food_modify_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Viewmodel.SortedFoods.Clear();

            if (comboBox_food_modify_cathegory.SelectedValue.ToString().Equals(Viewmodel.FoodCathergory[0]))
            {
                IEnumerable<Food> foods = from x in XmlData.XDocFood.Root.Elements(XmlData.XmlFoodSecondRootElement)
                                          where x.Element(XmlData.XmlFoodChildElementName).Value.ToLower().Contains(textBox_food_modify_search.Text.ToLower())
                                          select new Food(x.Element(XmlData.XmlFoodChildElementName).Value,
                                                         int.Parse(x.Element(XmlData.XmlFoodChildElementCalorie).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlFoodChildElementCalorie).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlFoodChildElementProt).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlFoodChildElementCH).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlFoodChildElementFat).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlFoodChildElementFiber).Value),
                                                         (GI)Enum.Parse(typeof(GI), x.Element(XmlData.XmlFoodChildElementGI).Value),
                                                         NutritionType.Food);

                foreach (var i in foods)
                    Viewmodel.SortedFoods.Add(i);
            }
            else
            {
                IEnumerable<Food> foods = from x in XmlData.XDocFood.Root.Elements(XmlData.XmlSupplementSecondRootElement)
                                          where x.Element(XmlData.XmlSupplementChildElementName).Value.ToLower().Contains(textBox_food_modify_search.Text.ToLower())
                                          select new Food(x.Element(XmlData.XmlSupplementChildElementName).Value,
                                                         int.Parse(x.Element(XmlData.XmlSupplementChildElementCalorie).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlSupplementChildElementCalorie).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlSupplementChildElementProt).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlSupplementChildElementCH).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlSupplementChildElementFat).Value),
                                                         Convert.ToDouble(x.Element(XmlData.XmlSupplementChildElementFiber).Value),
                                                         (GI)Enum.Parse(typeof(GI), x.Element(XmlData.XmlSupplementChildElementGI).Value),
                                                         NutritionType.Supplement);

                foreach (var i in foods)
                    Viewmodel.SortedFoods.Add(i);
            }
        }
        void textBox_food_PCHF_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox senderBox = (TextBox)sender;

            if (senderBox.Foreground == Brushes.Gray)
            {
                senderBox.Clear();
                (senderBox).Foreground = Brushes.Black;
            }
        }
        void textBox_food_PCHF_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox senderBox = (TextBox)sender;

            if (senderBox.Text.Trim().Equals(""))
            {
                senderBox.Foreground = Brushes.Gray;
                senderBox.Text = Communication.formatUsual;
            }
            else
                Correction(sender);
        }
        void textBox_food_energy_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox senderBox = (TextBox)sender;

            if (senderBox.Text.Trim().Equals(""))
            {
                (senderBox).Foreground = Brushes.Gray;
                (senderBox).Text = Communication.formatEnergy;
            }
        }
        void textBox_food_energy_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox senderBox = (TextBox)sender;

            if (senderBox.Foreground == Brushes.Gray)
            {
                (senderBox).Clear();
                (senderBox).Foreground = Brushes.Black;
            }
        }
        void textBox_food_All_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) && e.Key != Key.OemComma && !NumpadControl(e))
                e.Handled = true;
        }

        void comboBox_food_modify_cathegory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListFoods();
            textBox_food_modify_search.Clear();
        }
        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ordering();
        }
        void comboBox_newdiet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedValue.Equals(DietBasedOn.Weight.ToString()))
            {
                slider_newdiet_weeks.IsEnabled = false;
                textBox_newdiet_goalbodyweight.IsEnabled = true;
            }
            else
            {
                textBox_newdiet_goalbodyweight.IsEnabled = false;
                slider_newdiet_weeks.IsEnabled = true;
            }
        }
        void comboBox_food_modify_order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
                Ordering();
        }

        void checkBox_food_modify_enable_Checked(object sender, RoutedEventArgs e)
        {
            listBox_food_modify.SelectedItem = null;

            button_food_modify_modify.IsEnabled = true;
            button_food_modify_delete.IsEnabled = true;

            button_food_record.IsEnabled = false;
            textBox_food_name.IsEnabled = false;
            radioButton_food_food.IsEnabled = false;
            radioButton_food_supplement.IsEnabled = false;

            controlObject.ClearAll();
            TextBoxHintSetter();
        }
        void checkBox_food_modify_enable_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox_food_name.IsEnabled = true;
            button_food_record.IsEnabled = true;
            radioButton_food_food.IsEnabled = true;
            radioButton_food_supplement.IsEnabled = true;

            button_food_modify_modify.IsEnabled = false;
            button_food_modify_delete.IsEnabled = false;

            controlObject.ClearAll();
            TextBoxHintSetter();
        }

        void radioButton_food_modify_order_descendant_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
                Ordering();
        }
        void radioButton_food_modify_order_ascendant_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
                Ordering();
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DateSelector();
        }
        void listBox_food_modify_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedItem == null)
            {
                e.Handled = true;
            }

            if (checkBox_food_modify_enable.IsChecked == true && (sender as ListBox).SelectedItem != null)
            {
                Food selected = (Food)((sender as ListBox).SelectedItem);

                textBox_food_name.Text = selected.Name;

                textBox_food_energy.Foreground = Brushes.Black;
                textBox_food_energy.Text = selected.Calorie.ToString();

                textBox_food_protein.Foreground = Brushes.Black;
                textBox_food_protein.Text = selected.Protein.ToString();

                textBox_food_CH.Foreground = Brushes.Black;
                textBox_food_CH.Text = selected.CH.ToString();

                textBox_food_fat.Foreground = Brushes.Black;
                textBox_food_fat.Text = selected.Fat.ToString();

                textBox_food_fiber.Foreground = Brushes.Black;
                textBox_food_fiber.Text = selected.Fiber.ToString();

                comboBox_food_GI.SelectedItem = selected.GI.ToString();

                if (comboBox_food_modify_cathegory.SelectedValue.ToString() == Viewmodel.FoodCathergory[0])
                {
                    radioButton_food_food.IsChecked = true;
                    radioButton_food_supplement.IsChecked = false;
                }
                else
                {
                    radioButton_food_food.IsChecked = false;
                    radioButton_food_supplement.IsChecked = true;
                }

                if (!textBox_food_protein.Text.Contains(","))
                    textBox_food_protein.Text += ",0";

                if (!textBox_food_CH.Text.Contains(","))
                    textBox_food_CH.Text += ",0";

                if (!textBox_food_fat.Text.Contains(","))
                    textBox_food_fat.Text += ",0";

                if (!textBox_food_fiber.Text.Contains(","))
                    textBox_food_fiber.Text += ",0";
            }
        }
        void slider_newdiet_calorie_modify_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBox_newdiet_modify.Text = ((int)(slider_newdiet_calorie_modify.Value)).ToString();
            textBox_newdiet_all.Text = (Formula.dailyCalorie + int.Parse(textBox_newdiet_modify.Text.ToString())).ToString();
        }
        #endregion
    }
}
