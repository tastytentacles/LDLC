using Godot;
using System;

public class GenericCard
{
	// The unique name of the card. Should be lowercase and only "a-z/0-9/-/_".
	public string name { get; set; } ;
	
	// Name used to display it. 
	public string display_name { get; set; } ;
	
	// A small description of the card.
	public string description { get; set; } ;
	
	// The card type. Should be lowercase and only "a-z/0-9/-/_".
	public string type { get; set; } = "generic";
}
