using System.Diagnostics;

namespace FireCrackerAPI;

public class Drive
{
    public string Path { get; private set; }
    public bool IsMounted { get; private set; }

    public Drive(string path, int sizeMb)
    {
        Path = path;
        IsMounted = false;
    }

    public async Task MountDrive(string mountDirectory)
    {
        if(IsMounted)
            return;
        
        
        IsMounted = true;
    }

    public async Task UnmountDrive()
    {
        if(!IsMounted)
            return;
        
        
        IsMounted = false;
    }
    
    

}