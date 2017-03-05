using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqJoinGroup
{
    class Program
    {
        static void Main(string[] args)
        {
            int terminalRef = 1;
            DateTime hourStart = new DateTime(2017, 03, 05, 20, 0, 0);

            List<AirlineAircraftSubType> airlineAircraftSubTypes = new List<AirlineAircraftSubType>();
            airlineAircraftSubTypes.Add(new AirlineAircraftSubType(1, 1, 100));
            airlineAircraftSubTypes.Add(new AirlineAircraftSubType(1, 2, 120));
            airlineAircraftSubTypes.Add(new AirlineAircraftSubType(2, 1, 200));
            airlineAircraftSubTypes.Add(new AirlineAircraftSubType(2, 2, 220));
            airlineAircraftSubTypes.Add(new AirlineAircraftSubType(3, 1, 300));
            Console.WriteLine("Values:");
            foreach (var aast in airlineAircraftSubTypes)
            {
                Console.WriteLine($"Airline: {aast.AirlineRef}; Aircraft Type: {aast.AircraftSubTypeRef}; Value: {aast.InsuredValue}");
            }
            Console.WriteLine("");
            Console.WriteLine("Traffic:");
            List<DataClasses> hourTerminalTraffic = new List<DataClasses>();            
            hourTerminalTraffic.Add(new DataClasses(1, 1));
            hourTerminalTraffic.Add(new DataClasses(1, 1));
            hourTerminalTraffic.Add(new DataClasses(1, 1));
            hourTerminalTraffic.Add(new DataClasses(1, 2));
            hourTerminalTraffic.Add(new DataClasses(1, 2));
            hourTerminalTraffic.Add(new DataClasses(2, 2));
            hourTerminalTraffic.Add(new DataClasses(2, 2));
            hourTerminalTraffic.Add(new DataClasses(3, 1));
            foreach (var htt in hourTerminalTraffic)
            {
                Console.WriteLine($"Airline: {htt.AirlineRef}; Aircraft Type: {htt.AircraftSubTypeRef}");
            }
            Console.WriteLine("");
            var hourTerminalAccumulations =
                    hourTerminalTraffic
                    .Join(airlineAircraftSubTypes
                          , htt => new { htt.AirlineRef, htt.AircraftSubTypeRef }
                          , aast => new { aast.AirlineRef, aast.AircraftSubTypeRef }
                          , (htt, aast) => new { htt, aast })
                    .GroupBy(airline => airline.htt.AirlineRef)
                    .Select(hta => new AirlineAccumulation(hta.Key
                                                            , terminalRef
                                                            , hourStart
                                                            , hta.Count()
                                                            , hta.Sum(airline => airline.aast.InsuredValue)));
            Console.WriteLine("Airline Accumulations (LINQ):");
            foreach (var hta in hourTerminalAccumulations)
            {
                Console.WriteLine($"Airline: {hta.AirlineRef}; NumberOfAircraft: {hta.NumberOfAircraft}; Exposure: {hta.Exposure}");
            }
            Console.WriteLine("");

            var orderedTT = from DataClasses htt in hourTerminalTraffic
                            orderby htt.AirlineRef, htt.AircraftSubTypeRef
                            select htt;
            var orderedAAST = (from AirlineAircraftSubType ast in airlineAircraftSubTypes
                              orderby ast.AirlineRef, ast.AircraftSubTypeRef
                              select ast).ToList();
            int previousAirline = 0;
            bool airlTypeFound = false;
            int i = 0;
            AirlineAircraftSubType AirlType = new AirlineAircraftSubType(0,0,0);
            AirlineAccumulation AirlAcc = new AirlineAccumulation(0,0,hourStart,0,0);
            List<AirlineAccumulation> airlineAccumulations = new List<AirlineAccumulation>();
            foreach (DataClasses tt in orderedTT)
            {
                if (previousAirline != tt.AirlineRef)
                {
                    if (previousAirline != 0) airlineAccumulations.Add(AirlAcc);
                    AirlAcc = new AirlineAccumulation(tt.AirlineRef, terminalRef, hourStart, 0, 0);                    
                }
                airlTypeFound = false;
                while (!airlTypeFound)
                {
                    AirlType = orderedAAST[i];
                    if (AirlType.AirlineRef == tt.AirlineRef && AirlType.AircraftSubTypeRef == tt.AircraftSubTypeRef)
                    {
                        airlTypeFound = true;
                    }
                    else
                    {
                        i++;
                    }
                }
                AirlAcc.NumberOfAircraft++;
                AirlAcc.Exposure += AirlType.InsuredValue;
                previousAirline = tt.AirlineRef;
            }
            if (previousAirline != 0) airlineAccumulations.Add(AirlAcc);

            Console.WriteLine("Airline Accumulations (foreach):");
            foreach (var hta in airlineAccumulations)
            {
                Console.WriteLine($"Airline: {hta.AirlineRef}; NumberOfAircraft: {hta.NumberOfAircraft}; Exposure: {hta.Exposure}");
            }
            Console.WriteLine("");

            if (System.Diagnostics.Debugger.IsAttached) Console.ReadLine();
        }
    }
}
