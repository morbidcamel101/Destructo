using UnityEngine;
using System.Collections;
using System;
using System.Linq;



[AddComponentMenu("Small World/Character Manager")]
public sealed class CharacterManager : BehaviorBase
{

	void Awake()
	{
		_instance = this;
	}

	public CharacterDefinition[] characters;


	// Singleton
	private static CharacterManager _instance;
	public static CharacterManager Instance
	{
		get {
			CheckValid();
			return _instance;
		}
	}

	private static void CheckValid()
	{
		if (_instance == null)
					throw new InvalidOperationException("Character Manager not initialized!");
	}

	public CharacterDefinition GetCharacter(string id)
	{
		return this.characters.FirstOrDefault(c => c.id == id);
	}


}

