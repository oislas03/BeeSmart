using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class cuadroDialogoController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //EstadoJuego.estadoJuego

        EstadoJuego.estadoJuego.palabra = "";
        EstadoJuego.estadoJuego.NumIntentos = 0;



    }



    public void cambiar() {
        int valor = GameObject.Find("controller").transform.FindChild("Intentos").GetComponent<Dropdown>().value;

        EstadoJuego.estadoJuego.setIntentos(valor);
    }



    // Update is called once per frame
    void Update () {
		
	}
}
