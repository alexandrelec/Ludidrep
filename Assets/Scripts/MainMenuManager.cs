using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private string dberreurname;
    private string dbsessionidname;
    private void Start()
    {
        MainManager.Instance.currentidPatient = -1;
        dberreurname = "URI=file:" + Application.persistentDataPath + "/Erreursession.s3db";
        dbsessionidname = "URI=file:" + Application.persistentDataPath + "/Erreursessionidentifiant.s3db";
        CreateDB();
    }
    public void LoadNouveau()
    {
        SceneManager.LoadScene("NouveauPatient");
    }

    public void LoadQuelTheme()
    {
        SceneManager.LoadScene("QuelTheme");
    }

    public void LoadPatient()
    {
        SceneManager.LoadScene("Patient");
    }


    public void CreateDB()
    {
        using (var connection = new SqliteConnection(dberreurname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS tableErreursession (date VARCHAR(30), jeu VARCHAR(30), idpatient INT, erreur VARCHAR(500), idsession INT, temps VARCHAR(30), fini VARCHAR(30));";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        using (var connection = new SqliteConnection(dbsessionidname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS tableIdentifiantsession (id INT);";
                command.ExecuteNonQuery();
            }
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO tableIdentifiantsession (id) VALUES (1);";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        Debug.Log("DBcreated");
    }

    public void Quitter()
    {
        Application.Quit();
    }
}
