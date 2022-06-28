/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
//package dummyplayerserver;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

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
            while (count < 100) {
                ++count;
                System.out.println("GAME: " + count);
                Socket clientSocket = serverSocket.accept();
                DataOutputStream os = new DataOutputStream(clientSocket.getOutputStream());
                DataInputStream is = new DataInputStream(clientSocket.getInputStream());
                int loop = 0;
                while (loop < 1) {
                    int n = is.readInt();
                    float x = is.readInt() / 1000.0f;
                    float y = is.readInt() / 1000.0f;
                    float z = is.readInt() / 1000.0f;
                    loop = is.readInt();
                    System.out.println(n + "(" + x + "," + y + "," + z + ")");

                    if (loop < 1) {
		        if (n == 50) {
                            int mx = (int) (Math.random() * 1000) - 500;
                       	    int my = 0;
                            int mz = (int) (Math.random() * 1000) - 500;

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
