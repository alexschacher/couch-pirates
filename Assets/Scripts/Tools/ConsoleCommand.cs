using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleCommand : MonoBehaviour
{
    public ConsoleCommand INSTANCE;
    private bool isTakingInput = false;

    [SerializeField] private InputField inputField;
    [SerializeField] private KeyCode activateKey = KeyCode.Tab;
    [SerializeField] private KeyCode executeKey = KeyCode.Return;
    [SerializeField] private KeyCode cancelKey = KeyCode.Escape;
    [SerializeField] private bool displayDebugMessages = true;
    [SerializeField] private List<Text> textList;
    [SerializeField] private float messageDisplayTime = 4f;
    private float messageDisplayTimer;

    private void ExecuteCommand(string input)
    {
        SetDebugMessage(input);

        string[] strArray = input.Split(" "[0]);
        string command = strArray[0];
        string arg1 = "";
        string arg2 = "";
        if (strArray.Length > 1)
        {
            arg1 = strArray[1];
        }
        if (strArray.Length > 2)
        {
            arg2 = strArray[2];
        }

        switch (command)
        {
            case "spawn": {

                    if (arg1 == "left" || arg1 == "l")
                    {
                        TestGamemodeController.INSTANCE.SpawnEnemyShip(true); break;
                    }
                    else if (arg1 == "right" || arg1 == "r")
                    {
                        TestGamemodeController.INSTANCE.SpawnEnemyShip(false); break;
                    }
                    else
                    {
                        SetDebugMessage("ship spawned: optional: spawn left/right");
                        TestGamemodeController.INSTANCE.SpawnEnemyShip(); break;
                    }
                }

            case "sink": {

                    if (arg1 == "enemies")
                    {
                        EnemyShipController[] list = FindObjectsOfType<EnemyShipController>();
                        foreach (EnemyShipController e in list)
                        {
                            e.ForceSink();
                        }
                    }
                    else if (arg1 == "disable")
                    {
                        TestGamemodeController.INSTANCE.preventSink = true;
                    }
                    else if (arg1 == "enable")
                    {
                        TestGamemodeController.INSTANCE.preventSink = false;
                    }
                    else
                    {
                        SetDebugMessage("sink enemies/disable/enable");
                    }
                    break;
                }

            case "damage": {

                    if (arg1 == "fix")
                    {
                        FixSpot[] list = FindObjectsOfType<FixSpot>();
                        foreach (FixSpot f in list)
                        {
                            f.SetAll(FixSpot.State.Fixed);
                        }
                    }
                    else if (arg1 == "all")
                    {
                        FixSpot[] list = FindObjectsOfType<FixSpot>();
                        foreach (FixSpot f in list)
                        {
                            f.SetAll(FixSpot.State.Broken);
                        }
                    }
                    else if (arg1 == "disable")
                    {
                        FixSpot[] list = FindObjectsOfType<FixSpot>();
                        foreach (FixSpot f in list)
                        {
                            f.canTakeDamage = false;
                        }
                    }
                    else if (arg1 == "enable")
                    {
                        FixSpot[] list = FindObjectsOfType<FixSpot>();
                        foreach (FixSpot f in list)
                        {
                            f.canTakeDamage = true;
                        }
                    }
                    else
                    {
                        SetDebugMessage("damage all/fix/disable/enable");
                    }
                    break;
                }

            case "water": {

                    if (arg1 == "disable")
                    {
                        TestGamemodeController.INSTANCE.preventWater = true;
                    }
                    else if (arg1 == "enable")
                    {
                        TestGamemodeController.INSTANCE.preventWater = false;
                    }
                    else if (arg1 == "empty")
                    {
                        TestGamemodeController.INSTANCE.SetHullWaterEmpty();
                    }
                    else if (arg1 == "fill" || arg1 == "full")
                    {
                        TestGamemodeController.INSTANCE.SetHullWaterFull();
                    }
                    else
                    {
                        SetDebugMessage("water disable/enable/fill/empty");
                    }
                    break;
                }

            case "set": {

                    if (arg1 == "enemyfirerate")
                    {
                        float result;
                        if (float.TryParse(arg2, out result))
                        {
                            EnemyShipController[] list = FindObjectsOfType<EnemyShipController>();
                            foreach (EnemyShipController e in list)
                            {
                                e.SetTimeBetweenShots(result);
                            }
                        }
                        else
                        {
                            SetDebugMessage("Must enter a fire rate");
                        }
                    }
                    else
                    {
                        SetDebugMessage("set enemyfirerate (amount)");
                    }

                    break;
                }

            default: SetDebugMessage("valid commands: spawn, sink, damage, water, set"); break;
        }
    }

    private void Start()
    {
        INSTANCE = this;
    }

    private void Update()
    {
        UpdateMessageDisplayTimer();
        GetInput();
    }

    private void UpdateMessageDisplayTimer()
    {
        if (messageDisplayTimer > 0)
        {
            messageDisplayTimer -= Time.deltaTime;
        }
        else if (messageDisplayTimer > -100f && textList[0] != null)
        {
            textList[0].enabled = false;
            messageDisplayTimer = -101f;
        }
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(cancelKey) && isTakingInput)
        {
            DeactivateInputField();
        }

        if (Input.GetKeyDown(activateKey) && !isTakingInput)
        {
            ActivateInputField();
        }

        else if (Input.GetKeyDown(executeKey) && isTakingInput)
        {
            if (inputField.text != "")
            {
                ExecuteCommand(inputField.text);
            }
            DeactivateInputField();
        }
    }

    private void ActivateInputField()
    {
        isTakingInput = true;
        inputField.ActivateInputField();
        inputField.Select();
    }

    private void DeactivateInputField()
    {
        isTakingInput = false;
        inputField.text = "";
        inputField.DeactivateInputField();
    }

    public void SetDebugMessage(string message)
    {
        SetDebugMessage(message, 0);
    }

    public void SetDebugMessage(string message, int index)
    {
        if (!displayDebugMessages) return;
        if (textList == null) return;
        if (index >= textList.Count) return;

        textList[index].text = message;
        if (index == 0)
        {
            textList[0].enabled = true;
            messageDisplayTimer = messageDisplayTime;
        }
    }
}