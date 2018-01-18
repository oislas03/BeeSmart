//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
//using System;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using System.Linq;




//public class EstadoJuego : MonoBehaviour
//{

//    public static EstadoJuego estadoJuego;
//    public List<Player> players;
//    public Player ActivePlayer;

//    public string rutaArchivo;

//    void Awake()
//    {
//        rutaArchivo = Application.persistentDataPath + "/datos.dat";
//        if (estadoJuego == null)
//        {
//            estadoJuego = this;
//            DontDestroyOnLoad(gameObject);

//        }
//        else if (estadoJuego != this)
//        {

//            Destroy(gameObject);
//        }
//        players = new List<Player>();
//        //   cargarJugadores();
//    }
//    // Use this for initialization
//    void Start()
//    {
//    }

//    // Update is called once per frame
//    void Update()
//    {

//        string nameScene = SceneManager.GetActiveScene().name;

//        if (nameScene.Equals("seleccionarUsuario") && players.Count != 0)
//        {
//            Player p = players.Find(item => item.id == 1);
//            GameObject.Find("NombreNiño").GetComponent<Text>().text = p.name;
//        }
//        else if (nameScene.Equals("seleccionarUsuario"))
//        {
//            GameObject.Find("NombreNiño").GetComponent<Text>().text = "no hay usuarios";

//        }


//    }


//    public string obtenerNombre(int i)
//    {
//        Debug.Log(i);
//        if (this.ActivePlayer.id + i < players.Count && this.ActivePlayer.id + i >= 0)
//        {
//            Debug.Log(this.ActivePlayer.id);

//            return players.Find(item => item.id == this.ActivePlayer.id + i).name;
//        }

//        return "vuelve";
//    }






//    public void guardarJugador()
//    {
//        Player ActivePlayer = new Player();
//        players.Add(ActivePlayer);
//        players.RemoveAt(players.IndexOf(ActivePlayer));
//        BinaryFormatter bf = new BinaryFormatter();
//        FileStream file = File.Create(rutaArchivo);
//        bf.Serialize(file, players);
//        file.Close();
//        cargarJugadores();
//    }

//    public void guardarNuevo(String nombre)
//    {
//        int index = players.Count == 0 ? 0 : players.Count;

//        this.ActivePlayer = new Player(index, nombre);
//        players.Add(ActivePlayer);
//        Debug.Log(ActivePlayer.name);
//        BinaryFormatter bf = new BinaryFormatter();
//        FileStream file = File.Create(rutaArchivo);
//        bf.Serialize(file, players);
//        file.Close();
//    }

//    public void cargarUsuario()
//    {

//    }

//    public void cargarJugadores()
//    {
//        if (File.Exists(rutaArchivo))
//        {
//            BinaryFormatter bf = new BinaryFormatter();
//            FileStream file = File.Open(rutaArchivo, FileMode.Open);
//            players = (List<Player>)bf.Deserialize(file);
//            file.Close();
//            foreach (Player s in players)
//            {
//                Debug.Log("Aqui deberia de estar? " + s.id + s.ToString());
//            }
//        }
//        this.ActivePlayer = new Player();
//    }



//}

//[System.Serializable]
//public class Player
//{
//    public int id;
//    public string name;
//    public int[] stats;


//    public Player()
//    {
//        //this.id = 04;
//        //this.name = "Hola";
//        //this.stats = new int[3] { 1,35,6};
//    }

//    public Player(int id, string name, int[] stats)
//    {
//        this.id = id;
//        this.name = name;
//        this.stats = stats;


//    }

//    public Player(int id)
//    {
//        this.id = id;



//    }
//    public Player(int id, String name)
//    {
//        this.id = id;
//        this.name = name;

//    }
//    public string toString()
//    {
//        return "id: " + this.id + " name: " + this.name;
//    }


//    public override bool Equals(object obj)
//    {
//        Player p = (Player)obj;
//        if (p == null)
//            return false;
//        else
//            return this.id.Equals(p.id);
//    }

//    public static explicit operator Player(UnityEngine.Object v)
//    {
//        throw new NotImplementedException();
//    }
//}

