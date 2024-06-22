using Godot;
using System;

namespace Wappo;
public partial class Menu : Node2D
{
	[Signal]
	public delegate void NewGameEventHandler();
	
	private void _on_new_game_pressed()
	{
		EmitSignal(SignalName.NewGame);
	}

	private void _on_exit_pressed()
	{
		GetTree().Quit();
	}
}

