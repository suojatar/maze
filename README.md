# Maze Exercise

This solution was developed in Visual Studio 2015. It is implemented as an ASP.NET Web API exposing a single endpoint, /solveMaze.

## Approach

The application uses the Depth-first search algorithm. It will not necessarily deliver the shortest path from Sstart to Finish; rather, it will return the first VALID solution if one exists.

### To run from Visual Studio:

1. Clone the repository to a location on your hard drive.
2. Open the VS solution and set the startup page to index.html (this page is a small client that places an AJAX call to the service).
3. Run a build to ensure all NuGet packages are installed.
4. Press Ctrl + F5 to run without debugging.