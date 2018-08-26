# Maze Exercise

This solution was developed in Visual Studio 2015. It is implemented as an ASP.NET Web API exposing a single endpoint, /solveMaze.

## Approach

The application uses two search algorithms: (1) Depth-first search (DFS) and (2) Breadth-first search (BFS). While DFS is a simpler algotithm based on recursion, will not necessarily deliver the shortest path from Start to Finish; rather, it will return the first VALID solution if one exists. BFS, on the other hand, is meant to return the SHORTEST possible solution.

### To run from Visual Studio:

1. Clone the repository to a location on your hard drive.
2. Open the VS solution and set the startup page to index.html (this page is a small client that places an AJAX call to the service).
3. Run a build to ensure all NuGet packages are installed.
4. Press Ctrl + F5 to run without debugging.

### To run from full IIS:

1. Clone the repository to a location on your hard drive.
2. Open the VS solution and publish the project MazeExcersize using your preferred Publish Methid
3. In IIS Manager, create a new site and point it to the folder with the deployed solution. Specify port: **8080** and Application pool: **DefaultAppPool** (for test purposes only).
4. Run a build to ensure all NuGet packages are installed.
5. In your web browser, navigate to http:/localhost:8080/index.html.
