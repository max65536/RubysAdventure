using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Congratulations : MonoBehaviour
{
    static Image image;
    static AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static void succeed()
    {
        image.enabled = true;
        audioSource.Play();
    }
}
