using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace FrontlineTest
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Program started!");

            string inputString = @"(id,created,employee(id,firstname,employeeType(id), lastname),location)";

            Answer answer = new Answer(inputString);
            var node = answer.BuildTreeFromString();
            Console.WriteLine("-------------Result-----------------");
            answer.PrintTree(node);



            Console.WriteLine("-------------Bonus-----------------");
            answer.PrintBonusTree();
        }
    }
}
