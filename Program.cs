using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.IO;

namespace Selimm_Code_Analyzer
{
    /**
     * The Program is complete on one part, the only thing remaining is to get the correct regex 
     * strings. There are some Console customizations in the code, they arent neccessary so you can strip them off.
     *
    **/
    class Program
    {
        static void Main(string[] args)
        {
            new Program().init();
        }

        public void init()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Title = "Code Complexity Analyzer";
            Console.Write("Initializing Program  \r");
            Thread.Sleep(1000);
            Console.WriteLine("Please Enter the name of source file you wish to analyze  ");
            //all the source code files must be in a folder named code right beside the program exe
            //the source file name here is simply the name of the source file you wanna analyze
            this.buildMatches();
            this.sourceFile = Console.ReadLine();
            string type = sourceFile.Substring(sourceFile.LastIndexOf("."));
            if (!langKeywords.ContainsKey(type))
            {
                Console.WriteLine("Error: Currently cannot analyze this source type.");
                return;
            }
            this.loadCode();
            this.calculate();
            Console.ReadKey();
        }
        private void loadCode()
        {
            this.reader = new StreamReader(String.Format("./code/{0}", this.sourceFile));
            codeList = new List<CodeLineProfile>();
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            string curBuf;
            while (!reader.EndOfStream)
            {
                //Console.Clear();
                curBuf = reader.ReadLine().Trim();

                if (curBuf.Length == 0)
                {
                    ++this.curLineNumber;
                    continue;
                }

                Console.Write("Analyzing \t line {0}  \n", this.curLineNumber);
                Console.WriteLine(curBuf);
                codeList.Add(this.matchPredicate(curBuf));
                Console.Write("Done with line {0}  \n\n", this.curLineNumber);
                ++this.curLineNumber;
                //Thread.Sleep(1000);
            }
        }
        private void buildMatches()
        {
            //Match the following forms of varibles declaration context
            this.matches = new Dictionary<string, string>();
            matches["hasVariable"] = @"[_$A-Za-z][\w$_0-9]*\s+[_$A-Za-z][\w$_0-9]*\s*;";
            matches["hasVariableEqual"] = @"[_$A-Za-z][\w$_0-9]*\s+[_$A-Za-z][\w$_0-9]*\s*=";
            matches["hasVariableMore"] = @"[_$A-Za-z][\w$_0-9]*\s+[_$A-Za-z][\w$_0-9]*\s*,";
            matches["hasVariableMore2"] = @",\s*[_$A-Za-z][\w$_0-9]*\s*,";
            matches["hasLoop"] = @".*(for|while|foreach)\s*\(";
            matches["hasFunctionCall"] = @"[_$A-Za-z][\w$_]*\s*\(.*\)";
            matches["hasCondition"] = @"(if|switch|else)";
            //sequence is matched if all above fails

            //language dependent
            // Do not match the following keywords,so has not to create confusion
            langKeywords = new Dictionary<string, List<string>>();
            langKeywords[".cs"] = new List<string>() {"Using","using","System","namespace","class", "public","private",
                "return"};
            langKeywords[".cpp"] = new List<string>() { "#", "delete", "assert", "namespace", "class", "public",
                "private", "return","cout","cin","<<",">>" };
            langKeywords[".java"] = new List<string>() { "#", "delete", "assert", "import", "package",
                "namespace", "class", "public", "private", "return" };
            ////leave
           


        }
        private CodeLineProfile matchPredicate(string code)
        {
            //what if it has a loop and condition? then we sum the CWU together
            CodeLineProfile cdp = new CodeLineProfile();
            int stmtMatch = 0;
            if ((this.mc = Regex.Matches(code, matches["hasVariable"])).Count != 0 ||
                (this.mc = Regex.Matches(code, matches["hasVariableEqual"])).Count != 0 ||
                (this.mc = Regex.Matches(code, matches["hasVariableMore"])).Count != 0 ||
                (this.mc = Regex.Matches(code, matches["hasVariableMore2"])).Count != 0) //matches only one, need to be slpit into more ifs
            {
                stmtMatch++;
                string type = sourceFile.Substring(sourceFile.LastIndexOf("."));

                string variableType = "";
                foreach (string key in langKeywords[type])
                {
                    foreach (Match match in this.mc)
                    {
                        foreach (Capture capture in match.Captures)
                        {
                            variableType = capture.Value.Substring(0, capture.Value.IndexOf(" ")).Trim();

                            if (key == variableType)
                            {
                                return cdp;
                            }
                            cdp.weightIndex = 1;
                        }
                    }
                }
                cdp = this.addCodeProfile("variable", cdp);
            }
            if ((this.mc = Regex.Matches(code, matches["hasLoop"])).Count != 0)
            {
                stmtMatch++;
                cdp = this.addCodeProfile("loop", cdp);
            }
            
            if ((this.mc = Regex.Matches(code, matches["hasFunctionCall"])).Count != 0)
            {
                // introduced so as to fix the issue of finding loop,sequence and function call on the same line

                if (Regex.Match(code, @"(if|switch|else)").Length == 1)
                {
                    stmtMatch++;
                    cdp = this.addCodeProfile("functioncall", cdp);
                }
            }
            if ((this.mc = Regex.Matches(code, matches["hasCondition"])).Count != 0)
            {
                stmtMatch++;
                cdp = this.addCodeProfile("condition", cdp);
            }
            if (stmtMatch == 0) //then its a sequence
                cdp = this.addCodeProfile("sequence", cdp);

            return cdp;
        }
        private CodeLineProfile addCodeProfile(string profileVar, CodeLineProfile newCdp) //this.mc can be used here
        {
            switch (profileVar)
            {
                case "variable":
                    newCdp.numberofVars += this.mc.Count;
                    Console.WriteLine("\tFound {0} variables on line {1}", newCdp.numberofVars, this.curLineNumber);
                    break;
                case "loop":
                    newCdp.weightIndex += LOOP_CWU;
                    Console.WriteLine("\tFound Loop on line {0}", this.curLineNumber);
                    break;
                case "functioncall":
                    newCdp.weightIndex += FUNCALL_CWU;
                    Console.WriteLine("\tFound Function call on line {0}", this.curLineNumber);
                    break;
                case "condition":
                    newCdp.weightIndex += CONDITION_CWU;
                    Console.WriteLine("\tFound condition on line {0}", this.curLineNumber);
                    break;
                default:
                    newCdp.weightIndex += SEQUENCE_CWU;
                    Console.WriteLine("\tLine {0} is a sequence", this.curLineNumber);
                    break;
            }

            ++newCdp.flag;
            return newCdp;
        }

