using System;
using UnityEngine;


public abstract class BehaviorBase: MonoBehaviour
{
	private Player player;
	protected void Ensure(object value, string message = "Value expected for property!")
	{
		this.Assert(value != null, message);
	}

	protected void Log(string message, params object[] args)
	{
		if (args.Length > 0)
			message = string.Format(message, args);

		Debug.Log(message, this);

	}

	void Start()
	{
		var obj = GameObject.FindGameObjectWithTag("Player");
        player = obj != null ? obj.GetComponent<Player>() : default(Player);
        Ensure(player);
	}

    public Player Player
    {
        get
        {
            return player;
        }
    }
}

