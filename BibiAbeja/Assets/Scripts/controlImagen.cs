using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;
using System;
using UnityEngine.UI;

public class controlImagen : MonoBehaviour
{
    public changeImg[] imagenes = new changeImg[8];
    String path = "";
    int index = 0;
    int numParticiones;
    //float timeLeft = 3;




    // Use this for initialization
    void Start()

    {


        //EstadoJuego.estadoJuego.setTema("geometria");
        //EstadoJuego.estadoJuego.setNivel(1);
        //EstadoJuego.estadoJuego.setUsuario(3);
        //EstadoJuego.estadoJuego.setPalabra("triangulo");
        //EstadoJuego.estadoJuego.NumIntentos = 1;
        //EstadoJuego.estadoJuego.NumIntentosActual = 1;



        String palabra = EstadoJuego.estadoJuego.palabra;
        path = palabra + "Completo";
        numParticiones = EstadoJuego.estadoJuego.NumIntentos;
        int indiceParticiones = EstadoJuego.estadoJuego.NumIntentosActual-1;
        //imgDesbloqueadas = EstadoJuego.estadoJuego.cargarImagenesDesbloqueadasporPalabra();

        GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = false;
        GameObject.Find("marco").transform.FindChild("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = false;
        GameObject.Find("marco").transform.FindChild("parte3").transform.FindChild("Imagen3").GetComponent<Image>().enabled = false;
        GameObject.Find("marco").transform.FindChild("parte4").transform.FindChild("Imagen4").GetComponent<Image>().enabled = false;
        GameObject.Find("marco").transform.FindChild("parte5").transform.FindChild("Imagen5").GetComponent<Image>().enabled = false;
        GameObject.Find("marco").transform.FindChild("parte6").transform.FindChild("Imagen6").GetComponent<Image>().enabled = false;
        GameObject.Find("marco").transform.FindChild("parte7").transform.FindChild("Imagen7").GetComponent<Image>().enabled = false;
        GameObject.Find("marco").transform.FindChild("parte8").transform.FindChild("Imagen8").GetComponent<Image>().enabled = false;


        switch (numParticiones)
        {
            case 1:
                index = 0;
                Invoke("ponerImagen", 1);
                break;
            case 2:
                if (indiceParticiones == 0)
                {

                    index = 0;
                    Invoke("ponerImagen", 1);

                }
                else if (indiceParticiones == 1)
                {
                    GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte7").transform.FindChild("Imagen7").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte8").transform.FindChild("Imagen8").GetComponent<Image>().enabled = true;
                    imagenes[0].colocarImagen(path);
                    imagenes[1].colocarImagen(path);
                    imagenes[6].colocarImagen(path);
                    imagenes[7].colocarImagen(path);
                    index = 1;
                    Invoke("ponerImagen", 1);
                }
                break;
            case 4:
                if (indiceParticiones == 0)
                {
                    index = 0;
                    Invoke("ponerImagen", 1);

                }
                else if (indiceParticiones == 1)
                {
                    GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = true;
                    imagenes[0].colocarImagen(path);
                    imagenes[1].colocarImagen(path);
                    index = 1;
                    Invoke("ponerImagen", 1);
                }
                else if (indiceParticiones == 2)
                {
                    GameObject.Find("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                    GameObject.Find("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = true;
                    GameObject.Find("parte3").transform.FindChild("Imagen3").GetComponent<Image>().enabled = true;
                    GameObject.Find("parte4").transform.FindChild("Imagen4").GetComponent<Image>().enabled = true;
                    imagenes[0].colocarImagen(path);
                    imagenes[1].colocarImagen(path);
                    imagenes[2].colocarImagen(path);
                    imagenes[3].colocarImagen(path);
                    index = 2;
                    Invoke("ponerImagen", 1);
                }
                else if (indiceParticiones == 3)
                {
                    GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte3").transform.FindChild("Imagen3").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte4").transform.FindChild("Imagen4").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte7").transform.FindChild("Imagen7").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte8").transform.FindChild("Imagen8").GetComponent<Image>().enabled = true;

                    imagenes[0].colocarImagen(path);
                    imagenes[1].colocarImagen(path);
                    imagenes[2].colocarImagen(path);
                    imagenes[3].colocarImagen(path);
                    imagenes[6].colocarImagen(path);
                    imagenes[7].colocarImagen(path);
                    index = 3;
                    Invoke("ponerImagen", 1);
                }
                break;
            case 6:
                if (indiceParticiones == 0)
                {
                    index = 0;
                    Invoke("ponerImagen", 1);
                }
                else if (indiceParticiones == 1)
                {
                    GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                    imagenes[0].colocarImagen(path);
                    index = 1;
                    Invoke("ponerImagen", 1);
                }
                else if (indiceParticiones == 2)
                {
                    GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte3").transform.FindChild("Imagen3").GetComponent<Image>().enabled = true;

                    imagenes[0].colocarImagen(path);
                    imagenes[1].colocarImagen(path);
                    imagenes[2].colocarImagen(path);
                    index = 2;
                    Invoke("ponerImagen", 1);
                }
                else if (indiceParticiones == 3)
                {
                    GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte3").transform.FindChild("Imagen3").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte4").transform.FindChild("Imagen4").GetComponent<Image>().enabled = true;
                    imagenes[0].colocarImagen(path);
                    imagenes[1].colocarImagen(path);
                    imagenes[2].colocarImagen(path);
                    imagenes[3].colocarImagen(path);
                    index = 3;
                    Invoke("ponerImagen", 1);
                }
                else if (indiceParticiones == 4)
                {
                    GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte3").transform.FindChild("Imagen3").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte4").transform.FindChild("Imagen4").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte8").transform.FindChild("Imagen8").GetComponent<Image>().enabled = true; imagenes[0].colocarImagen(path);
                    imagenes[1].colocarImagen(path);
                    imagenes[2].colocarImagen(path);
                    imagenes[3].colocarImagen(path);
                    imagenes[7].colocarImagen(path);
                    index = 4;
                    Invoke("ponerImagen", 1);
                }
                else if (indiceParticiones == 5)
                {
                    GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte3").transform.FindChild("Imagen3").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte4").transform.FindChild("Imagen4").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte8").transform.FindChild("Imagen8").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte6").transform.FindChild("Imagen6").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte7").transform.FindChild("Imagen7").GetComponent<Image>().enabled = true;
                    imagenes[0].colocarImagen(path);
                    imagenes[1].colocarImagen(path);
                    imagenes[2].colocarImagen(path);
                    imagenes[3].colocarImagen(path);
                    imagenes[7].colocarImagen(path);
                    imagenes[5].colocarImagen(path);
                    imagenes[6].colocarImagen(path);
                    index = 5;
                    Invoke("ponerImagen", 1);
                }
                break;
            default:
                break;

        }










        //if (i)
        //{
        //    Debug.Log("sigo aqui?");
        //    EstadoJuego.estadoJuego.guardarImagenDesbloqueada();
        //    imgDesbloqueadas = EstadoJuego.estadoJuego.cargarImagenesDesbloqueadasporPalabra();

        //}else {
        //    List<string> palabrasTemas = EstadoJuego.estadoJuego.obtenerPalabrasTema();

        //for (int i = 1; i <= palabrasTemas.Count; i++)
        //{

        //    imgDesbloqueadas = EstadoJuego.estadoJuego.cargarImagenesDesbloqueadasporPalabra();

        //}
        //    foreach (string pb in palabrasTemas)
        //    {

        //        imgDesbloqueadas = EstadoJuego.estadoJuego.cargarImagenesDesbloqueadasporPalabra(pb);
        //        if (imgDesbloqueadas.Count != 4)
        //        {
        //            Debug.Log(imgDesbloqueadas + "ESaTAN AQUI???");

        //            EstadoJuego.estadoJuego.guardarImagenDesbloqueada(pb);
        //            imgDesbloqueadas = EstadoJuego.estadoJuego.cargarImagenesDesbloqueadasporPalabra(pb);
        //            break;

        //        }
        //    }
        //    Debug.Log(imgDesbloqueadas+"ESaTAN AQUI???");


        //}
        //foreach (Imagen pb in imgDesbloqueadas)
        //{
        //    Debug.Log(pb.path);

        //}
        //Debug.Log(imgDesbloqueadas.Find(item => item.id == 0));

        //   int i = 0;
        //foreach (Imagen img in imgDesbloqueadas)
        //{
        //    if (i < imgDesbloqueadas.Count - 1)
        //    {

        //        imagenes[i].colocarImagen(img.path);
        //    }
        //    else if (imgDesbloqueadas.Count - 1 == i)
        //    {
        //        index = i;
        //        this.imagenActiva = img;
        //        Invoke("ponerImagen", 1);

        //    }
        //    i++;
        //}

    }

    // Update is called once per frame

    void update()
    {

    }

    public void ponerImagen()
    {
        this.GetComponent<AudioSource>().Play();


        switch (numParticiones)
        {
            case 1:
                index = 0;
                Debug.Log(path);
                GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                GameObject.Find("marco").transform.FindChild("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = true;
                GameObject.Find("marco").transform.FindChild("parte3").transform.FindChild("Imagen3").GetComponent<Image>().enabled = true;
                GameObject.Find("marco").transform.FindChild("parte4").transform.FindChild("Imagen4").GetComponent<Image>().enabled = true;
                GameObject.Find("marco").transform.FindChild("parte5").transform.FindChild("Imagen5").GetComponent<Image>().enabled = true;
                GameObject.Find("marco").transform.FindChild("parte6").transform.FindChild("Imagen6").GetComponent<Image>().enabled = true;
                GameObject.Find("marco").transform.FindChild("parte7").transform.FindChild("Imagen7").GetComponent<Image>().enabled = true;
                GameObject.Find("marco").transform.FindChild("parte8").transform.FindChild("Imagen8").GetComponent<Image>().enabled = true;

                break;
            case 2:
                if (index == 0)
                {
                    Console.WriteLine("Aqui estoy!");

                    GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte7").transform.FindChild("Imagen7").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte8").transform.FindChild("Imagen8").GetComponent<Image>().enabled = true;
                    imagenes[0].colocarImagen(path);
                    imagenes[1].colocarImagen(path);
                    imagenes[6].colocarImagen(path);
                    imagenes[7].colocarImagen(path);
                }
                else if (index == 1)
                {
                    GameObject.Find("marco").transform.FindChild("parte3").transform.FindChild("Imagen3").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte4").transform.FindChild("Imagen4").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte5").transform.FindChild("Imagen5").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte6").transform.FindChild("Imagen6").GetComponent<Image>().enabled = true;
                    imagenes[2].colocarImagen(path);
                    imagenes[3].colocarImagen(path);
                    imagenes[5].colocarImagen(path);
                    imagenes[4].colocarImagen(path);
                    // EstadoJuego.estadoJuego.guardarImagenDesbloqueada(EstadoJuego.estadoJuego.palabra);
                }
                break;
            case 4:
                if (index == 0)
                {
                    GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = true;
                    imagenes[0].colocarImagen(path);
                    imagenes[1].colocarImagen(path);
                }
                else if (index == 1)
                {
                    GameObject.Find("marco").transform.FindChild("parte3").transform.FindChild("Imagen3").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte4").transform.FindChild("Imagen4").GetComponent<Image>().enabled = true;
                    imagenes[2].colocarImagen(path);
                    imagenes[3].colocarImagen(path);
                }
                else if (index == 2)
                {
                    GameObject.Find("marco").transform.FindChild("parte7").transform.FindChild("Imagen7").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte8").transform.FindChild("Imagen8").GetComponent<Image>().enabled = true;

                    imagenes[6].colocarImagen(path);
                    imagenes[7].colocarImagen(path);
                }
                else if (index == 3)
                {
                    GameObject.Find("marco").transform.FindChild("parte5").transform.FindChild("Imagen5").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte6").transform.FindChild("Imagen6").GetComponent<Image>().enabled = true;
                    imagenes[4].colocarImagen(path);
                    imagenes[5].colocarImagen(path);
                    //EstadoJuego.estadoJuego.guardarImagenDesbloqueada(EstadoJuego.estadoJuego.palabra);

                }
                break;
            case 6:
                if (index == 0)
                {
                    GameObject.Find("marco").transform.FindChild("parte1").transform.FindChild("Imagen1").GetComponent<Image>().enabled = true;
                    imagenes[0].colocarImagen(path);

                }
                else if (index == 1)
                {
                    GameObject.Find("marco").transform.FindChild("parte2").transform.FindChild("Imagen2").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte3").transform.FindChild("Imagen3").GetComponent<Image>().enabled = true;

                    imagenes[1].colocarImagen(path);
                    imagenes[2].colocarImagen(path);

                }
                else if (index == 2)
                {
                    GameObject.Find("marco").transform.FindChild("parte4").transform.FindChild("Imagen4").GetComponent<Image>().enabled = true;
                    imagenes[3].colocarImagen(path);
                }
                else if (index == 3)
                {
                    GameObject.Find("marco").transform.FindChild("parte8").transform.FindChild("Imagen8").GetComponent<Image>().enabled = true;
                    imagenes[7].colocarImagen(path);
                }
                else if (index == 4)
                {

                    GameObject.Find("marco").transform.FindChild("parte6").transform.FindChild("Imagen6").GetComponent<Image>().enabled = true;
                    GameObject.Find("marco").transform.FindChild("parte7").transform.FindChild("Imagen7").GetComponent<Image>().enabled = true;
                    imagenes[5].colocarImagen(path);
                    imagenes[6].colocarImagen(path);

                }
                else if (index == 5)
                {
                    GameObject.Find("marco").transform.FindChild("parte5").transform.FindChild("Imagen5").GetComponent<Image>().enabled = true;
                    imagenes[4].colocarImagen(path);
                    //EstadoJuego.estadoJuego.guardarImagenDesbloqueada(EstadoJuego.estadoJuego.palabra);

                }
                break;
            default:
                break;

        }

    }



}


