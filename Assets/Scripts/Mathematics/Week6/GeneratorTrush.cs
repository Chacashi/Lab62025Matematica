using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GeneratorTrush : MonoBehaviour
{
    [SerializeField] private GameObject trushPrefab;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private float delay;
    [SerializeField] private int maxNumberTrush;
    [SerializeField] private int maxNumberMeteor;
    [SerializeField] private Transform earth;
    [SerializeField] private float speedMeteor;
    private int numberOfTrush;
    private int numberOfMeteor;
    private int countTrush;
    private int countMeteor;
    Vector3 randomPosition;
    private void Start()
    {
        countTrush = 0;
        countMeteor = 0;
        StartCoroutine(GenerateTrush());
        StartCoroutine(GenerateMeteor());
    }

    private void Update()
    {
        if(countTrush >= maxNumberTrush)
        {
            StopCoroutine(GenerateTrush());
            
        }

        if(countMeteor >= maxNumberMeteor)
        {
            StopCoroutine(GenerateMeteor());
        }
    }

    IEnumerator GenerateTrush()
    {
        while (true)
        {
            numberOfTrush = Random.Range(1, 2);
            countTrush += numberOfTrush;
            for (int i = 0; i < numberOfTrush; i++)
            {
                randomPosition = new Vector3(Random.Range(-6.8f, 7f), Random.Range(-1.8f, 5f), 20.3f);
              GameObject trush =  Instantiate(trushPrefab, randomPosition, Quaternion.identity);
                trush.transform.SetParent(earth);
            }
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator GenerateMeteor()
    {
        while (true)
        {
            numberOfMeteor = Random.Range(1, 2);
            countMeteor += numberOfMeteor;
            for (int i = 0; i < numberOfMeteor; i++)
            {
                randomPosition = new Vector3(Random.Range(-6.8f, 7f), Random.Range(-1.8f, 5f), 40f);
                GameObject meteor = Instantiate(meteorPrefab, randomPosition, Quaternion.identity);
                Rigidbody rbMeteor = meteor.GetComponent<Rigidbody>();
                rbMeteor.linearVelocity = new Vector3(rbMeteor.linearVelocity.x, rbMeteor.linearVelocity.y, -speedMeteor);
                Destroy(meteor, 10f);
            }
            yield return new WaitForSeconds(delay);
        }
    }


    private void OnEnable()
    {
        PlaneController.OnPlayerTakeTrush += AddCountTrush;
        PlaneController.OnPlayerTakeMeteor += AddCountMeteor;
    }

    private void OnDisable()
    {
        PlaneController.OnPlayerTakeTrush -= AddCountTrush;
        PlaneController.OnPlayerTakeMeteor -= AddCountMeteor;
    }

    void AddCountTrush()
    {
        countTrush--;
        if (countTrush < maxNumberTrush)
        {
            StartCoroutine(GenerateTrush());
        }
    }


    void AddCountMeteor()
    {
        countMeteor--;
        if (countMeteor < maxNumberMeteor)
        {
            StartCoroutine(GenerateMeteor());
        }
    }
}

