using MazeExercise.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

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

		public MazeSolution Solve()
		{
			int dimX = myMaze.DimensionX, dimY = myMaze.DimensionY;

			var visitedPositions = new HashSet<int>(); //stores indices of cells that have been visited
			var solutionPath = new List<int>(); //stores indices of cells that are part of the solution path

			bool solutionResult = IsEndIndexReachedRecursive(myMaze.StartIndex, solutionPath, visitedPositions);

			if (!solutionResult)
			{
				throw new InvalidOperationException("This maze has no solution!");
			}

			solutionPath.Reverse(); //the Solution path cells are stored in backward order, we need to reverse it

			//build map with solution path
			var strBldr = new StringBuilder((dimX + 2) * dimY); //allocate space for \r\n on each line (hence 2 more characters)

			using (var strRdr = new StringReader(myMaze.MazeMap))
			{
				string textLine;
				int rowIndex = 0;
				int cellIndex = 0;

				while ((textLine = strRdr.ReadLine()) != null)
				{
					for (int i = 0; i < textLine.Length; ++i)
					{
						cellIndex = rowIndex * dimX + i;
						strBldr.Append((solutionPath.Contains(cellIndex) && textLine[i] != END_TOKEN) ? PATH_TOKEN : textLine[i]);
					}

					if (cellIndex < dimX * dimY - 1)
					{
						strBldr.AppendLine();
						++rowIndex;
					}
				}
			}

			MazeSolution solution = new MazeSolution(solutionPath.Count, strBldr.ToString());
			return solution;
		}


		/// <summary>
		/// Depth-first search.
		/// This algotithm is very fast and efficient (a 1,000,000 x 1,000,000 matrix would probably cause stack overflow but for reasonable sizes it's quite good).
		/// It NOT meant to return the shortest solution, instead, the method will return the FIRST valid solution (there may be others that are shorter).
		/// </summary>
		private bool IsEndIndexReachedRecursive(int cur, List<int> solution, HashSet<int> visited)
		{
			if (myMaze.GetCellType(cur) == CellType.End)
				return true;

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
	}
}