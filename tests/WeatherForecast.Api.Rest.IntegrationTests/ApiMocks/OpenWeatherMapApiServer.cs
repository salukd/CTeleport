using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace WeatherForecast.Api.Rest.IntegrationTests.ApiMocks;

public class OpenWeatherMapApiServer
{
    private WireMockServer _server = null!;

    public string Url => "http://localhost:9092/";

    public void Start()
    {
        _server = WireMockServer.Start(new WireMockServerSettings
        {
            Urls = new[] { Url },
            ReadStaticMappings = true
        });
    }

    public void SetupSuccess()
    {
        _server!.Given(Request.Create()
                .WithPath($"/data/2.5/forecast")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithBody(GenerateResponseBody())
                .WithHeader("content-type", "application/json; charset=utf-8")
                .WithStatusCode(200));
    }

    public void Dispose()
    {
        _server!.Stop();
        _server.Dispose();
    }

    private string GenerateResponseBody()
    {
        var response = @"{
                ""cod"": ""200"",
                ""message"": 0,
                ""cnt"": 40,
                ""list"": [
                  {
                    ""dt"": 1722373200,
                    ""main"": {
                      ""temp"": 22.49,
                      ""feels_like"": 22.65,
                      ""temp_min"": 21.8,
                      ""temp_max"": 22.49,
                      ""pressure"": 1007,
                      ""sea_level"": 1007,
                      ""grnd_level"": 940,
                      ""humidity"": 71,
                      ""temp_kf"": 0.69
                    },
                    ""weather"": [
                      {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 0
                    },
                    ""wind"": {
                      ""speed"": 3.7,
                      ""deg"": 332,
                      ""gust"": 6.22
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-07-30 21:00:00""
                  },
                  {
                    ""dt"": 1722384000,
                    ""main"": {
                      ""temp"": 21.71,
                      ""feels_like"": 21.92,
                      ""temp_min"": 21.15,
                      ""temp_max"": 21.71,
                      ""pressure"": 1007,
                      ""sea_level"": 1007,
                      ""grnd_level"": 939,
                      ""humidity"": 76,
                      ""temp_kf"": 0.56
                    },
                    ""weather"": [
                      {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 0
                    },
                    ""wind"": {
                      ""speed"": 3.76,
                      ""deg"": 327,
                      ""gust"": 7.73
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-07-31 00:00:00""
                  },
                  {
                    ""dt"": 1722394800,
                    ""main"": {
                      ""temp"": 21.27,
                      ""feels_like"": 21.36,
                      ""temp_min"": 21.27,
                      ""temp_max"": 21.27,
                      ""pressure"": 1008,
                      ""sea_level"": 1008,
                      ""grnd_level"": 940,
                      ""humidity"": 73,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 801,
                        ""main"": ""Clouds"",
                        ""description"": ""few clouds"",
                        ""icon"": ""02d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 19
                    },
                    ""wind"": {
                      ""speed"": 4.93,
                      ""deg"": 325,
                      ""gust"": 10.27
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-07-31 03:00:00""
                  },
                  {
                    ""dt"": 1722405600,
                    ""main"": {
                      ""temp"": 24.39,
                      ""feels_like"": 24.45,
                      ""temp_min"": 24.39,
                      ""temp_max"": 24.39,
                      ""pressure"": 1008,
                      ""sea_level"": 1008,
                      ""grnd_level"": 941,
                      ""humidity"": 60,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 803,
                        ""main"": ""Clouds"",
                        ""description"": ""broken clouds"",
                        ""icon"": ""04d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 56
                    },
                    ""wind"": {
                      ""speed"": 5.12,
                      ""deg"": 327,
                      ""gust"": 8.64
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-07-31 06:00:00""
                  },
                  {
                    ""dt"": 1722416400,
                    ""main"": {
                      ""temp"": 29.59,
                      ""feels_like"": 29.03,
                      ""temp_min"": 29.59,
                      ""temp_max"": 29.59,
                      ""pressure"": 1007,
                      ""sea_level"": 1007,
                      ""grnd_level"": 940,
                      ""humidity"": 38,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 804,
                        ""main"": ""Clouds"",
                        ""description"": ""overcast clouds"",
                        ""icon"": ""04d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 100
                    },
                    ""wind"": {
                      ""speed"": 5.84,
                      ""deg"": 332,
                      ""gust"": 8.25
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-07-31 09:00:00""
                  },
                  {
                    ""dt"": 1722427200,
                    ""main"": {
                      ""temp"": 30.31,
                      ""feels_like"": 29.82,
                      ""temp_min"": 30.31,
                      ""temp_max"": 30.31,
                      ""pressure"": 1005,
                      ""sea_level"": 1005,
                      ""grnd_level"": 939,
                      ""humidity"": 38,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 500,
                        ""main"": ""Rain"",
                        ""description"": ""light rain"",
                        ""icon"": ""10d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 56
                    },
                    ""wind"": {
                      ""speed"": 6.51,
                      ""deg"": 349,
                      ""gust"": 7.93
                    },
                    ""visibility"": 10000,
                    ""pop"": 0.2,
                    ""rain"": {
                      ""3h"": 0.33
                    },
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-07-31 12:00:00""
                  },
                  {
                    ""dt"": 1722438000,
                    ""main"": {
                      ""temp"": 26.42,
                      ""feels_like"": 26.42,
                      ""temp_min"": 26.42,
                      ""temp_max"": 26.42,
                      ""pressure"": 1007,
                      ""sea_level"": 1007,
                      ""grnd_level"": 940,
                      ""humidity"": 50,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 500,
                        ""main"": ""Rain"",
                        ""description"": ""light rain"",
                        ""icon"": ""10d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 0
                    },
                    ""wind"": {
                      ""speed"": 7.33,
                      ""deg"": 345,
                      ""gust"": 11.54
                    },
                    ""visibility"": 10000,
                    ""pop"": 1,
                    ""rain"": {
                      ""3h"": 1.31
                    },
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-07-31 15:00:00""
                  },
                  {
                    ""dt"": 1722448800,
                    ""main"": {
                      ""temp"": 21.98,
                      ""feels_like"": 21.98,
                      ""temp_min"": 21.98,
                      ""temp_max"": 21.98,
                      ""pressure"": 1010,
                      ""sea_level"": 1010,
                      ""grnd_level"": 942,
                      ""humidity"": 67,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 500,
                        ""main"": ""Rain"",
                        ""description"": ""light rain"",
                        ""icon"": ""10n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 0
                    },
                    ""wind"": {
                      ""speed"": 6.44,
                      ""deg"": 326,
                      ""gust"": 13.05
                    },
                    ""visibility"": 10000,
                    ""pop"": 0.87,
                    ""rain"": {
                      ""3h"": 0.32
                    },
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-07-31 18:00:00""
                  },
                  {
                    ""dt"": 1722459600,
                    ""main"": {
                      ""temp"": 20.09,
                      ""feels_like"": 20.09,
                      ""temp_min"": 20.09,
                      ""temp_max"": 20.09,
                      ""pressure"": 1012,
                      ""sea_level"": 1012,
                      ""grnd_level"": 943,
                      ""humidity"": 74,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 0
                    },
                    ""wind"": {
                      ""speed"": 5.24,
                      ""deg"": 331,
                      ""gust"": 10.65
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-07-31 21:00:00""
                  },
                  {
                    ""dt"": 1722470400,
                    ""main"": {
                      ""temp"": 18.44,
                      ""feels_like"": 18.51,
                      ""temp_min"": 18.44,
                      ""temp_max"": 18.44,
                      ""pressure"": 1012,
                      ""sea_level"": 1012,
                      ""grnd_level"": 943,
                      ""humidity"": 83,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 0
                    },
                    ""wind"": {
                      ""speed"": 4.55,
                      ""deg"": 331,
                      ""gust"": 9.18
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-08-01 00:00:00""
                  },
                  {
                    ""dt"": 1722481200,
                    ""main"": {
                      ""temp"": 18.67,
                      ""feels_like"": 18.68,
                      ""temp_min"": 18.67,
                      ""temp_max"": 18.67,
                      ""pressure"": 1013,
                      ""sea_level"": 1013,
                      ""grnd_level"": 944,
                      ""humidity"": 80,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 3
                    },
                    ""wind"": {
                      ""speed"": 3.77,
                      ""deg"": 329,
                      ""gust"": 6.94
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-01 03:00:00""
                  },
                  {
                    ""dt"": 1722492000,
                    ""main"": {
                      ""temp"": 23.02,
                      ""feels_like"": 22.89,
                      ""temp_min"": 23.02,
                      ""temp_max"": 23.02,
                      ""pressure"": 1012,
                      ""sea_level"": 1012,
                      ""grnd_level"": 944,
                      ""humidity"": 58,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 802,
                        ""main"": ""Clouds"",
                        ""description"": ""scattered clouds"",
                        ""icon"": ""03d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 42
                    },
                    ""wind"": {
                      ""speed"": 3.59,
                      ""deg"": 330,
                      ""gust"": 4.68
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-01 06:00:00""
                  },
                  {
                    ""dt"": 1722502800,
                    ""main"": {
                      ""temp"": 28.48,
                      ""feels_like"": 28.1,
                      ""temp_min"": 28.48,
                      ""temp_max"": 28.48,
                      ""pressure"": 1011,
                      ""sea_level"": 1011,
                      ""grnd_level"": 943,
                      ""humidity"": 40,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 803,
                        ""main"": ""Clouds"",
                        ""description"": ""broken clouds"",
                        ""icon"": ""04d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 71
                    },
                    ""wind"": {
                      ""speed"": 3.04,
                      ""deg"": 342,
                      ""gust"": 3.11
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-01 09:00:00""
                  },
                  {
                    ""dt"": 1722513600,
                    ""main"": {
                      ""temp"": 30.67,
                      ""feels_like"": 30.24,
                      ""temp_min"": 30.67,
                      ""temp_max"": 30.67,
                      ""pressure"": 1009,
                      ""sea_level"": 1009,
                      ""grnd_level"": 942,
                      ""humidity"": 38,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 803,
                        ""main"": ""Clouds"",
                        ""description"": ""broken clouds"",
                        ""icon"": ""04d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 63
                    },
                    ""wind"": {
                      ""speed"": 2.55,
                      ""deg"": 40,
                      ""gust"": 1.99
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-01 12:00:00""
                  },
                  {
                    ""dt"": 1722524400,
                    ""main"": {
                      ""temp"": 28.38,
                      ""feels_like"": 28.6,
                      ""temp_min"": 28.38,
                      ""temp_max"": 28.38,
                      ""pressure"": 1009,
                      ""sea_level"": 1009,
                      ""grnd_level"": 942,
                      ""humidity"": 47,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 803,
                        ""main"": ""Clouds"",
                        ""description"": ""broken clouds"",
                        ""icon"": ""04d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 72
                    },
                    ""wind"": {
                      ""speed"": 2.11,
                      ""deg"": 90,
                      ""gust"": 3.05
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-01 15:00:00""
                  },
                  {
                    ""dt"": 1722535200,
                    ""main"": {
                      ""temp"": 25.66,
                      ""feels_like"": 25.72,
                      ""temp_min"": 25.66,
                      ""temp_max"": 25.66,
                      ""pressure"": 1011,
                      ""sea_level"": 1011,
                      ""grnd_level"": 943,
                      ""humidity"": 55,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 803,
                        ""main"": ""Clouds"",
                        ""description"": ""broken clouds"",
                        ""icon"": ""04n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 67
                    },
                    ""wind"": {
                      ""speed"": 2.72,
                      ""deg"": 326,
                      ""gust"": 2.84
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-08-01 18:00:00""
                  },
                  {
                    ""dt"": 1722546000,
                    ""main"": {
                      ""temp"": 22.97,
                      ""feels_like"": 23.12,
                      ""temp_min"": 22.97,
                      ""temp_max"": 22.97,
                      ""pressure"": 1011,
                      ""sea_level"": 1011,
                      ""grnd_level"": 943,
                      ""humidity"": 69,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 500,
                        ""main"": ""Rain"",
                        ""description"": ""light rain"",
                        ""icon"": ""10n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 100
                    },
                    ""wind"": {
                      ""speed"": 3.41,
                      ""deg"": 328,
                      ""gust"": 5.08
                    },
                    ""visibility"": 10000,
                    ""pop"": 0.65,
                    ""rain"": {
                      ""3h"": 0.55
                    },
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-08-01 21:00:00""
                  },
                  {
                    ""dt"": 1722556800,
                    ""main"": {
                      ""temp"": 21.58,
                      ""feels_like"": 21.86,
                      ""temp_min"": 21.58,
                      ""temp_max"": 21.58,
                      ""pressure"": 1012,
                      ""sea_level"": 1012,
                      ""grnd_level"": 943,
                      ""humidity"": 79,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 500,
                        ""main"": ""Rain"",
                        ""description"": ""light rain"",
                        ""icon"": ""10n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 96
                    },
                    ""wind"": {
                      ""speed"": 2.16,
                      ""deg"": 333,
                      ""gust"": 2.09
                    },
                    ""visibility"": 10000,
                    ""pop"": 0.92,
                    ""rain"": {
                      ""3h"": 0.63
                    },
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-08-02 00:00:00""
                  },
                  {
                    ""dt"": 1722567600,
                    ""main"": {
                      ""temp"": 22.24,
                      ""feels_like"": 22.48,
                      ""temp_min"": 22.24,
                      ""temp_max"": 22.24,
                      ""pressure"": 1012,
                      ""sea_level"": 1012,
                      ""grnd_level"": 944,
                      ""humidity"": 75,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 500,
                        ""main"": ""Rain"",
                        ""description"": ""light rain"",
                        ""icon"": ""10d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 100
                    },
                    ""wind"": {
                      ""speed"": 0.86,
                      ""deg"": 317,
                      ""gust"": 0.52
                    },
                    ""visibility"": 10000,
                    ""pop"": 0.2,
                    ""rain"": {
                      ""3h"": 0.1
                    },
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-02 03:00:00""
                  },
                  {
                    ""dt"": 1722578400,
                    ""main"": {
                      ""temp"": 24.93,
                      ""feels_like"": 24.94,
                      ""temp_min"": 24.93,
                      ""temp_max"": 24.93,
                      ""pressure"": 1012,
                      ""sea_level"": 1012,
                      ""grnd_level"": 944,
                      ""humidity"": 56,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 804,
                        ""main"": ""Clouds"",
                        ""description"": ""overcast clouds"",
                        ""icon"": ""04d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 96
                    },
                    ""wind"": {
                      ""speed"": 2.42,
                      ""deg"": 135,
                      ""gust"": 4.17
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-02 06:00:00""
                  },
                  {
                    ""dt"": 1722589200,
                    ""main"": {
                      ""temp"": 28.74,
                      ""feels_like"": 28.19,
                      ""temp_min"": 28.74,
                      ""temp_max"": 28.74,
                      ""pressure"": 1011,
                      ""sea_level"": 1011,
                      ""grnd_level"": 943,
                      ""humidity"": 38,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 803,
                        ""main"": ""Clouds"",
                        ""description"": ""broken clouds"",
                        ""icon"": ""04d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 66
                    },
                    ""wind"": {
                      ""speed"": 3.32,
                      ""deg"": 119,
                      ""gust"": 4.72
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-02 09:00:00""
                  },
                  {
                    ""dt"": 1722600000,
                    ""main"": {
                      ""temp"": 30.93,
                      ""feels_like"": 29.96,
                      ""temp_min"": 30.93,
                      ""temp_max"": 30.93,
                      ""pressure"": 1008,
                      ""sea_level"": 1008,
                      ""grnd_level"": 941,
                      ""humidity"": 33,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 803,
                        ""main"": ""Clouds"",
                        ""description"": ""broken clouds"",
                        ""icon"": ""04d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 52
                    },
                    ""wind"": {
                      ""speed"": 3.68,
                      ""deg"": 136,
                      ""gust"": 3.71
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-02 12:00:00""
                  },
                  {
                    ""dt"": 1722610800,
                    ""main"": {
                      ""temp"": 29.59,
                      ""feels_like"": 28.84,
                      ""temp_min"": 29.59,
                      ""temp_max"": 29.59,
                      ""pressure"": 1008,
                      ""sea_level"": 1008,
                      ""grnd_level"": 941,
                      ""humidity"": 36,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 801,
                        ""main"": ""Clouds"",
                        ""description"": ""few clouds"",
                        ""icon"": ""02d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 12
                    },
                    ""wind"": {
                      ""speed"": 3.49,
                      ""deg"": 140,
                      ""gust"": 3.21
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-02 15:00:00""
                  },
                  {
                    ""dt"": 1722621600,
                    ""main"": {
                      ""temp"": 25.46,
                      ""feels_like"": 25.29,
                      ""temp_min"": 25.46,
                      ""temp_max"": 25.46,
                      ""pressure"": 1009,
                      ""sea_level"": 1009,
                      ""grnd_level"": 942,
                      ""humidity"": 47,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 1
                    },
                    ""wind"": {
                      ""speed"": 0.41,
                      ""deg"": 16,
                      ""gust"": 1.03
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-08-02 18:00:00""
                  },
                  {
                    ""dt"": 1722632400,
                    ""main"": {
                      ""temp"": 24.24,
                      ""feels_like"": 24.03,
                      ""temp_min"": 24.24,
                      ""temp_max"": 24.24,
                      ""pressure"": 1009,
                      ""sea_level"": 1009,
                      ""grnd_level"": 941,
                      ""humidity"": 50,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 801,
                        ""main"": ""Clouds"",
                        ""description"": ""few clouds"",
                        ""icon"": ""02n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 22
                    },
                    ""wind"": {
                      ""speed"": 1.22,
                      ""deg"": 322,
                      ""gust"": 0.99
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-08-02 21:00:00""
                  },
                  {
                    ""dt"": 1722643200,
                    ""main"": {
                      ""temp"": 22.74,
                      ""feels_like"": 22.56,
                      ""temp_min"": 22.74,
                      ""temp_max"": 22.74,
                      ""pressure"": 1009,
                      ""sea_level"": 1009,
                      ""grnd_level"": 941,
                      ""humidity"": 57,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 801,
                        ""main"": ""Clouds"",
                        ""description"": ""few clouds"",
                        ""icon"": ""02n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 11
                    },
                    ""wind"": {
                      ""speed"": 1.69,
                      ""deg"": 317,
                      ""gust"": 1.68
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-08-03 00:00:00""
                  },
                  {
                    ""dt"": 1722654000,
                    ""main"": {
                      ""temp"": 22.85,
                      ""feels_like"": 22.71,
                      ""temp_min"": 22.85,
                      ""temp_max"": 22.85,
                      ""pressure"": 1009,
                      ""sea_level"": 1009,
                      ""grnd_level"": 941,
                      ""humidity"": 58,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 0
                    },
                    ""wind"": {
                      ""speed"": 1.81,
                      ""deg"": 318,
                      ""gust"": 1.88
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-03 03:00:00""
                  },
                  {
                    ""dt"": 1722664800,
                    ""main"": {
                      ""temp"": 28.61,
                      ""feels_like"": 28.3,
                      ""temp_min"": 28.61,
                      ""temp_max"": 28.61,
                      ""pressure"": 1008,
                      ""sea_level"": 1008,
                      ""grnd_level"": 940,
                      ""humidity"": 41,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 0
                    },
                    ""wind"": {
                      ""speed"": 1.29,
                      ""deg"": 142,
                      ""gust"": 1.63
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-03 06:00:00""
                  },
                  {
                    ""dt"": 1722675600,
                    ""main"": {
                      ""temp"": 32,
                      ""feels_like"": 31.19,
                      ""temp_min"": 32,
                      ""temp_max"": 32,
                      ""pressure"": 1006,
                      ""sea_level"": 1006,
                      ""grnd_level"": 939,
                      ""humidity"": 33,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 1
                    },
                    ""wind"": {
                      ""speed"": 3.4,
                      ""deg"": 137,
                      ""gust"": 4.19
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-03 09:00:00""
                  },
                  {
                    ""dt"": 1722686400,
                    ""main"": {
                      ""temp"": 33.98,
                      ""feels_like"": 33.09,
                      ""temp_min"": 33.98,
                      ""temp_max"": 33.98,
                      ""pressure"": 1004,
                      ""sea_level"": 1004,
                      ""grnd_level"": 937,
                      ""humidity"": 29,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 1
                    },
                    ""wind"": {
                      ""speed"": 3.84,
                      ""deg"": 142,
                      ""gust"": 4.05
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-03 12:00:00""
                  },
                  {
                    ""dt"": 1722697200,
                    ""main"": {
                      ""temp"": 32.78,
                      ""feels_like"": 31.6,
                      ""temp_min"": 32.78,
                      ""temp_max"": 32.78,
                      ""pressure"": 1003,
                      ""sea_level"": 1003,
                      ""grnd_level"": 937,
                      ""humidity"": 29,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 801,
                        ""main"": ""Clouds"",
                        ""description"": ""few clouds"",
                        ""icon"": ""02d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 13
                    },
                    ""wind"": {
                      ""speed"": 3.73,
                      ""deg"": 153,
                      ""gust"": 4.3
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-03 15:00:00""
                  },
                  {
                    ""dt"": 1722708000,
                    ""main"": {
                      ""temp"": 27.5,
                      ""feels_like"": 27.49,
                      ""temp_min"": 27.5,
                      ""temp_max"": 27.5,
                      ""pressure"": 1006,
                      ""sea_level"": 1006,
                      ""grnd_level"": 939,
                      ""humidity"": 44,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 500,
                        ""main"": ""Rain"",
                        ""description"": ""light rain"",
                        ""icon"": ""10n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 43
                    },
                    ""wind"": {
                      ""speed"": 1.31,
                      ""deg"": 345,
                      ""gust"": 0.95
                    },
                    ""visibility"": 10000,
                    ""pop"": 0.34,
                    ""rain"": {
                      ""3h"": 0.43
                    },
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-08-03 18:00:00""
                  },
                  {
                    ""dt"": 1722718800,
                    ""main"": {
                      ""temp"": 25.47,
                      ""feels_like"": 25.53,
                      ""temp_min"": 25.47,
                      ""temp_max"": 25.47,
                      ""pressure"": 1006,
                      ""sea_level"": 1006,
                      ""grnd_level"": 939,
                      ""humidity"": 56,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 500,
                        ""main"": ""Rain"",
                        ""description"": ""light rain"",
                        ""icon"": ""10n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 100
                    },
                    ""wind"": {
                      ""speed"": 2.67,
                      ""deg"": 326,
                      ""gust"": 2.97
                    },
                    ""visibility"": 10000,
                    ""pop"": 0.33,
                    ""rain"": {
                      ""3h"": 0.23
                    },
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-08-03 21:00:00""
                  },
                  {
                    ""dt"": 1722729600,
                    ""main"": {
                      ""temp"": 23.91,
                      ""feels_like"": 24.08,
                      ""temp_min"": 23.91,
                      ""temp_max"": 23.91,
                      ""pressure"": 1006,
                      ""sea_level"": 1006,
                      ""grnd_level"": 939,
                      ""humidity"": 66,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 500,
                        ""main"": ""Rain"",
                        ""description"": ""light rain"",
                        ""icon"": ""10n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 95
                    },
                    ""wind"": {
                      ""speed"": 2.24,
                      ""deg"": 323,
                      ""gust"": 2.68
                    },
                    ""visibility"": 10000,
                    ""pop"": 0.51,
                    ""rain"": {
                      ""3h"": 0.51
                    },
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-08-04 00:00:00""
                  },
                  {
                    ""dt"": 1722740400,
                    ""main"": {
                      ""temp"": 24.04,
                      ""feels_like"": 24.17,
                      ""temp_min"": 24.04,
                      ""temp_max"": 24.04,
                      ""pressure"": 1007,
                      ""sea_level"": 1007,
                      ""grnd_level"": 939,
                      ""humidity"": 64,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 802,
                        ""main"": ""Clouds"",
                        ""description"": ""scattered clouds"",
                        ""icon"": ""03d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 48
                    },
                    ""wind"": {
                      ""speed"": 1.68,
                      ""deg"": 324,
                      ""gust"": 1.86
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-04 03:00:00""
                  },
                  {
                    ""dt"": 1722751200,
                    ""main"": {
                      ""temp"": 29.75,
                      ""feels_like"": 29.87,
                      ""temp_min"": 29.75,
                      ""temp_max"": 29.75,
                      ""pressure"": 1006,
                      ""sea_level"": 1006,
                      ""grnd_level"": 939,
                      ""humidity"": 44,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 802,
                        ""main"": ""Clouds"",
                        ""description"": ""scattered clouds"",
                        ""icon"": ""03d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 28
                    },
                    ""wind"": {
                      ""speed"": 0.74,
                      ""deg"": 92,
                      ""gust"": 0.87
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-04 06:00:00""
                  },
                  {
                    ""dt"": 1722762000,
                    ""main"": {
                      ""temp"": 33.99,
                      ""feels_like"": 33.45,
                      ""temp_min"": 33.99,
                      ""temp_max"": 33.99,
                      ""pressure"": 1005,
                      ""sea_level"": 1005,
                      ""grnd_level"": 938,
                      ""humidity"": 31,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 0
                    },
                    ""wind"": {
                      ""speed"": 2.8,
                      ""deg"": 138,
                      ""gust"": 2.03
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-04 09:00:00""
                  },
                  {
                    ""dt"": 1722772800,
                    ""main"": {
                      ""temp"": 36.15,
                      ""feels_like"": 35.46,
                      ""temp_min"": 36.15,
                      ""temp_max"": 36.15,
                      ""pressure"": 1003,
                      ""sea_level"": 1003,
                      ""grnd_level"": 937,
                      ""humidity"": 26,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 800,
                        ""main"": ""Clear"",
                        ""description"": ""clear sky"",
                        ""icon"": ""01d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 0
                    },
                    ""wind"": {
                      ""speed"": 2.97,
                      ""deg"": 138,
                      ""gust"": 2.74
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-04 12:00:00""
                  },
                  {
                    ""dt"": 1722783600,
                    ""main"": {
                      ""temp"": 32.36,
                      ""feels_like"": 32.8,
                      ""temp_min"": 32.36,
                      ""temp_max"": 32.36,
                      ""pressure"": 1004,
                      ""sea_level"": 1004,
                      ""grnd_level"": 938,
                      ""humidity"": 40,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 801,
                        ""main"": ""Clouds"",
                        ""description"": ""few clouds"",
                        ""icon"": ""02d""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 16
                    },
                    ""wind"": {
                      ""speed"": 4.75,
                      ""deg"": 331,
                      ""gust"": 5.59
                    },
                    ""visibility"": 10000,
                    ""pop"": 0,
                    ""sys"": {
                      ""pod"": ""d""
                    },
                    ""dt_txt"": ""2024-08-04 15:00:00""
                  },
                  {
                    ""dt"": 1722794400,
                    ""main"": {
                      ""temp"": 25.21,
                      ""feels_like"": 25.54,
                      ""temp_min"": 25.21,
                      ""temp_max"": 25.21,
                      ""pressure"": 1008,
                      ""sea_level"": 1008,
                      ""grnd_level"": 941,
                      ""humidity"": 67,
                      ""temp_kf"": 0
                    },
                    ""weather"": [
                      {
                        ""id"": 500,
                        ""main"": ""Rain"",
                        ""description"": ""light rain"",
                        ""icon"": ""10n""
                      }
                    ],
                    ""clouds"": {
                      ""all"": 8
                    },
                    ""wind"": {
                      ""speed"": 6.35,
                      ""deg"": 330,
                      ""gust"": 11.74
                    },
                    ""visibility"": 10000,
                    ""pop"": 0.24,
                    ""rain"": {
                      ""3h"": 0.21
                    },
                    ""sys"": {
                      ""pod"": ""n""
                    },
                    ""dt_txt"": ""2024-08-04 18:00:00""
                  }
                ],
                ""city"": {
                  ""id"": 611717,
                  ""name"": ""Tbilisi"",
                  ""coord"": {
                    ""lat"": 41.6941,
                    ""lon"": 44.8337
                  },
                  ""country"": ""GE"",
                  ""population"": 1049498,
                  ""timezone"": 14400,
                  ""sunrise"": 1722304379,
                  ""sunset"": 1722356461
                }
              }";

        return response;
    }
}