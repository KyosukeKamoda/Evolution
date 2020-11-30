using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Impulse : MonoBehaviour
{
    private float timeElapsed = 0.0f; //経過時間を計測
    public float timeOut = 0.1f; //0.1秒毎
    private int n;
    public List<float> list = new List<float> { -20.0f, -30.0f, -60.0f, -80.0f, 30.0f, 90.0f };


    // Start is called before the first frame update
    void Start()
    {
        n = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeOut)
        {
            Force();
            timeElapsed = 0.0f;
            n++;
            if (n >= 6) n = 0;
        }
    }

    void Force()
    {
        //Rigidbody rb = this.GetComponent<Rigidbody>();
        //Vector3 force = new Vector3(0.0f, 0.0f, list[n]);
        this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.0f, list[n]), ForceMode.Impulse);
    }
}
