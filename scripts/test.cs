using UnityEngine;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using TMPro;

public class test : MonoBehaviour {
    public TMP_InputField textInput;

    public int selectedCommand = 0;

    public List<string> usedCommands = new List<string>();
    public Dictionary<string, eevee.config> config = new Dictionary<string, eevee.config>() {
        {
            "nextCommand", new eevee.config {
                displayName = "nextCommand",
                KEYBOARD_code = (int)KeyCode.UpArrow,
                CONTROLLER_name = ""
            }
        },
        {
            "lastCommand", new eevee.config {
                displayName = "nextCommand",
                KEYBOARD_code = (int)KeyCode.DownArrow,
                CONTROLLER_name = ""
            }
        }
    };

    void Start() {
        // inject inputs
        eevee.inject.install(config);

        // idk
        textInput = GetComponent<TMP_InputField>();
        textInput.ActivateInputField();
    }

    void Update() {
        if (eevee.input.Collect("nextCommand")) {
            if (usedCommands.Count > 0) {
                if (selectedCommand == 0)
                    selectedCommand = usedCommands.Count - 1;
                else
                    selectedCommand--;

                textInput.text = usedCommands[selectedCommand];
            }
        }

        if (eevee.input.Collect("lastCommand")) {
            if (usedCommands.Count > 0) {
                if (selectedCommand == usedCommands.Count - 1)
                    selectedCommand = 0;
                else
                    selectedCommand++;

                textInput.text = usedCommands[selectedCommand];
            }
        }
    }

    public void RunCommand() {
        // text management
        commandGLI.execute.ViaString(textInput.text);
        usedCommands.Add(textInput.text);
        textInput.text = "";

        selectedCommand = 0;

        // reselecting the input field
        textInput.ActivateInputField();
    }
}