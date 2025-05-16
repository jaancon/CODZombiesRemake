using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Money : MonoBehaviour
{
    public static Money scriptInstance;
    public int money { get; private set; } = 0;

    private void Awake()
    {
        scriptInstance = this;
    }

    public void LoseMoney(int amount)
    {
        money -= amount;

        if (money < 0)
        {
            money = 0;
        }
    }

    public void GainMoney(int amount)
    {
         money += amount;
    }
}
