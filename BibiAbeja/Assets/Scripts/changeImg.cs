using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeImg : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void colocarImagen(string path)
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
     //   Vector3 pos = new Vector3(-109.4F,-62.019F,0);
     //   gameObject.GetComponent<RectTransform>().position= pos;
    }
}
