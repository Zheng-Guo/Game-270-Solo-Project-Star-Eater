using UnityEngine;
using System.Collections;

public class BlackholeControl : MonoBehaviour {

    public int Mass;
    private float EffectiveRadius;
    public GameObject Monster;
    public GameObject RangeIndicator;
    private float ImageObjectRatio = 1.2f;
    private SpriteRenderer image;
	// Use this for initialization
	void Start () {
        Mass = Random.Range(GameConstant.MassThreshold3, GameConstant.MassThreshold5);
        EffectiveRadius = Mathf.Pow(Mass, 1f / 3f) / 20f;
        image = GetComponent<SpriteRenderer>();
        image.color =  Monster.GetComponent<Player>()!=null&&Mass <= Monster.GetComponent<Player>().Mass ? Color.white : Color.grey;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, Random.Range(-1, -5), 0);
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-100, 100);
        Monster = GameObject.FindGameObjectWithTag("Player");
        RangeIndicator.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(EffectiveRadius / ImageObjectRatio, EffectiveRadius / ImageObjectRatio, 0);
	}

    void OnEnable()
    {
        Player.OnApproach += Suck;
        Player.OnMassChange += UpdateHighlight;
    }

    void OnDisable()
    {
        Player.OnApproach -= Suck;
        Player.OnMassChange -= UpdateHighlight;
    }

    void Suck()
    {
        Vector3 v1 = gameObject.transform.position;
        Vector3 v2 = Monster.transform.position;
        Vector3 distance = v1-v2;
        if (distance.magnitude-Monster.GetComponent<Player>().Radius*Monster.transform.localScale.x/ImageObjectRatio <= EffectiveRadius && Monster.GetComponent<Player>().Mass>0)
        {
            int massLose = (int)(((float)Mass / 10000f) / distance.magnitude / distance.magnitude * GameConstant.SuckingMassLoseRate);
            Monster.GetComponent<Player>().Mass -= massLose;
        }
    }

    void UpdateHighlight()
    {
        image.color = Mass <= Monster.GetComponent<Player>().Mass ? Color.white : Color.gray;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shot"))
        {
            Mass -= GameConstant.FireDamage;
            UpdateHighlight();
            if (Mass < 0)
                Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("GameController"))
        {
            Destroy(gameObject);
        }
    }
}
