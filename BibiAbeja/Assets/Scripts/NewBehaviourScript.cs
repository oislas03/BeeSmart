using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        GetComponent<AudioSource>().Play();
        Invoke("cargarJuego", GetComponent<AudioSource>().clip.length);

    }

    private void cargarJuego()
    {
        SceneManager.LoadScene("story");
   }
}
