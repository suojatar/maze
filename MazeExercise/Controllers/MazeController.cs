using MazeExercise.Application;
using MazeExercise.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace MazeExercise.Controllers
{
	public class MazeController : ApiController
	{
		[HttpPost]
		[Route("solveMaze")]
		public IHttpActionResult GetMazeSolution(MazePostData mazeMap)
		{
			if (mazeMap == null || string.IsNullOrWhiteSpace(mazeMap.Map))
			{
				return BadRequest("Invalid Maze Map!");
			}

			Maze maze = Maze.Parse(mazeMap.Map);
			Solver solver = new Solver(maze);
			MazeSolution solution = null;

			try
			{
				solution = solver.Solve(mazeMap.SearchType);
			}
			catch (InvalidOperationException ex)
			{
				solution = new MazeSolution(0, ex.Message); //it would be nice to add another property to MazeSolution indicating/describing success or failure (or any kind of status and the reason for it):(
			}

			return Ok(solution);
		}
	}
}
