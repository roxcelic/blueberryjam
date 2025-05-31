using UnityEngine;
using UnityEngine.UI;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using TMPro;

namespace commandGLI {

    // variables
    public class var {
        public static string commandDisplayName = "Canvas/Image/ScrollView/Viewport/commandHost";
        public static string prefix = "> ";

        public static string inputStart = "(";
        public static string inputClose = ")";
    }
    
    // the command data type/stack
    public class command {
        public string Description;
        public string Useage;
        public string Help;

        public bool Hidden = false;

        public Action<string, string> action;
        public Dictionary<string, command> expand;
    }

    // utils
    public class utils {
        public static void Log(string userInput, bool prefix = true, string program = "", bool newLine = true){
            if (commandGLI.var.commandDisplayName == ""){
                Debug.Log(userInput);
                return;
            }
            
            GameObject DisplayCommand = GameObject.Find(commandGLI.var.commandDisplayName);
            
            if (DisplayCommand == null && DisplayCommand.GetComponent<TMP_Text>() == null){
                Debug.Log(userInput);
                return;
            }

            string message = "";

            if (newLine)
                message = "\n";

            if (prefix)
                message += $"{program}{commandGLI.var.prefix}{userInput}";
            else
                message += $"{userInput}";
            
            DisplayCommand.GetComponent<TMP_Text>().text += message;
        }

        public static string GatherData(Dictionary<string, command> commandList, string prefix) {
            string hold = "";

            foreach (string Key in commandList.Keys){
                if (!commandList[Key].Hidden) {
                    hold += $"\n{prefix}{Key} --- Description: {commandList[Key].Description} --- Useage: {commandList[Key].Useage}";
                
                    if (commandList[Key].expand != null)
                        hold += commandGLI.utils.GatherData(commandList[Key].expand, prefix + "\\t");
                }
            }

            return hold;
        }

        // monobehaviour manipulation
        public static MonoBehaviour GenerateMonoBehaviour(string name){
            GameObject tmpGameObject = new GameObject(name);
            MonoBehaviour tmpMonoBehaviour = tmpGameObject.AddComponent<TMPMonoBehaviour>();

            return tmpMonoBehaviour;
        }

        public static void clearMonoBehaviour(MonoBehaviour tmpMonoBehaviour) {GameObject.Destroy(tmpMonoBehaviour.gameObject);}
    }

    // run commands
    public class execute {
        public static void ViaString(string userInput) {
            if (userInput == "") return;

            commandGLI.utils.Log(userInput, true, "user -- ");

            // initialising commands
            string[] userInputs = userInput.Split(".");

            // grabbing the input
            string parsedInput = "";

            int firstOpen = userInput.IndexOf(commandGLI.var.inputStart);
            int lastClose = userInput.LastIndexOf(commandGLI.var.inputClose);
            if (firstOpen != -1 && lastClose != -1 && firstOpen < lastClose)
                parsedInput = userInput.Substring(firstOpen + 1, lastClose - firstOpen - 1);
            
            // getting the commands            
            if (parsedInput != "") {
                retreiveCommand(userInput.Substring(0, userInput.LastIndexOf($"{commandGLI.var.inputStart}{parsedInput}{commandGLI.var.inputClose}"))).action(parsedInput, userInput);
            } else {
                retreiveCommand(userInput).action("", userInput);
            }
        }

        public static commandGLI.command retreiveCommand(string userInput) {
            userInput = userInput.ToLower();

            // variables
            string[] userInputs = userInput.Split(".");
            Dictionary<string, command> workingCommandList = commandGLI_Assets.data.commandList;
            commandGLI.command currentCommand = commandGLI_Assets.data.failCommand;
            
            // search for the command
            foreach (string commandInput in userInputs){
                if (workingCommandList != null && workingCommandList.ContainsKey(commandInput)){
                    currentCommand = workingCommandList[commandInput.Split("(")[0]];
                    workingCommandList = currentCommand.expand;
                } else {
                    break;
                }
            }

            // return the command
            return currentCommand;
        }
    }
}

public class TMPMonoBehaviour : MonoBehaviour {}