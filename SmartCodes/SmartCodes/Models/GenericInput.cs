using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCodes.Models
{
    internal class GenericInput
    {
        public static T NumberInput<T>(string str, decimal min = decimal.MinValue, decimal max = decimal.MaxValue)
        {
        Point:
            try
            {
                Console.Write(str + " :");
                dynamic temp = Console.ReadLine();
                temp = (T)Convert.ChangeType(temp, typeof(T));
                if (temp >= min || temp <= max)
                    return temp;
                throw new Exception("Incorrect Number");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto Point;
            }
        }

        public static string StringInput(string str)
        {
            string name;
            do
            {
                Console.Write(str+":");
                name = Console.ReadLine();
            } while (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name));
            return name;
        }


    }
}
