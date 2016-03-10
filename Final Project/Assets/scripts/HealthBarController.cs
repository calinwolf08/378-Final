using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour {
    public float health;
    public Image bar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        HandleBar();
	}

    private void HandleBar()
    {
        bar.fillAmount = health;
    }

    public void setHealth(float newHealth)
    {
        health = newHealth;
    }

    public void takeDamage(float damage)
    {
        health -= damage / 100;
    }
}
