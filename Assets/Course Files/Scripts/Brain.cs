using UnityEngine;

public class Brain : MonoBehaviour
{
    int dnaLength = 2; // Num of genes.
    PopulationManager population;
    float timeAlive; // Survival time.
    float timeWalking; // How much time walked.
    DNA dna;

    [SerializeField] GameObject eyes; // Eyes of character.
    bool alive = true;
    bool seeGround = true;
    [SerializeField] float moveSpeed = 0.1f;

    public DNA GetDNA()
    {
        return dna;
    }

    public float GetTimeAlive()
    {
        return timeAlive;
    }

    public float GetTimeWalking()
    {
        return timeWalking;
    }


    // bots who fall of from platform dies.
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("dead"))
        {
            Debug.Log("dies");
            alive = false;
            timeAlive = 0;
            timeWalking = 0;

            gameObject.SetActive(false);
        }
    }

    // Initialize dna.
    public void Init()
    {
        // 0 forward
        // 1 left
        // 2 right
        dna = new(dnaLength, 2);
        timeAlive = 0;
        alive = true; // Revive dead bots if needed.
    }


    void Update()
    {
        if (!alive) { return; }

        // Visual ray for display only.
        // Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red, 10);

        seeGround = false;
        RaycastHit hit;
        // Perform a raycast from the position of the 'eyes' object, in the forward direction.
        // 'out hit': Store hit details in the 'hit' variable if the ray hits something.
        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward * 10, out hit))
        {
            // Check if the object hit by the ray has the tag 'platform'.
            if (hit.collider.gameObject.CompareTag("platform"))
            {
                seeGround = true;
            }
        }
        timeAlive = FindObjectOfType<PopulationManager>().GetElapsed();
    }

    void FixedUpdate()
    {
        if (!alive) { return; }
        
        // Read DNA
        float turn = 0; // Rotation
        float move = 0; // Movement

        if (seeGround)
        {
            if (dna.GetGene(0) == 0) { move = 1; timeWalking++; }
            else if (dna.GetGene(0) == 1) { turn = -90; }
            else if (dna.GetGene(0) == 2) { turn = 90; }
        }
        else
        {
            if (dna.GetGene(1) == 0) { move = 1; timeWalking++; }
            else if (dna.GetGene(1) == 1) { turn = -90; }
            else if (dna.GetGene(1) == 2) { turn = 90; }
        }

        transform.Translate(0, 0, move * moveSpeed);
        transform.Rotate(0, turn, 0);
    }
}