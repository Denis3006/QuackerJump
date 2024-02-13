using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float gravity;
    public float jumpVelocity;
    public AudioClip jumpSound;
    public AudioClip gameOverSound;
    public AudioClip newHighscoreSound;
    public float leftBound, rightBound;
    AudioSource audioSource;
    bool facingRight = true;
    float playerWidth;
    SpriteRenderer spriteRenderer;
    Vector2 velocity = new(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        movementSpeed *= PlayerPrefs.GetFloat("sensitivity", 0.7f);
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume", 0.5f);
        ;
        if (SystemInfo.supportsGyroscope) {
            Input.gyro.enabled = true;
        }

        playerWidth = GetComponent<BoxCollider2D>().size.x * transform.localScale.x;
        var cameraBounds = GameObject.Find("Main Camera").GetComponent<CameraController>().OrthographicBounds();
        leftBound = cameraBounds.min.x;
        rightBound = cameraBounds.max.x;
    }

    // Update is called once per frame
    void Update()
    {
        var xVelocity = SystemInfo.supportsGyroscope
            ? -Input.gyro.rotationRate.z * movementSpeed
            : Input.GetAxis("Horizontal") * movementSpeed;

        velocity = new Vector2(xVelocity, velocity.y);

        if (velocity.y <= 0) {
            Vector2 rayOrigin = transform.position - new Vector3(0, playerWidth / 2, 0);
            var hit = Physics2D.BoxCast(rayOrigin, new Vector2(playerWidth, 0.1f), 0, -transform.up, 0.2f);
            if (hit.collider != null) {
                var jumpVel = jumpVelocity;
                if (hit.collider.CompareTag(PlatformType.Fragile.ToString())) {
                    hit.collider.gameObject.SetActive(false);
                }
                else if (hit.collider.CompareTag(PlatformType.Slowing.ToString())) {
                    jumpVel /= Platform.SlowingFactor;
                }

                Jump(jumpVel);
            }
        }

        facingRight = (facingRight && Mathf.Abs(velocity.x) < 0.1) || velocity.x > 0;
        spriteRenderer.flipX = !facingRight;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void Jump(float jumpVel)
    {
        velocity.y = jumpVel;
        audioSource.PlayOneShot(jumpSound, 0.75f);
    }

    void HandleMovement()
    {
        Vector2 position = transform.position;
        position += velocity * Time.fixedDeltaTime;
        velocity.y -= gravity * Time.fixedDeltaTime;
        position.x = Mathf.Min(Mathf.Max(position.x, leftBound + playerWidth / 2),
            rightBound - playerWidth / 2);
        transform.position = position;
    }

    public float JumpHeight(float jumpVel)
    {
        var t = jumpVel / gravity;
        return jumpVel * t - 0.5f * (gravity * Mathf.Pow(t, 2));
    }

    public float JumpDistance(float jumpVel)
    {
        var t = jumpVel / gravity;
        return movementSpeed * t;
    }
}