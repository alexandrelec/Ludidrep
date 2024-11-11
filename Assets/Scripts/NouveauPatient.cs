using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;

public class NouveauPatient : MonoBehaviour
{

    public InputField nomInput;
    public InputField prenomInput;
    public InputField dateInput;
    public InputField infoInput;
    public Text patientList;
    public Text errormessage;

    private string dbname;
    private string dbname2;

    private const int maxCharacters = 10; // Maximum de caractères dans la date (jour + mois + année + 2 slashes)
    private string previousText;



    // Start is called before the first frame update
    void Start()
    {
        dbname = "URI=file:" + Application.persistentDataPath + "/Patient.s3db";
        dbname2 = "URI=file:" + Application.persistentDataPath + "/Identifiant.s3db";
        Debug.Log("Database path: " + dbname);
        errormessage.text = "";
        CreateDB();

        DisplayPatient();

        // Dans la fonction Start()
        dateInput.onValueChanged.AddListener(OnDateValueChanged);
        previousText = dateInput.text;

    }


    public void CreateDB()
    {
        using (var connection = new SqliteConnection(dbname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS tablePatient (nom VARCHAR(30), prenom VARCHAR(30), date VARCHAR(30), info VARCHAR(500), id INT, prenomnom VARCHAR(61), nomprenom VARCHAR(61));";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
        using (var connection = new SqliteConnection(dbname2))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS tableIdentifiant (id INT);";
                command.ExecuteNonQuery();
            }
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO tableIdentifiant (id) VALUES (1);";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
        Debug.Log("DBcreated");
    }

    public void DisplayPatient()
    {
        patientList.text = "";

        using (var connection = new SqliteConnection(dbname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT nom, prenom, date FROM tablePatient ORDER BY nom;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())

                        patientList.text += reader["nom"] + "\t\t" + reader["prenom"] + "\t\t" + reader["date"] + "\n";

                }
            }
            connection.Close();

        }
    }

    public void AddPatient()
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
            int nombre = 0;
            using (var connection = new SqliteConnection(dbname2))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT id FROM tableIdentifiant";
                    nombre = Convert.ToInt32(command.ExecuteScalar());

                }
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE tableIdentifiant SET id = @id";
                    command.Parameters.AddWithValue("@id", nombre + 1);
                    command.ExecuteNonQuery();

                }
                connection.Close();
            }

            using (var connection = new SqliteConnection(dbname))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {

                    command.CommandText = "INSERT INTO tablePatient (nom, prenom, date, info, id, prenomnom, nomprenom) " +
                              "VALUES (@nom, @prenom, @date, @info, @id, @prenomnom, @nomprenom)";

                    // Ajouter des paramètres avec des valeurs sécurisées
                    command.Parameters.AddWithValue("@nom", nomInput.text);
                    command.Parameters.AddWithValue("@prenom", prenomInput.text);
                    command.Parameters.AddWithValue("@date", dateInput.text);
                    command.Parameters.AddWithValue("@info", infoInput.text);
                    command.Parameters.AddWithValue("@id", nombre);
                    command.Parameters.AddWithValue("@prenomnom", prenomInput.text + " " + nomInput.text);
                    command.Parameters.AddWithValue("@nomprenom", nomInput.text + " " + prenomInput.text);
                    command.ExecuteNonQuery();

                }
                connection.Close();
                DisplayPatient();

            }
            SceneManager.LoadScene("MainMenu");
        }
    }


    void OnDateValueChanged(string newText)
    {
        // Vérifier si la longueur de texte dépasse la longueur maximale
        if (newText.Length > 10)
        {
            dateInput.text = previousText; // Restaurer le texte précédent
            dateInput.caretPosition = 10; // Positionner le curseur à la fin
            return;
        }

        // Remplacer les caractères non numériques
        string cleanedText = Regex.Replace(newText, @"[^0-9/]", "");

        // Ajouter automatiquement les barres obliques après les deux premiers et les cinq premiers caractères
        if (cleanedText.Length == 2 && newText.Length > previousText.Length)
        {
            // Ajouter le slash après les deux premiers caractères
            cleanedText = cleanedText.Insert(2, "/");
        }
        else if (cleanedText.Length == 5 && newText.Length > previousText.Length)
        {
            // Ajouter le slash après les cinq premiers caractères
            cleanedText = cleanedText.Insert(5, "/");
        }

        // Mettre à jour le texte du champ de saisie
        dateInput.text = cleanedText;
        // Garder une copie du texte actuel
        previousText = cleanedText;
        // Positionner le curseur à la fin
        dateInput.caretPosition = cleanedText.Length;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
