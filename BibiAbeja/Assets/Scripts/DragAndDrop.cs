using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour {

    private Ray ray;
    private RaycastHit hit;
    private bool isCursorPen = EstadoJuego.estadoJuego.isCursorPen;
    private bool isCursorSyllable = EstadoJuego.estadoJuego.isCursorSyllable;
    private bool llamarCorutina = EstadoJuego.estadoJuego.llamarCorutina;
    private EstadoJuego estadoJuego;
    private float tiempoDeJuego = 0;
    private int terminar = 0;
    private string carpeta = "";

    public Texture2D cursorImage;
    public Vector3 mousePos;
    public Vector3 v3 = new Vector3();
    public bool primeraVez = true;
    public string pasoNombre;
    public Texture2D[] manos = new Texture2D[4];
    public GameObject txtRetroDirecta;
    public GameObject btnSiguiente;
    public GameObject btnReintentar;
    public GameObject btnSalir;
    public Text txtPalabraFinal;
    public GameObject PanelSilabasEspacios;

    public string fichaAgarrada;
    public string[] fichas;
    public int tamanioFichas;
    public int aciertosFichas = 0;
    public string txtPalabra;
    public float tiempo;

    private HandCursor handCursor;

    // Use this for initialization
    void Start () {
        //handCursor = HandCursor.Instance;
	}
	
	// Update is called once per frame
	void Update () {
        //cursorImage = handCursor.cursorImage;
        mousePos = handCursor.mousePos;
        //// no v3
        //pasoNombre = handCursor.pasoNombre;
    }

    /// <summary>
    /// Método que permite repartir las piezas al azar en el tablero de piezas, si las piezas necesarias no son iguales al tamaño máximo
    /// de piezas posibles en el tablero, estos espacios se llenan con piezas al azar que no necesariamente corresponden a las piezas
    /// correctas necesarias para ganar.
    /// </summary>
    public void repartirPiezas()
    {
        handCursor = HandCursor.Instance;
        cursorImage = handCursor.cursorImage;
        mousePos = handCursor.mousePos;
        // no v3
        pasoNombre = handCursor.pasoNombre;
        tamanioFichas = handCursor.tamanioFichas;
        fichas = handCursor.fichas;
        ray = handCursor.ray;
        hit = handCursor.hit;

        // Se declara un arreglo de sprites para guardar las imagenes a cargar
        Sprite[] arregloSprites = null;
        string[] piezasAlAzar = null;
        int numeroRandom = 0;
        int iterador = 0;
        if (pasoNombre == "paso 3")
        {
            carpeta = "Silabas";
            numeroRandom = 4;
            arregloSprites = new Sprite[5];
            piezasAlAzar = new string[] { "cua", "trian", "tan", "lla", "cir" };
            iterador = piezasAlAzar.Length;
        }
        else if (pasoNombre == "paso 4")
        {
            carpeta = "Letras";
            numeroRandom = 11;
            arregloSprites = new Sprite[12];
            piezasAlAzar = new string[] { "c", "v", "m", "s", "p", "q", "w", "r", "t", "y", "g", "j" };
            iterador = piezasAlAzar.Length;
        }
        Debug.Log("valor del iterador para repartir piezas: " + iterador);
        // Se itera cuantas veces se necesite para llenar el tamaño de fichas posibles
        for (int i = 0; i < iterador; i++)
        {
            // Primero se añaden las piezas que corresponden
            if (i < tamanioFichas)
            {
                arregloSprites[i] = (Resources.Load("Sprites/Fichas/" + carpeta + "/" + fichas[i].ToString(), typeof(Sprite)) as Sprite);
            }
            // Si se requieren menos espacios de los máximos, se llenan los espacios disponibles con piezas al azar
            else if (i >= tamanioFichas && i < iterador)
            {
                int random = UnityEngine.Random.Range(0, numeroRandom);
                arregloSprites[i] = (Resources.Load("Sprites/Fichas/" + carpeta + "/" + piezasAlAzar[random].ToString(), typeof(Sprite)) as Sprite);
            }
        }
        // Se crea una lista de índices para cargar al azar los índices de los espacios en blanco
        List<int> espacioIndices = new List<int>();
        if (pasoNombre == "paso 3")
        {
            espacioIndices.AddRange(new int[] { 1, 2, 3, 4, 5 });
        }
        else if (pasoNombre == "paso 4")
        {
            espacioIndices.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
        }
        // Se cargan los sprites dentro de las piezas necesitadas
        for (int i = 0; i < iterador; i++)
        {
            // Si no hay elementos nulos, se establecen los espacios vacíos al valor del sprite en la escena
            if (arregloSprites[i] != null)
            {
                // Se busca un índice entre 1 y el máximo de fichas
                int num = UnityEngine.Random.Range(0, espacioIndices.Count);
                // Se establece ese número al azar como el índice para buscar en el arreglo de sprites
                int indice = espacioIndices[num];
                // Se encuentra el espacio en blanco para poner un sprite en la pieza de acuerdo a los valores al azar
                GameObject espacio = GameObject.Find("SilabaLetras" + indice);
                // Se establece el sprite encontrado en la pieza vacía encontrada
                SpriteRenderer imagenSilaba = espacio.GetComponent<SpriteRenderer>();
                imagenSilaba.sprite = arregloSprites[i];
                //Por último se elimina el índice que ya usamos una vez de las posibles variables índice que se pueden elegir
                espacioIndices.Remove(indice);
            }
        }

        // Se desactivan los espacios para fichas que no son necesarios
        int espaciosDesactivables = iterador - tamanioFichas; int x = iterador; // Variables para manejar los espacios a desactivar y los nombres de estos Ej: SilabaEspacio5
        for (int i = 0; i < espaciosDesactivables; i++)
        {
            string nombre = "SilabaEspacio" + x.ToString();
            GameObject.Find(nombre).SetActive(false);
            x--;
        }
    }

    /// <summary>
    /// Corrutina que permite llamar al método de agarrarPieza.
    /// </summary>
    public void agarrar_pieza()
    {
        StartCoroutine("agarrarPieza");
    }

    /// <summary>
    /// Método que permite agarrar una pieza con el cursor del tablero de piezas.
    /// </summary>
    /// <returns></returns>
    IEnumerator agarrarPieza()
    {
        if (EstadoJuego.estadoJuego.isCursorPen)
        {
            float tiempoRestante = tiempo;
            while (tiempoRestante > 0)
            {
                Debug.Log("Tiempo restante en agarrar pieza: " + tiempoRestante);
                yield return new WaitForSeconds(1);
                tiempoRestante--;
            }
            cursorImage = Resources.Load("Textures/mano1", typeof(Texture2D)) as Texture2D;

            ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject != null)
                {
                    // Se obtiene el objeto al que toca el Raycast
                    GameObject objetoSilaba = hit.transform.gameObject;
                    // Se obtiene el nombre del source image que tiene la sílaba que se tocó
                    string silaba = objetoSilaba.GetComponent<SpriteRenderer>().sprite.name;
                    fichaAgarrada = silaba;
                    // Se carga la nueva textura y se le da el tamaño adecuado
                    Texture2D imagen = Resources.Load("Textures/Fichas/" + carpeta + "/" + silaba, typeof(Texture2D)) as Texture2D;
                    cursorImage = imagen;

                    // Atributos de semáforo cambian de estado
                    EstadoJuego.estadoJuego.isCursorPen = false;
                    EstadoJuego.estadoJuego.isCursorSyllable = true;
                    //EstadoJuego.estadoJuego.llamarCorutina = true;

                    StartCoroutine("tiempoLibre");

                    // Se desactiva la imagen de la sílaba que se agarró con el cursor
                    objetoSilaba.GetComponent<SpriteRenderer>().sprite = null;
                }
            }
            else
            {
                EstadoJuego.estadoJuego.llamarCorutina = true;
            }
        }
        else
        {
            Debug.Log("El cursor es una sílaba");
        }
        yield return null;
    }

    /// <summary>
    /// Corrutina que permite llamar al método de colocarPieza.
    /// </summary>
    public void colocar_pieza()
    {
        StartCoroutine("colocarPieza");
    }

    /// <summary>
    /// Método que permite colocar una pieza con el cursor dentro de los espacios vacíos en el pizarrón.
    /// </summary>
    /// <returns></returns>
    IEnumerator colocarPieza()
    {
        if (EstadoJuego.estadoJuego.isCursorSyllable)
        {
            float tiempoRestante = tiempo;
            while (tiempoRestante > 0)
            {
                Debug.Log("Tiempo restante en colocar pieza: " + tiempoRestante);
                yield return new WaitForSeconds(1);
                tiempoRestante--;

            }
            ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject != null)
                {
                    // Se obtiene el GameObject del espacio vacío, después se coloca la pieza del cursor sobre él
                    GameObject espacioVacio = hit.transform.gameObject;

                    int indice = int.Parse(espacioVacio.name.Substring(13));
                    indice -= 1;
                    if (fichas[indice].Equals(fichaAgarrada))
                    {
                        SpriteRenderer imagenSilaba = espacioVacio.GetComponent<SpriteRenderer>();
                        imagenSilaba.sprite = Resources.Load("Sprites/Fichas/" + carpeta + "/" + fichaAgarrada, typeof(Sprite)) as Sprite;

                        // Una vez colocada la pieza en su lugar, se devuelve al cursor la imagen de la pluma
                        cursorImage = Resources.Load("Textures/mano1", typeof(Texture2D)) as Texture2D;

                        // Se aumentan los aciertos para saber cuándo terminará el ejercicio
                        aciertosFichas++;

                        // Atributos de semáforo cambian de estado
                        EstadoJuego.estadoJuego.isCursorPen = true;
                        EstadoJuego.estadoJuego.isCursorSyllable = false;
                        fichaAgarrada = "";
                        //EstadoJuego.estadoJuego.llamarCorutina = true;

                        StartCoroutine("tiempoLibre");

                        if (aciertosFichas == tamanioFichas)
                            ganar();
                        else
                            Debug.Log("Aún no has ganado");
                    }
                    else
                    {
                        // Se utiliza un índice al azar para asignar una de dos retroalimentaciones de error auditiva.
                        string audioError = "";
                        int random = UnityEngine.Random.Range(1, 3);
                        if (random == 1)
                            audioError = "Oh-Oh";
                        else if (random == 2)
                            audioError = "Ups";
                        AudioClip audioClip;
                        audioClip = Resources.Load("Sonidos/EnCasoDeError/" + audioError, typeof(AudioClip)) as AudioClip;
                        AudioSource sonido = GameObject.Find("sonido").GetComponent<AudioSource>();
                        reproducirSonido(sonido, audioClip);

                        StartCoroutine("tiempoLibre");
                        // Se establece en positivo la llamada a una corrutina
                        //EstadoJuego.estadoJuego.llamarCorutina = true;
                    }
                }
            }
            else
            {
                EstadoJuego.estadoJuego.llamarCorutina = true;
            }
        }
        else
        {
            Debug.Log("El cursor es una pluma");
        }
        yield return null;
    }

    /// <summary>
    /// Corrutina que permite llamar al método devolverPieza.
    /// </summary>
    public void devolver_pieza()
    {
        StartCoroutine("devolverPieza");
    }

    /// <summary>
    /// Método que permite devolver una pieza al tablero de piezas.
    /// </summary>
    /// <returns></returns>
    IEnumerator devolverPieza()
    {
        float tiempoRestante = tiempo;
        while (tiempoRestante > 0)
        {
            Debug.Log("Tiempo restante en colocar pieza: " + tiempoRestante);
            yield return new WaitForSeconds(1);
            tiempoRestante--;
        }
        ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject != null)
            {
                // Se obtiene el GameObject del espacio vacío, después se coloca la pieza del cursor sobre él
                GameObject espacioVacio = hit.transform.gameObject;
                SpriteRenderer imagenSilaba = espacioVacio.GetComponent<SpriteRenderer>();
                imagenSilaba.sprite = Resources.Load("Sprites/Fichas/" + carpeta + "/" + fichaAgarrada, typeof(Sprite)) as Sprite;

                // Una vez colocada la pieza en su lugar, se devuelve al cursor la imagen de la pluma
                cursorImage = Resources.Load("Textures/mano1", typeof(Texture2D)) as Texture2D;

                // Atributos de semáforo cambian de estado
                EstadoJuego.estadoJuego.isCursorPen = true;
                EstadoJuego.estadoJuego.isCursorSyllable = false;
                fichaAgarrada = "";
                //EstadoJuego.estadoJuego.llamarCorutina = true;
                StartCoroutine("tiempoLibre");
            }
        }
        else
        {
            EstadoJuego.estadoJuego.llamarCorutina = true;
        }
        yield return null;
    }

    /// <summary>
    /// Al realizar una acción con el cursor sobre las piezas, se tiene que esperar el tiempo especificado antes de poder realizar
    /// una nueva acción.
    /// </summary>
    /// <returns></returns>
    IEnumerator tiempoLibre()
    {
        float tiempoDeEspera = 2.0f;
        while (tiempoDeEspera > 0)
        {
            Debug.Log("Tiempo de espera para una nueva acción del cursor: " + tiempoDeEspera);
            yield return new WaitForSeconds(1);
            tiempoDeEspera--;
        }
        EstadoJuego.estadoJuego.llamarCorutina = true;
        yield return null;
    }

    /// <summary>
    /// Método que se llama cuando se determina que el jugador ha terminado la actividad.
    /// </summary>
    public void ganar()
    {
        txtRetroDirecta.SetActive(true);
        btnSiguiente.SetActive(true);
        btnReintentar.SetActive(false);
        btnSalir.SetActive(false);
        PanelSilabasEspacios.SetActive(false);
        txtPalabraFinal.gameObject.SetActive(true);
        txtPalabraFinal.text = txtPalabra.ToString();
        Debug.Log("Has ganado " + tiempoDeJuego.ToString());
        EstadoJuego.estadoJuego.registrarIntento(tiempoDeJuego, 1);
    }

    /// <summary>
    /// Método que permite reproducir un audio en tiempo de ejecución de acuerdo a los parámetros recibidos.
    /// </summary>
    /// <param name="sonido">Componente que reproducirá el sonido</param>
    /// <param name="audioClip">Audio que se reproducirá</param>
    public void reproducirSonido(AudioSource sonido, AudioClip audioClip)
    {
        sonido.clip = audioClip;
        sonido.Play();
    }

    /// <summary>
    /// Método que permite reproducir un arreglo de texturas con forma de mano para simular una animación con el cursor.
    /// </summary>
    /// <param name="framesPerSecond"></param>
    public void reproducirManoGif()
    {
        float index = Time.time * 10.0f;
        index = index % manos.Length;
        cursorImage = manos[(int)index];
    }
}
