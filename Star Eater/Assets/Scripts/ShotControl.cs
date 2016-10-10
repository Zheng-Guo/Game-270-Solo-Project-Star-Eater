using UnityEngine;
using System.Collections;

public class ShotControl : MonoBehaviour {

    public GameObject Monster;
    public int Damage;
    private float Radius = 0.6f;
    private float ImageObjectRatio = 6f;
    private float Speed=10;
	// Use this for initialization
	void Start () {
        Monster = GameObject.FindGameObjectWithTag("Player");
        int MonsterMass=Monster.GetComponent<Player>().Mass;
        float numberOfFramePerSecond = 1f / Time.deltaTime;
        transform.localScale = new Vector3(Radius / ImageObjectRatio, Radius / ImageObjectRatio, 0);
        GetComponent<CircleCollider2D>().radius = Radius - 0.1f;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, Speed);
	}


    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        } 
    }
}
