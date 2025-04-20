using System.IO;
using UnityEngine;

public static class FileManager
{
    //public static bool WriteToFile(string fileName, string fileContent, string fileLocation = null)
    //{
    //    string filePath;

    //    if (fileLocation == null)
    //    {
    //        filePath = Path.Combine(Application.dataPath, fileName);
    //    }
    //    else
    //    {
    //        filePath = Path.Combine(Application.dataPath + "/../" + fileLocation, fileName);
    //    }

    //    try
    //    {
    //        File.WriteAllText(filePath, fileContent);
    //        return true;
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogException(e);
    //        return false;
    //    }
    //}

    public static bool WriteToFile(string fileName, string fileContent)
    {
        string filePath = Path.Combine(Application.dataPath, fileName);

        try
        {
            File.WriteAllText(filePath, fileContent);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    //   public static string LoadFromFile(string fileName, string fileLocation = null)
    //{
    //       string filePath;

    //       if (fileLocation == null)
    //       {
    //           filePath = Path.Combine(Application.dataPath + "../saves/", fileName);
    //       }
    //       else
    //       {
    //           filePath = Path.Combine(Application.dataPath + fileLocation, fileName);
    //       }

    //       string fileContent = "";

    //       try
    //       {
    //           fileContent = File.ReadAllText(filePath);
    //       }
    //       catch (System.Exception e)
    //       {
    //           Debug.LogException(e);
    //       }

    //       return fileContent;
    //   }

    public static string LoadFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.dataPath, fileName);

        string fileContent = "";

        try
        {
            fileContent = File.ReadAllText(filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }

        return fileContent;
    }
}
