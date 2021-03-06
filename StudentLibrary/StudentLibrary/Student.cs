﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLibrary
{
    public class Student
    {
        public string firstName;
        public string lastName;
        public int pointsEarned;

        public void TakeExam(int pointsForExam)
        {
            this.pointsEarned += pointsForExam;
        }

        public void ExtraCredit(int extraPoints)
        {
            this.pointsEarned += extraPoints;
        }

        public int PointsEarned { get { return pointsEarned; } }

        public string GetFormattedName()
        {
            var name = firstName;
            name.PadRight(20);
            string.Concat(name, lastName);
            name.PadRight(40);
            return name;
        }
    }
}
