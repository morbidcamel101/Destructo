using System;
using UnityEngine;


public abstract class BehaviorBase: MonoBehaviour
{
	private Player player = null;
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


    public Player Player
    {
        get
        {
        	if (player == null)
        	{
				var obj = GameObject.FindGameObjectWithTag("Player");
		        player = obj != null ? obj.GetComponent<Player>() : default(Player);
		        Ensure(player);
        	}
            return player;
        }
    }
}

