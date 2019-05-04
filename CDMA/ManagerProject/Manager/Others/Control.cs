using CommonProject;
using ManagerProject;
using System;
using System.Linq;
using System.Windows.Media;

namespace Manager
{
    class ControlView
    {
        ManagerWindow mgr;
        string name;
        string protein;
        string ch;
        string fat;
        string fiber;
        string energy;
        bool food;

        public ControlView(ManagerWindow mgr)
        {
            this.mgr = mgr;
            GetValues();
        }

        void GetValues()
        {
            name = mgr.textBox_food_name.Text;
            protein = mgr.textBox_food_protein.Text;
            ch = mgr.textBox_food_CH.Text;
            fat = mgr.textBox_food_fat.Text;
            fiber = mgr.textBox_food_fiber.Text;
            energy = mgr.textBox_food_energy.Text;
            food = mgr.radioButton_food_food.IsChecked == true ? true : false;
        }
        public bool IsEmpty()
        {
            GetValues();
            if (String.IsNullOrWhiteSpace(name) ||
                String.IsNullOrWhiteSpace(protein) ||
                String.IsNullOrWhiteSpace(ch) ||
                String.IsNullOrWhiteSpace(fiber) ||
                String.IsNullOrWhiteSpace(energy) ||
                String.IsNullOrWhiteSpace(fat) ||
                ValueWrong())
                throw new TextBoxesAreEmtpy();
            return false;
        }
        public bool CorrectFormat()
        {
            if ((Regexs.food1.IsMatch(protein) ||
                 Regexs.food2.IsMatch(protein) ||
                 Regexs.food3.IsMatch(protein)) &&

                (Regexs.food1.IsMatch(ch) ||
                 Regexs.food2.IsMatch(ch) ||
                 Regexs.food3.IsMatch(ch)) &&

                (Regexs.food4.IsMatch(energy) ||
                 Regexs.food5.IsMatch(energy) ||
                 Regexs.food6.IsMatch(energy) ||
                 Regexs.food7.IsMatch(energy)) &&

                (Regexs.food1.IsMatch(fiber) ||
                 Regexs.food2.IsMatch(fiber) ||
                 Regexs.food3.IsMatch(fiber)) &&

                (Regexs.food1.IsMatch(fat) ||
                 Regexs.food2.IsMatch(fat) ||
                 Regexs.food3.IsMatch(fat)))
            {
                return true;
            }
            else
                throw new IncorrectFormat();
        }
        public bool AlreadyExists()
        {
            if (food)
            {
                XmlManager.XmlFoodControl();
                var foods = from x in XmlData.XDocFood.Root.Elements(XmlData.XmlFoodSecondRootElement)
                            where x.Element(XmlData.XmlFoodChildElementName).Value.ToLower().Equals(name.ToLower())
                            select x;

                if (foods.Any())
                    throw new FoodAlreadyExists();

                return true;
            }
            else
            {
                XmlManager.XmlSupplementControl();
                var supplements = from x in XmlData.XDocSupplement.Root.Elements(XmlData.XmlSupplementSecondRootElement)
                                  where x.Element(XmlData.XmlSupplementChildElementName).Value.ToLower().Equals(name.ToLower())
                                  select x;

                if (supplements.Any())
                    throw new FoodAlreadyExists();

                return true;
            }
        }
        public bool SumControl(double sum)
        {
            double p = Convert.ToDouble(protein);
            double c = Convert.ToDouble(ch);
            double f = Convert.ToDouble(fat);
            double r = Convert.ToDouble(fiber);
            if ((p + c + f + r) <= sum)
                return true;
            else
                throw new IncorrectSum();
        }
        public void ClearAll()
        {
            mgr.textBox_food_energy .Clear();
            mgr.textBox_food_protein.Clear();
            mgr.textBox_food_CH.Clear();
            mgr.textBox_food_fat.Clear();
            mgr.textBox_food_fiber.Clear();
            mgr.textBox_food_name.Clear();
            mgr.comboBox_food_GI.SelectedIndex = 0;
            mgr.radioButton_food_food.IsChecked = true;
            mgr.radioButton_food_supplement.IsChecked = false;
        }
        public bool ValueWrong()
        {
            if (mgr.textBox_food_name.Foreground == Brushes.Gray ||
                mgr.textBox_food_protein.Foreground == Brushes.Gray ||
                mgr.textBox_food_CH.Foreground == Brushes.Gray ||
                mgr.textBox_food_fiber.Foreground == Brushes.Gray ||
                mgr.textBox_food_energy.Foreground == Brushes.Gray ||
                mgr.textBox_food_fat.Foreground == Brushes.Gray)
                return true;
            return false;
        }
    }
}
