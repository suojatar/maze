namespace MazeExercise.Domain
{
	public class MazePostData
	{
		public string Map { get; set; }
		public int SearchType { get; set; } = 0; //TODO: implement Breadth-first search to find the shortest solutution
	}
}
