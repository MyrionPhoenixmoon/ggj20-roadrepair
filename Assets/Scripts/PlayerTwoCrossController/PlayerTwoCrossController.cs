using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTwoCrossController : MonoBehaviour
{

    public Image buttonUpOn;
    public Image buttonUpOff;

    public Image buttonRightOn;
    public Image buttonRightOff;

    public Image buttonDownOn;
    public Image buttonDownOff;

    public Image buttonLeftOn;
    public Image buttonLeftOff;

    // Start is called before the first frame update
    void Start()
    {
        this.buttonUpOn.gameObject.SetActive(false);
        this.buttonRightOn.gameObject.SetActive(false);
        this.buttonDownOn.gameObject.SetActive(false);
        this.buttonLeftOn.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("P2RepairUp"))
        {
            this.buttonUpOn.gameObject.SetActive(true);
        } else
        {
            this.buttonUpOn.gameObject.SetActive(false);
        }

        if (Input.GetButton("P2RepairRight"))
        {
            this.buttonRightOn.gameObject.SetActive(true);
        }
        else
        {
            this.buttonRightOn.gameObject.SetActive(false);
        }

        if (Input.GetButton("P2RepairDown"))
        {
            this.buttonDownOn.gameObject.SetActive(true);
        }
        else
        {
            this.buttonDownOn.gameObject.SetActive(false);
        }

        if (Input.GetButton("P2RepairLeft"))
        {
            this.buttonLeftOn.gameObject.SetActive(true);
        }
        else
        {
            this.buttonLeftOn.gameObject.SetActive(false);
        }

    }
}
