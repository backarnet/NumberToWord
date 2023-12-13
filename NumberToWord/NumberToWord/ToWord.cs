using System;
namespace NumberToWord
{
    class ToWord
    {
        private static long _intergerValue;
        private static int _decimalValue;

        private static string GetDecimalValue(string decimalPart)
        {
            const int POINT = 2;
            string result;
            if (decimalPart.Length != POINT)
            {
                int decimalPartLength = decimalPart.Length;
                for (int i = 0; i < POINT - decimalPartLength; i++)
                {
                    decimalPart += "0";
                }
                result = string.Format("{0}.{1}",
                    decimalPart.Substring(0, POINT),
                    decimalPart.Substring(POINT, decimalPart.Length - POINT));
                result = Math.Round(Convert.ToDecimal(result)).ToString();
            }
            else
                result = decimalPart;
            for (int i = 0; i < POINT - result.Length; i++)
            {
                result += "0";
            }
            return result;
        }

        private static void ExtractIntegerAnddecimalParts(decimal num)
        {
            string[] splits = num.ToString().Split('.');
            _intergerValue = Convert.ToInt64(splits[0]);
            _decimalValue = splits.Length > 1 ? Convert.ToInt32(GetDecimalValue(splits[1])) : 0;
        }

        private static string[] arabicOnes =
        {
            "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة",
            "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر"
        };
        private static string[] arabicTens =
        {
            "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون"
        };
        private static string[] arabicHundreds =
        {
            "", "مائة", "مئتان", "ثلاثمائة", "أربعمائة", "خمسمائة", "ستمائة", "سبعمائة", "ثمانمائة","تسعمائة"
        };
        private static string[] arabicAppendedTwos =
        {
            "مئتا", "ألفا", "مليونا", "مليارا", "ترليونا"
        };
        private static string[] arabicTwos =
        {
            "مئتان", "ألفان", "مليونان", "ملياران", "ترليونان"
        };
        private static string[] arabicGroup =
        {
            "مائة", "ألف", "مليون", "مليار", "ترليون"
        };
        private static string[] arabicAppendedGroup =
        {
            "", "ألفاً", "مليوناً", "ملياراً", "ترليوناً"
        };
        private static string[] arabicPluralGroups =
        {
            "", "آلاف", "ملايين", "مليارات", "ترليونات"
        };

        private static string ProcessArabicGroup(int groupNumber, int groupLevel, decimal remainingNumber)
        {
            int tens = groupNumber % 100;
            int hundreds = groupNumber / 100;
            string retVal = "";
            if (hundreds > 0)
            {
                if (tens == 0 && hundreds == 2)
                {
                    retVal = string.Format("{0}", arabicAppendedTwos[0]);
                }
                else
                {
                    retVal = string.Format("{0}", arabicHundreds[hundreds]);
                }
            }
            if (tens > 0)
            {
                if (tens < 20)
                {
                    if (tens == 2 && hundreds == 0 && groupLevel > 0)
                    {
                        if (_intergerValue == (2000 | 2000000 | 2000000000 | 2000000000000 | 2000000000000000 | 2000000000000000000))
                        {
                            retVal = string.Format("{0}", arabicAppendedTwos[groupLevel]);
                        }
                        else
                        {
                            retVal = string.Format("{0}", arabicTwos[groupLevel]);
                        }
                    }
                    else
                    {
                        if (retVal != "")
                        {
                            retVal += " و";
                        }
                        if (tens == 1 && groupLevel > 0 && hundreds == 0)
                        {
                            retVal += " ";
                        }
                        else if ((tens == (1 | 2)) && (groupLevel == (0 | -1)) && hundreds == 0 && remainingNumber == 0)
                        {
                            retVal += "";
                        }
                        else
                        {
                            retVal += arabicOnes[tens];
                        }
                    }
                }
                else
                {
                    int ones = tens % 10;
                    tens = (tens / 10) - 2;
                    if (ones > 0)
                    {
                        if (retVal != "")
                        {
                            retVal += " و";
                        }
                        retVal += arabicOnes[ones];
                    }
                    if (retVal != "")
                    {
                        retVal += " و";
                    }
                    retVal += arabicTens[tens];
                }
            }
            return retVal;
        }

        public static string ConvertToArabic(decimal num)
        {
            ExtractIntegerAnddecimalParts(num);
            decimal tempNumber = num;
            if (tempNumber == 0)
            {
                return "صفر";
            }
            string decimalstring = ProcessArabicGroup(_decimalValue, -1, 0);
            string retVal = "";
            byte group = 0;
            while (tempNumber >= 1)
            {
                int numberToProcess = (int)(tempNumber % 1000);
                tempNumber = tempNumber / 1000;
                string groupDescription = ProcessArabicGroup(numberToProcess, group, Math.Floor(tempNumber));
                if (groupDescription != "")
                {
                    if (group > 0)
                    {
                        if (retVal != "")
                        {
                            retVal = string.Format("{0}{1}", "و", retVal);
                        }
                        if (numberToProcess != 2)
                        {
                            if (numberToProcess % 100 != 1)
                            {
                                if (numberToProcess >= 3 && numberToProcess <= 10)
                                {
                                    retVal = string.Format("{0} {1}", arabicPluralGroups[group], retVal);
                                }
                                else
                                {
                                    if (retVal != "")
                                    {
                                        retVal = string.Format("{0} {1}", arabicAppendedGroup[group], retVal);
                                    }
                                    else
                                    {
                                        retVal = string.Format("{0} {1}", arabicGroup[group], retVal);
                                    }
                                }
                            }
                            else
                            {
                                retVal = string.Format("{0} {1}", arabicGroup[group], retVal);
                            }
                        }
                    }
                    retVal = string.Format("{0} {1}", groupDescription, retVal);
                }
                group++;
            }
            string formattedNumber = "";
            formattedNumber += (retVal != "") ? retVal.Trim() : "";
            formattedNumber += (_decimalValue != 0) ? "، و" + decimalstring + " بالمائة." : ".";
            return formattedNumber;
        }
    }
}
