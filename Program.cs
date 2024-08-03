using System;
using System.IO;

namespace LinHexShell
{
    class Program
    {
        static void Main(string[] args)
        {
            HexDisplay hexDisplay = new HexDisplay();
            bool fileLoaded = false;

            Console.WriteLine("+-----------------------------------------------------------------------------------------------------+");
            Console.WriteLine("|#####################################################################################################|");
            Console.WriteLine("|#                                                                                                   #|");
            Console.WriteLine("|#                                                                                                   #|");
            Console.WriteLine("|#                                                                                                   #|");
            Console.WriteLine("|#     +-+            +-----------+   +----------+   +-+       +-+  +-+-----------+ +-+      +-+     #|");
            Console.WriteLine("|#     | |            +----+++----+   +-+------+-+   | |       | |  | +-----------+ | |      | |     #|");
            Console.WriteLine("|#     | |                 |+|        | |      | |   | |       | |  | |             +-+-+  +-+-+     #|");
            Console.WriteLine("|#     | |                 |+|        | |      | |   | |       | |  | |               |+|  |+|       #|");
            Console.WriteLine("|#     | |                 |+|        | |      | |   | |       | |  | |               |+|  |+|       #|");
            Console.WriteLine("|#     | |                 |+|        | |      | |   +-+-------+-+  | +---+-+---+-+   +-+--+-+       #|");
            Console.WriteLine("|#     | |                 |+|        | |      | |   +-+-------+-+  | |+++| |+++| |     |  |         #|");
            Console.WriteLine("|#     | |                 |+|        | |      | |   | |       | |  | +---+-+---+-+   +-+--+-+       #|");
            Console.WriteLine("|#     | |                 |+|        | |      | |   | |       | |  | |               |+|  |+|       #|");
            Console.WriteLine("|#     | |                 |+|        | |      | |   | |       | |  | |               |+|  |+|       #|");
            Console.WriteLine("|#     | |                 |+|        | |      | |   | |       | |  | |             +-+-+  +-+-+     #|");
            Console.WriteLine("|#     | +----------+ +----+++----+   | |      | |   | |       | |  | +-----------+ | |      | |     #|");
            Console.WriteLine("|#     +-+----------+ +-----------+   +-+      +-+   +-+       +-+  +-+-----------+ +-+      +-+     #|");
            Console.WriteLine("|#                                                                                                   #|");
            Console.WriteLine("|#                                                                                                   #|");
            Console.WriteLine("|#      Linux Hex ©2020-present DaZiDian All Copyrights Reserved.                                    #|");
            Console.WriteLine("|#                                                                                                   #|");
            Console.WriteLine("|#      LinHex is starting up...                                                                     #|");
            Console.WriteLine("|#      Your current operating mode is: Shell                                                        #|");
            Console.WriteLine("|#                                                                                                   #|");
            Console.WriteLine("|#      Feedback:                                                                                    #|");
            Console.WriteLine("|#      Email:sdsfttt1@outlook.com / dazidian2007@gmail.com                                          #|");
            Console.WriteLine("|#      Discord:DaZiDian#3588                                                                        #|");
            Console.WriteLine("|#      YouTube:DaZiDian                                                                             #|");
            Console.WriteLine("|#      Github:https://github.com/DaZiDian                                                           #|");
            Console.WriteLine("|#      QQ:2489043224                                                                                #|");
            Console.WriteLine("|#                                                                                                   #|");
            Console.WriteLine("+-----------------------------------------------------------------------------------------------------+");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                string[] commandParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (commandParts.Length == 0) continue;

                string command = commandParts[0].ToLower();

                switch (command)
                {
                    case "open":
                        if (commandParts.Length != 2)
                        {
                            Console.WriteLine("用法: open <文件路径>");
                            break;
                        }
                        string filePath = commandParts[1];
                        if (File.Exists(filePath))
                        {
                            hexDisplay.LoadFile(filePath);
                            fileLoaded = true;
                        }
                        else
                        {
                            Console.WriteLine("文件不存在.");
                        }
                        break;

                    case "save":
                        if (commandParts.Length != 2)
                        {
                            Console.WriteLine("用法: save <文件路径>");
                            break;
                        }
                        if (fileLoaded)
                        {
                            hexDisplay.SaveFile(commandParts[1]);
                            Console.WriteLine("文件已保存.");
                        }
                        else
                        {
                            Console.WriteLine("没有打开的文件.");
                        }
                        break;

                    case "selecthex":
                        if (commandParts.Length != 3)
                        {
                            Console.WriteLine("用法: selecthex <开始位置> <结束位置>");
                            break;
                        }
                        hexDisplay.SelectHexRange(commandParts[1], commandParts[2]);
                        break;

                    case "reverse":
                        if (!fileLoaded || !hexDisplay.HasSelection())
                        {
                            Console.WriteLine("请先选择一个范围.");
                        }
                        else
                        {
                            hexDisplay.ReverseSelection();
                            Console.WriteLine("选定的字节已反转.");
                        }
                        break;

                    case "xor":
                        if (commandParts.Length != 2)
                        {
                            Console.WriteLine("用法: xor <字节值>");
                            break;
                        }
                        if (byte.TryParse(commandParts[1], out byte xorValue))
                        {
                            if (!fileLoaded || !hexDisplay.HasSelection())
                            {
                                Console.WriteLine("请先选择一个范围.");
                            }
                            else
                            {
                                hexDisplay.XorSelection(xorValue);
                                Console.WriteLine("选定的字节已进行XOR操作.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("无效的字节值.");
                        }
                        break;

                    case "or":
                        if (commandParts.Length != 2)
                        {
                            Console.WriteLine("用法: or <字节值>");
                            break;
                        }
                        if (byte.TryParse(commandParts[1], out byte orValue))
                        {
                            if (!fileLoaded || !hexDisplay.HasSelection())
                            {
                                Console.WriteLine("请先选择一个范围.");
                            }
                            else
                            {
                                hexDisplay.OrSelection(orValue);
                                Console.WriteLine("选定的字节已进行OR操作.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("无效的字节值.");
                        }
                        break;

                    case "and":
                        if (commandParts.Length != 2)
                        {
                            Console.WriteLine("用法: and <字节值>");
                            break;
                        }
                        if (byte.TryParse(commandParts[1], out byte andValue))
                        {
                            if (!fileLoaded || !hexDisplay.HasSelection())
                            {
                                Console.WriteLine("请先选择一个范围.");
                            }
                            else
                            {
                                hexDisplay.AndSelection(andValue);
                                Console.WriteLine("选定的字节已进行AND操作.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("无效的字节值.");
                        }
                        break;

                    case "invert":
                        if (!fileLoaded || !hexDisplay.HasSelection())
                        {
                            Console.WriteLine("请先选择一个范围.");
                        }
                        else
                        {
                            hexDisplay.InvertSelection();
                            Console.WriteLine("选定的字节已取反.");
                        }
                        break;

                    case "rot13":
                        if (!fileLoaded || !hexDisplay.HasSelection())
                        {
                            Console.WriteLine("请先选择一个范围.");
                        }
                        else
                        {
                            hexDisplay.Rot13Selection();
                            Console.WriteLine("选定的字节已进行ROT13操作.");
                        }
                        break;

                    case "rotateleft":
                        if (commandParts.Length != 2)
                        {
                            Console.WriteLine("用法: rotateleft <位数>");
                            break;
                        }
                        if (int.TryParse(commandParts[1], out int count))
                        {
                            if (!fileLoaded || !hexDisplay.HasSelection())
                            {
                                Console.WriteLine("请先选择一个范围.");
                            }
                            else
                            {
                                hexDisplay.RotateLeftSelection(count);
                                Console.WriteLine("选定的字节已进行左旋转操作.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("无效的位数.");
                        }
                        break;

                    case "selectall":
                        if (!fileLoaded)
                        {
                            Console.WriteLine("没有打开的文件.");
                        }
                        else
                        {
                            hexDisplay.SelectAll();
                            Console.WriteLine("所有字节已选中.");
                        }
                        break;

                    case "page":
                        if (commandParts.Length != 2)
                        {
                            Console.WriteLine("用法: page <命令>");
                            break;
                        }

                        string pageCommand = commandParts[1].ToLower();
                        switch (pageCommand)
                        {
                            case "<":
                                hexDisplay.PreviousPage();
                                break;

                            case ">":
                                hexDisplay.NextPage();
                                break;

                            case "<首页":
                                hexDisplay.FirstPage();
                                break;

                            case ">最后一页":
                                hexDisplay.LastPage();
                                break;

                            default:
                                if (int.TryParse(pageCommand, out int pageNumber))
                                {
                                    hexDisplay.Page(pageNumber);
                                }
                                else
                                {
                                    Console.WriteLine("无效的命令.");
                                }
                                break;
                        }
                        break;

                    case "help":
                        Console.WriteLine("+----------------------------------------------------------------------------------------------------+");
                        Console.WriteLine("|  open <文件路径>        - 打开一个文件并显示其十六进制表示。                                       |");
                        Console.WriteLine("|  save <文件路径>        - 保存当前字节数组到指定文件路径。                                         |");
                        Console.WriteLine("|  selecthex <开始> <结束> - 选择从开始到结束位置的字节。                                            |");
                        Console.WriteLine("|  selectall              - 选择所有字节。                                                           |");
                        Console.WriteLine("|  reverse                - 反转选定范围的字节。                                                     |");
                        Console.WriteLine("|  xor <字节值>           - 对选定范围的字节进行XOR操作。                                            |");
                        Console.WriteLine("|  or <字节值>            - 对选定范围的字节进行OR操作。                                             |");
                        Console.WriteLine("|  and <字节值>           - 对选定范围的字节进行AND操作。                                            |");
                        Console.WriteLine("|  invert                 - 取反选定范围的字节。                                                     |");
                        Console.WriteLine("|  rot13                  - 对选定范围的字节进行ROT13操作。                                          |");
                        Console.WriteLine("|  rotateleft <位数>      - 将选定范围的字节左旋转指定位数。                                         |");
                        Console.WriteLine("|  page 页码              - 跳转至指定的页码。也可以使用page <回到上一页，page >去到下一页           |");
                        Console.WriteLine("+----------------------------------------------------------------------------------------------------+");
                        break;

                    case "exit":
                        Console.WriteLine("退出程序.");
                        return;

                    default:
                        Console.WriteLine("未知命令. 使用 'help' 命令查看可用命令.");
                        break;
                }
            }
        }
    }
}
