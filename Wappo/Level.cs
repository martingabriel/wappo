using Godot;
using System;

namespace Wappo;
public partial class Level : Node
{
	public Vector2I playerPosition = new Vector2I(0, 0);
	public Vector2I enemyPosition = new Vector2I(0, 0);
	public Vector2I goalPosition = new Vector2I(0, 0);

	public int ActualLevel = 0;

	/* 
		0 = empty	| 
		1 = left	|
		2 = right	| 21 = left_right
		3 = top		| 31 = top_left		| 32 = top_right	| 312 = top_left_right
		4 = bottom	| 41 = bottom_left	| 42 = bottom_right	| 43 = bottom_top	| 412 = bottom_left_right	| 413 = bottom_left_top	| 423 = bottom_right_top
	*/
	public int[,] LevelMap =
	{
		{ 0, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 0 }
	};

	public int[,] Level_1Map =
	{
		{ 0, 4, 2, 1, 0, 0 },
		{ 0, 3, 0, 0, 0, 0 },
		{ 0, 0, 42, 1, 0, 0 },
		{ 0, 2, 31, 2, 21, 1 },
		{ 0, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 0 }
	};

	public int[,] Level_2Map =
	{
		{ 0, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 0 },
		{ 4, 0, 2, 1, 2, 1 },
		{ 3, 4, 0, 4, 0, 0 },
		{ 0, 3, 4, 3, 0, 0 },
		{ 2, 1, 3, 0, 2, 1 }
	};
	public int[,] Level_3Map =
	{
		{ 0, 0, 4, 0, 0, 0 },
		{ 2, 1, 3, 2, 41, 0 },
		{ 0, 0, 0, 0, 32, 1 },
		{ 4, 0, 0, 2, 1, 0 },
		{ 3, 0, 0, 0, 2, 1 },
		{ 0, 0, 0, 0, 0, 0 }
	};
	public int[,] Level_4Map =
	{
		{ 0, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 2, 1, 4 },
		{ 0, 0, 0, 0, 0, 3 },
		{ 0, 0, 0, 0, 0, 4 },
		{ 0, 0, 2, 1, 4, 3 },
		{ 0, 0, 2, 1, 3, 0 }
	};
	public int[,] Level_5Map =
	{
		{ 0, 0, 4, 0, 0, 0 },
		{ 0, 2, 312, 41, 0, 0 },
		{ 0, 0, 0, 3, 0, 4 },
		{ 0, 0, 0, 2, 41, 3 },
		{ 0, 0, 2, 1, 3, 0 },
		{ 0, 0, 0, 0, 0, 0 }
	};
	public int[,] Level_6Map =
	{
		{ 0, 0, 2, 1, 0, 0 },
		{ 0, 0, 0, 0, 0, 0 },
		{ 0, 0, 4, 4, 0, 0 },
		{ 0, 0, 3, 32, 21, 1 },
		{ 2, 1, 2, 1, 2, 1 },
		{ 0, 0, 0, 0, 0, 0 }
	};
	public int[,] Level_7Map =
	{
		{ 0, 0, 0, 2, 1, 0 },
		{ 0, 0, 0, 0, 0, 0 },
		{ 0, 0, 4, 0, 0, 0 },
		{ 0, 2, 31, 0, 0, 0 },
		{ 4, 0, 0, 42, 1, 0 },
		{ 3, 0, 0, 32, 1, 0 }
	};
	public int[,] Level_8Map =
	{
		{ 0, 0, 2, 1, 0, 0 },
		{ 0, 0, 4, 0, 0, 0 },
		{ 0, 0, 423, 1, 0, 0 },
		{ 0, 42, 31, 2, 1, 0 },
		{ 0, 3, 4, 0, 0, 0 },
		{ 0, 0, 3, 0, 0, 0 }
	};
	public int[,] Level_9Map =
	{
		{ 4, 0, 0, 0, 0, 0 },
		{ 3, 0, 2, 1, 0, 0 },
		{ 0, 2, 1, 4, 0, 0 },
		{ 0, 2, 41, 3, 0, 0 },
		{ 4, 0, 3, 0, 2, 1 },
		{ 3, 0, 0, 0, 0, 0 }
	};

	public Vector2I Level_1PlayerPosition = new Vector2I(4, 1);
	public Vector2I Level_1EnemyPosition = new Vector2I(5, 4);
	public Vector2I Level_1GoalPosition = new Vector2I(0, 5);

	public Vector2I Level_2PlayerPosition = new Vector2I(0, 3);
	public Vector2I Level_2EnemyPosition = new Vector2I(1, 4);
	public Vector2I Level_2GoalPosition = new Vector2I(5, 0);

	public Vector2I Level_3PlayerPosition = new Vector2I(4, 4);
	public Vector2I Level_3EnemyPosition = new Vector2I(5, 4);
	public Vector2I Level_3GoalPosition = new Vector2I(0, 0);

	public Vector2I Level_4PlayerPosition = new Vector2I(5, 0);
	public Vector2I Level_4EnemyPosition = new Vector2I(5, 3);
	public Vector2I Level_4GoalPosition = new Vector2I(0, 4);

	public Vector2I Level_5PlayerPosition = new Vector2I(1, 3);
	public Vector2I Level_5EnemyPosition = new Vector2I(1, 0);
	public Vector2I Level_5GoalPosition = new Vector2I(0, 0);

	public Vector2I Level_6PlayerPosition = new Vector2I(5, 5);
	public Vector2I Level_6EnemyPosition = new Vector2I(5, 1);
	public Vector2I Level_6GoalPosition = new Vector2I(0, 0);

