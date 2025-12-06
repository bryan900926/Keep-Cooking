
using System.Collections;
using UnityEngine;

public class BeerDemo : MonoBehaviour
{
    [SerializeField] private FloatingEnergyBar floatingBeerBar;

    [SerializeField] private GameObject beerGameObject;

    private float currentBeer = 0f;
    private const float maxBeer = 100f;

    void Start()
    {
        beerGameObject.SetActive(false);
    }
    void Update()
    {
        currentBeer += Time.deltaTime * 20f;
        if (currentBeer > maxBeer)
        {
            beerGameObject.SetActive(true);
            currentBeer = 0f;
            StartCoroutine(HideBeerAfterDelay(2f));
        }
        floatingBeerBar.UpdateEnergy(currentBeer / maxBeer);
    }

    private IEnumerator HideBeerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        beerGameObject.SetActive(false);
    }

}
