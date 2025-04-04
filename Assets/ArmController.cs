using System.IO.Ports;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    public Transform armUpper;
    public Transform armLower;
    public Transform wrist;
    public Transform clawR;
    public Transform clawL;

    private SerialPort serialPort;
    private float rotationSpeed = 50f;
    private bool portAvailable = false;

    void Start()
    {
        try
        {
            serialPort = new SerialPort("COM5", 9600); // Ajusta el puerto según necesites
            serialPort.Open();
            portAvailable = true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("No se pudo abrir el puerto COM: " + e.Message);
            portAvailable = false;
        }
    }

    void Update()
    {
        // Movimiento del brazo superior
        if (Input.GetKey(KeyCode.Q))
        {
            armUpper.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            SendToSerial("U+");
        }
        if (Input.GetKey(KeyCode.A))
        {
            armUpper.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
            SendToSerial("U-");
        }

        // Movimiento del brazo superior (horizontal)
        if (Input.GetKey(KeyCode.Z))
        {
            armUpper.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            SendToSerial("U+");
        }
        if (Input.GetKey(KeyCode.X))
        {
            armUpper.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
            SendToSerial("U-");
        }

        // Movimiento del brazo inferior
        if (Input.GetKey(KeyCode.W))
        {
            armLower.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            SendToSerial("L+");
        }
        if (Input.GetKey(KeyCode.S))
        {
            armLower.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
            SendToSerial("L-");
        }

        // Movimiento de la muñeca
        if (Input.GetKey(KeyCode.E))
        {
            wrist.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            SendToSerial("W+");
        }
        if (Input.GetKey(KeyCode.D))
        {
            wrist.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
            SendToSerial("W-");
        }

        // Movimiento de las garras
        if (Input.GetKey(KeyCode.R))
        {
            clawR.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            clawL.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
            SendToSerial("C+");
        }
        if (Input.GetKey(KeyCode.F))
        {
            clawR.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
            clawL.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            SendToSerial("C-");
        }
    }

    void SendToSerial(string command)
    {
        if (portAvailable && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                serialPort.Write(command);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error al enviar al puerto COM: " + e.Message);
                portAvailable = false;
            }
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
