using UnityEngine;
using System.Collections;

public class SupernovaControl : MonoBehaviour {

    public GameObject Explosion;
    public int Mass;
    private int ExplodeCount;
    private float ImageObjectRatio = 10f;

	// Use this for initialization
	void Start () {
        Mass = Random.Range(GameConstant.MassThreshold3,GameConstant.MassThreshold4);
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        ExplodeCount = 150;
        transform.localScale =  new Vector3(1f / ImageObjectRatio, 1f / ImageObjectRatio, 0);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, Random.Range(-1, -5), 0);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        switch (ExplodeCount)
        {
            case 100: gameObject.GetComponent<SpriteRenderer>().color = Color.yellow; break;
            case 50: gameObject.GetComponent<SpriteRenderer>().color = Color.white; break;
            case 0: var thisExplosion = Instantiate(Explosion, gameObject.transform.position, Quaternion.identity) as GameObject;
                thisExplosion.GetComponent<ExplosionControl2>().Mass = Mass;Destroy(gameObject);
                //gameObject.SetActive(false); 
                break;
        }
        ExplodeCount--;	
	}
}
