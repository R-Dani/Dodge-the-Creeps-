using Godot;
using System;
using System.ComponentModel;

public partial class Main : Node
{
	// No olvides reconstruir el proyecto para que el editor sepa de la nueva variable de exportacion //
	[Export]
	public PackedScene MobScene {get; set;}

	private int _score;

		public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<Hud>("HUD").ShowGameOver();
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();
	}

	public void NewGame()
	{
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		_score = 0;

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");

		GetNode<Timer>("StartTimer").Start();

		var hud = GetNode<Hud>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
		GetNode<AudioStreamPlayer>("Music").Play();
	}

	private void OnMobTimerTimeOut()
	{
		// Crea una nueva instancia del Mob scene //
		Mob mob = MobScene.Instantiate<Mob>();

		// Elije una ubicacion aleatoria Path2D //
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		// Establece la direccion del enemigo perpendicular a la direccion del camino //
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		// Coloca la posicion del enemigo en una ubicacion aleatoria //
		mob.Position = mobSpawnLocation.Position;

		// Aniade algo de aleatoridad a la direccion //
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

		// Elige la velocidad //
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		// Haz aparecer el mob aniadiendolo a la escena principal //
		AddChild(mob);
	}

	private void OnScoreTimerTimeout()
	{
		_score++;

		GetNode<Hud>("HUD").UpdateScore(_score);
	}

	private void OnStartTimerTimeOut()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

}
