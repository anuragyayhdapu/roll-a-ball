/*
    Edited by Anurag , 16 July.
    Use 12 july Unity file
         5 july Server file
*/


/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package dummyplayerserver;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.Map;
import java.util.TreeMap;

/**
 *
 * @author dell
 */
public class DummyPlayerServer {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        try {
            ServerSocket serverSocket = new ServerSocket(9090);

            int count = 0;
            int x50 = 0, z50 = 0, x103 = 0, x102 = 0, z103 = 0, z102 = 0;
            int mx = 0, my = 0, mz = 0;

            TreeMap<Integer, Integer> Less_force = new TreeMap<>();
            TreeMap<Integer, Integer> More_force = new TreeMap<>();

            while (count < 10000) {
                ++count;
                System.out.println("GAME: " + count);
                Socket clientSocket = serverSocket.accept();
                DataOutputStream os = new DataOutputStream(clientSocket.getOutputStream());
                DataInputStream is = new DataInputStream(clientSocket.getInputStream());
                int loop = 0;
                while (loop < 1) {
                    int n = is.readInt();
                    int x = is.readInt();
                    int y = is.readInt();
                    int z = is.readInt();
                    loop = is.readInt();
                    //System.out.println(n + "(" + x + "," + y + "," + z + ")");

                    if (n == 50) {
                        x50 = x;
                        z50 = z;
                    }
                    if (n == 102) {
                        x102 = x;
                        z102 = z;
                    }
                    if (n == 103) {
                        x103 = x;
                        z103 = z;
                        if (x103 == x102 && z103 == z102) {
                            Less_force.put(x50, mx);
                            More_force.put(x50, mx);
                            System.out.println("Right force (.) \n");
                        }
                        if (x103 > x102 || z103 > z102) {
                            Less_force.put(x50, mx);
                            System.out.println("Less than initial force (+) \n");
                        }
                        if (x103 < x102 || z103 < z102) {
                            More_force.put(x50, mx);
                            System.out.println("More than initial force (-) \n");
                        }
                    }

                    if (loop < 1) {
                        if (n == 51) {
			    // learning goes here, 
                            // determine less and more force components from respective trees and compute mx and mz

                            Map.Entry<Integer, Integer> l = Less_force.floorEntry(x50);
                            Map.Entry<Integer, Integer> u = More_force.ceilingEntry(x50);

                            if (l == null && u == null) {
                                mx = (int) (Math.random() * 1000);
                                System.out.println("l "+l+"\nu "+u);
                               // System.out.println("<x50,mx> <"+x50+","+mx+">");
                            } else if (l == null) {
                                mx = u.getValue() / 2;
                                System.out.println("l "+l+"\nu "+u+"\nmx "+mx);
                            } else if (u == null) {
                                mx = l.getValue() * 2;
                                System.out.println("l "+l+"\nu "+u+"\nmx "+mx);
                            } else {
                                mx = (l.getValue() + u.getValue()) / 2;
                                System.out.println("l "+l+"\nu "+u+"\nmx "+mx);
                            }

                            //mx = (int) (Math.random() * 1000);
                            //my = 0;
                            //mz = (int) (Math.random() * 1000);
                            //System.out.println("(" + mx + "," + my + "," + mz + ")");                
                            os.writeInt(mx);
                            os.writeInt(my);
                            os.writeInt(mz);
                        }
                    }
                }
                clientSocket.close();
            }
        } catch (IOException ex) {
            System.out.println("Socket Exception. " + ex);
            System.exit(-1);
        }
    }
}
