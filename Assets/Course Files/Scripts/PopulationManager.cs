using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class PopulationManager : MonoBehaviour
{
    [SerializeField] GameObject botPrefab;
    [SerializeField] int populationSize = 50;
    List<GameObject> population = new();

    float elapsed; // Time
    [SerializeField] float trialTime = 5;
    int generation = 1;

    [SerializeField] int walkWeight = 1;
    [SerializeField] int aliveWeight = 1;

    // UI stuff.
    [SerializeField] TMP_Text time_text;
    [SerializeField] TMP_Text generation_text;

    public float GetElapsed()
    {
        return elapsed;
    }


    // Instantiate all population of bots.
    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            // Some bots start location will differ from others. It is for clarity of visual.
            int x_rand = Random.Range(-2, 2);
            int z_rand = Random.Range(-2, 2);
            Vector3 startingPos = new(transform.position.x + x_rand, transform.position.y, transform.position.z + z_rand);

            // Instantiate and initialize the bot.
            GameObject bot = Instantiate(botPrefab, startingPos, transform.rotation);
            bot.GetComponent<Brain>().Init();
            population.Add(bot);
        }
    }

    // Calculate time.
    void Update()
    {
        elapsed += Time.deltaTime;
        // New generation once in every trialTime.
        if (elapsed > trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }

        // Update UI.
        time_text.SetText("Time: " + elapsed);
        generation_text.SetText("Generation: " + generation);
    }

    // Breed new generation from upper half of bots (ordered by survival time).
    void BreedNewPopulation()
    {
        // Sort population by how far bot runned. (Old Test)
        List<GameObject> sortedList =
                population.OrderBy(o =>
                    o.GetComponent<Brain>().GetTimeWalking() * walkWeight
                    + o.GetComponent<Brain>().GetTimeAlive() * aliveWeight).ToList();

        // Sort population by survival time.
        //List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().GetTimeAlive()).ToList();
        population.Clear();

        // Populate upper half of sorted list.
        for (int i = (int)(sortedList.Count / 2f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        // Destroy older population.
        foreach (GameObject bot in sortedList)
        {
            Destroy(bot);
        }
        generation++;
    }

    // Combine brains of 2 parents in a new bot.
    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        // Some bots start location will differ from others. It is for clarity of visual.
        int x_rand = Random.Range(-2, 2);
        int z_rand = Random.Range(-2, 2);
        Vector3 startingPos = new(transform.position.x + x_rand, transform.position.y, transform.position.z + z_rand);

        // Instantiate offspring bot and get all 3 brains (parent1, parent2 and offspring).
        GameObject offspring = Instantiate(botPrefab, startingPos, transform.rotation);
        Brain brain1 = parent1.GetComponent<Brain>();
        Brain brain2 = parent2.GetComponent<Brain>();
        Brain brain3 = offspring.GetComponent<Brain>();

        if (Random.Range(1, 100) == 1) // Mutate %1.
        {
            brain3.Init();
            brain3.GetDNA().Mutate();
        }
        else // Combine brains of parents.
        {
            brain3.Init();
            brain3.GetDNA().Combine(brain1.GetDNA(), brain2.GetDNA());
        }
        return offspring;
    }
}