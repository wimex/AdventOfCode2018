using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Day04
{
    public class Program
    {
        static void Main(string[] args)
        {
            //minutes the selected guard spends asleep
            Dictionary<int, List<int>> asleepMinutes = new Dictionary<int, List<int>>();

            string[] lines = File.ReadAllLines("input.txt");
            Regex regex = new Regex(@"\[(\d+)-(\d+)-(\d+) (\d+):(\d+)\] [A-z ]+\#?([0-9]+)?[A-z ]+");
            
            int currentGuardId = -1;
            DateTime previousEvent = DateTime.MinValue;
            foreach (string line in lines.OrderBy(l => l))
            {
                Console.WriteLine(line);

                Match match = regex.Match(line);
                DateTime timestamp = new DateTime(
                    int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value),
                    int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value), 0);

                if (line.Contains("begins shift"))
                {
                    currentGuardId = int.Parse(match.Groups[6].Value);
                    continue;
                }

                if (!asleepMinutes.ContainsKey(currentGuardId))
                    asleepMinutes.Add(currentGuardId, new List<int>());

                if (line.Contains("falls asleep"))
                {
                    asleepMinutes[currentGuardId].Add(timestamp.Minute);
                    previousEvent = timestamp.AddMinutes(1);
                    continue;
                }

                if (line.Contains("wakes up"))
                {
                    if (previousEvent != DateTime.MinValue && previousEvent.Day != timestamp.Day)
                        throw new Exception("Invalid event order");
                    
                    while (previousEvent < timestamp)
                    {
                        asleepMinutes[currentGuardId].Add(previousEvent.Minute);
                        previousEvent = previousEvent.AddMinutes(1);
                    }
                }
            }

            Dictionary<int, Dictionary<int, int>> statistics1 = asleepMinutes
                .OrderByDescending(s => s.Value.Count)                      //start with the guard that has the most asleep minutes
                .ToDictionary(                                              //convert the result to a dictionary
                    k => k.Key,                                             //the key is the guardId
                    v => v.Value                                            //the value is...
                        .GroupBy(g => g)                                    //grouped by the minute
                        .OrderByDescending(g => g.Count())                  //ordered by the minute appearing most often
                        .ToDictionary(k => k.Key, i => i.Count()));         //and converted to another dictionary where the key is the item, the value is the occurance
            
            int mostAsleepGuardId = statistics1.First().Key;
            int totalAsleepTime = statistics1[mostAsleepGuardId].Values.Sum();
            int mostOftenAsleep = statistics1[mostAsleepGuardId].Keys.First();
            Console.WriteLine($"Guard #{mostAsleepGuardId} is asleep {totalAsleepTime} minutes in total, most often asleep at {mostOftenAsleep} (checksum: {mostAsleepGuardId * mostOftenAsleep})");

            var statistics2 = statistics1
                .Select(s => new                                            //select...
                {                                                           
                    s.Key,                                                  //the guardId
                    Item = s.Value.Keys.First(),                            //the minute he is most often asleep
                    Cnt = s.Value[s.Value.Keys.First()]                     //the number of times this minute appears
                })
                .OrderByDescending(s => s.Cnt)                              //order by the most often appearing minute
                .ToList();                    
            Console.WriteLine($"Guard #{statistics2.First().Key} sleeps most often at {statistics2.First().Item} - {statistics2.First().Cnt} times (checksum: {statistics2.First().Key * statistics2.First().Item})");
            Console.ReadKey();
        }
    }
}
