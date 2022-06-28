/* Edited by Anurag 31 July 2015 
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
 *  4.  
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

    private int lastRet;
    private int saveRet;

    private int frameCount;
    private Vector3 direction;
    private Vector3 f1;    // ramdom force applied at beginning
    //double f2Magnitude;    // magnitude of force applied at 51st frame
    [HideInInspector]
    public static float px_f;
    private int px;
    private int py;
    private int pz;
    private Vector3 p3;     //  position of ball
    private bool reload;
    [HideInInspector]
    public static float mx; // sorry
    [HideInInspector]
    public static float fx;
    //private Vector3 p3Old;
    //private long frameCount = 1;
    float x50, x100, x101;

    void Start()
    {
        reload = false;
        Time.timeScale = 10.0F;                         // It makes time fly, 
        frameCount = 1;
        mx = 0.0f;
        px_f = 0.0f;

        p3 = new Vector3(0f, 0f, 0f);                  // initialize p3 

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
        rb.AddForce(f1 * speed * 4);
        lastRet = 0;
        saveRet = -1; // apply random force
    }

    int Send()
    {
        lastRet++;
        p3 = rb.position;                                               // set p3 to position of ball 
        px = (int)(1000.0 * p3.x);                                      // multiply componts x, y and z by 1000 for sending to client
        if (frameCount == 50) { px_f = px / 1000.0f; }
        py = (int)(1000.0 * p3.y);
        pz = (int)(1000.0 * p3.z);
        //writer.Write(IPAddress.HostToNetworkOrder(Time.frameCount));    // writing frame no. to the client
        writer.Write(IPAddress.HostToNetworkOrder(frameCount));
        writer.Write(IPAddress.HostToNetworkOrder(px));                 // writing x, y and z component of position to the client
        writer.Write(IPAddress.HostToNetworkOrder(py));
        writer.Write(IPAddress.HostToNetworkOrder(pz));

        // if ball falls, py equals < 0,
        if (py < 0 || reload == true || frameCount > 103)       // quickely resetting the level, learning is done at this point
        {
            writer.Write(IPAddress.HostToNetworkOrder(1));     // write loop = 1
            writer.Close();
            reader.Close();
            client.Client.Close();                             // close the connection
            Application.LoadLevel(0);                          // reload level
            //frameCount = Time.frameCount;                    // resetting the frame count
            //Debug.Log ("Load Level Called");
            saveRet = lastRet;
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

        //if (Time.frameCount == frameCount)    // required for multiple step learning
        //    return;
        //p3Old = p3;

        //Debug.Log ("lastRet "+lastRet);
        if (saveRet == lastRet)
            return;
        int ret = Send();   // confusing code

        if (ret < 0)
            return;

        if (frameCount < 50)
        {

        }


        if ((frameCount) == 50)
        {
            x50 = p3.x;
            //direction = (p3Old - p3).normalized;
            direction = -f1.normalized;                                             // opposite direction of initial force
            mx = IPAddress.NetworkToHostOrder(reader.ReadInt32()) / 1000.0f;        // read x, y and z force magnitude from server
            float my = IPAddress.NetworkToHostOrder(reader.ReadInt32()) / 1000.0f;
            float mz = IPAddress.NetworkToHostOrder(reader.ReadInt32()) / 1000.0f;
            my = mz = 0.0f;                                                         // currently y and z magnitude are zero

            //f2Magnitude = Math.Sqrt((mx * mx) + (my * my) + (mz * mz));             // compute total magnitude of force, it introduced error in square rooting, use mx in rb.addforce
            //Debug.Log(" mx : "+mx+" f2magnitude"+f2Magnitude );
            rb.AddForce(direction * mx * speed);                                 // apply force with above magnitude in opposite direction
            // Debug.Log("frame: " + (frameCount));                                    // Log framecount in Unity console

            // Gizmo.plus(p3.x, mx); // draw when francount == 103
        }

        Plotting();
    }

    public void ReloadLevel()
    {
        reload = true;
    }

    void Plotting()
    {
        if (frameCount == 100)
        {
            x100 = p3.x;
        }
        if (frameCount == 101)
        {
            /////////////////////
            float mul_x = 1;
            float mul_z = 1;
            ////////////////////

            x101 = p3.x;
            // log
            //x50 = (float)Math.Log10(x50);
            //mx = (float)Math.Log10(mx);
            if (x101 > x100)
            {
                //Debug.Log("Less Force,plus,  x101 :" + x101 + " x100 :" + x100);
                //Gizmo.plus(x50 * mul_x, mx * mul_z);    // * for adjusting in graph, lower for viewing in small window
            }
            if (x101 < x100)
            {

                //Debug.Log("More force,minus, x101 :" + x101 + " x100 :" + x100);
                //Gizmo.minus(x50 * mul_x, mx * mul_z);
            }
            if (x101 == x100)
            {
                System.IO.StreamWriter w = System.IO.File.AppendText("log.txt");
                w.WriteLine("" + x50 + " " + mx);
                w.Close();
                Gizmo.dash(x50 * mul_x, mx * mul_z);	// red plus, exact force

            }
        }
    }



}
