namespace MazeExercise.Domain
{
	public class MazeSolution
	{
		public int Steps { get; set; }
		public string Solution { get; set; }

		public MazeSolution(int steps, string solution)
		{
			Steps = steps;
			Solution = solution;
		}
	}
}