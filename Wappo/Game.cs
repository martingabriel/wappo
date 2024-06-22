using Godot;
using System;

namespace Wappo;

public enum MovementDirection
{
	Up,
	Down, 
	Left,
	Right,
	None
}

public partial class Game : Node2D
{
	private const int GridSize = 6;
	private const int TileSize = 64;

	private Node2D playerSprite;
	private Node2D enemySprite;
	private Node2D goalSprite;
	private Vector2 gridOffset;
	
	private Level level;
	
	private Node2D gameOverNode;
	private Node2D levelCompletedNode;
	private Node2D menu;
	
	private Label levelLabel;
	
	private AudioStreamPlayer clapSound;
	private AudioStreamPlayer swordSound;
	
	private int enemyMoves = 0;

	public override void _Ready()
	{
		SetProcess(true);
		
		// Calculate offset to center the grid on the screen
		var screenSize = GetViewportRect().Size;
		var gridSizeInPixels = new Vector2(GridSize * TileSize, GridSize * TileSize);
		gridOffset = (screenSize - gridSizeInPixels) / 2;
		
		level = GetNode<Level>("Level");		
		gameOverNode = GetNode<Node2D>("GameOver");
		levelCompletedNode = GetNode<Node2D>("LevelCompleted");
		menu = GetNode<Node2D>("Menu");
		
		levelLabel = GetNode<Label>("LevelLabel");
		
		clapSound = GetNode<AudioStreamPlayer>("ClapSound");
		swordSound = GetNode<AudioStreamPlayer>("SwordSound");
		
		DrawGridBorders();
		DrawLevelMap();

		// Create and position the player, enemy, and goal sprites
		playerSprite = CreateSprite("res://img/player.png", level.playerPosition);
		playerSprite.TopLevel = true;
		enemySprite = CreateSprite("res://img/enemy.png", level.enemyPosition);
		enemySprite.TopLevel = true;
		goalSprite = CreateSprite("res://img/goal.png", level.goalPosition);
		goalSprite.TopLevel = true;
	}
	
	private void _on_game_over_level_retry()
	{
		level.LoadLevel(level.ActualLevel);
		gameOverNode.Hide();
	}
	
	private void _on_menu_new_game()
	{
		level.LoadLevel(1);
		
		DrawLevelMap();
		UpdatePositions();
		
		menu.Hide();
	}
	
	private void _on_level_completed_next_level()
	{
		int nextLevel = level.ActualLevel + 1;
		level.LoadLevel(nextLevel);
		
		DrawLevelMap();
		UpdatePositions();
		
		levelLabel.Text = "Level: " + nextLevel;
		levelCompletedNode.Hide();
	}
	
	public override void _Process(double delta)
	{
		MovementDirection direction = MovementDirection.None;
			
		if (Input.IsActionJustPressed("ui_up") && level.playerPosition.Y > 0)
			direction = MovementDirection.Up;
		else if (Input.IsActionJustPressed("ui_down") && level.playerPosition.Y < GridSize - 1)
			direction = MovementDirection.Down;
		else if (Input.IsActionJustPressed("ui_left") && level.playerPosition.X > 0)
			direction = MovementDirection.Left;
		else if (Input.IsActionJustPressed("ui_right") && level.playerPosition.X < GridSize - 1)
			direction = MovementDirection.Right;
			
		if (Input.IsActionJustPressed("ui_cancel"))
			menu.Visible = !menu.Visible;

		CheckPlayerCollision(direction);
		CheckGameOver();
		UpdatePositions();
		UpdateEnemyPosition();
	}
	
	private void CheckPlayerCollision(MovementDirection direction)
	{
		if (!level.IsInCollision(level.playerPosition, direction))	
		{
			if (direction == MovementDirection.Up)
			{
				level.playerPosition.Y--;
				enemyMoves = 2;
			}
			else if (direction == MovementDirection.Down)
			{
				level.playerPosition.Y++;
				enemyMoves = 2;
			}
			else if (direction == MovementDirection.Left)
			{
				level.playerPosition.X--;
				enemyMoves = 2;
			}
			else if (direction == MovementDirection.Right)
			{
				level.playerPosition.X++;
				enemyMoves = 2;
			}
		}
	}

	private Node2D CreateSprite(string texturePath, Vector2I position)
	{
		var sprite = new Node2D();
		var texture = GD.Load<Texture2D>(texturePath);
		var spriteTexture = new Sprite2D();
		spriteTexture.Texture = texture;
		spriteTexture.Position = position * TileSize + gridOffset;
		sprite.AddChild(spriteTexture);
		AddChild(sprite);
		return sprite;
	}

	private void UpdatePositions()
	{
		((Sprite2D)playerSprite.GetChild(0)).Position = (level.playerPosition * TileSize + gridOffset) + new Vector2(TileSize / 2, TileSize / 2);
		((Sprite2D)enemySprite.GetChild(0)).Position = (level.enemyPosition * TileSize + gridOffset) + new Vector2(TileSize / 2, TileSize / 2);
		((Sprite2D)goalSprite.GetChild(0)).Position = (level.goalPosition * TileSize + gridOffset) + new Vector2(TileSize / 2, TileSize / 2);
	}

