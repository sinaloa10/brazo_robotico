using System.IO.Ports;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    public Transform baseArm;
    public Transform armUpper;
    public Transform armLower;
    public Transform wrist;
    public Transform clawR;
    public Transform clawL;

    private SerialPort serialPort;
    private float rotationSpeed = 50f;
    private bool portAvailable = false;
    private string portName = "COM3"; 
    private int baudRate = 9600; 

    private float baseArmRotationY = 0f;
    private float armUpperRotationX = 0f;
    private float armLowerRotationX = 0f;
    private float wristRotationX = 0f;
    private float clawAngle = 0f;

    void Start()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
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
        float deltaRotation = rotationSpeed * Time.deltaTime;

        // Movimiento horizontal de la base (270°)
        if (Input.GetKey(KeyCode.Z))
        {
            if (baseArmRotationY < 135f)
            {
                baseArm.Rotate(Vector3.up * deltaRotation);
                baseArmRotationY += deltaRotation;
                if (portAvailable) SendToSerial("U+");
            }
        }
        if (Input.GetKey(KeyCode.X))
        {
            if (baseArmRotationY > -135f)
            {
                baseArm.Rotate(Vector3.down * deltaRotation);
                baseArmRotationY -= deltaRotation;
                if (portAvailable) SendToSerial("U-");
            }
        }

        // Movimiento brazo superior (120°)
        if (Input.GetKey(KeyCode.A))
        {
            if (armUpperRotationX < 60f)
            {
                armUpper.Rotate(Vector3.forward * deltaRotation);
                armUpperRotationX += deltaRotation;
                if (portAvailable) SendToSerial("U+");
            }
        }
        if (Input.GetKey(KeyCode.Q))
        {
            if (armUpperRotationX > -60f)
            {
                armUpper.Rotate(Vector3.back * deltaRotation);
                armUpperRotationX -= deltaRotation;
                if (portAvailable) SendToSerial("U-");
            }
        }

        // Movimiento brazo inferior (180°)
        if (Input.GetKey(KeyCode.S))
        {
            if (armLowerRotationX < 10f)
            {
                armLower.Rotate(Vector3.forward * deltaRotation);
                armLowerRotationX += deltaRotation;
                if (portAvailable) SendToSerial("L+");
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (armLowerRotationX > -90f)
            {
                armLower.Rotate(Vector3.back * deltaRotation);
                armLowerRotationX -= deltaRotation;
                if (portAvailable) SendToSerial("L-");
            }
        }

        // Movimiento vertical de la muñeca (tenaza) (120°)
        if (Input.GetKey(KeyCode.D))
        {
            if (wristRotationX < 60f)
            {
                wrist.Rotate(Vector3.forward * deltaRotation);
                wristRotationX += deltaRotation;
                if (portAvailable) SendToSerial("W+");
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (wristRotationX > -60f)
            {
                wrist.Rotate(Vector3.back * deltaRotation);
                wristRotationX -= deltaRotation;
                if (portAvailable) SendToSerial("W-");
            }
        }

        // Movimiento de la garra (máx apertura de 4.5cm ≈ 30° por lado)
        if (Input.GetKey(KeyCode.F))
        {
            if (clawAngle < 15f)
            {
                clawR.Rotate(Vector3.right * deltaRotation);
                clawL.Rotate(Vector3.left * deltaRotation);
                clawAngle += deltaRotation;
                if (portAvailable) SendToSerial("C+");
            }
        }
        if (Input.GetKey(KeyCode.R))
        {
            if (clawAngle > 0f)
            {
                clawR.Rotate(Vector3.left * deltaRotation);
                clawL.Rotate(Vector3.right * deltaRotation);
                clawAngle -= deltaRotation;
                if (portAvailable) SendToSerial("C-");
            }
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