	public Vector2I Level_7PlayerPosition = new Vector2I(4, 0);
	public Vector2I Level_7EnemyPosition = new Vector2I(4, 5);
	public Vector2I Level_7GoalPosition = new Vector2I(0, 2);

	public Vector2I Level_8PlayerPosition = new Vector2I(2, 5);
	public Vector2I Level_8EnemyPosition = new Vector2I(1, 1);
	public Vector2I Level_8GoalPosition = new Vector2I(0, 0);

	public Vector2I Level_9PlayerPosition = new Vector2I(5, 0);
	public Vector2I Level_9EnemyPosition = new Vector2I(3, 4);
	public Vector2I Level_9GoalPosition = new Vector2I(4, 5);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ActualLevel = 1;
		LoadLevel(ActualLevel);
	}

	public void LoadLevel(int levelNumber)
	{
		switch (levelNumber)
		{
			case 1:
				LevelMap = Level_1Map;
				playerPosition = Level_1PlayerPosition;
				enemyPosition = Level_1EnemyPosition;
				goalPosition = Level_1GoalPosition;
				ActualLevel = 1;
				break;
			case 2:
				LevelMap = Level_2Map;
				playerPosition = Level_2PlayerPosition;
				enemyPosition = Level_2EnemyPosition;
				goalPosition = Level_2GoalPosition;
				ActualLevel = 2;
				break;
			case 3:
				LevelMap = Level_3Map;
				playerPosition = Level_3PlayerPosition;
				enemyPosition = Level_3EnemyPosition;
				goalPosition = Level_3GoalPosition;
				ActualLevel = 3;
				break;
			case 4:
				LevelMap = Level_4Map;
				playerPosition = Level_4PlayerPosition;
				enemyPosition = Level_4EnemyPosition;
				goalPosition = Level_4GoalPosition;
				ActualLevel = 4;
				break;
			case 5:
				LevelMap = Level_5Map;
				playerPosition = Level_5PlayerPosition;
				enemyPosition = Level_5EnemyPosition;
				goalPosition = Level_5GoalPosition;
				ActualLevel = 5;
				break;
			case 6:
				LevelMap = Level_6Map;
				playerPosition = Level_6PlayerPosition;
				enemyPosition = Level_6EnemyPosition;
				goalPosition = Level_6GoalPosition;
				ActualLevel = 6;
				break;
			case 7:
				LevelMap = Level_7Map;
				playerPosition = Level_7PlayerPosition;
				enemyPosition = Level_7EnemyPosition;
				goalPosition = Level_7GoalPosition;
				ActualLevel = 7;
				break;
			case 8:
				LevelMap = Level_8Map;
				playerPosition = Level_8PlayerPosition;
				enemyPosition = Level_8EnemyPosition;
				goalPosition = Level_8GoalPosition;
				ActualLevel = 8;
				break;
			case 9:
				LevelMap = Level_9Map;
				playerPosition = Level_9PlayerPosition;
				enemyPosition = Level_9EnemyPosition;
				goalPosition = Level_9GoalPosition;
				ActualLevel = 9;
				break;
			default:
				LevelMap = Level_1Map;
				playerPosition = Level_1PlayerPosition;
				enemyPosition = Level_1EnemyPosition;
				goalPosition = Level_1GoalPosition;
				ActualLevel = 1;
				break;
		}
	}

	public bool IsInCollision(Vector2I position, MovementDirection direction)
	{
		// left collision
		if (LevelMap[position.Y, position.X] == 1 && direction == MovementDirection.Left ||
			LevelMap[position.Y, position.X] == 31 && direction == MovementDirection.Left ||
			LevelMap[position.Y, position.X] == 41 && direction == MovementDirection.Left ||
			LevelMap[position.Y, position.X] == 312 && direction == MovementDirection.Left ||
			LevelMap[position.Y, position.X] == 412 && direction == MovementDirection.Left ||
			LevelMap[position.Y, position.X] == 413 && direction == MovementDirection.Left)
		{
			return true;
		}
		// right collision
		if (LevelMap[position.Y, position.X] == 2 && direction == MovementDirection.Right ||
			LevelMap[position.Y, position.X] == 42 && direction == MovementDirection.Right ||
			LevelMap[position.Y, position.X] == 412 && direction == MovementDirection.Right ||
			LevelMap[position.Y, position.X] == 423 && direction == MovementDirection.Right)
		{
			return true;
		}
		// up collision
		if (LevelMap[position.Y, position.X] == 3 && direction == MovementDirection.Up ||
			LevelMap[position.Y, position.X] == 31 && direction == MovementDirection.Up ||
			LevelMap[position.Y, position.X] == 32 && direction == MovementDirection.Up ||
			LevelMap[position.Y, position.X] == 312 && direction == MovementDirection.Up ||
			LevelMap[position.Y, position.X] == 423 && direction == MovementDirection.Up)
		{
			return true;
		}
		// down collision
		if (LevelMap[position.Y, position.X] == 4 && direction == MovementDirection.Down ||
			LevelMap[position.Y, position.X] == 41 && direction == MovementDirection.Down ||
			LevelMap[position.Y, position.X] == 42 && direction == MovementDirection.Down ||
			LevelMap[position.Y, position.X] == 43 && direction == MovementDirection.Down ||
			LevelMap[position.Y, position.X] == 412 && direction == MovementDirection.Down ||
			LevelMap[position.Y, position.X] == 413 && direction == MovementDirection.Down ||
			LevelMap[position.Y, position.X] == 423 && direction == MovementDirection.Down)
		{
			return true;
		}

		return false;
	}
}
