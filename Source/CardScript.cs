using Godot;
using System;

using System.Collections.Generic;

public class CardScript : Node2D {
    public bool drag_flag;
    public bool hover_flag;
    public Vector2 offset;

    [Export] public Vector2 base_scale = new Vector2(.25f, .25f);
    [Export] public Vector2 hover_scale = new Vector2(.3f, .3f);
    [Export] public Vector2 hold_scale = new Vector2(.1f, .1f);
    [Export] public Vector2 place_scale = new Vector2(.15f, .15f);

    private List<Area2D> over_list;
    private Vector2 root_point;
    private RoomScript over_room;

    private Tween tween = new Tween();


    public void mouse_in() {
        hover_flag = true;
        Scale = hover_scale;
    }

    public void mouse_out() {
        hover_flag = false;
        Scale = base_scale;
        GD.Print("mosue out");
    }


    public void drop_in(Area2D a) {
        if (a.GetParent() is RoomScript room && !tween.IsActive()) {
            GD.Print("over room " + room.Name);
            Scale = place_scale;
            // root_point = room.Position;

            over_room = room;
        }
    }

    public void drop_out(Area2D a) {
        if (a.GetParent() == over_room) {
            Scale = hold_scale;
            over_room = null;
        }
    }



    public override void _Ready() {
        AddChild(tween);

        GetNode<Area2D>("hitbox").Connect("mouse_entered", this, nameof(mouse_in));
        GetNode<Area2D>("hitbox").Connect("mouse_exited", this, nameof(mouse_out));

        GetNode<Area2D>("drop_zone").Connect("area_entered", this, nameof(drop_in));
        GetNode<Area2D>("drop_zone").Connect("area_exited", this, nameof(drop_out));
    }

    public override void _Input(InputEvent e) {
        if (e is InputEventMouse em) {
            if (em.IsPressed()) {
                if (over_room != null) {
                    Scale = place_scale;
                    root_point = over_room.Position;
                } else {
                    Scale = hold_scale;
                    offset = Position - em.Position;
                }

                drag_flag = !drag_flag;
            }
        }
    }

    public override void _Process(float delta) {
        if (drag_flag) {
            tween.StopAll();

            Position = GetViewport().GetMousePosition() + offset;
        } else {
            tween.InterpolateProperty(this, "position", Position, root_point, .1f);
            tween.Start();
            // Position = root_point;
        }
    }
}
