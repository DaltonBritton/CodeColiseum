using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using FireCrackerAPI.Configs;

namespace FireCrackerAPI;

public class FireCrackerClient
{
    private readonly HttpClient _httpClient;
    
    public FireCrackerClient(UnixDomainSocketEndPoint endpoint)
    {
        _httpClient = new HttpClient(new SocketsHttpHandler
        {
            ConnectCallback = async (_, _) =>
            {
                var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
                await socket.ConnectAsync(endpoint);
                return new NetworkStream(socket, ownsSocket: true);
            }
        });

        // Doesn't matter just needs to be something as it will be overwritten by handler defined above
        _httpClient.BaseAddress = new Uri("http://localhost:5000");
    }

    public async Task Test()
    {

    }
    
    public async Task SetMachineConfig(MachineConfig machineConfig)
    {
        await SendRequest("PUT", "/machine-config", machineConfig);
    }

    public async Task SetBootSource(BootSourceConfig bootSourceConfig)
    {
        await SendRequest("PUT", "/boot-source", bootSourceConfig);
    }

    public async Task AddRootDrive(DriveConfig driveConfig)
    {
        await SendRequest("PUT", "/drives/rootfs", driveConfig);
    }
    public async Task AddDrive(DriveConfig driveConfig)
    {
        await SendRequest("PUT", "/drives/extra_drive", driveConfig);
    }
    
    public async Task StartInstance()
    {
        var payload = new { action_type = "InstanceStart" };
        await SendRequest("PUT", "/actions", payload);
    }

    public async Task StopInstance()
    {
        var payload = new { action_type = "SendCtrlAltDel" };
        await SendRequest("PUT", "/actions", payload);
    }

    public async Task ForceStopInstance()
    {
        var payload = new { action_type = "Exit" };
        await SendRequest("PUT", "/actions", payload);
    }
    
    private async Task SendRequest<T>(string method, string endpoint, T payload)
    {
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = method switch
        {
            "PUT" => await _httpClient.PutAsync(endpoint, content),
            "POST" => await _httpClient.PostAsync(endpoint, content),
            _ => throw new NotImplementedException($"Method {method} is not implemented")
        };

        string responseText = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"[{DateTime.UtcNow}] {method} {endpoint} -> {response.StatusCode} : {responseText}");
    }
}