using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;

public class PatientManager : MonoBehaviour
{
    public int idPatient;
    public Text date;
    public Text prenomnom;
    public Text informations;
    public GameObject popup;
    public Text textpopup;
    public GameObject maskPanel;

    private string dbname;

    // Start is called before the first frame update
    void Start()
    {
        dbname = "URI=file:" + Application.persistentDataPath + "/Patient.s3db";
        if (MainManager.Instance != null)
        {
            idPatient = MainManager.Instance.currentidPatient;
            Debug.Log("idPatient : " + idPatient);
            date.text = "";
            prenomnom.text = "";
            informations.text = "";

            using (var connection = new SqliteConnection(dbname))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT prenom, nom, date, info FROM tablePatient WHERE id = @id;";
                    Debug.Log("idPatient : " + idPatient);
                    command.Parameters.AddWithValue("@id", idPatient);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prenomnom.text += reader["prenom"] + " " + reader["nom"];
                            date.text += reader["date"];
                            informations.text += reader["info"];
                        }
                    }
                }
                connection.Close();

            }
        }
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Modifier()
    {
        SceneManager.LoadScene("ModifierPatient");
    }


    public void Jouer()
    {
        SceneManager.LoadScene("QuelTheme");
    }

    public void Supprimer()
    {
        
        idPatient = MainManager.Instance.currentidPatient;
        maskPanel.SetActive(true);
        popup.SetActive(true);
        textpopup.text = "Voulez-vous vraiment \n supprimer ce patient : \n";
        using (var connection = new SqliteConnection(dbname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT prenom, nom, date, info FROM tablePatient WHERE id = @id;";
                command.Parameters.AddWithValue("@id", idPatient);

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        textpopup.text += reader["prenom"] + " " + reader["nom"] + "\n" + reader["date"];
                    }
                }
            }
            connection.Close();

        }
    }

    public void Annuler()
    {
        maskPanel.SetActive(false);
        popup.SetActive(false);
        textpopup.text = "";
    }
    public void Valider()
    {
        idPatient = MainManager.Instance.currentidPatient;
        using (var connection = new SqliteConnection(dbname))
        {
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM tablePatient WHERE id = @id;";
                    command.Parameters.AddWithValue("@id", idPatient);
                    int rowsAffected = command.ExecuteNonQuery();
                    Debug.Log("Nombre de lignes supprimées : " + rowsAffected);
                }
                connection.Close();
            }

        }
        string dbsessionidname;
        dbsessionidname = "URI=file:" + Application.persistentDataPath + "/Erreursessionidentifiant.s3db";
        using (var connection = new SqliteConnection(dbsessionidname))
        {
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM tableErreursession WHERE idpatient = @id;";
                    command.Parameters.AddWithValue("@id", idPatient);
                    command.ExecuteNonQuery();
                    
                }
                connection.Close();
            }

        }
        popup.SetActive(false);
        maskPanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void Historique()
    {
        SceneManager.LoadScene("Historique");
    }
}
