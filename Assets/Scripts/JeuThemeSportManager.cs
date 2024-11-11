using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;


public class JeuThemeSportManager : MonoBehaviour
{
    int currentid;
    public Text idjoueur;

    private string dbname;

    public void Start()
    {
        dbname = "URI=file:" + Application.persistentDataPath + "/Patient.s3db";
        if (MainManager.Instance != null)
        {
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
        else
        {
            Debug.Log("MainManager.Instance est null");
        }

    }
    public void LoadBackScene()
    {
        SceneManager.LoadScene("QuelTheme");
    }

    public void LoadQuelSport()
    {
        SceneManager.LoadScene("Quel Sport");
    }
    public void LoadMonSac()
    {
        SceneManager.LoadScene("Niveau Chaud");
    }
}
