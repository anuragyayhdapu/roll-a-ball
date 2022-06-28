using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DsplText : MonoBehaviour
{

    static Text displText;

    void Awake()
    {
        displText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        displText.text = "Displacement    " + PlayerController.px_f;
    }
}
