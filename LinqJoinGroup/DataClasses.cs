using System;

namespace LinqJoinGroup
{
    class DataClasses
    {
        public DataClasses(int Airline, int AircraftSubType)
        {
            AirlineRef = Airline;
            AircraftSubTypeRef = AircraftSubType;
        }
        public int AirlineRef { get; set; }
        public int AircraftSubTypeRef { get; set; }
    }

    class AirlineAircraftSubType
    {
        public AirlineAircraftSubType(int Airline, int AircraftSubType, decimal Value)
        {
            AirlineRef = Airline;
            AircraftSubTypeRef = AircraftSubType;
            InsuredValue = Value;
        }
        public int AirlineRef { get; set; }
        public int AircraftSubTypeRef { get; set; }
        public decimal InsuredValue { get; set; }
    }

    class AirlineAccumulation
    {
        public AirlineAccumulation(int Airline, int Terminal, DateTime Hour, int Num, decimal Exp)
        {
            AirlineRef = Airline;
            TerminalRef = Terminal;
            HourStart = Hour;
            NumberOfAircraft = Num;
            Exposure = Exp;
        }
        public int AirlineRef { get; set; }
        public int TerminalRef { get; set; }
        public DateTime HourStart { get; set; }
        public int NumberOfAircraft { get; set; }
        public decimal Exposure { get; set; }
    }
}
