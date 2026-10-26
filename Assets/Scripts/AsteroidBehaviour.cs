using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    Border verticalScreenBoundary;

    [SerializeField]
    Border horizontalScreenBoundary;

    bool IsDying = false;
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < verticalScreenBoundary.min)
        {
            Reset();

        }
    }

    private void FixedUpdate()
    {
        if (IsDying)
        {
            transform.Rotate(0, 0, 5);
            transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x - 0.05f, 0, 1), Mathf.Clamp(transform.localScale.y - 0.05f, 0, 1), 1);
            Reset();
        }
    }
    public void DestroyingSequence()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        IsDying = true;
    }
    private void Reset()
    {
        transform.position = new Vector3(Random.Range(horizontalScreenBoundary.min, horizontalScreenBoundary.max),
                                                          verticalScreenBoundary.max, transform.position.z);

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        IsDying = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
    }
}
