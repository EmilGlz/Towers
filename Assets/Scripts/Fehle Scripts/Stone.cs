using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public int gainingMoney;
    public int health;
    public TextMesh healthText;
    public bool isDestroyed = false;
    private void Start()
    {
        healthText.text = health.ToString();
    }

    public bool BeDamagedBy(int damage)
    {
        if (health <= 0)
        {
            healthText.text = health.ToString();
            if (!isDestroyed) OnStoneDestroy();
            return false;
        }
        health -= damage;
        if (health < 0)
        {
            health = 0;
            healthText.text = health.ToString();
            if (!isDestroyed) OnStoneDestroy();
            return false;
        }
        healthText.text = health.ToString();
        return true;
    }

    async void OnStoneDestroy()
    {
        isDestroyed = true;
        FehleController.Instance.OnStoneDestroyed(this);
        await System.Threading.Tasks.Task.Delay(3000);
        Destroy(gameObject);
    }
}