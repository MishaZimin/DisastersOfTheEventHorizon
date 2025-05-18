using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public int startMoney = 300;

    public static int Lives;
    public int startLives = 10;

    [Header("Passive Income")]
    public int incomePerSecond = 5;

    void Start()
    {
        Money = startMoney;
        Lives = startLives;

        StartCoroutine(AddMoneyOverTime());
    }

    IEnumerator AddMoneyOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Money += incomePerSecond;
        }
    }
}