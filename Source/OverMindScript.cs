using Godot;
using Godot.Collections;
using System;

public class OverMindScript : Node2D {
    public int cash;

    private Camera2D cam;
    private bool drag_flag;
    private Vector2 drag_start;
    private Vector2 drag_origin;

    public override void _Ready() {
        cam = GetNode<Camera2D>("Camera2D");
    }

    public override void _Input(InputEvent e) {
        if (e is InputEventMouse em) {
            if (em.IsAction("right_click")) {
                drag_start = em.Position;
                drag_origin = cam.Position;
                drag_flag = !drag_flag;
            }
        }
    }

    public override void _Process(float delta) {
        if (drag_flag) {
            cam.Position = drag_origin
                + (drag_start - GetViewport().GetMousePosition());
        }
    }
}
