using Godot;
using System;

using System.Collections.Generic;

public class CardScript : Node2D {
    public bool drag_flag;
    public bool hover_flag;
    public bool place_flag;

    [Export] public Vector2 base_scale = new Vector2(.25f, .25f);
    [Export] public Vector2 hover_scale = new Vector2(.3f, .3f);
    [Export] public Vector2 hold_scale = new Vector2(.1f, .1f);
    [Export] public Vector2 place_scale = new Vector2(.05f, .05f);

    private Vector2 offset;
    private List<Area2D> over_list;
    private Vector2 root_point;
    private RoomScript over_room;
    private RoomScript last_room;

    private PlayerHandScript over_hand;
    private PlayerHandScript last_hand;

    private Tween tween = new Tween();
    private Area2D hitbox;

    public void mouse_in() {
        hover_flag = true;
        
        Godot.Collections.Array overlap = hitbox.GetOverlappingAreas();
        for (int i = 0; i < overlap.Count; ++i) {
            Area2D cache = (Area2D) overlap[i];            
            if (cache.IsInGroup("card_box")) {
                cache.GetParent<CardScript>().mouse_out();
            }
        }
    }

    public void mouse_out() {
        hover_flag = false;
    }


    public void drop_in(Area2D a) {
        if (a.GetParent() is RoomScript room && !tween.IsActive()) {
            // GD.Print("over room " + room.Name);

            over_room = room;
        }

        if (a.GetParent() is PlayerHandScript hand) {
            over_hand = hand;
        }
    }

    public void drop_out(Area2D a) {
        if (a.GetParent() == over_room) {
            over_room = null;
        }

        if (a.GetParent() is PlayerHandScript hand) {
            over_hand = null;
        }
    }



    public void add_to(Node2D target) {
        if (target is RoomScript target_room) {
            (bool answer, Vector2 at) req = target_room.request_staff_slot(this);
                    
            if (req.answer) {
                if (last_room != null) {
                    last_room.vacate_staff_slot(this);
                    last_room = null;
                }

                if (last_hand != null) {
                    last_hand.vacate_slot(this);
                    last_hand = null;
                }

                place_flag = true;
                root_point = target_room.Position + req.at;
                last_room = target_room;
            }

            return;
        }

        if (target is PlayerHandScript target_hand) {
            (bool answer, Vector2 at) req = target_hand.request_slot(this);
                    
            if (req.answer) {
                if (last_room != null) {
                    last_room.vacate_staff_slot(this);
                    last_room = null;
                }

                if (last_hand != null) {
                    last_hand.vacate_slot(this);
                    last_hand = null;
                }

                // place_flag = true;
                root_point = target_hand.Position + req.at;
                last_hand = target_hand;
            }
        }
    }

    public void shift_root(Vector2 to, Node2D by) {
        root_point = by.Position + to;
    }

    public override void _Ready() {
        AddChild(tween);
        hitbox = GetNode<Area2D>("hitbox");

        hitbox.Connect("mouse_entered", this, nameof(mouse_in));
        hitbox.Connect("mouse_exited", this, nameof(mouse_out));

        GetNode<Area2D>("drop_zone").Connect("area_entered", this, nameof(drop_in));
        GetNode<Area2D>("drop_zone").Connect("area_exited", this, nameof(drop_out));

        root_point = Position;
    }

    public override void _Input(InputEvent e) {
        if (e is InputEventMouse em) {
            // if (em.IsAction("left_click") & (hover_flag | drag_flag)) {
            //     if (over_room != null & em.GetActionStrength("left_click") < 1) {
            //         add_to(over_room);
            //     } else {
            //         offset = Position - em.Position;
            //     }

            //     drag_flag = !drag_flag;
            // }

            if (em.IsAction("left_click")) {
                if (em.IsPressed()) {
                    if (hover_flag) {
                        offset = Position - em.Position;
                        drag_flag = true;
                    }
                } else {
                    if (over_room != null) {
                        add_to(over_room);
                    }
                    
                    if (over_hand != null) {
                        add_to(over_hand);
                    }

                    drag_flag = false;
                }
            }
        }
    }

    public override void _Process(float delta) {
        Scale = base_scale;
        ZIndex = 0;
        
        if (place_flag) {
            Scale = place_scale;
        }

        if (hover_flag) {
            Scale = hover_scale;
            ZIndex = 10;
        }

        if (drag_flag) {
            tween.StopAll();
            Position = GetViewport().GetMousePosition() + offset;

            Scale = hold_scale;
        } else {
            tween.InterpolateProperty(this, "position", Position, root_point, .1f);
            tween.Start();
        }
    }
}
