using Godot;
using System;

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
	}

	public void NewGame()
	{
		_score = 0;

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");

		GetNode<Timer>("StartTimer").Start();
	}

	private void OnScoreTimerTimeout()
	{
		_score++;
	}

	private void OnStartTimerTimeOut()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	private void OnMobTimerTimeOut()
	{
		// Crea una nueva instancia del Mob scene //
		Mob mob = MobScene.Instantiate<Mob>();

		// Elije una ubicacion aleatoria Path2D //
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath?MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		// Establece la direccion del enemigo perpendicular a la direccion del camino //
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		// Coloca la posicion del enemigo en una ubicacion aleatoria //
		mob.Position = mobSpawnLocation.Position;
	}
}
