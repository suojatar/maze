using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MazeExercise.Application
{
	/// <summary>
	/// This class mainly operates with the index of the Cell in the one-dimensional array of cells, not the row/column positioning of the given cell.
	/// By knowing the cell's index and the dimensions of the board, we can easily calculate the X/Y positioning of the cell.
	/// </summary>
	public class Maze
	{
		public enum CellType : uint
		{
			Blocked,
			Open,
			Start,
			End
		}

		//conventional
		public const char START_TOKEN = 'A';
		public const char END_TOKEN = 'B';
		public const char OPEN_TOKEN = '.';
		public const char BLOCKED_TOKEN = '#';
		public const char PATH_TOKEN = '@';

		//will be set in constructor for immutability
		private readonly int myDimensionX, myDimensionY;
		private readonly CellType[] myCells;
		private readonly int myStartIndex, myEndIndex;
		private readonly string[] myMazeRows;

		public int DimensionX { get { return myDimensionX; } }
		public int DimensionY { get { return myDimensionY; } }

		public int StartIndex { get { return myStartIndex; } }
		public int EndIndex { get { return myEndIndex; } }

		public string[] MazeRows { get { return myMazeRows; } }

		//private Maze(int dimensionX, int dimensionY, CellType[] cells, int startIndex, int endIndex, string mazeMap)
		private Maze(int dimensionX, int dimensionY, CellType[] cells, int startIndex, int endIndex, string[] mazeRows)
		{
			myDimensionX = dimensionX;
			myDimensionY = dimensionY;
			myCells = cells;
			myStartIndex = startIndex;
			myEndIndex = endIndex;
			myMazeRows = mazeRows;
		}


		public static Maze Parse(string mazeMap)
		{
			int dimensionX = 0, dimensionY = 0;
			int startIndex = -1, endIndex = -1;

			var cellList = new List<CellType>();
			var rowList = new List<string>(); //stores a collection of maze rows - we'll need this when we generate the solution map

			Action<string> iProcessLine = line =>
			{
				rowList.Add(line);

				foreach (char aIt in line)
				{
					switch (aIt)
					{
						case BLOCKED_TOKEN:
							cellList.Add(CellType.Blocked);
							break;

						case START_TOKEN:
							startIndex = cellList.Count;
							cellList.Add(CellType.Start);
							break;

						case END_TOKEN:
							endIndex = cellList.Count;
							cellList.Add(CellType.End);
							break;

						case OPEN_TOKEN:
							cellList.Add(CellType.Open);
							break;

						default:
							throw new ArgumentException("Invalid character found!", nameof(mazeMap));
					}
				};

				++dimensionY; //move to the next row
			};

			using (var strRdr = new StringReader(mazeMap)) //need to read line-by-line so we can determine the X-dimension
			{
				var textLine = strRdr.ReadLine(); //read the first line and determine X-Dimension
				dimensionX = textLine.Length;

				iProcessLine(textLine);

				while ((textLine = strRdr.ReadLine()) != null) //parse the rest of the lines
					iProcessLine(textLine); //calling this Action to preserve all the in-memory values

				//now we've got all the pieces needed to create a Maze object
				//return new Maze(dimensionX, dimensionY, cellList.ToArray(), startIndex, endIndex, mazeMap);
				return new Maze(dimensionX, dimensionY, cellList.ToArray(), startIndex, endIndex, rowList.ToArray());
			}
		}

		/// <summary>
		/// We are creating a collection of neighboring cells. There cannot be more than 4 (left-right-up-down).
		/// Since the lookup order is hard-set, the array will always contain the same set of elements (this is by design but can be changed).
		/// </summary>
		public IEnumerable<int> Neighbors(int current)
		{
			if ((current % myDimensionX) > 0) //look left first
			{
				var newCellIndex = current - 1;
				var newCellType = GetCellType(newCellIndex);

				if (newCellType == CellType.Open || newCellType == CellType.End)
					yield return newCellIndex;
			}

			if ((current % myDimensionX) < myDimensionX - 1) //then right
			{
				var newCellIndex = current + 1;
				var newCellType = GetCellType(newCellIndex);

				if (newCellType == CellType.Open || newCellType == CellType.End)
					yield return newCellIndex;
			}

			if ((current / myDimensionX) > 0) //then up
			{
				var newCellIndex = current - myDimensionX;
				var newCellType = GetCellType(newCellIndex);

				if (newCellType == CellType.Open || newCellType == CellType.End)
					yield return newCellIndex;
			}

			if ((current / myDimensionX) < myDimensionY - 1) //and, finally, down
			{
				var newCellIndex = current + myDimensionX;
				var newCellType = GetCellType(newCellIndex);

				if (newCellType == CellType.Open || newCellType == CellType.End)
					yield return newCellIndex;
			}
		}

		public CellType GetCellType(int index)
		{
			return myCells[index];
		}
	}
}