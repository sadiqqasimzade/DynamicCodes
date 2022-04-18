using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SmartCodes.Models
{
    internal class GatheringAllMethods  //bu class-in icindeki butun metodlari yigir
    {
        public static List<MethodInfo> MethodInfos; //.invoke(null,null)

        static GatheringAllMethods()
        {
            MethodInfos = new List<MethodInfo>();
            foreach (var method in typeof(GatheringAllMethods).GetMethods()) //DeclaredOnly BiddingFlag ancaq bu classda olanlar
                if (method.ReturnType.Name == typeof(void).Name) //metodun tipi   PS:Butun metodlari gotursek .Equals .GetType .Hascode da goturelecek
                    MethodInfos.Add(method);
        }
        

    }
}
