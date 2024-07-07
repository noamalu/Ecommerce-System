using Newtonsoft.Json.Linq;
using EcommerceAPI.Controllers;
using EcommerceAPI.Models.Dtos;
using MarketBackend.Services.Interfaces;
using MarketBackend.Domain.Shipping;
using MarketBackend.Domain.Payment;
using MarketBackend.Services;
using MarketBackend.DAL.DTO;

namespace EcommerceAPI.initialize;

public class Configurate
{
    private readonly IMarketService _service;
    private readonly IClientService _clientService;


    public Configurate(IMarketService service,  IClientService clientService)
    {
        _service = service;
        _clientService = clientService;
    }

    public string Parse()
    {
        string PATH = Path.Combine(Environment.CurrentDirectory, "initialize\\config.json");        

        string textJson = "";
        try
        {
            textJson = File.ReadAllText(PATH);
        }
        catch (Exception e)
        {            
            throw new Exception("open config file fail");
        }
        JObject scenarioDtoDict = JObject.Parse(textJson);
        if (scenarioDtoDict["Local"].Value<bool>())

            DBcontext.SetLocalDB();
        else
            DBcontext.SetRemoteDB();
        if (scenarioDtoDict["Initialize"].Value<bool>())
        {
            string initPATH = Path.Combine(Environment.CurrentDirectory, "initialize\\" + scenarioDtoDict["InitialState"]);
            DBcontext.GetInstance().Dispose();
            new SceanarioParser( _service, _clientService).Parse(initPATH);
        }
        if (scenarioDtoDict["Local"].Value<bool>())

            DBcontext.SetLocalDB();
        else
            DBcontext.SetRemoteDB();        
        return scenarioDtoDict["Port"].ToString();
    }

    public static bool VerifyJsonStructure(string filePath)
    {
        string expectedJson = @"
        {        
            ""AdminUsername"": ""string"",
            ""AdminPassword"": ""string"",
            ""InitialState"": ""string"",
            ""Port"": 0,
            ""ExternalServices"": false,        
            ""Local"": false,
            ""Initialize"": true
        }";

        JObject expectedObject = JObject.Parse(expectedJson);
        JObject actualObject = JObject.Parse(System.IO.File.ReadAllText(filePath));

        foreach (var property in expectedObject.Properties())
        {
            if (!actualObject.ContainsKey(property.Name) ||
                actualObject[property.Name].Type != GetJTokenType(property.Value))
            {
                return false;
            }
        }

        return true;
    }

    private static JTokenType GetJTokenType(JToken value)
    {
        if (value.Type == JTokenType.String)
        {
            return JTokenType.String;
        }
        else if (value.Type == JTokenType.Boolean)
        {
            return JTokenType.Boolean;
        }
        else if (value.Type == JTokenType.Integer || value.Type == JTokenType.Float)
        {
            return JTokenType.Integer;
        }

        return JTokenType.Null;
    }
}
