using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using Mono.Data.Sqlite;
using System.Text.RegularExpressions;

using System;

public class ButtonHistoButton : MonoBehaviour
{
    [SerializeField]
    private Text mytext;
    public int sessionId; // Identifiant du patient associé à ce bouton
    public Text infos;
    private string dberreurname;
    public void SetText(string textString)
    {
        mytext.text = textString;
    }



    // Méthode appelée lors du clic sur le bouton
    public void OnButtonClick()
    {

        Debug.Log("Session ID: " + sessionId);

        infos.text = "";

        dberreurname = "URI=file:" + Application.persistentDataPath + "/Erreursession.s3db";
        int nombreerreur = 0;
        using (var connection = new SqliteConnection(dberreurname))
        {
            connection.Open();


            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT temps, fini, erreur, jeu FROM tableErreursession WHERE idsession = @idsession;";
                command.Parameters.AddWithValue("@idsession", sessionId);
                

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string erreurs = reader["erreur"].ToString();
                        nombreerreur = erreurs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
                       

                        infos.text = "Temps : " + reader["temps"] + "\n \n";
                        infos.text += "Le jeu a été fini : " + reader["fini"] + "\n \n";
                        Debug.Log(reader["jeu"]);
                        if (reader["jeu"].ToString() == "Mon Sac") 
                        {
                            Debug.Log("Dans Mon Sac");
                            string[] erreursArray = erreurs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            infos.text += "Nombre total d'erreurs : " + nombreerreur/3 + "/18 \n \n";
                            infos.text += "Erreurs : \n\n";

                         
                            int compteur = 0;
                            // Affichage des erreurs pour vérification
                            foreach (string erreur in erreursArray)
                            {   
                                compteur ++;
                                Debug.Log(erreur);
                                if (compteur == 3)
                                {
                                    infos.text += erreur + " " + "\n";
                                    compteur = 0;
                                    Debug.Log("dans compteur");
                                }
                                else
                                {
                                    infos.text += erreur + " ";
                                }
                            }
                        }
                        else
                        {
                            string[] erreursArray = erreurs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            infos.text += "Nombre total d'erreurs : " + nombreerreur + "/9 \n \n";
                            infos.text += "Erreurs : \n\n";

                            foreach (string erreur in erreursArray)
                            {
                                infos.text += erreur + "\n";
                            }
                        }
                        
                        Debug.Log("les erreurs sont : " + reader["erreur"]);
                    }
                    
                }
            }
            connection.Close();
        }
        

    }
}
