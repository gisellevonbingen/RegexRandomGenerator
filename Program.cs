using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexRandomGenerator
{
    public static class Program
    {
        public static RegexRandomizer Randomizer = new RegexRandomizer() { TimesMinimumInsclusive = 2, TimesMaximumExclusive = 4 };

        public static void Main(string[] args)
        {
            Test();
            Generater();
        }

        private static void Test()
        {
            var end = ParseAndPrint("]");
            var escape = ParseAndPrint("\\\\_");

            var c1 = ParseAndPrint("\\d");
            var c2 = ParseAndPrint("\\.");
            var c3 = ParseAndPrint("\\\\");
            var c4 = ParseAndPrint("\\65");
            var c5 = ParseAndPrint("\\1234");

            var or1 = ParseAndPrint("gray|grey");
            var or2 = ParseAndPrint("\\d|\\w");

            var group1 = ParseAndPrint("gro(u|o)p");
            var group2 = ParseAndPrint("gro(u|(oo))p");
            var group3 = ParseAndPrint("gro((u|(oo|ooo)))p");

            var optional1 = ParseAndPrint("gra?yg?rey");
            var optional2 = ParseAndPrint("gr(a|e)?y");

            var multiple1 = ParseAndPrint("a*b*c");
            var multiple2 = ParseAndPrint("(a|b)*c");
            var multiple3 = ParseAndPrint("a+b+c");
            var multiple4 = ParseAndPrint("(a|b)+c");

            var times1 = ParseAndPrint("(ab){1}");
            var times2 = ParseAndPrint("(ab){2,}");
            var times3 = ParseAndPrint("(ab){1,3}");
            var times4 = ParseAndPrint("(ab){4,4}");
            var times5 = ParseAndPrint("(ab){,5}");

            var set1 = ParseAndPrint("[]");
            var set2 = ParseAndPrint("[abc]");
            var set3 = ParseAndPrint("[a-z]");
            var set4 = ParseAndPrint("[0-9a-zA-Z]");
            var set5 = ParseAndPrint("[0-]");
            var set6 = ParseAndPrint("[----0a^]");
            var set7 = ParseAndPrint("[-]");
            var set8 = ParseAndPrint("[^0-9]");
            var set9 = ParseAndPrint("[a-zA-Z0-9+-\\_.]+");
            var set10 = ParseAndPrint("[a-zA-Z0-9+-\\\\_.]+");
            var set11 = ParseAndPrint("(.[_a-z0-9-]+)*");
        }

        private static void Generater()
        {
            var test1 = ParseAndRandom(@"\d{3}-\d{4}-\d{3}");
            var test2 = ParseAndRandom(@"(0|(1(01*0)*1))*");
            var test3 = ParseAndRandom(@"01[069]-\d\d\d\d?(-\d\d\d\d){2}");
            var test4 = ParseAndRandom(@"안녕하세요 제 이름은 [김이박최황안][가-힇]{2} (입니다|일까요\?)");
            var test5 = ParseAndRandom(@"가{10,}");
            var test6 = ParseAndRandom(@"(void|char|int|double) [A-Za-z_]\w{8} \((int|char) [A-Za-z_]\w{3}(, (int|char) [A-Za-z_]\w{3}){1,2}\);");
        }

        public static RegexNode ParseAndPrint(string input)
        {
            Console.WriteLine("==========");
            var node = RegexNode.Parse(input);
            Console.WriteLine(node);

            return node;
        }

        public static RegexNode ParseAndRandom(string input)
        {
            var node = ParseAndPrint(input);
            var regex = new Regex(input);

            for (var i = 0; i < 10; i++)
            {
                var choice = node.ChoiceRandom(Randomizer);
                Console.WriteLine($"{i}: {choice}");

                if (regex.IsMatch(choice) == false)
                {
                    throw new Exception();
                }

            }

            return node;
        }

    }

}
