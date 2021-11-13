
using System;

namespace IsuExtra
{
    internal class Program
    {
        private static void Main()
        {
            string str1 = "london";
            string str2 = "london";
 
            Console.WriteLine(str1 == str2); // true
            str1.Equals(str2); // true
        }
    }
}
