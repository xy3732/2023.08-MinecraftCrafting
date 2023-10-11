using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DebugController : MonoBehaviour
{
    [SerializeField] GameObject InventoryObejct;

    bool showConsole;
    bool showHelp;
    bool showInventory = false;
    string input = "help";

    string itemList = "";

    public static DebugCommand HELP;
    public static DebugCommand<string,int> ADD_ITEM;
    public static DebugCommand ITEM_LIST;
    public List<object> commandList;

    private void Start()
    {
        for (int i = 0; i < ItemsManager.Instance.Items.Count; i++)
        {
            itemList += ItemsManager.Instance.Items[i].Name + ", ";
        }
        Debug.Log(itemList);

        CommandListInit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote)) OnToggleDebug();
        if (Input.GetKeyDown(KeyCode.Return)) OnReturn();
        if (Input.GetKeyDown(KeyCode.I)) ShowInventory();
    }

    private void ShowInventory()
    {
        showInventory = !showInventory;
        InventoryObejct.SetActive(showInventory);
        InventoryWindow.Instance.Open();
    }

    private void CommandListInit()
    {
        ADD_ITEM = new DebugCommand<string, int>("additem", "add item to inveotory", "additem <itemName string> <Amount int>", (x1, x2) =>
         {
             TestDebug.instance.SetItem(x1,x2);
         });

        ITEM_LIST = new DebugCommand(null, itemList, "itemList", () => 
        { 
        
        });


        HELP = new DebugCommand("Help", "show a list of command", "Help", () =>
        {
            showHelp = true;
        });

        commandList = new List<object>
        {
            HELP,
            ADD_ITEM,
            ITEM_LIST,
        };

    }

    public void OnToggleDebug()
    {
        showConsole = !showConsole;
    }

    public void OnReturn()
    {
        if (showConsole)
        {
            if (input == "help") showHelp = true;
            else showHelp = false;
            HandleInput();
            input = "";
        }
    }

    private void HandleInput()
    {
        string[] properties = input.Split(' ');

        for(int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (properties[0].Equals(commandBase.commandID))
            {
                if(commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<string> != null)
                {
                    try
                    {
                        (commandList[i] as DebugCommand<string>).Invoke(properties[1]);
                    }
                    catch (System.IndexOutOfRangeException ex)
                    {
                        Debug.Log("this is errror T1");
                    }
                }
                else if(commandList[i] as DebugCommand<string,int> != null)
                {

                    try
                    {
                        (commandList[i] as DebugCommand<string, int>).Invoke(properties[1], int.Parse(properties[2]));
                    }
                    catch (System.IndexOutOfRangeException ex)
                    {

                    }
                }
            }
        }
    }

    public Vector3 string2Vector3(string value)
    {
        string[] temp = value.Split(',');
        return new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
    }

    Vector2 scroll;
    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0f;

        // 디버그 콘솔 백그라운드 설정
        GUI.Box(new Rect(0, y, Screen.width, 30), "");

        // 텍스트 박스
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.color = new Color32(255, 255, 255, 255);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

        if (showHelp)
        {
            y += 30f;
            GUI.backgroundColor = new Color32(75, 75, 75, 175);
            GUI.color = new Color32(255, 255, 255, 255);
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            // 콘솔 명령어 갯수만큼 스크롤 뷰 크기 조정
            Rect viewport = new Rect(0,0,Screen.width-30, 20 * commandList.Count);
            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

            // 스크롤 뷰 안에 디버그 콘솔 명령어 추가
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label = $"{command.commandFormat} : {command.commandDescription}";

                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100 , 20);

                GUI.Label(labelRect, label);
            }
            GUI.EndScrollView();

            y+= 100;
        }
    }
}
