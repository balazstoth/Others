using CommonProject;
using System;

namespace Manager
{
    public class Food
    {
        #region Fields
        string name;
        double calorie;
        double protein;
        double cH;
        double fat;
        double fiber;
        int energy;
        GI gI;
        NutritionType nutritionType;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        public double Calorie
        {
            get
            {
                return calorie;
            }

            set
            {
                calorie = value;
            }
        }
        public double Protein
        {
            get
            {
                return protein;
            }

            set
            {
                protein = value;
            }
        }
        public double CH
        {
            get
            {
                return cH;
            }

            set
            {
                cH = value;
            }
        }
        public double Fat
        {
            get
            {
                return fat;
            }

            set
            {
                fat = value;
            }
        }
        public double Fiber
        {
            get
            {
                return fiber;
            }

            set
            {
                fiber = value;
            }
        }
        public GI GI
        {
            get
            {
                return gI;
            }

            set
            {
                gI = value;
            }
        }
        public NutritionType NutritionType
        {
            get
            {
                return nutritionType;
            }

            set
            {
                nutritionType = value;
            }
        }
        public int Energy
        {
            get
            {
                return energy;
            }

            set
            {
                energy = value;
            }
        }
        #endregion

        public Food(string name, int energy, double calorie, double protein, double cH, double fat, double fiber, GI gI,  NutritionType nutritionType)
        {
            this.name = name;
            this.energy = energy;
            this.calorie = calorie;
            this.protein = protein;
            this.cH = cH;
            this.fat = fat;
            this.fiber = fiber;
            this.gI = gI;
            this.nutritionType = nutritionType;
        }

        public override string ToString()
        {
            return String.Format("Név: {0}, Kalória: {1}", Name, Calorie);
        }
    }
}
