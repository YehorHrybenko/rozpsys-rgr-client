using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using static SwarmData;

public class SwarmClient : MonoBehaviour
{
    private Dictionary<int, Vector3> controls = new();

    readonly HttpClient client = new HttpClient(new HttpClientHandler())
    {
        BaseAddress = new Uri("http://localhost:8080")
    };

    public static SwarmClient Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public void UpdateRequest(Dictionary<int, DroneData> data)
    {
        var tgt = SwarmTargetManager.currentTarget.transform.position;

        var requestData = JsonConvert.SerializeObject(new Dictionary<string, string>() {
            ["droneCount"] = data.Count.ToString(),
            ["data"] = JsonConvert.SerializeObject(data),
            ["altitude"] = 20f.ToString(),
            ["target"] = JsonConvert.SerializeObject(SerializableVector.FromVector3(tgt))
        });

        var res = Task.Run(async () =>
        {
            try
            {
                var content = new StringContent(requestData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"/update", content);
                response.EnsureSuccessStatusCode();
                var resString = await response.Content.ReadAsStringAsync();
                var resCustomVector = JsonConvert.DeserializeObject<Dictionary<int, SerializableVector>>(resString);
                var res = resCustomVector.Select((v) => (v.Key, v.Value.ToVector3())).ToDictionary(v => v.Key, v => v.Item2);
                lock (controls) 
                {
                   controls = res;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        });
    }

    public Dictionary<int, Vector3>GetControls()
    {
        return controls;
    }

    public static void UpdateGroup(Dictionary<int, DroneData> groupData) => Instance.UpdateRequest(groupData); 

    public static (bool, Leadable) GetLeader(Leadable leadable) => SwarmRoles.GetLeader(leadable);
}
