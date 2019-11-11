using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;

namespace yrno
{
    public class Forecast
    {
        private readonly HttpClient _client;
        private List<ForecastRecordModel> _records;

        public Forecast(HttpClient client)
        {
            _client = client;
        }

        public List<ForecastRecordModel> Get()
        {
            try
            {
                // ziskame XML data
                var response = _client.GetAsync($"https://www.yr.no/place/Czech_Republic/Zl%C3%ADn/Vizovice/forecast.xml");
                var respXml = response.Result.Content.ReadAsStringAsync().Result;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(respXml);
                string xpath = "weatherdata/forecast/tabular/time";
                var nodes = xmlDoc.SelectNodes(xpath);

                _records = new List<ForecastRecordModel>(40);

                foreach (XmlNode node in nodes)
                {
                    string from = node.Attributes["from"].Value;
                    string to = node.Attributes["to"].Value;
                    string temperature = node.SelectSingleNode("temperature").Attributes["value"].Value;

                    ForecastRecordModel frm = new ForecastRecordModel()
                    {
                        From = DateTime.Parse(from),
                        To = DateTime.Parse(to),
                        Temperature = Decimal.Parse(temperature)
                    };

                    _records.Add(frm);
                }
            }
            catch (Exception)
            {
                return null;
            }

            // setřídíme výsledky a vrátíme jako nový seznam
            return _records.OrderBy(x => x.From).ToList();
        }

    }
}
