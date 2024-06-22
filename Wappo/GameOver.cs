using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wappo;
public partial class GameOver : Node2D
{
	[Signal]
	public delegate void LevelRetryEventHandler();
	
	private void _on_exit_pressed()
	{
		GetTree().Quit();
	}
	
	private void _on_retry_pressed()
	{
		EmitSignal(SignalName.LevelRetry);
	}
}
