using UnityEngine;
using UnityEngine.Events;
using System.IO.Ports;
using System;

[System.Serializable]
public class IntEvent : UnityEvent<int> {}

public class TemplateArduinoAutoWriter : MonoBehaviour
{
    [Header("Verbindings Opties")]
    [Tooltip("Vink dit aan om Unity zelf de Arduino te laten zoeken (negeert Bluetooth).")]
    public bool automatischVerbinden = true;

    [Tooltip("Windows: vul 'COM3' in. Mac: vul '/dev/cu.usbmodem1234' in.")]
    public string handmatigePoort = "COM3";

    [Header("Digitale Input 1 (Pin 2)")]
    public UnityEvent onDigital1Aan;
    public UnityEvent onDigital1Uit;

    [Header("Digitale Input 2 (Pin 3)")]
    public UnityEvent onDigital2Aan;
    public UnityEvent onDigital2Uit;

    [Header("Digitale Input 3 (Pin 4)")]
    public UnityEvent onDigital3Aan;
    public UnityEvent onDigital3Uit;

    [Header("Analoge Input 0 (Pin A0)")]
    [Tooltip("Vink dit aan om de inkomende waarde van A0 te printen in de console.")]
    public bool printDebugA0 = false;
    [Tooltip("Kies hier het omslagpunt (0-1023). Standaard is 512 (precies het midden).")]
    public int analog0Drempelwaarde = 512;
    [Tooltip("Triggert eenmalig zodra de sensorwaarde BOVEN de drempelwaarde komt.")]
    public UnityEvent onAnalog0BovenDrempel;
    [Tooltip("Triggert eenmalig zodra de sensorwaarde ONDER de drempelwaarde zakt.")]
    public UnityEvent onAnalog0OnderDrempel;
    [Tooltip("Geeft altijd de ruwe, glijdende waarde (0-1023) door.")]
    public IntEvent onAnalog0Changed;

    [Header("Analoge Input 1 (Pin A1)")]
    [Tooltip("Vink dit aan om de inkomende waarde van A1 te printen in de console.")]
    public bool printDebugA1 = false;
    [Tooltip("Kies hier het omslagpunt (0-1023). Standaard is 512 (precies het midden).")]
    public int analog1Drempelwaarde = 512;
    [Tooltip("Triggert eenmalig zodra de sensorwaarde BOVEN de drempelwaarde komt.")]
    public UnityEvent onAnalog1BovenDrempel;
    [Tooltip("Triggert eenmalig zodra de sensorwaarde ONDER de drempelwaarde zakt.")]
    public UnityEvent onAnalog1OnderDrempel;
    [Tooltip("Geeft altijd de ruwe, glijdende waarde (0-1023) door.")]
    public IntEvent onAnalog1Changed;

    private SerialPort serialPort;
    
    private bool analog0WasBoven = false;
    private bool analog1WasBoven = false;

    void Start()
    {
        if (automatischVerbinden) AutoConnectToArduino();
        else ConnectManually(handmatigePoort);
    }

    void ConnectManually(string port)
    {
        try {
            serialPort = new SerialPort(port, 9600);
            serialPort.ReadTimeout = 20; 
            serialPort.Open();
            Debug.Log("<color=green><b>SUCCES:</b></color> Handmatig verbonden met Arduino op poort: " + port);
        } catch (Exception e) { 
            Debug.LogError("<color=red><b>FOUT:</b></color> Kon niet verbinden. Fout: " + e.Message); 
        }
    }

    void AutoConnectToArduino()
    {
        string[] ports = SerialPort.GetPortNames();
        foreach (string port in ports)
        {
            if (!port.ToLower().StartsWith("com") && !port.ToLower().Contains("usb")) continue;
            try {
                serialPort = new SerialPort(port, 9600);
                serialPort.ReadTimeout = 20;
                serialPort.Open();
                Debug.Log("<color=green><b>SUCCES:</b></color> Automatisch verbonden met Arduino op poort: " + port);
                return; 
            } catch {
                if (serialPort != null && serialPort.IsOpen) serialPort.Close();
            }
        }
        Debug.LogError("<color=red><b>FOUT:</b></color> Geen Arduino gevonden via Auto-Connect. Zit de USB-kabel er goed in?");
    }

    void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try {
                while (serialPort.BytesToRead > 0)
                {
                    string message = serialPort.ReadLine().Trim();
                    ParseArduinoMessage(message);
                }
            } catch (TimeoutException) { }
            catch (Exception e) { Debug.LogWarning("Leesfout: " + e.Message); }
        }
    }

    void ParseArduinoMessage(string message)
    {
        if (message.StartsWith("DIG1:")) {
            if (message.Substring(5) == "1") onDigital1Aan.Invoke(); else onDigital1Uit.Invoke();
        }
        else if (message.StartsWith("DIG2:")) {
            if (message.Substring(5) == "1") onDigital2Aan.Invoke(); else onDigital2Uit.Invoke();
        }
        else if (message.StartsWith("DIG3:")) {
            if (message.Substring(5) == "1") onDigital3Aan.Invoke(); else onDigital3Uit.Invoke();
        }
        else if (message.StartsWith("ANA0:")) {
            if (int.TryParse(message.Substring(5), out int val)) 
            {
                if (printDebugA0) Debug.Log("Analoge Sensor 0 (A0): " + val);

                onAnalog0Changed.Invoke(val); 
                
                bool isNuBoven = val >= analog0Drempelwaarde;
                if (isNuBoven && !analog0WasBoven) {
                    onAnalog0BovenDrempel.Invoke();
                    analog0WasBoven = true;
                } else if (!isNuBoven && analog0WasBoven) {
                    onAnalog0OnderDrempel.Invoke();
                    analog0WasBoven = false;
                }
            }
        }
        else if (message.StartsWith("ANA1:")) {
            if (int.TryParse(message.Substring(5), out int val)) 
            {
                if (printDebugA1) Debug.Log("Analoge Sensor 1 (A1): " + val);

                onAnalog1Changed.Invoke(val); 
                
                bool isNuBoven = val >= analog1Drempelwaarde;
                if (isNuBoven && !analog1WasBoven) {
                    onAnalog1BovenDrempel.Invoke();
                    analog1WasBoven = true;
                } else if (!isNuBoven && analog1WasBoven) {
                    onAnalog1OnderDrempel.Invoke();
                    analog1WasBoven = false;
                }
            }
        }
    }

    public void SendCommand(string command)
    {
        if (serialPort != null && serialPort.IsOpen) {
            serialPort.Write(command);
            Debug.Log("Commando naar Arduino gestuurd: " + command);
        } else {
            Debug.LogWarning("Kan commando '" + command + "' niet sturen: Arduino is niet verbonden.");
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen) {
            serialPort.Write("m"); 
            serialPort.Write("b"); 
            serialPort.Write("S"); 
            serialPort.Write("l"); 
            serialPort.Close();
        }
    }
}