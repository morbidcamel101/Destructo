﻿using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.AI;



[AddComponentMenu("Small World/Character Manager")]
public sealed class CharacterManager : BehaviorBase
{
	public enum State { Initializing, WaitingForDrop, Monitoring, Spawn };
	public State state;
	public float statusInterval = 1f;
	public CharacterDefinition[] characters;
	public float walkRadius = 1f;
	public Prototype spawnPointPrefab;
	public int spawnPointCount = 20;
	public float dropHeight = 20;
	public int maxRetries = 1000;
	public TerainManager terrain;
	public float dropWaitTime = 15f;
	public float spawnDelay = 15f;
	// Controls difficulty
	public float spawnInterval = 10f;
	public int population = 100;
	public int activePopulation = 20;
	public float strengthMultiplier = 1f;



	internal SpawnPoint[] spawnPoints;
	internal List<GameObject> spawned = new List<GameObject>();
	private float resumeTime;

	void Awake()
	{
		_instance = this;
		Ensure(spawnPointPrefab);
		Ensure(terrain);

		foreach(var character in characters)
		{
			this.Assert(character.character != null, "Character prototype not selected!");
		}

		resumeTime = Time.time + 2f;
		state = State.Initializing;
	}

	void Update()
	{
		if (Time.time < resumeTime)
			return;

		switch(state)
		{
			case State.Initializing:
				//GenerateSpawnPoints();  --> Re-enable this - CHALLENGE!!
				spawnPoints = GetComponentsInChildren<SpawnPoint>();
				if (spawnPoints.Length == 0)
				{
					Debug.LogError("There are now spawn points defined!");
					enabled = false;
				}
				resumeTime = Time.time + dropWaitTime; 
				state = State.WaitingForDrop;
				break;

			case State.WaitingForDrop:
				if (Time.time < resumeTime)
					break;

				state = State.Monitoring;
				break;
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

	private void GenerateSpawnPoints()
	{
		// MorbidCamel - Couldn't get this to work, created them manually
		List<SpawnPoint> spawns = new List<SpawnPoint>();
		for(int i = 0; i < spawnPointCount; i++)
		{
			// Credits - http://answers.unity3d.com/questions/475066/how-to-get-a-random-point-on-navmesh.html
			// Modified

			NavMeshHit hit = default(NavMeshHit);
			Vector3 randomDirection;
			var retries = 0;
			do 
			{
				randomDirection = Random.insideUnitSphere * 3f; // not sure
				randomDirection += transform.position;
		 	}
			while (++retries < maxRetries && !NavMesh.SamplePosition(randomDirection, out hit, 3000, 1));

			if (retries >= maxRetries)
				continue;

			var pos = this.terrain.transform.position + hit.position + (this.terrain.transform.up * dropHeight);
			var obj = Spawner.Spawn(spawnPointPrefab, false, pos, Quaternion.identity);
			spawns.Add(obj.GetComponent<SpawnPoint>());
			obj.transform.parent = this.transform;
		}
		spawnPoints = GetComponentsInChildren<SpawnPoint>();
		if (spawnPoints.Length == 0)
		{
			Debug.LogError("There are now spawn points defined!");
			enabled = false;
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
		if (population == 0)
		{
			Log("WAVE Completed!");
			return;
		}
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

		this.population--;
	}

	public void Dead(GameObject obj)
	{
		Spawner.Recycle(obj);
		this.spawned.Remove(obj);
	}


}

