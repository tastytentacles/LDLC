using System;
using Godot;

public class GenericStaffType
{
	// The unique name of the card. Should be lowercase and only "a-z/0-9/-/_".
	public string name;
	
	// Name used to display it. 
	public string display_name;
	
	// A small description of the card.
	public string description;
	
	// An array of GenericSkills objects that are possessed by the Staff type
	public GenericSkill[] skills;

	// An array of GenericSkills objects from which one-or more-skills may be added to the skills object of the instance
	public GenericSkill[] bonusSkills;

	// Values used during combat. This value may be increased, during calculation, with skills
	public int base_health;
	public int base_attack;
	public int base_defense;

	// An array containg the cards equipped to the staff card
	public List<GenericCard> equipped;
}