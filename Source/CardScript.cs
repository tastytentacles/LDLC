using Godot;
using System;

public class CardScript : TextureRect {
    public bool drag_flag;
    public bool hover_flag;
    public Vector2 offset;

    [Export] public Vector2 base_scale = new Vector2(.25f, .25f);
    [Export] public Vector2 hover_scale = new Vector2(.3f, .3f);
    [Export] public Vector2 place_scale = new Vector2(.1f, .1f);

    public void mouse_in() {
        hover_flag = true;
        RectScale = hover_scale;
    }

    public void mouse_out() {
        hover_flag = false;
        RectScale = base_scale;

    }

    public override void _Ready() {
        Connect("mouse_entered", this, nameof(mouse_in));
        Connect("mouse_exited", this, nameof(mouse_out));
    }

    public override void _Input(InputEvent e) {
        if (e.IsPressed() & drag_flag) {
            GD.Print("trace: " + Name);
            drag_flag = false;
            MouseFilter = MouseFilterEnum.Stop;
            return;
        }
        
        if (e is InputEventMouse em) {
            if (em.IsPressed() & hover_flag) {
                offset = RectPosition - em.Position;
                drag_flag = true;
                MouseFilter = MouseFilterEnum.Ignore;
            }
        }
    }

    public override void _Process(float delta) {
        if (drag_flag) {
            RectPosition = GetViewport().GetMousePosition() + offset;
        }
    }
}
