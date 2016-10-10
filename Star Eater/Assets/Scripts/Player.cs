using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public int Mass;
    public int PreviousMass;
    public float speed;
    private SpriteRenderer image;
    private GameObject Aurora;
    public Text MassText;
    private float Image_Object_Ratio = 6f;
    public GameObject Shot;
    public Transform ShotSpawn;
    private float FireRate=0.25f;
    private float NextFire;
    public GameObject Explosion;
    public Text Instruction;
    public bool GameStart;
    public Text TimeText;
    public float TimeRemaining;
    public Text EndingMessage;
    private int EndCount;
    public float Radius;
    public bool GameOver;

    public delegate void Interaction();
    public static event Interaction OnApproach;
    public static event Interaction OnMassChange;

    void Start()
    {
        Mass = PreviousMass = GameConstant.PlayerInitialMass;
        Radius = 1;
        image=GetComponent<SpriteRenderer>();
        Aurora = GameObject.FindGameObjectWithTag("Aurora");
        transform.localScale = new Vector3(1.2f / Image_Object_Ratio, 1.2f / Image_Object_Ratio, 0);
        MassText.text = "Mass: " + Mass.ToString();
        image.color = Color.green;
        GameStart = false;
        TimeRemaining = GameConstant.GameDuration;
        TimeText.text = "Time Left: "+System.String.Format("{0:00.00}s", TimeRemaining);
        EndCount = GameConstant.BufferTime / 2;
        GameOver = false;
        Aurora.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Z) && Mass >= GameConstant.PlayerFullStrengthMass && Time.time > NextFire)
        {
            NextFire = Time.time + FireRate;
            Instantiate(Shot, ShotSpawn.position, ShotSpawn.rotation);
        }
    }

	void FixedUpdate () {
        if (GameStart)
        {
            if (TimeRemaining <= 0)
            {
                GameStart = false;
                GameOver = true;
                TimeText.text = "Time Left: 0.00s";
            }
            else
            {
                TimeRemaining -= Time.deltaTime;
                TimeText.text = "Time Left: " + System.String.Format("{0:00.00}s", TimeRemaining);
            }
        }
        if (GameOver)
        {
                if (EndCount > 0)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                    EndCount--;
                }
                else
                {
                    EndingMessage.text = "Game Over!\nYour mass: " + Mass.ToString();
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 8, 0);
                }
                return;
        }
        if (Mass > GameConstant.PlayerFullStrengthMass)
        {
            Radius = Mathf.Pow(Mass, 1f / 3f) / 15f;
        }
        if (Radius > 1)
        {
            transform.localScale = new Vector3(1.2f / Image_Object_Ratio * Radius, 1.2f / Image_Object_Ratio*Radius, 0);
        }
        else
        {
            transform.localScale = new Vector3(1.2f / Image_Object_Ratio, 1.2f / Image_Object_Ratio, 0);
        }
        Instruction.text = gameObject!=null?GameConstant.Instruction1:Instruction.text;
        float speedCoefficient = Input.GetKey(KeyCode.X) ? 2f : 1;
        bool acceleration = Input.GetKey(KeyCode.X) && Mass >= GameConstant.PlayerCriticalMass;
        bool fire = Input.GetKey(KeyCode.Z) && Mass >= GameConstant.PlayerFullStrengthMass;
        float numberOfFramePerSecond = 1f / Time.deltaTime;
        int MassDeduction = acceleration ? Mathf.CeilToInt(((float)Mass/100f*(float)GameConstant.AccelerationMassLosePercentage/numberOfFramePerSecond)) : 0;
        MassDeduction += fire ? Mathf.CeilToInt(((float)Mass / 100f * (float)GameConstant.FireMassLosePercentage / numberOfFramePerSecond)) : 0;
        Aurora.SetActive(acceleration);
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, moveVertical,0);
        gameObject.GetComponent<Rigidbody2D>().velocity = movement * speed * speedCoefficient;
        if (!GameOver)
        {
            gameObject.GetComponent<Rigidbody2D>().position = new Vector3
            (
            Mathf.Clamp(gameObject.GetComponent<Rigidbody2D>().position.x, GameConstant.LeftBound, GameConstant.RightBound),
            Mathf.Clamp(gameObject.GetComponent<Rigidbody2D>().position.y, GameConstant.BottomBound, GameConstant.TopBound),
            0
            );
        }
        Mass -= MassDeduction;
        if (OnApproach != null)
        {
            OnApproach();
        }
        if (OnMassChange != null && Mass != PreviousMass)
        {
            OnMassChange();
            PreviousMass = Mass;
        }
        if (Mass < 0)
            Mass = 0;
        if (Mass < GameConstant.MinimumStarMass)
        {
            //gameObject.SetActive(false);
            EndingMessage.text = "Game Over!\nYour mass: " + Mass.ToString();
            Instruction.text += GameConstant.Instruction2;
            Instruction.text += GameConstant.Instruction4;
            GameStart = false;
            GameOver = true;
            Destroy(gameObject);
            //gameObject.SetActive(false);
            Instantiate(Explosion, transform.position, transform.rotation);
        }
        else if (Mass < GameConstant.PlayerCriticalMass)
        {
            image.color = Color.red;
            Instruction.text += GameConstant.Instruction2;
            Instruction.text += GameConstant.Instruction4;
        }
        else if (Mass >= GameConstant.PlayerCriticalMass && Mass < GameConstant.PlayerFullStrengthMass)
        {
            image.color = Color.green;
            Instruction.text += GameConstant.Instruction3;
            Instruction.text += GameConstant.Instruction4;
        }
        else if (Mass >= GameConstant.PlayerFullStrengthMass)
        {
            image.color = Color.white;
            Instruction.text += GameConstant.Instruction3;
            Instruction.text += GameConstant.Instruction5;
        }
        MassText.text = "Mass: " + Mass.ToString();
	}

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Star")&&!GameOver)
        {
            Destroy(other.gameObject);
            if (Mass >= other.gameObject.GetComponent<StarControl>().Mass)
            {
                Mass += other.gameObject.GetComponent<StarControl>().Mass;
                if (OnMassChange != null)
                    OnMassChange();
                MassText.text = "Mass: " + Mass.ToString();
            }
            else
            {
                EndingMessage.text = "Game Over!\nYour mass: " + Mass.ToString();
                GameOver = true;
                Destroy(gameObject);
                //gameObject.SetActive(false);
                Instantiate(Explosion, transform.position, transform.rotation);
            }
        }
        if (other.gameObject.CompareTag("Blackhole") && !GameOver)
        {
            Destroy(other.gameObject);
            if (Mass >= other.gameObject.GetComponent<BlackholeControl>().Mass)
            {
                Mass += other.gameObject.GetComponent<BlackholeControl>().Mass;
                if (OnMassChange != null)
                    OnMassChange();
                MassText.text = "Mass: " + Mass.ToString();
            }
            else
            {
                GameOver = true;
                EndingMessage.text = "Game Over!\nYour mass: " + Mass.ToString();
                Destroy(gameObject);
                //gameObject.SetActive(false);
                Instantiate(Explosion, transform.position, transform.rotation);
            }
        }
        if (other.gameObject.CompareTag("Supernova") && !GameOver)
        {
            GameOver = true;
            EndingMessage.text = "Game Over!\nYour mass: " + Mass.ToString();
            Destroy(other.gameObject);
            Destroy(gameObject);
            Instantiate(Explosion, transform.position, transform.rotation);
        }
        if (other.gameObject.CompareTag("Explosion") && !GameOver)
        {
            Vector3 distance = gameObject.transform.localScale - other.gameObject.transform.localScale;
            float damageRange = distance.magnitude - Radius * transform.localScale.x;
            float damage = other.gameObject.GetComponent<ExplosionControl2>().Mass / damageRange / damageRange / 1000;
            Mass -= (int)damage;
        }
    }
}
