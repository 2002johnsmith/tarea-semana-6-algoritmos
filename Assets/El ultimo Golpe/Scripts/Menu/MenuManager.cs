using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button buttonAddActions;
    [SerializeField] private Button buttonPlayActions;
    [SerializeField] private Button buttonDeleteActions;
    [SerializeField] private TMP_Text textMessage;
    [SerializeField] private Enemy enemy;
    [SerializeField] private int numActions;
    [SerializeField] private int delayActions;



    private void Start()
    {
        buttonAddActions.onClick.AddListener(AddActions);
        buttonPlayActions.onClick.AddListener(() => StartCoroutine("PlayActions"));
        buttonDeleteActions.onClick.AddListener(DeleteActions);
    }
    void AddActions()
    {

        enemy.AddActions(numActions);
    }


    IEnumerator PlayActions()
    {
        enemy.ManagerActions(Enemy.Actions.Aparition);
        yield return new WaitForSeconds(delayActions);
        int aux = enemy.QueueActions.Count;
        for (int i = 0; i < aux; i++)
        {
           
            enemy.ManagerActions(enemy.QueueActions.Dequeue());
            ChangueMessage(enemy.Message);
            Debug.Log("Siguiente accion");
            yield return new WaitForSeconds(delayActions);
        }
        enemy.ManagerActions(Enemy.Actions.ExitScreen);
    }

    void DeleteActions()
    {
        Debug.Log("Se borraron los mensajes");
        enemy.QueueActions.Clear();
    }

    void ChangueMessage(string message)
    {
        textMessage.text = message;
    }
}
