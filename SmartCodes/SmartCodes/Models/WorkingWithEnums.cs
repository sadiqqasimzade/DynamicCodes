using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCodes.Models
{
    internal class WorkingWithEnums
    {

        public static string SwitchCaseEnum()
        {
            byte choise;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var @enum in Enum.GetValues(typeof(MyEnum)))
                stringBuilder.Append($"{(int)@enum}){@enum.ToString()} ");
            do
            {
                choise = GenericInput.NumberInput<byte>("-----------ChoiseType-----------\n" + stringBuilder + "\nChoise");
                foreach (int @enum in Enum.GetValues(typeof(MyEnum)))
                    if (choise == @enum)
                        return Enum.ToObject(typeof(MyEnum), (byte)@enum).ToString();
            } while (true);
        }



        enum MyEnum
        {
            enum1, enum2
        }
    }
}
