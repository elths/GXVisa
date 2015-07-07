using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace visa.CLEINT.Log
{
   public static  class WriteLog
    {

     public static void WriteErorrLog(Exception ex)  
        {  
         try
         {
          if(ex == null)  
            return;                         //ex = null 返回  
          StreamWriter  write = null;  
          DateTime dt = DateTime.Now;       // 设置日志时间  
          string time = dt.ToString("yyyyMMdd"); //年-月-日 时：分：秒  
          string LogName = time+".log";       //日志名称  
          string LogDiretory = "Log";   //日志存放路径  
          string LogFullPath = LogDiretory + "/" + LogName;   //路径 + 名称  

          if (!Directory.Exists(LogDiretory))
         {
             Directory.CreateDirectory(LogDiretory);   //创建文件夹  
         }
          if (!File.Exists(LogFullPath))             //是否存在  
          {
              write = File.CreateText(LogFullPath);     // 创建日志  
              write.Dispose();
          }

          write = File.AppendText(LogFullPath);         //追加，添加错误信息；  

         write.WriteLine(dt.ToString());
         write.WriteLine("");
         write.WriteLine(ex.Message);  
         write.WriteLine(" 异常信息："+ex.ToString());
         if (ex.InnerException!=null)
             {
                 write.WriteLine(" 内部错误：" + ex.InnerException.ToString());
             }
         if (ex.StackTrace != null)
         {
             write.WriteLine(" 异常堆栈：" + ex.StackTrace.ToString());
         }
         write.WriteLine("");
         write.WriteLine("");
         write.WriteLine("");
         write.Flush();  
         write.Dispose();
         }
         catch (System.Exception e)
         {

         }

        }

     public static void WriteOperateLog(string logString)
     {
         try
         {
             if (string.IsNullOrEmpty(logString))
                 return;                         //ex = null 返回  
             StreamWriter write = null;
             DateTime dt = DateTime.Now;       // 设置日志时间  
             string time = dt.ToString("yyyyMMdd"); //年-月-日 时：分：秒  
             string LogName = time + ".log";       //日志名称  
             string LogDiretory = "Log";   //日志存放路径  
             string LogFullPath = LogDiretory + "/" + LogName;   //路径 + 名称  

             if (!Directory.Exists(LogDiretory))
             {
                 Directory.CreateDirectory(LogDiretory);   //创建文件夹  
             }
             if (!File.Exists(LogFullPath))             //是否存在  
             {
                 write = File.CreateText(LogFullPath);     // 创建日志  
                 write.Dispose();
             }

             write = File.AppendText(LogFullPath);         //追加，添加错误信息；  

             write.WriteLine(logString);
             write.WriteLine("");
             write.Flush();
             write.Dispose();
         }
         catch (System.Exception e)
         {

         }

     }  

    }
}
