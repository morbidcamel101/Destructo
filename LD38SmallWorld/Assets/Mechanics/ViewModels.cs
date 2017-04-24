using System;
using UnityEngine;

public enum Difficulty { Easy, Normal, Extreme }

[Serializable]
public sealed class CharacterDefinition
{
	public string id;
	public string title;
	public float strengthMultiplier = 1f;
	public Prototype character;
	public int points = 100;

}

