using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;

namespace CssSpecificityTool.LabConsole {
    class Program {

        /*   
           (IDs, Classes, Elements)
           
            A: count the number of ID attributes in the selector (= b)
          
            B: count the number of other attributes and pseudo-classes in the selector (= c)
          
            C: count the number of element names and pseudo-elements in the selector (= d)
        */
        // http://www.csarven.ca/css-specificity-and-selectors#css_specificity_chart
        const string COLUMN_FORMAT = "  {0,-35}  {1,-10} {2,-10} {3,-15}";

        static void Main(string[] args) {
            StylesheetParser stylesheet = StylesheetParser.FromFile("style2.txt");

            var styleBlocks = stylesheet.Parse();

            foreach (var styleBlock in styleBlocks) {
                Console.WriteLine();
                Console.WriteLine("Selectors:");
                
                foreach (var selector in styleBlock.Selectors) {
                    Console.WriteLine(COLUMN_FORMAT, selector, new SelectorSpecificityScore(selector), null, null);
                }
                //Console.WriteLine(styleBlock.BlockContent);

            }
            //Console.WriteLine(styleBlocks);
            
            //TextWriter textWriter = new StreamWriter("resultStyle2.txt");
            //textWriter.Write(styleBlocks);
            //textWriter.Close();
        }




        private static void RunSelectorTests() {

            var tests = new[] {
                                  new SelectorTest("*", 0, 0, 0),
                                  new SelectorTest("li", 0, 0, 1),
                                  new SelectorTest("li:first-line", 0, 0, 2),
                                  new SelectorTest("ul li", 0, 0, 2),
                                  new SelectorTest("ul ol+li", 0, 0, 3),
                                  new SelectorTest("h1 + *[rel=up]", 0, 1, 1),
                                  new SelectorTest("ul ol li.red", 0, 1, 3),
                                  new SelectorTest("li.red.level", 0, 2, 1),
                                  new SelectorTest("#x34y", 1, 0, 0),
                                  new SelectorTest(".tweet .disabled a:hover", 0, 3, 1),
                                  new SelectorTest("p > em", 0, 0, 2),
                                  new SelectorTest(".secondary > p > a.resource", 0, 2, 2),
                                  new SelectorTest("#menu ul:last-child li a", 1, 1, 3),
                                  new SelectorTest("#menu ul:last-child li:last-child a", 1, 2, 3),
                                  new SelectorTest("#footer *:not(nav) li", 1, 0, 2),
                                  new SelectorTest("ul > li ul li ol li:first-letter", 0, 0, 7),
                                  new SelectorTest(".a.b.c.d.e.f.g.h.i.j.k", 0, 11, 0),
                                  new SelectorTest("#element", 1, 0, 0),
                                  new SelectorTest("div[class*=\"post\"]", 0, 1, 1),
                                  new SelectorTest(":lang(fr) > a#flag", 1, 1, 1),
                                  new SelectorTest("input:not([type=\"submit\"])", 0, 1, 1),
                                  new SelectorTest("h1 + p::first-line", 0, 0, 3),
                                  new SelectorTest("div#content p:first-child::first-line", 1, 1, 3),
                              };

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Test of CSS specificity scores: (IDs, Classes, Elements)");
            Console.WriteLine();
            Console.WriteLine(String.Format(COLUMN_FORMAT, "Selector", "Score", "Correct", "Result"));

            foreach (var test in tests) {
                var specificity = new SelectorSpecificityScore(test.Selector);
                bool testOk = test.Score.TotalScore == specificity.TotalScore;

                Console.ForegroundColor = testOk ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine(String.Format(COLUMN_FORMAT, test.Selector, specificity, test.Score, testOk ? "OK" : "FAIL"));
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
