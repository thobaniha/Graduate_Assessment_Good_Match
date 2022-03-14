using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Graduate_Assessment_Good_Match
{
    class Goodmatch
    {
        public static Dictionary<String, int> map = new Dictionary<String, int>();
        public static int CharOccurence(String seq,char ch) {
            int nummberOfOccurence = 0;
            for(int i =0 ;i < seq.Length; i++) { 
                if (seq[i] == ch)
                    nummberOfOccurence += 1;
            }
            return nummberOfOccurence;
       }
        public static String LetterOccurence(String name) {
            String temp = "";
            String output = "";
            for (int h = 0; h < name.Length; h++) {
                if (Char.IsLetter(name[h]) && !(temp.Contains(name[h])))
                {
                    output += CharOccurence(name, name[h]);
                    temp += name[h];
                }
               
            }
            return output;
        }
        public static int getScore(String number) {
            String reduced = "";
            int sum;
            LinkedList<String> list = new LinkedList<String>();
            for (int i = 0; i < number.Length; i++)
                list.AddLast(number[i].ToString());
            while (list.Count > 1) {
                sum = Int32.Parse(list.First.Value) + Int32.Parse(list.Last.Value);
                reduced += sum;
                list.RemoveFirst();
                list.RemoveLast();
            }
            if (list.Count == 1)
                reduced += list.First.Value;
            if (reduced.Length > 2)
                return getScore(reduced);
            else if (reduced.Length == 1)
                reduced += list.First.Value;

            return Int32.Parse(reduced);
        }
        public static void getMatch(String name) {
            String numbers = LetterOccurence(name);
                map.Add(name, getScore(numbers));
        }
        public static int getSmallSet(SortedSet<String> a, SortedSet<String> b) {
            if (a.Count < b.Count)
                return a.Count;
            else if (a.Count > b.Count)
                return b.Count;
            else
                return a.Count;
        }
        public static void finalOutput() {
            List<String> names = new List<String>();
            List<String> gender = new List<String>();

            SortedSet<String> males = new SortedSet<String>();
            SortedSet<String> femals = new SortedSet<String>();
            var dir = Directory.GetCurrentDirectory();
            String file =Path.Combine(dir,"output.txt");
            String csvPath = Path.Combine(dir, "Names.csv");
            using (var reader = new StreamReader(csvPath))
            {
         
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    names.Add(values[0]);
                    gender.Add(values[1]);
                }
            }

            for (int i = 0; i < names.Count; i++)
            {
                if (gender[i].ToString().Equals("m"))
                    males.Add(names[i]);
                else
                    femals.Add(names[i]);
            }
            String seq = " matches ";
            int small = getSmallSet(males, femals);
            for (int i = 0; i < small; i++)
            {
                getMatch(males.ElementAt(i) + seq + femals.ElementAt(i));
            }
            var items = from pair in map orderby pair.Value descending select pair;

            using (StreamWriter write = new StreamWriter(file))
            {

                foreach (KeyValuePair<String, int> pair in items)
                {
                    if (pair.Value >= 80)
                        write.WriteLine( pair.Key + " " + pair.Value + "% good match");
                    else
                        write.WriteLine(pair.Key + " " + pair.Value + "%");
                }
                
            }
        }
        static void Main(string[] args)
        {
            finalOutput();
            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine("NB: The results are stored in a file named output.txt \n Please Navigate to the following path for the results,(your current path):");
            Console.WriteLine();
            Console.WriteLine(dir);
            
            Console.ReadKey();
        }
    }
}
