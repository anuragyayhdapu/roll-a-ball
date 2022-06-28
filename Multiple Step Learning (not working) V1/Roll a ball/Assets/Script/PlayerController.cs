/* Edited by Anurag 18 July 2015 
 * 
 * To Do
 *   1. Dispaly parameters in Text Box: 
 *      - initial force (done)
 *      - opposing force (done)
 *   2. Display Graph 
 *      - displacement vs force (learning graph, with - and +) */
/*<--Next Task /* (done)
*   3. Everything working perfectly now, (with random force)
*      - one step learning (done)
*      - adjust Application.loadlevel (done)
*   4. 
*      - either error in code (was in code, removed)
*      - or error in logic (probably not )
 *  5.  
 *     - add multiple step learning
*/


using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;

public class PlayerController : MonoBehaviour
{

    public float speed = 1;
    private Rigidbody rb;
    private System.IO.BinaryWriter writer;
    private System.IO.BinaryReader reader;
    private TcpClient client;
    private NetworkStream stream;

    private int frameCount;
    private Vector3 direction;
    private Vector3 f1;    // ramdom force applied at beginning
    double f2Magnitude;    // magnitude of force applied at 51st frame

    private int px;
    private int py;
    private int pz;
    private Vector3 p3;     //  position of ball
    private Vector3 p3Old;
    private bool reload;

    [HideInInspector]
    public static float mx; // sorry
    [HideInInspector]
    public static float fx;
    [HideInInspector]
    public static float px_f;
    //private Vector3 p3Old;
    //private long frameCount = 1;
    float x50, x102, x103;

    void Start()
    {
        reload = false;
        Time.timeScale = 1.0F;                         // It makes time fly
        frameCount = 0;
        mx = 0.0f;
        px_f = 0.0f;

        p3 = new Vector3(0f, 0f, 0f);                  // initialize p3 
        p3Old = new Vector3(0f, 0f, 0f);

        rb = GetComponent<Rigidbody>();                // get the rigidbody component
        client = new TcpClient("127.0.0.1", 9090);     // Create Socket, ip address 127.0.0.1 and port 9090 (for local host)
        stream = client.GetStream();
        writer = new System.IO.BinaryWriter(stream);
        reader = new System.IO.BinaryReader(stream);

        // generated a random force
        System.Random random = new System.Random();     // for generating random no.
        fx = (float)random.NextDouble();              // random x component
        f1.x = fx;
        //Debug.Log("f1.x " + f1.x);
        f1.z = 0.0f;                                    // z component 0, 
        f1.y = 0.0f;                                    // y component 0,
        rb.AddForce(f1 * speed);                        // apply random force
    }

    int Send()
    {
        p3 = rb.position;                                               // set p3 to position of ball 
        if (frameCount == 50) { px_f = p3.x; }
        px = (int)(1000.0 * p3.x);                                      // multiply componts x, y and z by 1000 for sending to client
        py = (int)(1000.0 * p3.y);
        pz = (int)(1000.0 * p3.z);
        //writer.Write(IPAddress.HostToNetworkOrder(Time.frameCount));    // writing frame no. to the client
        writer.Write(IPAddress.HostToNetworkOrder(frameCount));
        writer.Write(IPAddress.HostToNetworkOrder(px));                 // writing x, y and z component of position to the client
        writer.Write(IPAddress.HostToNetworkOrder(py));
        writer.Write(IPAddress.HostToNetworkOrder(pz));

        // if ball falls, py equals < 0,
        if (py < 0 || reload == true /*|| frameCount > 103*/)
        {
            writer.Write(IPAddress.HostToNetworkOrder(1));     // write loop = 1
            client.Client.Close();                             // close the connection
            Application.LoadLevel(0);                          // reload level
            //frameCount = Time.frameCount;                    // resetting the frame count

            return -1;
        }

        // else, everything is all right,
        else
        {
            writer.Write(IPAddress.HostToNetworkOrder(0));    // write loop = 0
            return 0;
        }
    }

    void FixedUpdate()
    {
        frameCount = frameCount + 1;
        if (frameCount == 1)
            return;

        p3Old = p3;

        //if (Time.frameCount == frameCount)    // required for multiple step learning
        //    return;
        //p3Old = p3;


        if ((frameCount) % 50 == 0)
        {
            Debug.Log("framecont " + frameCount);
            int ret = Send();   // confusing code
            if (ret < 0)
                return;

            //x50 = p3.x;
            direction = (p3Old - p3).normalized;
            //direction = -f1.normalized;                                             // opposite direction of initial force
            mx = IPAddress.NetworkToHostOrder(reader.ReadInt32()) / 1000.0f;        // read x, y and z force magnitude from server
            float my = IPAddress.NetworkToHostOrder(reader.ReadInt32()) / 1000.0f;
            float mz = IPAddress.NetworkToHostOrder(reader.ReadInt32()) / 1000.0f;
            my = mz = 0.0f;                                                         // currently y and z magnitude are zero

            f2Magnitude = Math.Sqrt((mx * mx) + (my * my) + (mz * mz));             // compute total magnitude of force
            rb.AddForce(direction * (float)f2Magnitude * speed);                    // apply force with above magnitude in opposite direction
            // Debug.Log("frame: " + (frameCount));                                    // Log framecount in Unity console

            // Gizmo.plus(p3.x, mx); // draw when francount == 103
        }

        
        // Plotting();
    }

    public void ReloadLevel()
    {
        reload = true;
    }

    void Plotting()
    {
        if (frameCount == 102)
        {
            x102 = p3.x;
        }
        if (frameCount == 103)
        {
            x103 = p3.x;
            if (x103 > x102)
            {
                //Debug.Log("Less Force,plus,  x103 :" + x103 + " x102 :" + x102);
                Gizmo.plus(x50 * 10, mx * 10);    // * for adjusting in graph
            }
            if (x103 < x102)
            {
               // Debug.Log("More force,minus, x103 :" + x103 + " x102 :" + x102);
                Gizmo.minus(x50 * 10, mx * 10);
            }
            if (x103 == x102) { /* print dot */ }
        }
    }

}
