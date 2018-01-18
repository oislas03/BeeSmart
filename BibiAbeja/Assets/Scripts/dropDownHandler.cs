using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class dropDownHandler : MonoBehaviour {
    public GameObject panelPregunta;
    private bool continuar;
    public GameObject panelPregunta2;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void seleccionarPalabra(string nombre) {
        int valor = GameObject.Find(nombre).GetComponent<Dropdown>().value;

        string valorReal = GameObject.Find(nombre).GetComponent<Dropdown>().options[valor].text;

        EstadoJuego.estadoJuego.setPalabra(valorReal);
        Debug.Log("entroooooo");
        switch (nombre)
        {
            case "btnFiguras":
                EstadoJuego.estadoJuego.setTema("geometria");
                break;
            case "btnAnimales":
                EstadoJuego.estadoJuego.setTema("animales");
                break;
            case "btnMediosTransporte":
                EstadoJuego.estadoJuego.setTema("frutas");
                break;
            case "btnNaturaleza":
                EstadoJuego.estadoJuego.setTema("naturaleza");
                break;
            case "btnHogar":
                EstadoJuego.estadoJuego.setTema("hogar");
                break;
            case "btnFrutas":
                EstadoJuego.estadoJuego.setTema("frutas");
                break;
   
        }
        continuar = EstadoJuego.estadoJuego.ImagenIncompleta();

        if (continuar)
        {
            panelPregunta.SetActive(true);
        }
        else {

            if (EstadoJuego.estadoJuego.NumIntentos != 0)
            {
                SceneManager.LoadScene("paso " + EstadoJuego.estadoJuego.nivel);
            }
            else {
                this.panelPregunta2.SetActive(true);

            }

          

        }
    }



    public void pasar() {
        this.panelPregunta2.SetActive(false);
    }
    public  void preguntaContinuar(bool continuar){
        this.panelPregunta.SetActive(false);


        EstadoJuego.estadoJuego.establecerImagenActiva(continuar);
        SceneManager.LoadScene("paso " + EstadoJuego.estadoJuego.nivel);


    }

    private void OneChanged(int newPosition)
    {

        string realValue = GameObject.Find("btnFiguras").GetComponent<Dropdown>().options[newPosition].text;
        Debug.Log("que paso aqi?");


        // realValue is the integer value associated with this key index
        // do whatever you need to do with it here
    }
}
