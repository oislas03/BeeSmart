using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class cambiarEscena : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()

    {

        GetComponent<AudioSource>().Play();
        Invoke("cargarEscena", GetComponent<AudioSource>().clip.length);

    }



    private void cargarEscena()
    {
        string nameScene = SceneManager.GetActiveScene().name;
        string btnName = this.gameObject.name;
        string sceneName = "";



        if (nameScene.Equals("Main"))
        {
            if (btnName.Equals("btnSalir"))
            {

                Application.Quit();
            }
            else
            {

                sceneName = "story";

                SceneManager.LoadScene(sceneName);
            }



        }
        else if (nameScene.Equals("seleccionarUsuario"))
        {
            sceneName = btnName.Equals("btnNuevo") ? "escribirNombre" : btnName.Equals("btnSalir") ? "Main" : btnName.Equals("btnCargar") ? "eligeNivel" : "Main";

            SceneManager.LoadScene(sceneName);


        }
        else if (nameScene.Equals("escribirNombre"))
        {
            sceneName = btnName.Equals("btnAceptar") ? "eligeNivel" : btnName.Equals("btnSalir") ? "Main" : "Main";

            if (btnName.Equals("btnAceptar"))
            {
                string nin = GameObject.Find("nombreReal").GetComponent<Text>().text;
                EstadoJuego.estadoJuego.guardarNuevo(nin);
            }


            SceneManager.LoadScene(sceneName);

        }
        else if (nameScene.Equals("eligeTema"))
        {
            sceneName = btnName.Equals("btnSalir") ? "Main" : btnName.Equals("btnFotos") ? "verFotos" : "Main";

            SceneManager.LoadScene(sceneName);

        }
        else if (nameScene.Equals("eligeNivel"))
        {

            switch (btnName)
            {
                case "btnNivel1":
                    EstadoJuego.estadoJuego.setNivel(1);
                    break;
                case "btnNivel11":
                    EstadoJuego.estadoJuego.setNivel(1);
                    break;
                case "btnNivel2":
                    EstadoJuego.estadoJuego.setNivel(2);
                    break;
                case "btnNivel22":
                    EstadoJuego.estadoJuego.setNivel(2);
                    break;
                case "btnNivel3":
                    EstadoJuego.estadoJuego.setNivel(3);
                    break;
                case "btnNivel33":
                    EstadoJuego.estadoJuego.setNivel(3);
                    break;
                case "btnNivel4":
                    EstadoJuego.estadoJuego.setNivel(4);
                    break;
                case "btnNivel44":
                    EstadoJuego.estadoJuego.setNivel(4);
                    break;
            }

            sceneName = btnName.Equals("btnSalir") ? "Main" : "eligeTema";
            SceneManager.LoadScene(sceneName);

        }
        else if (nameScene.Equals("story"))
        {
            sceneName = btnName.Equals("saltar") ? "seleccionarUsuario" : "Main";
            SceneManager.LoadScene(sceneName);
        }
        else if (nameScene.Equals("verFotos"))
        {
            SceneManager.LoadScene("eligeTema");
        }
        else if (nameScene.Equals("reconocimiento"))
        {

            if (btnName.Equals("siguiente"))
            {

                //Este if  es por si ya no quedan intentos
                if (EstadoJuego.estadoJuego.NumIntentos == EstadoJuego.estadoJuego.NumIntentosActual)
                {
                    SceneManager.LoadScene("eligeTema");

                }
                else {
                // Aquí Iván y sus cambios.
                // Desactive lo que estaba antes y puse este codigo.
                //

                EstadoJuego.estadoJuego.establecerImagenActiva(true);
                    SceneManager.LoadScene("paso " + EstadoJuego.estadoJuego.nivel);

                    //EstadoJuego.estadoJuego.setPalabra(EstadoJuego.estadoJuego.obtenerPalabraSiguiente());
                    //sceneName = "paso 1";

                    //Esto funcionó
                }
                
            }
            else
            {
                sceneName = btnName.Equals("repetir") ? "paso 1" : btnName.Equals("volver") ? "eligeTema" : "eligeTema";
            }

            //un cambio rapido aqui
            if (!btnName.Equals("siguiente"))
            {
                SceneManager.LoadScene(sceneName);
            }
            
        }


        //        nameScene = nameScene.Equals("seleccionarUsuario") ? btnName.Equals("btnSalir") ? "Main":"Main": "Main";
    }
}
