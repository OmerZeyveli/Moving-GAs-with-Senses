using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBrain : MonoBehaviour
{
    int dnaLength = 2; // Num of genes.
    PopulationManager population;
    DNA dna;

    float distFlag;
    [SerializeField] Vector3 flagPos; // Flag position.
    [SerializeField] GameObject eyes; // Eyes of character.

    bool alive = true;
    bool seeWall = true;
    [SerializeField] float moveSpeed = 0.1f;

    public DNA GetDNA()
    {
        return dna;
    }

    public float GetDistFlag()
    {
        return distFlag;
    }


    // bots who fall of from platform dies.
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("dead"))
        {
            Debug.Log("dies");
            alive = false;

            distFlag = 0;
            gameObject.SetActive(false);
        }
    }

    // Initialize dna.
    public void Init()
    {
        // 360 in degrees.
        dna = new(dnaLength, 360);
        alive = true; // Revive dead bots if needed.
    }


    void Update()
    {
        if (!alive) { return; }

        seeWall = false;
        RaycastHit hit;
        // Perform a raycast from the position of the 'eyes' object, in the forward direction.
        // 'out hit': Store hit details in the 'hit' variable if the ray hits something.
        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward * 0.5f, out hit))
        {
            // Check if the object hit by the ray has the tag 'platform'.
            if (hit.collider.gameObject.CompareTag("wall"))
            {
                seeWall = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (!alive) { return; }

        // Read DNA
        float turn = 0; // Rotation
        float move = dna.GetGene(0); // Movement

        if (seeWall)
        {
            turn = dna.GetGene(1);
        }

        transform.Translate(0, 0, move * moveSpeed);
        transform.Rotate(0, turn, 0);
        distFlag = Vector3.Distance(transform.position, flagPos);

        transform.position.Set(transform.position.x, 0, transform.position.z);
        Quaternion rotationReset = Quaternion.Euler(0, transform.rotation.y + turn, 0);
        transform.rotation = rotationReset;
    }
}