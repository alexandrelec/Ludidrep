using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;

public class RechercheJoueur : MonoBehaviour
{
    public InputField champRecherche;
    public GameObject boutonTemplate; // Utilis� pour instancier les boutons de r�sultats
    public Transform parentBoutons; // Parent des boutons de r�sultats
    private string connectionString = "URI=file:Patient.db"; // Chemin de votre base de donn�es SQLite

    void Start()
    {
        RechercherJoueurs("");
    }

    public void RechercherJoueurs(string recherche)
    {
        // Effacer les r�sultats pr�c�dents
        EffacerResultats();

        // �crire votre requ�te SQL pour rechercher les joueurs correspondants � la saisie
        string requeteSQL = "SELECT nom, prenom, dateNaissance FROM joueurs WHERE nom LIKE '%" + recherche + "%' OR prenom LIKE '%" + recherche + "%'";

        // Connexion � la base de donn�es SQLite et ex�cution de la requ�te
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = requeteSQL;

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nom = reader.GetString(0);
                        string prenom = reader.GetString(1);
                        string dateNaissance = reader.GetString(2);

                        // Instancier un bouton de r�sultat
                        GameObject bouton = Instantiate(boutonTemplate) as GameObject;
                        bouton.SetActive(true);
                        bouton.GetComponent<ButtonListButton>().SetText(nom + " " + prenom + " - " + dateNaissance);

                        // D�finir le parent du bouton de r�sultat
                        bouton.transform.SetParent(parentBoutons, false);
                    }
                }
            }

            connection.Close();
        }
    }
    public void OnSearchFieldUpdated(string searchTerm)
    {
        RechercherJoueurs(searchTerm);
    }

    // Fonction pour effacer les r�sultats pr�c�dents
    private void EffacerResultats()
    {
        foreach (Transform child in parentBoutons)
        {
            Destroy(child.gameObject);
        }
    }
}