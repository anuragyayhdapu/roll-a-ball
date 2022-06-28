using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RndForceText : MonoBehaviour
{

    static Text rfText;
    //PlayerController playerController;

    void Awake()
    {
        rfText = GetComponent<Text>();
    }

    void Start()
    {
        //playerController = new PlayerController();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rfText.text = "Random force   " + PlayerController.fx;
    }
}
