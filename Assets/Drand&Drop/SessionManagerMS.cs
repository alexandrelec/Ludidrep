using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;

public class SessionManagerMS : MonoBehaviour
{

    public int idPatient;
    public string date;
    public string nomdujeu;
    public string erreur;
    private string dberreurname;
    private float startTime; // Temps de départ
    private int sessionid;
    private string dbsessionidname;
    private string fini;
    private string temps;

    public static SessionManagerMS InstanceMS;
    public bool started;

    private void Awake()
    {
        started = false;
        // start of new code
        if (InstanceMS != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        InstanceMS = this;
        DontDestroyOnLoad(gameObject);
    }
    // New function to destroy the instance
    public void DestroyInstance()
    {
        if (InstanceMS != null)
        {
            Destroy(gameObject);
            InstanceMS = null;
        }
    }


    public void Beginsession()
    {
        started = true;
        sessionid = 0;
        dbsessionidname = "URI=file:" + Application.persistentDataPath + "/Erreursessionidentifiant.s3db";
        using (var connection = new SqliteConnection(dbsessionidname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id FROM tableIdentifiantsession";
                sessionid = Convert.ToInt32(command.ExecuteScalar());

            }
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE tableIdentifiantsession SET id = @id";
                command.Parameters.AddWithValue("@id", sessionid + 1);
                command.ExecuteNonQuery();

            }
            connection.Close();
        }
        Debug.Log("debut de la session : " + sessionid);

        dberreurname = "URI=file:" + Application.persistentDataPath + "/Erreursession.s3db";

        idPatient = MainManager.Instance.currentidPatient;
        Debug.Log("idPatient : " + idPatient);
        date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        Debug.Log("date : " + date);
        nomdujeu = "Mon Sac";
        Debug.Log("nom du jeu :" + nomdujeu);
        fini = "Non";

        using (var connection = new SqliteConnection(dberreurname))
        {
            connection.Open();

            using (var insertCommand = connection.CreateCommand())
            {
                insertCommand.CommandText = "INSERT INTO tableErreursession (date, jeu, idpatient, idsession, fini) VALUES (@date, @jeu, @idpatient, @idsession, @fini)";
                insertCommand.Parameters.AddWithValue("@date", date);
                insertCommand.Parameters.AddWithValue("@jeu", nomdujeu);
                insertCommand.Parameters.AddWithValue("@idpatient", idPatient);
                insertCommand.Parameters.AddWithValue("@idsession", sessionid);
                insertCommand.Parameters.AddWithValue("@fini", fini);

                insertCommand.ExecuteNonQuery();
            }
            connection.Close();
        }
    }


    public void EndSession(int state)
    {
        if (state == 0)
        {
            fini = "Oui";
        }
        else if (state == 1)
        {
            fini = "Non, le joueur a quitté le jeu";
        }
        else if (state == 2)
        {
            fini = "Non, le joueur a perdu tout ses coeurs ";
        }

        using (var connection = new SqliteConnection(dberreurname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE tableErreursession SET fini = @fini, temps = @temps WHERE idsession = @idsession";
                command.Parameters.AddWithValue("@fini", fini);
                command.Parameters.AddWithValue("@temps", temps);
                command.Parameters.AddWithValue("@idsession", sessionid);
                command.ExecuteNonQuery();
            }

            Debug.Log("Session mise à jour.");
            connection.Close();
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        Debug.Log("start time : " + startTime);

    }

    public void StopTimer()
    {

        float t = Time.time - startTime; // Temps écoulé depuis le début
        int minutes = (int)t / 60;
        int seconds = (int)t % 60;
        Debug.Log("finish time : " + Time.time);
        Debug.Log("temps " + minutes + ":" + seconds);
        temps = minutes + ":" + seconds;
    }

    public void AddErreur(GameObject droppedObject)
    {
        erreur = droppedObject.name;
        Debug.Log("erreur :" + erreur);
        dberreurname = "URI=file:" + Application.persistentDataPath + "/Erreursession.s3db";


        if (MainManager.Instance.currentidPatient > -1)
        {
            using (var connection = new SqliteConnection(dberreurname))
            {
                connection.Open();

                string erreursExistantes = "";

                // Récupérer les erreurs existantes pour cette session
                using (var selectCommand = connection.CreateCommand())
                {
                    selectCommand.CommandText = "SELECT erreur FROM tableErreursession WHERE idsession = @idsession";
                    selectCommand.Parameters.AddWithValue("@idsession", sessionid);

                    using (var reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            erreursExistantes = reader["erreur"].ToString();
                        }
                    }
                }
                string niveau = SceneManager.GetActiveScene().name;
                // Ajouter la nouvelle erreur à la chaîne existante
                string erreursCombinees = string.IsNullOrEmpty(erreursExistantes) ? erreur + " (" + niveau + ")" : erreursExistantes + " " + erreur + " (" + niveau + ")";

                // Mettre à jour la base de données avec les erreurs combinées
                using (var updateCommand = connection.CreateCommand())
                {
                    updateCommand.CommandText = "UPDATE tableErreursession SET erreur = @erreur WHERE idsession = @idsession";
                    updateCommand.Parameters.AddWithValue("@erreur", erreursCombinees);
                    updateCommand.Parameters.AddWithValue("@idsession", sessionid);
                    updateCommand.ExecuteNonQuery();
                    Debug.Log("Erreur ajoutée : " + erreursCombinees);
                    Debug.Log("Idsession de l'erreur ajoutée: " + sessionid);
                }



                connection.Close();


            }
        }
    }
}
