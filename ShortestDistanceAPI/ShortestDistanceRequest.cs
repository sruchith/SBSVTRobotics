using System;
using System.Drawing;

namespace ShortestDistanceAPI
{
    public class ShortestDistanceRequest
    {
        public string LoadId { get; set; }

        public int xCoordinate { get; set; }

        public int yCoordinate { get; set; }
    }
}
