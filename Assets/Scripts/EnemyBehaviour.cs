using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    Border speedRange;

    float speed;

    [SerializeField]
    Border verticalScreenBoundary;

    [SerializeField]
    Border horizontalScreenBoundary;

    BulletManager bulletManager;
    GameController gameController;

    bool IsDying = false;

    void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();
        gameController = FindObjectOfType<GameController>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.down * speed * Time.deltaTime);

        float xPos = Mathf.PingPong(Time.time * speed, horizontalScreenBoundary.max - horizontalScreenBoundary.min) + horizontalScreenBoundary.min;
        transform.position = new Vector3(xPos, transform.position.y - speed * Time.deltaTime);

        if (transform.position.y < verticalScreenBoundary.min)
        {
            Reset();

        }

        if (gameController.score == 100)
        {
            gameController.ChangeScene(3);
        }

    }

    private void FixedUpdate()
    {
        if (IsDying)
        {
            transform.Rotate(0, 0, 5);
            transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x - 0.05f, 0, 1), Mathf.Clamp(transform.localScale.y - 0.05f, 0, 1), 1);
        }
    }

    public void DestroyingSequence()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        IsDying = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            DestroyingSequence();
            bulletManager.ReturnBullets(collision.gameObject);
            gameController.ChangeScore(5);
        }
    }
    private void Reset()
    {
        transform.position = new Vector3(Random.Range(horizontalScreenBoundary.min, horizontalScreenBoundary.max),
                                                          verticalScreenBoundary.max, transform.position.z);

        speed = Random.Range(speedRange.min, speedRange.max);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        IsDying = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
    }
}
