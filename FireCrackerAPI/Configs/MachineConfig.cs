// ReSharper disable InconsistentNaming

using System.Text.Json.Serialization;

namespace FireCrackerAPI.Configs;

public struct MachineConfig
{
    public int vcpu_count {get; set;}
    public int mem_size_mib {get; set;}
}