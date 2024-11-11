using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using Mono.Data.Sqlite;
using System.Xml.Linq;

public class QuelThemeManager : MonoBehaviour
{
    int currentid;
    public Text idjoueur;

    private string dbname;

    public void Start()
    {
        dbname = "URI=file:" + Application.persistentDataPath + "/Patient.s3db";
        currentid = MainManager.Instance.currentidPatient;

        if (currentid == -1)
        { 
            idjoueur.text = "Mode Libre";
        }
        else
        {
            idjoueur.text = "";
            using (var connection = new SqliteConnection(dbname))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT prenom, nom, date FROM tablePatient WHERE id = @id;";
                    command.Parameters.AddWithValue("@id", currentid);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idjoueur.text += reader["prenom"] + " " + reader["nom"] + "\n";
                            idjoueur.text += reader["date"];
                        }
                    }
                }
                connection.Close();

            }
        }

    }
    public void LoadSport()
    {
        SceneManager.LoadScene("JeuThemeSport");
    }

    public void LoadBackScene()
    {
        if (currentid == -1)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("Patient");
        }
    }
}
