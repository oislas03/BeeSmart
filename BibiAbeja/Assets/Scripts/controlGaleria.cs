
// Add this script to a GameObject. The Start() function fetches an
// image from the documentation site.  It is then applied as the
// texture on the GameObject.
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;




public class controlGaleria : MonoBehaviour
{


    List<string> nombreImagenes = new List<string>();
    public string[] paths = new string[4];

    public changeImg imgg = new changeImg();
    List<Imagen> imagenesObj = new List<Imagen>();
    int indice = 1;
    int count = 10;

    string nombreActivo = "";



    void Start()
    {
        //EstadoJuego.estadoJuego.setUsuario(1);

        //Se obtienen las imagenes que el niño ha desbloqueado
        imagenesObj = EstadoJuego.estadoJuego.cargarImagenesPorNino();



        //Debug.Log();

        //se recorren para no agregar nombres repetidos(recordar que son 4 imagenes con el mismo nombre)
        foreach (Imagen img in imagenesObj)
        {
            if (!nombreImagenes.Contains(img.nombre))
            {
                nombreImagenes.Add(img.nombre);
            }
        }

        //seteamos la imagen a colocar a la primera del arreglo
        this.nombreActivo = nombreImagenes[0];

        //colocamos la imagen
        ponerImagen();


    }


    public void CambiarImagenIzq()
    {

        Debug.Log("indice" + indice + " " + nombreActivo);

        //indica el indice del nombre activo
        int i = nombreImagenes.IndexOf(nombreActivo);


        if (indice < count)
        {
            Debug.Log("indice" + indice + "total: " + count);

            indice += 1;

            ponerImagen();

        }
        else
        {
            indice = 1;
            if ((i + 1) < nombreImagenes.Count)
            {
                this.nombreActivo = nombreImagenes[i + 1];

                ponerImagen();

            }
            else
            {
                this.nombreActivo = nombreImagenes[0];
                ponerImagen();

            }

        }


    }



    public void CambiarImagenDerecha()
    {
        Debug.Log("indice" + indice + " " + nombreActivo);


        //indica el indice del nombre activo
        int i = nombreImagenes.IndexOf(nombreActivo);


        if (indice <= count && indice > 1)
        {
            indice -= 1;
            ponerImagen();

        }
        else
        {
            indice = 4;
            if ((i) < nombreImagenes.Count && i > 0)
            {

                this.nombreActivo = nombreImagenes[i - 1];
                ponerImagen();

            }
            else
            {

                this.nombreActivo = nombreImagenes[nombreImagenes.Count - 1];
                ponerImagen();

            }
        }


    }

    public void ponerImagen()
    {
        AudioClip sonido;

        //Busca el componente del nombre la imagen para cargar el clip de audio de la misma
        GameObject.Find("nNino").GetComponent<Text>().text = this.nombreActivo;
        sonido = Resources.Load<AudioClip>("Sonidos/Figuras/" + this.nombreActivo);
        GetComponent<AudioSource>().PlayOneShot(sonido);

        //variable que sirve para contar cuantas imagenes de un mismo nombre ha desbloqueado el niño
        count = 0;
        bool entro = false;
        foreach (Imagen img in imagenesObj)
        {

            if (img.nombre.Equals(nombreActivo)) //recorre solo
            {
                //si la imagen es igual al indice que corresponde se coloca
                if (img.numPart == indice)
                {
                    Debug.Log("imagen " + img.numPart);
                    string imgpath = img.path;
                    imgg.colocarImagen(img.path);
                    entro = true;
                }

                count++;

            }

        }
        if (entro == false && indice == 4)
        {
            Debug.Log("imagen " + count);
            string imgpath = nombreActivo + count;
            imgg.colocarImagen(imgpath);
            indice = count;
        }


    }
}