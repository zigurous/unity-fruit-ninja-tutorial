using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;
    private ParticleSystem particleEffect;

    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        particleEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void Slice(Vector3 direction, Vector3 position)
    {
        FindObjectOfType<GameManager>().IncreaseScore(1);

        // Disable the whole fruit
        fruitCollider.enabled = false;
        whole.SetActive(false);

        // Enable the sliced fruit
        sliced.SetActive(true);
        particleEffect.Play();

        // Rotate based on the slice angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.eulerAngles = new Vector3(0f, 0f, angle);

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();

        // Add a force to each slice based on the blade direction
        foreach (Rigidbody slice in slices)
        {
            slice.velocity = fruitRigidbody.velocity;
            slice.AddForceAtPosition(direction * 5f, position, ForceMode.Impulse);
        }

        Destroy(sliced, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position);
        }
    }

}
