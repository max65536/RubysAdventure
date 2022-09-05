using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip hitSound_damagezone;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
    	RubyController controller = other.GetComponent<RubyController>();
    	
    	if (controller != null)
    	{
    		controller.ChangeHealth(-1, hitSound_damagezone);
            //controller.PlaySound(hitSound_damagezone);
    	}
    }
}
