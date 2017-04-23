using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using Random = UnityEngine.Random;



[AddComponentMenu("Small World/Character Manager")]
public sealed class CharacterManager : BehaviorBase
{
	public enum State { Monitoring, Spawn };
	public State state;
	public float statusInterval = 1f;
	public CharacterDefinition[] characters;

	// Controls difficulty
	public float spawnInterval = 10f;
	public int population = 10;
	public int activePopulation = 3;
	public float strengthMultiplier = 1f;



	internal SpawnPoint[] spawnPoints;
	internal List<GameObject> spawned = new List<GameObject>();
	private float resumeTime;

	void Awake()
	{
		_instance = this;
		spawnPoints = GetComponentsInChildren<SpawnPoint>();
		if (spawnPoints.Length == 0)
		{
			Debug.LogError("There are now spawn points defined!");
			enabled = false;
		}
		foreach(var character in characters)
		{
			this.Assert(character.character != null, "Character prototype not selected!");
		}
	}

	void Update()
	{
		if (Time.time < resumeTime)
			return;

		switch(state)
		{
			case State.Monitoring:
				if (spawned.Count < activePopulation)
				{
					state = State.Spawn;
				}
				break;

			case State.Spawn:
				Respawn();
				resumeTime = Time.time + statusInterval;
				state = State.Monitoring;
				break;
		}
	}

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
		{
			Debug.LogError("Character Manager not initialized!");
		}
	}

	public CharacterDefinition GetCharacter(string id)
	{
		return this.characters.FirstOrDefault(c => c.id == id);
	}

	private CharacterDefinition GetRandomCharacter()
	{
		var list = new List<CharacterDefinition>(this.characters);
		list.Sort((x,y) => Random.value.CompareTo(Random.value));
		return list[Random.Range(0, list.Count-1)]; // Min incl, max incl
	}

	private SpawnPoint GetNextSpawnPoint()
	{
		var list = new List<SpawnPoint>(this.spawnPoints);
		list.Sort((x,y) => Random.value.CompareTo(Random.value));

		foreach(var spawnPoint in list)
		{
			if (!spawnPoint.IsReady)
				continue;

			return spawnPoint;
		}
		return null;
	}

	public void Respawn()
	{
		var spawnPoint = GetNextSpawnPoint();
		if (spawnPoint == null)
			return;

		var character = GetRandomCharacter();

		var obj = Spawner.Spawn(character.character, false, spawnPoint.transform.position, spawnPoint.transform.rotation);
		var spawn = obj.GetComponent<CharacterBase>();
		if (spawn != null)
		{
			spawn.Randomize();
			spawned.Add(obj);
		}
	}

	public void Dead(GameObject obj)
	{
		Spawner.Recycle(gameObject, 5f);
		this.spawned.Remove(obj);
	}


}

