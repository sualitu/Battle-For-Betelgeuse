using UnityEngine;
using System.Collections;

public class MenuOption 
{
	public MenuOption Parent {get;set;}
	public MenuOption[] Children {get;set;}
	
	public string Title;
	
	public bool isImplemented = false;
	
	public MenuOption(string title, MenuOption parent, MenuOption[] children) {
		this.Title = title;
		this.Parent = parent;
		this.Children = children;
	}
	
	
}

