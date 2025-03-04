using System.Diagnostics;

namespace FireCrackerAPI.Tools;

public class CmdHelper
{
    public static async Task RunCommand(string command)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"{command}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = processInfo;
        process.Start();
        
        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (!string.IsNullOrEmpty(output)) Console.WriteLine(output);
        if (!string.IsNullOrEmpty(error)) Console.WriteLine($"[ERROR] {error}");
    }
}