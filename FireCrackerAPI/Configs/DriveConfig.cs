// ReSharper disable InconsistentNaming
namespace FireCrackerAPI.Configs;

public struct DriveConfig
{
    public string drive_id {get; set;}
    public string path_on_host {get; set;}
    public bool is_root_device {get; set;}
    public bool is_read_only {get; set;}
}