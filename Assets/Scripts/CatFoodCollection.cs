using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatFoodCollection : MonoBehaviour
{
   
    private int CatFood = 0;

    public TextMeshProUGUI catfoodText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "CatFood")
        {
            CatFood++;
            catfoodText.text = "CatFood: " + CatFood.ToString();
            Debug.Log(CatFood);
            Destroy(other.gameObject);
        }
    }
}
