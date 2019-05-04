using CommonProject;
using System;

namespace Manager
{
    public class BasicInfo
    {
        #region Fields
        int age;
        double weightCurrent;
        double weightGoal;
        Gender gender;
        int height;
        DateTime start;
        DietBasedOn basedOn;
        int weeks;
        ActivityPerWeek activity;
        int calorieBase;
        int calorieModified;
        bool descreasingCalorie;
        #endregion

        #region Properties
        public int calorieSum { get { return calorieBase + calorieModified; } }
        public int Age
        {
            get
            {
                return age;
            }

            set
            {
                age = value;
            }
        }
        public double WeightCurrent
        {
            get
            {
                return weightCurrent;
            }

            set
            {
                weightCurrent = value;
            }
        }
        public double WeightGoal
        {
            get
            {
                return weightGoal;
            }

            set
            {
                weightGoal = value;
            }
        }
        public Gender Gender
        {
            get
            {
                return gender;
            }

            set
            {
                gender = value;
            }
        }
        public int Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }
        public DateTime Start
        {
            get
            {
                return start;
            }

            set
            {
                start = value;
            }
        }
        public DietBasedOn BasedOn
        {
            get
            {
                return basedOn;
            }

            set
            {
                basedOn = value;
            }
        }
        public int Weeks
        {
            get
            {
                return weeks;
            }

            set
            {
                weeks = value;
            }
        }
        public ActivityPerWeek Activity
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
        public int CalorieBase
        {
            get
            {
                return calorieBase;
            }

            set
            {
                calorieBase = value;
            }
        }
        public int CalorieModified
        {
            get
            {
                return calorieModified;
            }

            set
            {
                calorieModified = value;
            }
        }
        public TypeOfDiet DietType
        {
            get
            {
                if (calorieModified > 0)
                    return TypeOfDiet.MassGaining;
                else
                {
                    if (calorieModified < 0)
                        return TypeOfDiet.Diet;
                    else
                        return TypeOfDiet.Maintenance;
                }
            }
        }
        public bool DescreasingCalorie
        {
            get
            {
                return descreasingCalorie;
            }

            set
            {
                descreasingCalorie = value;
            }
        } 
        #endregion

        public BasicInfo(int age, double weightCurrent, double weightGoal, Gender gender, int height, DateTime start, DietBasedOn basedOn,
                         int weeks, ActivityPerWeek activity, int calorieBase, int calorieModified, bool descreasingCalorie)
        {
            this.weeks = weeks;
            this.age = age;
            this.height = height;
            this.gender = gender;
            this.weightCurrent = weightCurrent;
            this.weightGoal = weightGoal;
            this.start = start;
            this.basedOn = basedOn;
            this.activity = activity;
            this.calorieBase = calorieBase;
            this.calorieModified = calorieModified;
            this.descreasingCalorie = descreasingCalorie;
        }
    }
}
