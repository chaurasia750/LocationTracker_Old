using LocationTracker.Enum;
using LocationTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LocationTracker.ApiServices
{
    public class TourService
    {
        public async Task<Tuple<bool, string, List<Tour>>> GetAllTours(ReportType reportType)
        {
            var tours = new List<Tour>();
            var url = DifferentUrls.GetAllTours;
            var isSuccess = false;
            string outErrorMessage = "";

            var queryString = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                               {
                                new KeyValuePair<string, string>("Username", Application.Current.Properties["Username"].ToString()),
                                new KeyValuePair<string, string>("Password", Application.Current.Properties["Password"].ToString())
                             });

            using (var httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, queryString);
                HttpResponseMessage response = await getResponse;
                if (response.IsSuccessStatusCode)
                {
                    var myContent = await response.Content.ReadAsStringAsync();
                    if (myContent == "False")
                    {
                        outErrorMessage = "Something Went Wrong.Sign In Again!!!";
                    }
                    else
                    {
                        tours = JsonConvert.DeserializeObject<List<Tour>>(myContent);
                        tours.ForEach(x => x.OwnwardStartDate = JsonConvert.DeserializeObject<DateTime>(x.OnwardFieldStartDate));
                        tours.ForEach(x => x.OwnwardEndDate = JsonConvert.DeserializeObject<DateTime>(x.OnwardFieldEndDate));

                        if (ReportType.DateTime== reportType)
                        {
                            tours.ForEach(x => x.OwnwardStartDateTime = $"{JsonConvert.DeserializeObject<DateTime>(x.OnwardFieldStartDate).ToString("D")}" +
                            $"{JsonConvert.DeserializeObject<DateTime>(x.OnwardFieldStartTime).ToString("t")}");
                            tours.ForEach(x => x.OwnwardEndDateTime = $"{JsonConvert.DeserializeObject<DateTime>(x.OnwardFieldEndDate).ToString("D")}"+
                            $"{JsonConvert.DeserializeObject<DateTime>(x.OnwardFieldStartTime).ToString("t")}");
                        }
                        else
                        {
                            tours.ForEach(x => x.OwnwardStartDateTime = $"{JsonConvert.DeserializeObject<DateTime>(x.OnwardFieldStartDate).ToString("D")} ");
                            tours.ForEach(x => x.OwnwardEndDateTime = $"{JsonConvert.DeserializeObject<DateTime>(x.OnwardFieldEndDate).ToString("D")} ");
                        }
                        

                        tours = tours.OrderBy(x => x.OwnwardStartDate).ToList();
                        isSuccess = true;
                    }

                }

            }

            return new Tuple<bool, string, List<Tour>>(isSuccess, outErrorMessage, tours);
        }
    }
}
