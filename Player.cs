using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int speed {get; set;} = 400; // Velocidad en la que se movera el jugador (pixeles/segundo)//

	[Signal]
	public delegate void HitEventHandler();
	func _on_body_entered(body):

	public Vector2 ScreenSize; // Tamanio de la ventana del juego //

    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
		Hide();
    }

    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero; // Vector de movimiento del jugador //

		if (Input.IsActionJustPressed("right"))
		{
			velocity.X += 1;
		}
		if (Input.IsActionJustPressed("left"))
		{
			velocity.X -= 1;
		}
		if (Input.IsActionJustPressed("down"))
		{
			velocity.Y += 1;
		}
		if (Input.IsActionJustPressed("up"))
		{
			velocity.Y -= 1;
		}

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}

		Position += velocity *(float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);

		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = "Walk";
			animatedSprite2D.FlipV = false;
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else if(velocity.Y != 0)
		{
			animatedSprite2D.Animation = "Up";
			animatedSprite2D.FlipV = velocity.Y > 0;
		}
    }


}
