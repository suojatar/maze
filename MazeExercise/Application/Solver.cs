using MazeExercise.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static MazeExercise.Application.Maze;

namespace MazeExercise.Application
{
	public class Solver
	{
		private readonly Maze myMaze;

		public Solver(Maze maze)
		{
			myMaze = maze; //set it in constructor for immutability
		}

		public MazeSolution Solve(int searchType)
		{
			const string ERROR_MESSAGE = "This maze has no solution!";

			var solutionPath = new List<int>(); //stores indices of cells that are part of the solution path

			if (searchType == 0)
			{
				bool solutionResult = IsEndIndexReachedRecursive(myMaze.StartIndex, solutionPath);

				if (!solutionResult)
					throw new InvalidOperationException(ERROR_MESSAGE);
			}
			else
			{
				solutionPath = TraverseUntilEndIndex();

				if (solutionPath == null)
					throw new InvalidOperationException(ERROR_MESSAGE);
			}

			solutionPath.Reverse(); //the Solution path cells are stored in backward order, we need to reverse it
			string mapWithSolution = BuildMapWithSolution(solutionPath);

			MazeSolution solution = new MazeSolution(solutionPath.Count, mapWithSolution);
			return solution;
		}


		////////////////////////////
		/// <summary>
		/// Breadth-first seach. This will return the shortest possible solution.
		/// </summary>
		////////////////////////////
		private List<int> TraverseUntilEndIndex()
		{
			var queuedCells = new Queue<int>(); //stores cell indices that are waiting to be visited, in the FIFO manner
			var visitedCells = new HashSet<int>(); //stores whether a cell index has already been visited
			var queuedCellsByIndex = new HashSet<int>(); //stores whether a cell index has already been queued for visiting. Contains the same cells as queuedCells; however, the search is optimized based on index.
			var refererCells = new Dictionary<int, int>(); //stores how to trace celld back in reverse order: from end to start

			queuedCells.Enqueue(myMaze.StartIndex); //add the start cell first

			while (queuedCells.Count != 0)
			{
				var currentIndex = queuedCells.Dequeue();

				if (currentIndex != myMaze.EndIndex)
				{
					foreach (var neighbor in myMaze.Neighbors(currentIndex))
					{
						if (visitedCells.Contains(neighbor))
							continue;

						if (!queuedCellsByIndex.Contains(neighbor))
						{
							refererCells[neighbor] = currentIndex; // specify how to trace back from neighbor to its referer, which is currentIndex

							queuedCellsByIndex.Add(neighbor);
							queuedCells.Enqueue(neighbor); //add neighbor to queue
						}
					}

					visitedCells.Add(currentIndex);
				}
				else //here we'll go backwards - from the end cell through every referer in refererCells
				{
					var solutionPath = new List<int>();

					solutionPath.Add(currentIndex);

					// Trace back path based on the information in the refererCells dictionary and returns the shortest path
					while ((currentIndex = refererCells[currentIndex]) != myMaze.StartIndex)
						solutionPath.Add(currentIndex);

					return solutionPath;
				}
			}

			return null;
		}


		/// <summary>
		/// Depth-first search.
		/// This algotithm is very fast and efficient (a 1,000,000 x 1,000,000 matrix would probably cause stack overflow but for reasonable sizes it's quite good).
		/// It NOT meant to return the shortest solution, instead, the method will return the FIRST valid solution (there may be others that are shorter).
		/// </summary>
		private bool IsEndIndexReachedRecursive(int cur, List<int> solution, HashSet<int> visited = null)
		{
			if (myMaze.GetCellType(cur) == CellType.End)
				return true;

			if (visited == null)
				visited = new HashSet<int>(); //stores indices of cells that have been visited

			foreach (var cell in myMaze.Neighbors(cur))
			{
				if (visited.Contains(cell))
					continue;

				visited.Add(cell);
				bool searchResult = IsEndIndexReachedRecursive(cell, solution, visited);

				if (searchResult == true)
				{
					// Once the end point is reached, it's added first, so as we go up in our recursion, the cells will be stored in reverse order
					solution.Add(cell);
					return true;
				}
			}

			return false;
		}


		public string BuildMapWithSolution(IEnumerable<int> solutionPath)
		{
			List<string> mazeRowsWithSolution = myMaze.MazeRows.ToList();

			foreach (var aIt in solutionPath)
			{
				if (aIt != myMaze.EndIndex)
				{
					int posX = aIt % myMaze.DimensionX;
					int posY = aIt / myMaze.DimensionX;

					StringBuilder row = new StringBuilder(mazeRowsWithSolution[posY]);
					row[posX] = PATH_TOKEN;

					mazeRowsWithSolution[posY] = row.ToString();
				}
			}

			return string.Join(Environment.NewLine, mazeRowsWithSolution);
		}
	}
}