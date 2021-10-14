using OceanicAirlines.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OceanicAirlines.Services
{
    public class PriceCalculationService : IPriceCalculationService
    {
        public double GetPrice(double height, double width, double depth, double weight)
        {
            double returnPrice;
            DimensionCategory group = GetDimensionGroup(height, width, depth);
            if (group == DimensionCategory.A)
            {
                if (weight < 1)
                {
                    returnPrice = 40;
                }
                else if (weight <= 5)
                {
                    returnPrice = 60;
                }
                else
                {
                    returnPrice = 80;
                }
            }
            else if (group == DimensionCategory.B)
            {
                if (weight < 1)
                {
                    returnPrice = 48;
                }
                else if (weight <= 5)
                {
                    returnPrice = 68;
                }
                else
                {
                    returnPrice = 88;
                }
            }
            else if (group == DimensionCategory.C)
            {
                if (weight < 1)
                {
                    returnPrice = 80;
                }
                else if (weight <= 5)
                {
                    returnPrice = 100;
                }
                else
                {
                    returnPrice = 120;
                }
            }
            else
            {
                returnPrice = -1;
            }
            return returnPrice;
        }
        private DimensionCategory GetDimensionGroup(double height, double width, double depth)
        {
            DimensionCategory returnCategory;
            if (height <= 0.25 && width <= 0.25 && depth <= 0.25)
            {
                returnCategory = DimensionCategory.A;
            }
            else if (height <= 0.4 && width <= 0.4 && depth <= 0.4)
            {
                returnCategory = DimensionCategory.B;
            }
            else if (height <= 2 && width <= 2 && depth <= 2)
            {
                returnCategory = DimensionCategory.C;
            }
            else
            {
                returnCategory = DimensionCategory.undeliverable;
            }
            return returnCategory;
        }
    }
}
