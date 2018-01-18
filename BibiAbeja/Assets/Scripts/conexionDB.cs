using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class conexionDB
{

    string conn;

    public conexionDB()
    {
        conn = "URI=file:" + Application.dataPath + "/StreamingAssets/BibiAbejaBD.db";
    }

    public List<Player> obtenerJugadores()
    {

        List<Player> jugadores = new List<Player>();
        Player jugador = new Player();
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT nombre, id " + "FROM Usuarios";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            string name = reader.GetString(0);
            int id = reader.GetInt32(1);

            jugador = new Player(id, name);
            jugadores.Add(jugador);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        return jugadores;

    }



    public void guardarNuevo(string nombre)
    {

        IDbConnection dbconn;

        dbconn = (IDbConnection)new SqliteConnection(conn);


        dbconn.Open(); //Open connection to the database.


        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "INSERT INTO Usuarios (nombre)  VALUES ('" + nombre + "')"; // FIXED
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }

    public List<String> obtenerPalabras(string tema)
    {

        List<String> palabras = new List<String>();
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "select nombre from Palabras where  tema='" + tema + "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            string palabra = reader.GetString(0);

            palabras.Add(palabra);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        return palabras;

    }



    public List<Imagen> obtenerImagenes()
    {

        List<Imagen> imagenes = new List<Imagen>();
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT id, numParte, nombre, path FROM Imagenes";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int numParte = reader.GetInt32(1);
            string nombre = reader.GetString(2);
            string path = reader.GetString(3);

            imagenes.Add(new Imagen(id, numParte, nombre, path));
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        return imagenes;

    }



    public int obtenerIndiceMaximo(int idUsuario, string palabra, int nivel)
    {
        int indiceMaximo = 0;
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT COUNT (id) FROM Intentos WHERE idUsuario = " + idUsuario + " AND palabra = '" + palabra + "' AND nivel = " + nivel;
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            indiceMaximo = reader.GetInt32(0);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        return indiceMaximo;
    }

    public List<Imagen> obtenerImagenesDesbloqueadasPorPalabra(int idUsuario, string palabra)
    {

        List<Imagen> imagenes = new List<Imagen>();
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "select Imagenes.Id, Imagenes.numParte,  Imagenes.nombre, Imagenes.path from Imagenes JOIN ImagenesDesbloqueadas  ON (Imagenes.id = ImagenesDesbloqueadas.idImagen)AND (Imagenes.nombre='" + palabra + "') AND(ImagenesDesbloqueadas.idUsuario='" + idUsuario + "')";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int numParte = reader.GetInt16(1);
            string nombre = reader.GetString(2);
            string path = reader.GetString(3);

            imagenes.Add(new Imagen(id, numParte, nombre, path));
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        return imagenes;

    }

    public List<Imagen> obtenerImagenesDesbloqueadasPorNino(int idUsuario)
    {

        List<Imagen> imagenes = new List<Imagen>();
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "select Imagenes.id, Imagenes.numParte, Imagenes.nombre, Imagenes.path from Imagenes JOIN ImagenesDesbloqueadas ON( Imagenes.id= ImagenesDesbloqueadas.idImagen) AND (ImagenesDesbloqueadas.idUsuario=" + idUsuario + ") AND (visible='si')";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int numParte = reader.GetInt32(1);
            string nombre = reader.GetString(2);
            string path = reader.GetString(3);

            imagenes.Add(new Imagen(id, numParte, nombre, path));
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        return imagenes;

    }

    public void guardarImgDesbloqueada(string nombre, int idUsuario)
    {

        IDbConnection dbconn;

        dbconn = (IDbConnection)new SqliteConnection(conn);


        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "INSERT INTO ImagenesDesbloqueadas (idUsuario,idImagen)  VALUES ('" + idUsuario + "',(1+(select "
            + "CAST(  CASE  WHEN MAX(idImagen) IS NOT NULL THEN Max(idImagen)     ELSE  (select Min(id) from Imagenes where nombre= '" + nombre + "')-1 END AS bit) as MAXIMO "
            + "from Imagenes JOIN ImagenesDesbloqueadas  ON (Imagenes.id = ImagenesDesbloqueadas.idImagen)AND (Imagenes.nombre='" + nombre + "') AND(ImagenesDesbloqueadas.idUsuario='" + idUsuario + "'))))"; // FIXED
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }

    public void guardarImgDesbloqueada(string nombre, int idUsuario, int divisor, int numCompletado)
    {
        int id = this.obtenerUltimoidImgDesbloqueada(idUsuario, nombre);

        IDbConnection dbconn;

        dbconn = (IDbConnection)new SqliteConnection(conn);


        dbconn.Open(); //Open connection to the database.
        string sqlQuery = "";
        if (id == -1)
        {
            sqlQuery = "INSERT INTO ImagenesDesbloqueadas (idUsuario,idImagen, divisor, numCompletado)  VALUES ('" + idUsuario + "',(1+(select "
                          + "CAST(  CASE  WHEN MAX(idImagen) IS NOT NULL THEN Max(idImagen)     ELSE  (select Min(id) from Imagenes where nombre= '" + nombre + "')-1 END AS bit) as MAXIMO "
                          + " from Imagenes JOIN ImagenesDesbloqueadas  ON (Imagenes.id = ImagenesDesbloqueadas.idImagen) AND (Imagenes.nombre='" + nombre + "') AND (ImagenesDesbloqueadas.idUsuario='" + idUsuario + "'))) , " + divisor + "," + numCompletado + ")"; // FIXED

        }
        else
        {
            Debug.Log(divisor + "  intentos" + numCompletado);
            sqlQuery = "UPDATE ImagenesDesbloqueadas  SET divisor = " + divisor + ", numCompletado = " + numCompletado + ", visible ='si' WHERE idImagen = ( SELECT MAX(ImagenesDesbloqueadas.idImagen)"
 + "from Imagenes JOIN ImagenesDesbloqueadas ON(Imagenes.id = ImagenesDesbloqueadas.idImagen) AND (Imagenes.nombre = '" + nombre + "') AND (ImagenesDesbloqueadas.idUsuario = '" + idUsuario + "') )";
        }

        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;


    }






    public void guardarIntento(int idUsuario, string palabra, float duracion, string path, int exitoso, int nivel, String[] datos)
    {

        IDbConnection dbconn;
        String fecha = datos[0];
        String hora = datos[1];
        String turno = datos[2];
        dbconn = (IDbConnection)new SqliteConnection(conn);

        dbconn.Open(); //Open connection to the database.


        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "insert into Intentos (idUsuario, tiempoDuracion, trazoPath, palabra, exitoso, nivel, fecha, hora, turno) VALUES (" + idUsuario + "," + duracion + ",'" + path + "','" + palabra + "'," + exitoso + "," + nivel + ",' " + fecha + "', '" + hora + "', '" + turno + "')"; // FIXED
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }

    public String obtenerSilabasPalabra(string palabra)
    {
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        dbconn.Open(); //Open connection to the database.

        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "select silabas from Palabras where nombre='" + palabra + "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            palabra = reader.GetString(0);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        return palabra;
    }



    public int obtenerUltimoidImgDesbloqueada(int idUsuario, string palabra)
    {
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);

        dbconn.Open(); //Open connection to the database.
        int id = 0;
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT CAST (CASE WHEN  MAX(idImagen) IS NOT NULL AND divisor<> numCompletado THEN MAX(idImagen)  ELSE (-1) END AS INTEGER) AS MAXIMO"
        + " from Imagenes JOIN ImagenesDesbloqueadas  ON(Imagenes.id = ImagenesDesbloqueadas.idImagen)AND(Imagenes.nombre = '" + palabra + "') AND(ImagenesDesbloqueadas.idUsuario = '" + idUsuario + "')";

        dbcmd.CommandText = sqlQuery;

        IDataReader reader = dbcmd.ExecuteReader();
        try
        {
            while (reader.Read())
            {
                id = reader.GetInt32(0);
            }

        }
        catch (Exception e)
        {

            Debug.Log(e.Message);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        return id;
    }

    public void borrarImgIncompleta(int id)
    {

        IDbConnection dbconn;

        dbconn = (IDbConnection)new SqliteConnection(conn);


        dbconn.Open(); //Open connection to the database.


        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "DELETE FROM ImagenesDesbloqueadas where idImagen=" + id; // FIXED
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }

    public Imagen obtenerUltimaImgDesbloqueada(int idUsuario, string palabra)
    {
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        Imagen x = new Imagen();
        dbconn.Open(); //Open connection to the database.
        int id = 0;
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT CAST (CASE WHEN  MAX(idImagen) IS NOT NULL AND divisor<> numCompletado THEN MAX(idImagen)  ELSE (-1) END AS INTEGER) AS MAXIMO, divisor, numCompletado"
        + " from Imagenes JOIN ImagenesDesbloqueadas  ON(Imagenes.id = ImagenesDesbloqueadas.idImagen)AND(Imagenes.nombre = '" + palabra + "') AND(ImagenesDesbloqueadas.idUsuario = '" + idUsuario + "')";

        dbcmd.CommandText = sqlQuery;

        IDataReader reader = dbcmd.ExecuteReader();
        try
        {
            while (reader.Read())
            {
                x.Id = reader.GetInt32(0);
                x.Divisor = reader.GetInt32(1);
                x.NumCompletado = reader.GetInt32(2);
            }

        }
        catch (Exception e)
        {

            Debug.Log(e.Message);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        return x;
    }

}
