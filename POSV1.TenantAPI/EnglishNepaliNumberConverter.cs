namespace POSV1.TenantAPI
{
    public static class EnglishNepaliNumberConverter
    {
        private static readonly string[] unitsMap = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static readonly string[] tensMap = { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        public static string ConvertToWords(decimal number)
        {
            if (number == 0)
                return "Zero";

            long wholePart = (long)number;
            int decimalPart = (int)((number - wholePart) * 100); // Convert decimal places to whole number

            string words = ConvertWholeNumberToWords(wholePart);

            if (decimalPart > 0)
                words += " Rupees and " + ConvertWholeNumberToWords(decimalPart) + " Paisa Only";
            else
                words += " Rupees Only";

            return words;
        }

        private static string ConvertWholeNumberToWords(long number)
        {
            if (number == 0)
                return "";

            if (number < 20)
                return unitsMap[number];

            if (number < 100)
                return tensMap[number / 10] + (number % 10 > 0 ? " " + unitsMap[number % 10] : "");

            if (number < 1000)
                return unitsMap[number / 100] + " Hundred" + (number % 100 > 0 ? " " + ConvertWholeNumberToWords(number % 100) : "");

            if (number < 100000)
                return ConvertWholeNumberToWords(number / 1000) + " Thousand" + (number % 1000 > 0 ? " " + ConvertWholeNumberToWords(number % 1000) : "");

            if (number < 10000000)
                return ConvertWholeNumberToWords(number / 100000) + " Lakh" + (number % 100000 > 0 ? " " + ConvertWholeNumberToWords(number % 100000) : "");

            if (number < 1000000000)
                return ConvertWholeNumberToWords(number / 10000000) + " Crore" + (number % 10000000 > 0 ? " " + ConvertWholeNumberToWords(number % 10000000) : "");

            return ConvertWholeNumberToWords(number / 1000000000) + " Arab" + (number % 1000000000 > 0 ? " " + ConvertWholeNumberToWords(number % 1000000000) : "");
        }
    }
}
