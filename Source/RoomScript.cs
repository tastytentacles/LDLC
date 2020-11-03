using Godot;
using Godot.Collections;
using System;

public class RoomScript : Node2D {
    [Export] public string room_type = "basic";
    [Export] public NodePath dependent_room_path;
    

    public Label room_name;
    public RoomScript dependent_room;
    public ImmediateGeometry line;

    public int slot_count;

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

    public override void _Ready() {
        room_name = GetNode<Label>("Graphics/room_name");

        LoadRoomData();

        if (dependent_room_path != null) {
            dependent_room = GetNode<RoomScript>(dependent_room_path);
        }
    }

    public override void _Draw() {
        // draw line to dependent rooms
        if (dependent_room != null) {
            Vector2 path = dependent_room.Position - Position;
            Vector2 path_normal = path.Normalized();
            float path_distance = path.Length();

            DrawLine(path_normal * (path_distance / 5), path * .80f, new Color(1, 0, 0), 1);
        }
    }
}
