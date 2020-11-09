using Godot;
using Godot.Collections;
using System;

using System.Collections.Generic;

public class RoomScript : Node2D {
    [Export] public string room_type = "basic";
    [Export] public bool room_abandoned = false;
    [Export] public NodePath dependent_room_path;
    

    public Label room_name;
    public ColorRect debug_indicator;
    public RoomScript dependent_room;
    public ImmediateGeometry line;

    public int slot_count;
    public int staff_slot_count;
    public List<CardScript> staff_cards = new List<CardScript>();
    public Vector2 staff_slot_root = Vector2.Down * 48f;
    public Vector2 staff_slot_offset = Vector2.Right * 32f;

    public void LoadRoomData() {
        // open data file
        File data_raw = new File();
        data_raw.Open("res://Data/RoomData.json", File.ModeFlags.Read);

        // parse data file
        Dictionary data = JSON.Parse(data_raw.GetAsText()).Result as Dictionary;
        if (data.Contains(room_type)) {
            // get room specific data set
            Dictionary room_data = data[room_type] as Dictionary;

            // load room specific data
            room_name.Text = (string) room_data["name"];
            slot_count = (int)(float) room_data["slots"];
        } else {
            GD.PrintErr("room type not found!");
        }
    }

    public (bool, Vector2) request_staff_slot(CardScript cs) {
        if (staff_slot_count + 1 > slot_count) {
            return (false, Vector2.Zero);
        }

        ++staff_slot_count;
        staff_cards.Add(cs);
        return (true, staff_slot_root + staff_slot_offset * (staff_slot_count - 1));
    }

    public void vacate_staff_slot(CardScript cs) {
        --staff_slot_count;
        staff_cards.Remove(cs);
    }

    public override void _Ready() {
        room_name = GetNode<Label>("Graphics/room_name");

        debug_indicator = GetNode<ColorRect>("Graphics/debug_square");

        LoadRoomData();

        // populate dependent room
        if (dependent_room_path != null) {
            dependent_room = GetNode<RoomScript>(dependent_room_path);

            if (dependent_room.room_abandoned) {
                room_abandoned = true;
            }
        }

        if (room_abandoned) {
            debug_indicator.Color = Colors.DimGray;
        }
    }

    public override void _Draw() {
        // draw line to dependent rooms
        if (dependent_room != null) {
            Vector2 path = dependent_room.Position - Position;
            Vector2 path_normal = path.Normalized();
            float path_distance = path.Length();

            DrawLine(
                path_normal * (path_distance / 5),
                path * .80f,
                Colors.Red,
                1);
        }

        for (int n = 0; n < slot_count; ++n) {
            DrawCircle(staff_slot_root + staff_slot_offset * n,
                4f,
                Colors.Green);
        }
    }
}
