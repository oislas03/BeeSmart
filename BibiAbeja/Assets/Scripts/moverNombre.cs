using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class moverNombre : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        if (gameObject.name.Equals("btnIzquierda"))
        {
         EstadoJuego.estadoJuego.cambiarUsuarioActivo(-1);
        }
        else if (gameObject.name.Equals("btnDerecha"))
        {
            EstadoJuego.estadoJuego.cambiarUsuarioActivo(+1);
        }



    }
}
