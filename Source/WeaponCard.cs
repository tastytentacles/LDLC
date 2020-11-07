using System;
using Godot;

public class WeaponCard : GenericCard
{
	// The weapon sub-type. Should be lowercase and only "a-z/0-9/-/_".
	public string subtype { get; set; } = "generic";

	// The attack given by the weapon
	public int attack;
}