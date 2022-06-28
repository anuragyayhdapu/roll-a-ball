using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OppForceText : MonoBehaviour
{

    static Text ofText;
    //PlayerController playerController;

    void Awake()
    {
        ofText = GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
        // playerController = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ofText.text = "Opposing force " + PlayerController.mx;
    }
}
