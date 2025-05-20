using System.IO;
using UnityEngine;

/// <summary>
/// Load and save the provided file
/// </summary>
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

    /// <summary>
    /// Try to write to disk the specified <paramref name="fileContent"/> inside a file with the name provided by <paramref name="fileName"/>
    /// </summary>
    /// <param name="fileName">Name of the file to write</param>
    /// <param name="fileContent">Content of the file to write</param>
    /// <returns></returns>
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

    /// <summary>
    /// Try to load from disk a file with the name provided by <paramref name="fileName"/>
    /// </summary>
    /// <param name="fileName">Name of the file to load</param>
    /// <returns></returns>
    public static string LoadFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.dataPath, fileName);

        string fileContent;

        try
        {
            fileContent = File.ReadAllText(filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            fileContent = null;
        }

        return fileContent;
    }
}
