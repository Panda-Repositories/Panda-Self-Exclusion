using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Panda_Self_Exclusion
{
    internal class Program
    {
        /*
         * Written By SkieHacker
         * ==================================================================
         * C# Exclude your Executor shit from Windows Defender
         * This is quite Safer Practice than Windows Defender Controller as it only Exclude The Argument Directory. DO NOT ABUSE IT FFS
         * Although this is quite dirty code, i didn't paste this source code
         * I Search only the Argument of Windows Defender on Powershell.. Oh yea it abuses PowerShell 
         * --------------------
         * This is pretty easy but heck if u paste this code, then go ahead
         */

        static void Main(string[] args)
        {
            if (args == null)
            {
                Console.WriteLine("Invalid Argument...");
                Console.WriteLine("-------------------------------------------------------------.");
                Console.WriteLine("Example: PSE.exe C:\\Windows.");
                Console.WriteLine("-------------------------------------------------------------.");
            }
            else
            {
                if (IsValidPath(args[0]))
                {
                    var PInf = new System.Diagnostics.Process();
                    Console.WriteLine("Checking for Directory...");
                    if (!Directory.Exists(System.IO.Path.GetTempPath() + "./ExcludeList/"))
                    {
                        Directory.CreateDirectory(System.IO.Path.GetTempPath() + "./ExcludeList/");
                    }
                    Console.WriteLine("Checking for List");
                    if (File.Exists(System.IO.Path.GetTempPath() + "./ExcludeList/List.txt"))
                    {
                        Console.WriteLine("File Readed...");
                        string read_arg = File.ReadAllText(System.IO.Path.GetTempPath() + "./ExcludeList/List.txt");
                        if (IsValidPath(read_arg))
                        {
                            Console.WriteLine("Removing Exist Directory to Exclusive List...");
                            PInf.StartInfo.FileName = "powershell";
                            PInf.StartInfo.Arguments = "-Command Remove-MpPreference -ExclusionPath " + read_arg;
                            PInf.StartInfo.RedirectStandardOutput = true;
                            PInf.StartInfo.CreateNoWindow = true;
                            PInf.StartInfo.UseShellExecute = false;
                            PInf.Start();
                            PInf.WaitForExit();

                            Console.WriteLine("Adding Directory to Exclusive List...");
                            PInf.StartInfo.FileName = "powershell";
                            PInf.StartInfo.Arguments = "-Command Add-MpPreference -ExclusionPath " + args[0];
                            PInf.StartInfo.RedirectStandardOutput = true;
                            PInf.StartInfo.CreateNoWindow = true;
                            PInf.StartInfo.UseShellExecute = false;
                            PInf.Start();
                            PInf.WaitForExit();
                        }
                        else
                        {
                            Console.WriteLine("Adding Directory to Exclusive List...");
                            PInf.StartInfo.FileName = "powershell";
                            PInf.StartInfo.Arguments = "-Command Add-MpPreference -ExclusionPath " + args[0];
                            PInf.StartInfo.RedirectStandardOutput = true;
                            PInf.StartInfo.CreateNoWindow = true;
                            PInf.StartInfo.UseShellExecute = false;
                            PInf.Start();
                            PInf.WaitForExit();
                        }
                    }

                    //Adding it to the List
                    Console.WriteLine("Saving List...");
                    if (Directory.Exists(System.IO.Path.GetTempPath() + "./ExcludeList/"))
                    {
                        Directory.CreateDirectory(System.IO.Path.GetTempPath() + "./ExcludeList/");
                    }
                    File.WriteAllText(System.IO.Path.GetTempPath() + "./ExcludeList/List.txt", args[0]);

                    Console.WriteLine("The Directory is Successfully Added to Exclusion List");
                    Console.WriteLine("--------------------------------------------------------");


                    // Panda Only
                    if (!File.Exists(System.IO.Path.GetTempPath() + "./ExcludeList/PASSED.cfg"))
                    {
                        File.WriteAllText(System.IO.Path.GetTempPath() + "./ExcludeList/PASSED.cfg", "TRUE");
                    }
                    return;
                }
                Console.WriteLine("You may possible enter the Wrong Argument");
                Console.WriteLine("-------------------------------------------------------------.");
                Console.WriteLine("Example: PSE.exe C:\\Windows.");
                Console.WriteLine("-------------------------------------------------------------.");
                return;
            }
        }

        static private bool IsValidPath(string path)
        {
            //Credit to Stackoverflow caz im lazy af. Lol
            Regex driveCheck = new Regex(@"^[a-zA-Z]:\\$");
            if (!driveCheck.IsMatch(path.Substring(0, 3))) return false;
            string strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
            strTheseAreInvalidFileNameChars += @":/?*" + "\"";
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            if (containsABadCharacter.IsMatch(path.Substring(3, path.Length - 3)))
                return false;

            DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(path));
            if (!dir.Exists)
                dir.Create();
            return true;
        }
    }
}
