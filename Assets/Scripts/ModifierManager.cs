using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Text.RegularExpressions;

public class ModifierManager : MonoBehaviour
{

    public InputField nomInput;
    public InputField prenomInput;
    public InputField dateInput;
    public InputField infoInput;
    public Text errormessage;

    public int idPatient;

    private string dbname;

    // Start is called before the first frame update
    void Start()
    {
        dbname = "URI=file:" + Application.persistentDataPath + "/Patient.s3db";
        idPatient = MainManager.Instance.currentidPatient;

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
                        prenomInput.text += reader["prenom"];
                        nomInput.text += reader["nom"];
                        dateInput.text += reader["date"];
                        infoInput.text += reader["info"];
                    }
                }
            }
            connection.Close();

        }


    }

    public void Valider()
    {
        if (string.IsNullOrEmpty(prenomInput.text))
        {
            errormessage.text = "Erreur : Le prenom est vide";
        }
        else if (prenomInput.text.Length >= 30)
        {
            errormessage.text = "Erreur : Le prenom est trop long (max 30 charactères)";
        }
        else if (string.IsNullOrEmpty(nomInput.text))
        {
            errormessage.text = "Erreur : Le nom est vide";
        }
        else if (nomInput.text.Length >= 30)
        {
            errormessage.text = "Erreur : Le nom est trop long (max 30 charactères)";
        }
        else if (string.IsNullOrEmpty(dateInput.text))
        {
            errormessage.text = "Erreur : La date de naissance est vide";
        }
        else if (infoInput.text.Length >= 200)
        {
            errormessage.text = "Erreur : Les infos sont trop longues (max 200 charactères)";
        }
        else
        {
            errormessage.text = "";
            using (var connection = new SqliteConnection(dbname))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {

                    command.CommandText = "UPDATE tablePatient SET nom = @nom, prenom = @prenom, date = @date, info = @info, prenomnom = @prenomnom, nomprenom = @nomprenom WHERE id = @id";

                    // Ajouter des paramètres avec des valeurs sécurisées
                    command.Parameters.AddWithValue("@nom", nomInput.text);
                    command.Parameters.AddWithValue("@prenom", prenomInput.text);
                    command.Parameters.AddWithValue("@date", dateInput.text);
                    command.Parameters.AddWithValue("@info", infoInput.text);
                    command.Parameters.AddWithValue("@prenomnom", prenomInput.text + " " + nomInput.text);
                    command.Parameters.AddWithValue("@nomprenom", nomInput.text + " " + prenomInput.text);
                    command.Parameters.AddWithValue("@id", idPatient);

                    command.ExecuteNonQuery();

                }
                connection.Close();


            }
            SceneManager.LoadScene("Patient");
        }
    }

    public void BackToPatient()
    {
        SceneManager.LoadScene("Patient");
    }


}
