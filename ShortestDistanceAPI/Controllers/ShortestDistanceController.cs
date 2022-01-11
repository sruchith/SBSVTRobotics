using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ShortestDistanceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShortestDistanceController : ControllerBase
    {
        private const string RobotsUrl = "https://60c8ed887dafc90017ffbd56.mockapi.io/robots";


        [HttpGet]
        public async Task<List<RobotCoordinates>> GetAsync()
        {
            try
            {

                List<RobotCoordinates> defaultPositions = GetRobotsAsync().Result.ToList();

                return defaultPositions.OrderBy(a => a.x).ToList();
            }

            catch (Exception ex)
            {
                return new List<RobotCoordinates>();
            }
        }

        [HttpPost]
        public ShortestDistanceResponse RobotShortestDistance(ShortestDistanceRequest shortestDistanceRequest)
        {
            List<RobotCoordinates> returnRobots = GetRobotsAsync().Result.ToList();

            List<ShortestDistanceResponse> shortestDistances = GetShortestDistances(returnRobots, shortestDistanceRequest.xCoordinate, shortestDistanceRequest.yCoordinate);

            shortestDistances = shortestDistances.OrderBy(a => a.DistanceTotal).ThenByDescending(a => a.BatteryLevel).ToList();

            return shortestDistances.FirstOrDefault();
        }


        public async Task<List<RobotCoordinates>> GetRobotsAsync()
        {

            List<RobotCoordinates> robotCoordinates = new List<RobotCoordinates>();

            using (var client = new HttpClient())
            {
                IPHostEntry hostInfo = Dns.Resolve(RobotsUrl);

                client.BaseAddress = new Uri(RobotsUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("https://60c8ed887dafc90017ffbd56.mockapi.io/robots");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    robotCoordinates = JsonConvert.DeserializeObject<List<RobotCoordinates>>(result);

                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }


            return robotCoordinates;
        }

        public List<ShortestDistanceResponse> GetShortestDistances(List<RobotCoordinates> robotCoordinates, int x, int y)
        {

            List<ShortestDistanceResponse> shortestDistances = new List<ShortestDistanceResponse>();

            foreach (RobotCoordinates robot in robotCoordinates)
            {
                ShortestDistanceResponse _singleRobot = new ShortestDistanceResponse();
                float xCoordinate = robot.x - x;
                float yCoordinate = robot.y - y;

                double _distance = Math.Sqrt(xCoordinate * xCoordinate + yCoordinate * yCoordinate);

                _singleRobot.DistanceTotal = _distance;
                _singleRobot.RobotId = robot.RobotId;
                _singleRobot.BatteryLevel = robot.BatteryLevel;

                shortestDistances.Add(_singleRobot);

            }

            return shortestDistances;

        }

        //private double RobotForTransportation(List<PointF> pm, PointF p1, PointF p2, out PointF closest)
        //{
        //    float dx = p2.X - p1.X;
        //    float dy = p2.Y - p1.Y;
        //    closest = default;
        //    foreach (PointF pt in pm)
        //    {

        //        if ((dx == 0) && (dy == 0))
        //        {
        //            closest = p1;
        //            dx = pt.X - p1.X;
        //            dy = pt.Y - p1.Y;
        //            return Math.Sqrt(dx * dx + dy * dy);
        //        }

        //        float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) /
        //            (dx * dx + dy * dy);

        //        if (t < 0)
        //        {
        //            closest = new PointF(p1.X, p1.Y);
        //            dx = pt.X - p1.X;
        //            dy = pt.Y - p1.Y;
        //        }
        //        else if (t > 1)
        //        {
        //            closest = new PointF(p2.X, p2.Y);
        //            dx = pt.X - p2.X;
        //            dy = pt.Y - p2.Y;
        //        }
        //        else
        //        {
        //            closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
        //            dx = pt.X - closest.X;
        //            dy = pt.Y - closest.Y;
        //        }
        //    }


        //    return Math.Sqrt(dx * dx + dy * dy);
        //}
    }

}
