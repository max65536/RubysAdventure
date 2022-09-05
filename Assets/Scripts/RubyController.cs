using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 5;
    public float speed = 5.0f;
    public float timeInvincible = 2.0f;
    
    int currentHealth;
    public int health { get {return currentHealth;}}
    
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    
    public GameObject projectilePrefab;
	AudioSource audioSource;
	public AudioClip throwSound;
    
    void Start()
    {
//        QualitySettings.vSyncCount = 0;
//        Application.targetFrameRate = 10;
		rigidbody2d = GetComponent<Rigidbody2D>();
		currentHealth = maxHealth;
		
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}

		// Update is called once per frame
	void Update()
	{
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
	//    	Debug.Log(horizontal);
	
		Vector2 move = new Vector2(horizontal, vertical);
	
		if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
		{
			lookDirection.Set(move.x, move.y);
			lookDirection.Normalize();
		}

		if (isInvincible)
		{
			invincibleTimer -= Time.deltaTime;
			if (invincibleTimer <0)
				isInvincible = false;
		}
		
		animator.SetFloat("Look X", lookDirection.x);
		animator.SetFloat("Look Y", lookDirection.y);
		animator.SetFloat("Speed", move.magnitude);
		
		// launch
		if (Input.GetKeyDown(KeyCode.C))
		{
			Launch();
		}

		//if (Input.GetKeyDown(KeyCode.X))
		{
			// Vector2.up * 0.2 means start from center of Ruby instead of feet (Pivot)
			RaycastHit2D hit =  Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, 
											lookDirection, 1.5f, LayerMask.GetMask("NPC"));
			if (hit.collider != null)
			{
				//Debug.Log("Raycast has hit the object" + hit.collider.gameObject);
				NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
				if (character != null)
				{
					character.DisplayDialog();
				}
			}

		}

    }
    
    void FixedUpdate()
    {
    	
		Vector2 position = transform.position;
		position.x = position.x + speed * horizontal * Time.deltaTime;
		position.y = position.y + speed * vertical * Time.deltaTime;
		
		//transform.position = position;
		rigidbody2d.MovePosition(position);
    }
    
    public void ChangeHealth(int amount, AudioClip clip)
    {
    	if (amount < 0)
    	{
    		animator.SetTrigger("Hit");  
    		if (isInvincible)
    			return;
    		isInvincible = true;
    		invincibleTimer = timeInvincible;
		
    	}
		PlaySound(clip);
    	currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    	//Debug.Log(currentHealth + "/" + maxHealth);
		UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);

    }
    
    void Launch()
    {
    	GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
    	
//    	Debug.Log("Vector2.up = " + Vector2.up);
    	
    	Projectile projectile = projectileObject.GetComponent<Projectile>();
    	projectile.Launch(lookDirection, 300);
		PlaySound(throwSound);
    }

	public void PlaySound(AudioClip clip)
	{
		audioSource.PlayOneShot(clip);
	}
}
