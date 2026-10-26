using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    float horizontalForce;

    [SerializeField]
    float verticalForce;

    [SerializeField]
    Border bounds;

    [SerializeField]
    [Range(0f, 1f)]
    float decay;

    [Header("Shooting")]
    [SerializeField]
    BulletManager bulletManager;

    [SerializeField]
    GameController gameController;

    [SerializeField]
    float fireRate = 0.25f;

    [SerializeField]
    float bulletSpawnOffset = 0.5f;

    float nextFireTime;

    private Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        if (bulletManager == null)
        {
            bulletManager = FindObjectOfType<BulletManager>();
        }
    }

    void Update()
    {
        Move();
        Shoot();

        if (gameController.health == 0)
        {
            gameController.ChangeScene(3);
        }
    }

    Vector2 GetTouchDirection()
    {
        if (Touchscreen.current == null)
        {
            return Vector2.zero;
        }

        var touch = Touchscreen.current.primaryTouch;

        if (!touch.press.IsPressed())
        {
            return Vector2.zero;
        }

        Vector2 touchPosition = touch.position.ReadValue();

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(touchPosition.x, touchPosition.y, 0f)
        );

        Vector2 direction =
            (Vector2)worldPosition - rigidbody.position;

        return direction.normalized;
    }

    void Move()
    {
        Vector2 direction = GetTouchDirection();

        rigidbody.AddForce(new Vector2(
            direction.x * horizontalForce,
            direction.y * verticalForce
        ));
    }

    void Shoot()
    {
        if (Keyboard.current != null &&
         Keyboard.current.spaceKey.isPressed &&
         Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireBullet()
    {
        if (bulletManager == null)
        {
            Debug.LogError("BulletManager not found!");
            return;
        }

        GameObject bullet = bulletManager.GetBullets();

        bullet.transform.position =
            transform.position + Vector3.up * bulletSpawnOffset;

        bullet.transform.rotation = Quaternion.identity;
    }

    void CheckBounds()
    {
        Vector2 pos = rigidbody.position;

        if (pos.x < bounds.min)
        {
            pos.x = bounds.min;
        }

        if (pos.x > bounds.max)
        {
            pos.x = bounds.max;
        }

        rigidbody.position = pos;
    }

    void FixedUpdate()
    {
        rigidbody.linearVelocity *= decay;
        CheckBounds();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("I got hit");
            gameController.HealthChange(5);

            collision.GetComponent<EnemyBehaviour>().DestroyingSequence();
        }

        if (collision.CompareTag("Asteroid"))
        {
            Debug.Log("I got hit");
            gameController.HealthChange(5);

            collision.GetComponent<AsteroidBehaviour>().DestroyingSequence();
        }

    }
}
