using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using TMPro;

namespace commandGLI_Assets {
    public class data {
        // commands
        public static Dictionary<string, commandGLI.command> commandList = new Dictionary<string, commandGLI.command>{
            ["test"] = new commandGLI.command {
                Description = "a test",
                Help = "a simple test command",

                action = (string input, string fullCommand) => {
                    commandGLI.utils.Log("test");
                },

                expand = new Dictionary<string, commandGLI.command>{
                    ["test"] = new commandGLI.command {
                        Description = "a test",
                        Help = "a simple test command but better",

                        action = (string input, string fullCommand) => {
                            commandGLI.utils.Log("supertest");
                        }
                    },
                    ["spawnmonobehaviour"] = new commandGLI.command {
                        Description = "spawns a mono behaviour",
                        Help = "spawns a mono behaviour not much to it",

                        action = (string input, string fullCommand) => {
                            commandGLI.utils.Log("spawning a mono behaviour");
                            MonoBehaviour tmpMonoBehaviour = commandGLI.utils.GenerateMonoBehaviour("creating a mono behaviour");
                        }
                    },
                    ["ienumeratortest"] = new commandGLI.command {
                        Description = "runs a test ienumerator",
                        Help = "spawns a test ienumerator so it uses realtime",

                        action = (string input, string fullCommand) => {
                            MonoBehaviour tmpMonoBehaviour = commandGLI.utils.GenerateMonoBehaviour("test.ienumeratortest");
                            tmpMonoBehaviour.StartCoroutine(commandGLI_Assets.data.TestCoroutine(tmpMonoBehaviour));
                        }
                    }
                }
            },
            ["echo"] = new commandGLI.command {
                Description = "a test",
                Help = "a simple test command",
                Useage = "echo(item_to_echo)",

                action = (string input, string fullCommand) => {
                    commandGLI.utils.Log(input);
                },
            },
            ["clear"] = new commandGLI.command {
                Description = "clears the terminal",
                Help = "clears the terminal",
                Useage = "clear // clear()",

                action = (string input, string fullCommand) => {
                    GameObject DisplayCommand = GameObject.Find(commandGLI.var.commandDisplayName);

                    if (DisplayCommand != null && DisplayCommand.GetComponent<TMP_Text>() != null)
                        DisplayCommand.GetComponent<TMP_Text>().text = "";
                }
            },
            ["help"] = new commandGLI.command {
                Description = "will return the help data for each command",
                Help = "a simple help command",
                Useage = "help(command to ask help for)",

                action = (string input, string fullCommand) => {
                    commandGLI.utils.Log(commandGLI.execute.retreiveCommand(input).Help, true, "help");
                    commandGLI.utils.Log("if that didnt help you try running listcommands", true, "help");
                },

                expand = new Dictionary<string, commandGLI.command>{
                    ["annoying_orange"] = new commandGLI.command {
                        Description = "...",
                        Help = "...",
                        Hidden = true,

                        action = (string input, string fullCommand) => {
                            commandGLI.utils.Log("he is here, run for your life");
                            Application.OpenURL("https://youtu.be/akT0wxv9ON8?si=Nb4LPGoxuupw9DzU");
                        }
                    }
                }
            },
            ["listcommands"] = new commandGLI.command {
                Description = "This does as it says really",
                Help = "will list all commands",
                Useage = "listcommands // listcommands()",

                action = (string input, string fullCommand) => {
                    commandGLI.utils.Log("---", true, "ListCommands", false);
                    string hold = commandGLI.utils.GatherData(commandGLI_Assets.data.commandList, commandGLI.var.prefix);

                    commandGLI.utils.Log(hold, false);
                    commandGLI.utils.Log("---", true, "ListCommands");
                }
            },
            ["exit"] = new commandGLI.command {
                Description = "closes the game",
                Help = "will close the game",
                Useage = "exit // exit()",

                action = (string input, string fullCommand) => {
                    // if (UnityEditor.EditorApplication.isPlaying)
                    //     UnityEditor.EditorApplication.isPlaying = false;
                    // else 
                        Application.Quit();
                }
            }
        };

        // ienumerators go here, so like yay new feature?
        private static IEnumerator TestCoroutine(MonoBehaviour tmpMonoBehaviour) {
            commandGLI.utils.Log("started second wait");
            yield return new WaitForSeconds(1f);
            commandGLI.utils.Log("ended second wait");
            commandGLI.utils.clearMonoBehaviour(tmpMonoBehaviour);
        } 

        // a basic fail command
        public static commandGLI.command failCommand = new commandGLI.command {
            Description = "unknown command",
            Help = "unknown command",

            action = (string input, string fullCommand) => {
                commandGLI.utils.Log($"unable to find command: {fullCommand}");
            }
        };
    }
}