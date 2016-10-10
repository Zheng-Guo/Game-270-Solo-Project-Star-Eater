using UnityEngine;
using System.Collections;

public class CelestialBodyControl : MonoBehaviour {

    public int Mass;
    public GameObject Monster;
    private SpriteRenderer image;
	// Use this for initialization
	void Start () {
        Mass = Random.Range(50, 100);
        image = GetComponent<SpriteRenderer>();
        image.color = Mass <= Monster.GetComponent<Player>().Mass ? Color.white : Color.grey;
	}

    void UpdateHighlight()
    {
        image.color = Mass <= Monster.GetComponent<Player>().Mass ? Color.white : Color.gray;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shot"))
        {
            Mass -= 20;
            UpdateHighlight();
            if (Mass < 0)
                Destroy(gameObject);
        }
    }
}
