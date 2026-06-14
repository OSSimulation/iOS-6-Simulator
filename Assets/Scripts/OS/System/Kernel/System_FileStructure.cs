using System.IO;
using UnityEngine;

public class System_FileStructure : MonoBehaviour
{
    private void Update()
    {
        VerifyAndCreateFS("var", new string[]
         {
            "mobile/Applications",
            "mobile/Library/Preferences",
            "mobile/Media/DCIM"
         });

        VerifyAndCreateFS("Library", new string[]
        {
            "Wallpapers",
            "Ringtones",
            "Preferences"
        });
    }

    private void VerifyAndCreateFS(string rootFolder, string[] directories)
    {
        string rootPath = Path.Combine(Path.Combine(Application.persistentDataPath, "rootFS"), rootFolder);

        if (!Directory.Exists(rootPath))
        {
            Debug.LogWarning($"Folders missing in directory '{rootFolder}'. Creating...");

            Directory.CreateDirectory(rootPath);
        }

        foreach (string dir in directories)
        {
            string fullPath = Path.Combine(rootPath, dir);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }
    }
}
