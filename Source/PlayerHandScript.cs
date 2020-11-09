using Godot;
using System;

using System.Collections.Generic;

public class PlayerHandScript : Node2D {
    [Export] public NodePath card_space_path;
    [Export] public PackedScene card_basic;
    
    public int hand_count;
    public int slot_count = 7;
    public List<CardScript> cards = new List<CardScript>();
    public Vector2 hand_offset = new Vector2(128, 0);
    public Node2D card_space;

    public (bool, Vector2) request_slot(CardScript cs) {
        if (hand_count + 1 > slot_count) {
            return (false, Vector2.Zero);
        }

        ++hand_count;
        cards.Add(cs);
        Vector2 a = hand_offset * (hand_count - 1);
        return (true, -a / 2 + a);
    }

    public void vacate_slot(CardScript cs) {
        --hand_count;
        cards.Remove(cs);
        shift_slots();
    }

    public void shift_slots() {
        Vector2 a;
        for (int c = 0; c < cards.Count; ++c) {
            a = hand_offset * c;
            cards[c].shift_root(-a / 2 + a, this as Node2D);
        }
    }

    public void draw_card() {
        if (hand_count < slot_count) {
            CardScript card = card_basic.Instance() as CardScript;
            card.Position = Position + Vector2.Down * 200;
            card_space.AddChild(card);
            card.add_to(this);

            shift_slots();
        }
    }

    public override void _Ready() {
        card_space = GetNode<Node2D>(card_space_path);
    }

    public override void _Input(InputEvent e) {
        if (e is InputEventKey ek) {
            if (ek.Scancode == (uint) KeyList.Q) {
                draw_card();
            }
        }
    }
}
