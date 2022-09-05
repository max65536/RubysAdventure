using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	static int numBrokenRobot = 0;
	public int numBroken { get {return numBrokenRobot;}}
	public float speed = 3.0f;
	public bool vertical;
	public float changeTime = 3.0f;
	
	Rigidbody2D rigidbody2D;
	float timer;
	int direction = 1;
	
	Animator animator;
	
	bool broken = true;

	// why not  "public GameObject smokeEffect" ?
	
	public ParticleSystem smokeEffect;
    // Start is called before the first frame update

	public AudioClip hitSound_robot;
	public AudioClip fixSound;
	AudioSource audioSource;
    void Start()
    {
		numBrokenRobot += 1;
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    
    	// if not broken then stop moving
        if (!broken)
        {
        	return;
        }
        timer -= Time.deltaTime;
        // 与物理无关， 所以Timer的操作不放在Fixedpdate()
        if (timer < 0 )
        {
        	direction = -direction;
        	timer = changeTime;
        }
        

    }
    
    void FixedUpdate()
    {
    
    	// if not broken then stop moving
        if (!broken)
        {
        	return;
        }
    	Vector2 position = rigidbody2D.position;
    	
    	if (vertical)
    	{
    		position.y = position.y + Time.deltaTime * speed * direction;
    		animator.SetFloat("Move X", 0);
    		animator.SetFloat("Move Y", direction);
    	}
    	else
    	{
    		position.x = position.x + Time.deltaTime * speed * direction;			    	
    		animator.SetFloat("Move X", direction);
    		animator.SetFloat("Move Y", 0);    		
    	}
    	
    	rigidbody2D.MovePosition(position);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
    	RubyController player = other.gameObject.GetComponent<RubyController>();
    	
    	if (player != null)
    	{
    		player.ChangeHealth(-1, hitSound_robot);
			//player.PlaySound(hitSound_robot);
    	}
    }
    
    public void Fix()
    {
    	broken = false;
    	rigidbody2D.simulated = false; // not collisable
    	
    	animator.SetTrigger("Fixed");
		smokeEffect.Stop();
		audioSource.PlayOneShot(fixSound);
		numBrokenRobot -= 1;
		// Debug.Log("numBrokenRobot = " + numBrokenRobot);
		if (numBrokenRobot <= 0)
		{
			Congratulations.succeed();
		}
    }
}
