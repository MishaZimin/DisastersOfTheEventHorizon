using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public static int Money;
	public int startMoney = 300;

	public static int Lives;
	public int startLives = 10;

	void Start ()
	{
		Money = startMoney;
		Lives = startLives;
	}

}