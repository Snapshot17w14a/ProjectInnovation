using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    private Image image;
    [SerializeField] private Vector2 initialVelocity;
    [SerializeField] private float acceleration;

    private Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        velocity = initialVelocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(velocity.x, velocity.y) * Time.fixedDeltaTime;
        velocity.y -= acceleration * Time.fixedDeltaTime;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
