using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button buttonAddActions;
    [SerializeField] private Button buttonPlayActions;
    [SerializeField] private Button buttonDeleteActions;
    [SerializeField] private TMP_Text textMessage;
    [SerializeField] private Enemy enemy;
    [SerializeField] private int numActions;
    [SerializeField] private int delayActions;

    public static event Action<bool> OnChangueAction;

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
        enemy.gameObject.SetActive(true);
        enemy.ManagerActions(Enemy.Actions.Aparition);
        ChangueMessage(enemy.Message);
        yield return new WaitForSeconds(delayActions);
        

        int aux = enemy.QueueActions.Count;
        for (int i = 0; i < aux; i++)
        {
            var valueAction = enemy.QueueActions.Dequeue();
           if (valueAction != default)
            {
                enemy.ManagerActions(valueAction);
                ChangueMessage(enemy.Message);
                Debug.Log("Siguiente accion");
                yield return new WaitForSeconds(delayActions);
                OnChangueAction?.Invoke(false);
            }
            else
            {
                break;
            }
              
        }
        enemy.ManagerActions(Enemy.Actions.ExitScreen);
        ChangueMessage(enemy.Message);
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
