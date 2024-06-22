using Godot;
using System;

namespace Wappo;
public partial class LevelCompleted : Node2D
{
	[Signal]
	public delegate void NextLevelEventHandler();
	
	private void _on_next_level_pressed()
	{
		EmitSignal(SignalName.NextLevel);
	}

	private void _on_exit_pressed()
	{
		GetTree().Quit();
	}
}
