using UnityEngine;
using System;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [Header("Atributtes")]
    Material material;
    string message;
    Queue<Actions> queueActions = new Queue<Actions>();

    [Header("Movements")]
    [SerializeField] private float rotationSpeed;
    private float moveSpeed = 2f;
    private float moveRange = 3f;
    Vector3 target;
    Rigidbody rb;
    private bool movingRight = true;
    private Vector3 startPosition;


    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float speedBullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float timeValueDestroy;
    Rigidbody rbBullet;



    [Header("Bool Actions")]
    bool isActionMove;
    bool isActionAttack;
    bool isActionRotation;


    public enum Actions
    {
        Aparition, Atack, Move, Rotate, ExitScreen
    }

    public Queue<Actions> QueueActions => queueActions;
    public string Message => message;


    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position; 
    }

    
    private void Update()
    {
        if(isActionMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                movingRight = !movingRight;
            }
        }

        if (isActionRotation)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        

    }
    private void FixedUpdate()
    {

        if (isActionAttack)
        {
            rbBullet.linearVelocity = Vector3.right * speedBullet;
        }
         
        
    }

    public void ManagerActions(Actions actions)
    {
        switch (actions)
        {
            case Actions.Atack:
                material.color = Color.red;
                message = "Attack";
                Attack();
                break;
            case Actions.Move:
                material.color = Color.blue;
                message = "Move";

                Movement();
                break;
            case Actions.Rotate:
                material.color = Color.yellow;
                message = "Rotate";
                Rotate();
                break;
            case Actions.Aparition:
                material.color = Color.gray;
                message = "Aparece";
                break;
            case Actions.ExitScreen:
                material.color = Color.green;
                message = "ExitScreen";
                ExitScreen();
                break;
        }
    }
    private void OnEnable()
    {
        MenuManager.OnChangueAction+= SetStateMoveAction;
        MenuManager.OnChangueAction+= SetStateAttackAction;
        MenuManager.OnChangueAction+= SetStateRotateAction;
    }

    private void OnDisable()
    {
        MenuManager.OnChangueAction -= SetStateMoveAction;
        MenuManager.OnChangueAction -= SetStateAttackAction;
        MenuManager.OnChangueAction -= SetStateRotateAction;
    }
    public void AddActions(int num)
    {
        Actions valueAction;
        for (int i = 0; i < num; i++)
        {
            Debug.Log("Se agrego la accion");
            valueAction = (Actions)Enum.GetValues(typeof(Actions)).GetValue(UnityEngine.Random.Range(1, (int)Enum.GetValues(typeof(Actions)).Length-1));
            Debug.Log(valueAction.ToString());

            queueActions.Enqueue(valueAction);
            Debug.Log(queueActions.Count);
        }
    }

   
    public void Movement()
    {
        SetStateMoveAction(true);
        target = startPosition + (movingRight ? Vector3.right : Vector3.left) * moveRange;
        
    }

    public void Rotate()
    {
        
       SetStateRotateAction(true);
        
    }

    public void Attack()
    {
        SetStateAttackAction(true);
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Destroy(bullet,timeValueDestroy);
            rbBullet = bullet.GetComponent<Rigidbody>();
           
        }
    }

 

    public void ExitScreen()
    {
        this.gameObject.SetActive(false);
    }

    void SetStateMoveAction(bool valueState)
    {
        isActionMove = valueState;
    }

    void SetStateAttackAction(bool valueState)
    {
        isActionAttack = valueState;
    }

    void SetStateRotateAction(bool valueState)
    {
        isActionRotation = valueState;
    }


}
