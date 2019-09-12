using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace taskmsg
{
   public class ToolMoveFiles
    {
 
 
        public static bool MoveDirectory(string path, string docID)
        {
           // string dosLine = @"net use " + path + " /User:test \"123456\" /PERSISTENT:YES";
            string dosLine = @"net use " + path + " /User:Guest \"\" /PERSISTENT:NO";
            connect(dosLine);
            //net use h: \\ip\folders password /user:域名\user
            //if (path.IndexOf(IpHelper.GetLocalIP().Trim())>0)
            //{
            //    return true;
            //}
            CopyFile(path, docID);
           // return Flag;
            return true;
        }

        public static string GetRootPath()
        {
            return "D:\\照片\\2\\55.9";
        }

        public static string GetWaitManagePath()
        {
            return "d:\\fenpei";
        }

        public static FileSystemInfo[] Director(string dir) 
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
            //foreach (FileSystemInfo fsinfo in fsinfos)
            //{
            //    if (fsinfo is DirectoryInfo)     //判断是否为文件夹
            //    {
            //        Director(fsinfo.FullName);//递归调用
            //    }
            //    else 
            //    {
            //        Console.WriteLine(fsinfo.FullName);//输出文件的全部路径
            //    }
            //}
            return fsinfos;
               
            
        }
  
        public static string CopyFile(string form_path, string toPath)//, string dosLine
        {

            Process proc = new Process();
            string cmd = string.Format("xmove {0} {1} /y /e /i /q",form_path,toPath); //xcopy \\10.122.55.4\websites\test E:\demo\test\ /D /E /Y /K
            try
            {
                return Docopy(proc, cmd);
            }
            catch (Exception ex)
            {
 
                cleanConnect();
                // connect(dosLine);
                Docopy(proc, cmd);
 
                return ex.Message;
            }
            //   return cmd;
        }
        //cmd命令拷贝 xcopy
        public static string Docopy(Process proc, string cmd)
        {
            string ret = "";
            proc.StartInfo.FileName = @"C:\Windows\System32\cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
 
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;//true表示不显示黑框，false表示显示dos界面
 
            // proc.StartInfo.Arguments = $" {cmd} ";// redirect ? @"/c " + "\"" + url  +"\"" : @"/k " + "\"" + url + "\"";
 
 
            proc.Start();
            proc.StandardInput.WriteLine(cmd);// (@"net use \\172.25.138.150User@123 /user:administrator");//xcopy \\eahis\netlogon\bmp c:\bmp /e/y
 
 
            proc.StandardInput.WriteLine("exit");

            int temp = 0;
            while (!proc.HasExited)
            {
                temp++;
                proc.WaitForExit(1000);
                if (temp > 4)
                {
                    ret = "分配异常！";
                    break;
                }
            }
            string errormsg = proc.StandardError.ReadToEnd();
            proc.StandardError.Close();
            if (string.IsNullOrEmpty(errormsg))
            {
                // Flag = true;
            }
            else
            {
                throw new Exception(errormsg);
            }
 
            proc.Close();
            // proc.Dispose();
 
 
            return ret;
        }
 
        //共享用户连接
        public static bool connect(string dosLine)
        {
            try
            {
                return connectState(dosLine);
            }
            catch (Exception ex)
            {
                if (cleanConnect())
                {
                    return connectState(dosLine);
                }
            }
            return false;
        }
 
        public static bool connectState(string dosLine)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                // string dosLine = @"net use " + path + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }
        public static bool cleanConnect()
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                // string dosLine = @"net use " + path + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                proc.StandardInput.WriteLine(" net use * /del /y");
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    // throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                // throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }
 
        //手动复制拷贝
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                //如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                //遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    //先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    //否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }
 
    }
}
