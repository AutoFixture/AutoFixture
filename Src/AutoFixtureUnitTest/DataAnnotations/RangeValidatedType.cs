﻿using System.ComponentModel.DataAnnotations;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class RangeValidatedType
    {
        public const int Minimum = 10;
        public const int Maximum = 20;
        
        public const double DoubleMinimum = 10.1;
        public const double DoubleMaximum = 20.2;

        public const string StringMinimum = "10.1";
        public const string StringMaximum = "20.2";

        [Range(Minimum, Maximum)]
        public decimal Field;

        [Range(Minimum, Maximum)]
        public decimal Property { get; set; }

        [Range(DoubleMinimum, DoubleMaximum)]
        public double Property2 { get; set; }

        [Range(DoubleMinimum, DoubleMaximum)]
        public decimal Property3 { get; set; }

        [Range(typeof(decimal), StringMinimum, StringMaximum)]
        public decimal Property4 { get; set; }

        [Range(Minimum, Maximum)]
        public int Property5 { get; set; }

        [Range(double.MinValue, Maximum)]
        public double PropertyWithMinimumDoubleMinValue { get; set; }

        [Range(Minimum, double.MaxValue)]
        public double PropertyWithMaximumDoubleMaxValue { get; set; }
    }
}