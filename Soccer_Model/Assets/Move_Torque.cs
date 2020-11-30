using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Torque : MonoBehaviour
{
    private float timeElapsed = 0.0f; //経過時間を計測
    public float timeOut = 0.1f; //0.1秒毎
    public float[] array = { -11.0f, -34.0f, -32.0f, 43.0f, 17.0f, 41.0f};
    private int n;

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
        Rigidbody rb = this.GetComponent<Rigidbody>();
        Vector3 force = new Vector3(0.0f, 0.0f, array[n]);
        rb.AddForce(force, ForceMode.Impulse);
    }
}
