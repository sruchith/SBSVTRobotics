# SBSVTRobotics

Uses cases handled by API

GetAsync - This endpoint is called by default when the API is up and running. It returns default list of Robots and their coordinate
RobotShortestDistance - This is a post call which returns the suitable robot for getting the job done by taking in the desired coordinates. 


Installation and Getting Started.

Clone the repository using Visual Studio 2019 (recommended)
Build the solution
Run the API to fire up the browser
From postman, configure a HttpPost call with the port# that the API is up and running. (For example: https://localhost:8080/RobotShortestDistance)

SampleRequestBody

{
xCoordinate: "2",
yCoordinate: "5"

}
