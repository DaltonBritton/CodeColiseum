// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using FireCrackerAPI;
using FireCrackerAPI.Configs;

Console.WriteLine("Hello, World!");

UnixDomainSocketEndPoint socketEndPoint = new("/tmp/firecracker.socket");

FireCrackerClient client = new(socketEndPoint);

MachineConfig machineConfig = new()
{
    vcpu_count = 2,
    mem_size_mib = 512,
};
await client.SetMachineConfig(machineConfig);


BootSourceConfig bootSourceConfig = new()
{
    kernel_image_path = "/home/testuser/vmlinux-5.10.225",
    boot_args = "console=ttyS0 reboot=k panic=1 pci=off"
};
await client.SetBootSource(bootSourceConfig);

DriveConfig rootDriveConfig = new()
{
    drive_id = "rootfs",
    path_on_host = "/home/testuser/ubuntu-24.04.ext4",
    is_root_device = true,
    is_read_only = false
};
await client.AddRootDrive(rootDriveConfig);

DriveConfig extraDriveConfig = new()
{
    drive_id = "extrafs",
    path_on_host = "",
    is_root_device = false,
    is_read_only = false
};
//await client.AddDrive(extraDriveConfig);

await client.StartInstance();

await Task.Delay(5000);

await client.StopInstance();

Console.WriteLine("Done");