using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;


namespace InterLink
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = new StreamReader(@"acme_worksheet.csv"))
            {
                List<string> dates = new List<string>();
                List<string> persons = new List<string>();

                Dictionary<string, Dictionary<string, double>> maindata = new Dictionary<string, Dictionary<string, double>>();

                bool headerfound = false;
                while (!reader.EndOfStream)
                {
                    

                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (headerfound) 
                    {
                        DateTime curdt = DateTime.ParseExact(values[1], "MMM dd yyyy", CultureInfo.InvariantCulture);
                        string curdtstr = curdt.ToString("yyyy-mm-dd");

                        if(!dates.Contains(curdtstr))
                            dates.Add(curdtstr);

                        if (!persons.Contains(values[0]))
                            persons.Add(values[0]);

                        if(maindata.ContainsKey(values[0]))
                        {
                            Dictionary<string, double> temp = maindata[values[0]];
                            temp.Add(curdtstr, Double.Parse(values[2], CultureInfo.InvariantCulture));
                        } 
                        else
                        {
                            Dictionary<string, double> temp = new Dictionary<string, double>();
                            temp.Add(curdtstr, Double.Parse(values[2], CultureInfo.InvariantCulture));
                            maindata.Add(values[0], temp);
                        }                       

                    }
                    headerfound = true;
                }

                dates.Sort();
                persons.Sort();

                
                StreamWriter sw = new StreamWriter(@"test.csv");

                string[] Dates = dates.ToArray();
                string DT = String.Join("; ", dates);
                sw.WriteLine($"Name/data; {DT}");

                foreach (string person in persons)
                {
                    List<string> row = new List<string>();
                    row.Add(person);
                    foreach (String dt in dates)
                    {
                        string hours= "0";
                        if (maindata[person].ContainsKey(dt))
                        {
                            hours = maindata[person][dt].ToString();
                        }
                        row.Add(hours);
                    }
                    
                    string[] rowAr = row.ToArray();

                    sw.WriteLine(String.Join("; ", rowAr));

                }

                sw.Close();

            }

        }
    }
}
