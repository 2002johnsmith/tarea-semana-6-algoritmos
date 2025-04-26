using UnityEngine;
using System;
using System.Collections;

public class Enemy : MonoBehaviour
{
    Material material;
    string message;
    Queue<Actions> queueActions = new Queue<Actions>();
    Rigidbody rb;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint; // el punto desde donde dispara, puedes usar la posición del enemigo si no quieres usar un transform

    // Variables para el movimiento
    private bool movingRight = true;
    private float moveSpeed = 2f;
    private float moveRange = 3f;
    private Vector3 startPosition;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position; // Guardamos la posición original
    }

    public enum Actions
    {
        Aparition,Atack, Move, Rotate,  ExitScreen
    }

    public Queue<Actions> QueueActions => queueActions;
    public string Message => message;

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
                break;
        }
    }

    public void AddActions(int num)
    {
        Actions valueAction;
        for (int i = 0; i < num; i++)
        {
            Debug.Log("Se agrego la accion");
            valueAction = (Actions)Enum.GetValues(typeof(Actions)).GetValue(UnityEngine.Random.Range(1, (int)Enum.GetValues(typeof(Actions)).Length));
            Debug.Log(valueAction.ToString());

            queueActions.Enqueue(valueAction);
            Debug.Log(queueActions.Count);
        }
    }

    // Movimiento: Se mueve de izquierda a derecha
    public void Movement()
    {
        Vector3 target = startPosition + (movingRight ? Vector3.right : Vector3.left) * moveRange;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);

        // Cambiar de dirección cuando llega al destino
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            movingRight = !movingRight; // Cambia de dirección
        }
    }

    public void Rotate()
    {
        float rotationSpeed = 90f; // grados por segundo
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    public void Attack()
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Si luego le das movimiento en su propio script, no necesitas más aquí
            // Pero si quieres que ya salga con una velocidad:
            Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
            if (rbBullet != null)
            {
                float force = 10f;
                rbBullet.linearVelocity = Vector3.right * force;
            }
        }
    }
}