	private void UpdateEnemyPosition()
	{
		Vector2I direction = level.playerPosition - level.enemyPosition;
		
		if (enemyMoves > 0)
		{
			// up
			if (Math.Sign(direction.Y) < 0)
			{
				//GD.Print("Math.Sign(direction.Y): " + Math.Sign(direction.Y));
				if (!level.IsInCollision(level.enemyPosition, MovementDirection.Up))
				{
					level.enemyPosition.Y += Math.Sign(direction.Y);
					enemyMoves--;
					return;
				}
			}
			
			// right
			if (Math.Sign(direction.X) > 0)
			{
				//GD.Print("Math.Sign(direction.X): " + Math.Sign(direction.X));
				if (!level.IsInCollision(level.enemyPosition, MovementDirection.Right))
				{
					level.enemyPosition.X += Math.Sign(direction.X);
					enemyMoves--;
					return;
				}
			}
			
			// down
			if (Math.Sign(direction.Y) > 0)
			{
				//GD.Print("Math.Sign(direction.Y): " + Math.Sign(direction.Y));
				if (!level.IsInCollision(level.enemyPosition, MovementDirection.Down))
				{
					level.enemyPosition.Y += Math.Sign(direction.Y);
					enemyMoves--;
					return;
				}
			}
			// left
			if (Math.Sign(direction.X) < 0)
			{
				//GD.Print("Math.Sign(direction.X): " + Math.Sign(direction.X));
				if (!level.IsInCollision(level.enemyPosition, MovementDirection.Left))
				{
					level.enemyPosition.X += Math.Sign(direction.X);
					enemyMoves--;
					return;
				}
			}
		}
	}

	private void CheckGameOver()
	{
		if (level.playerPosition == level.enemyPosition)
		{
			level.goalPosition = new Vector2I(0, 0);
			level.enemyPosition = new Vector2I(1, 0);
			level.playerPosition = new Vector2I(2, 0);
			swordSound.Play();
			gameOverNode.Show();
		}
		else if (level.playerPosition == level.goalPosition)
		{
			level.goalPosition = new Vector2I(0, 0);
			level.enemyPosition = new Vector2I(1, 0);
			level.playerPosition = new Vector2I(2, 0);
			clapSound.Play();
			enemyMoves = 0;
			levelCompletedNode.Show();
		}
	}
	
	private void DrawGridBorders()
	{
		for (int x = 0; x < GridSize; x++)
		{
			for (int y = 0; y < GridSize; y++)
			{
				var line = new Line2D();
				line.Width = 2;
				line.DefaultColor = new Color(255, 255, 255);
				var topLeft = new Vector2(x * TileSize, y * TileSize) + gridOffset;
				var topRight = new Vector2((x + 1) * TileSize, y * TileSize) + gridOffset;
				var bottomRight = new Vector2((x + 1) * TileSize, (y + 1) * TileSize) + gridOffset;
				var bottomLeft = new Vector2(x * TileSize, (y + 1) * TileSize) + gridOffset;

				line.AddPoint(topLeft);
				line.AddPoint(topRight);
				line.AddPoint(bottomRight);
				line.AddPoint(bottomLeft);
				line.AddPoint(topLeft); // Close the loop

				AddChild(line);
			}
		}
	}

	private void DrawLevelMap()
	{
		/* 
			0 = empty	| 
			1 = left	|
			2 = right	| 21 = left_right
			3 = top		| 31 = top_left		| 32 = top_right	| 312 = top_left_right
			4 = bottom	| 41 = bottom_left	| 42 = bottom_right	| 43 = bottom_top	| 412 = bottom_left_right	| 413 = bottom_left_top	| 423 = bottom_right_top
		*/

		int index = 0;
		for (int x = 0; x < GridSize; x++)
		{
			for (int y = 0; y < GridSize; y++)
			{
				string img_path = "res://img/tile_undefined.png";
				
				switch (level.LevelMap[x,y])
				{
					case 0:
						img_path = "res://img/tile.png";
						break;
					case 1:
						img_path = "res://img/barrier_left.png";
						break;
					case 2:
						img_path = "res://img/barrier_right.png";
						break;
					case 3:
						img_path = "res://img/barrier_top.png";
						break;
					case 4:
						img_path = "res://img/barrier_bottom.png";
						break;
					case 21:
						img_path = "res://img/barrier_left_right.png";
						break;
					case 31:
						img_path = "res://img/barrier_left_top.png";
						break;
					case 32:
						img_path = "res://img/barrier_right_top.png";
						break;
					case 41:
						img_path = "res://img/barrier_left_bottom.png";
						break;
					case 42:
						img_path = "res://img/barrier_right_bottom.png";
						break;
					case 312:
						img_path = "res://img/barrier_left_right_top.png";
						break;
					case 412:
						img_path = "res://img/barrier_left_right_bottom.png";
						break;
					case 423:
						img_path = "res://img/barrier_right_bottom_top.png";
						break;
					default:
						img_path = "res://img/tile_undefined.png";
						break;
				}
				
				Vector2I tilePosition = new Vector2I(y, x);
				Node2D tile = new Node2D();
				tile.Name = $"Tile_{y}_{x}";
				//tile.ShowBehindParent = true;
				var tileSprite = CreateSprite(img_path, tilePosition);
				((Sprite2D)tileSprite.GetChild(0)).Position = (tilePosition * TileSize + gridOffset) + new Vector2(TileSize / 2, TileSize / 2);
				AddChild(tileSprite);
				
				index++;
			}
		}
	}
}