        private void calculate()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            //Console.Clear();
            Console.WriteLine("Now Calculating Code Complexity...");
            string loader = "||";
            foreach (CodeLineProfile codeProfile in codeList)
            {
                if (codeProfile.flag == 0)
                    continue;
                this.complexity += codeProfile.weightIndex * codeProfile.numberofVars;
                //database.addEntry("Line: {0}: WeightIndex: {1} NumberofVars: {2}",curLineNumber, codeProfile.weightIndex, codeProfile.numberofVars);
                Console.Write("\r{0}  ", loader.Equals("||") ? "//" : "||");
                loader = loader.Equals("||") ? "//" : "||";
            }

            //Console.Clear();
            Console.Beep();
            Console.WriteLine("\t\tCode Complexity for {0} is {1}", this.sourceFile, complexity);

        }

        //if no source code file is entered, close the analyzer
        ~Program()
        {
            if (reader != null)
                reader.Close();
        }

        private StreamReader reader;

        private int complexity = 0,
            curLineNumber = 1;

        private string sourceFile = "";

        private List<CodeLineProfile> codeList;

        //private ArrayList variables;
        //keeping a dictionary of keywords
        private Dictionary<string, string> matches;
        private Dictionary<string, List<string>> langKeywords;

        private MatchCollection mc;

        private const int SEQUENCE_CWU = 1,
            CONDITION_CWU = 2,
            LOOP_CWU = 3,
            FUNCALL_CWU = 2;
    }

    class CodeLineProfile //structs were not getting along well
    {
        public int weightIndex = 0;
        public int numberofVars = 0;
        public int flag = 0; //tells if the profile is empty(0) or not (1)
        public int tmpWeight = -1;
    }
}
