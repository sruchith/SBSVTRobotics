using System;

namespace ShortestDistanceAPI
{
    public class ShortestDistanceResponse
    {
        public int RobotId { get; set; }

        public double DistanceTotal { get; set; }

        public int BatteryLevel { get; set; }
    }
}
