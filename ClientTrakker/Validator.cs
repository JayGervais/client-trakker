using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClientTrakker
{
    public static class Validator
    {
        // all methods in a static class are static

        // checks valid phone numbers
        public static bool IsPhoneNumber(TextBox tb, string name)
        {
            bool valid = true;
            if(!Regex.Match(tb.Text, @"^(\(?\d{3}\)?-? *\d{3}-? *-?\d{4})$").Success)
            {
                valid = false;
                MessageBox.Show("Add a valid " + name);
                tb.Focus();
            }
            return valid;
        }

        // checks for letter string value
        public static bool IsLetters(TextBox tb, string name)
        {
            bool valid = true;
            if (!Regex.Match(tb.Text, @"^[a-zA-Z]+$").Success)
            {
                valid = false;
                MessageBox.Show("Add a valid " + name);
                tb.Focus();
            }
            return valid;
        }
        
        // checks for letters and spaces
        public static bool IsLettersAndSpaces(TextBox tb, string name)
        {
            bool valid = true;
            if (!Regex.Match(tb.Text, @"^[0-9A-Za-z ]+$").Success)
            {
                valid = false;
                MessageBox.Show("Add a valid " + name);
                tb.Focus();
            }
            return valid;
        }

        public static bool IsAddress(TextBox tb, string name)
        {
            bool valid = true;
            if (!Regex.Match(tb.Text, @"^[a-zA-Z0-9 .]*$").Success)
            {
                valid = false;
                MessageBox.Show("Add a valid " + name);
                tb.Focus();
            }
            return valid;
        }

        public static bool IsEmail(TextBox tb, string name)
        {
            bool valid = true;
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            if (!Regex.Match(tb.Text.ToLower(), validEmailPattern).Success)
            {
                valid = false;
                MessageBox.Show("Add a valid " + name);
                tb.Focus();
            }
            return valid;
        }

        // tests for an empty string
        public static bool IsNotEmpty(TextBox tb, string name)
        {
            bool valid = true; // setting parameter
            if (tb.Text == "") // empty
            {
                valid = false;
                MessageBox.Show(name + " is required");
                tb.Focus();
            }
            return valid;
        }

        // test if text box contains integer value
        public static bool IsInteger(TextBox tb, string name)
        {
            bool valid = true;
            int value;
            if(!Int32.TryParse(tb.Text, out value)) // parse unsuccessful
            {
                valid = false;
                MessageBox.Show(name + " must be a whole number");
                tb.SelectAll(); // select all content for replacement
                tb.Focus();
            }
            return valid;
        }

        // test if text box contains double value
        public static bool IsDouble(TextBox tb, string name)
        {
            bool valid = true;
            double value;
            if (!Double.TryParse(tb.Text, out value)) // parse unsuccessful
            {
                valid = false;
                MessageBox.Show(name + " has to be a decimal number");
                tb.SelectAll(); // select all content for replacement
                tb.Focus();
            }
            return valid;
        }

        // test if text box contains non-negative integer value
        public static bool IsNonNegativeInt(TextBox tb, string name)
        {
            bool valid = true;
            int value;
            if (!Int32.TryParse(tb.Text, out value)) // parse unsuccessful
            {
                valid = false;
                MessageBox.Show(name + " must be a positive number");
                tb.SelectAll(); // select all content for replacement
                tb.Focus();
            }
            else // parsed correctly
            {
                if(value < 0)
                {
                    valid = false;
                    MessageBox.Show(name + " has to be positive or zero");
                    tb.SelectAll(); // select all content for replacement
                    tb.Focus();
                }
            }
            return valid;
        }

        // test if text box contains non-negative integer value
        public static bool IsNonNegativeDouble(TextBox tb, string name)
        {
            bool valid = true;
            double value;
            if (!Double.TryParse(tb.Text, out value)) // parse unsuccessful
            {
                valid = false;
                MessageBox.Show(name + " has to be a decimal number");
                tb.SelectAll(); // select all content for replacement
                tb.Focus();
            }
            else // parsed correctly
            {
                if (value < 0)
                {
                    valid = false;
                    MessageBox.Show(name + " has to be positive or zero");
                    tb.SelectAll(); // select all content for replacement
                    tb.Focus();
                }
            }
            return valid;
        }

        public static string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

    } // class
} // namespace
