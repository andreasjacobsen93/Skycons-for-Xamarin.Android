using Android.Content;
using Android.Views;

namespace Learn2Run.UI.Android.Skycons
{
    public class WeatherUtils
    {
        private Context context;


        /*
         * 
         * 
         * Parse more data from 
         * http://openweathermap.org/weather-conditions
         * 
         * 
         * 
         * 
         */

        public WeatherUtils(Context context)
        {
            this.context = context;
        }

        public string GetTypeFromIcon(string icon)
        {
            var text = "";
            switch (icon)
            {
                case "01d":
                    text = "Clear Day";
                    break;
                case "01n":
                    text = "Clear Night";
                    break;
                case "02d":
                    text = "Few Clouds Day";
                    break;
                case "02n":
                    text = "Few Clouds Night";
                    break;
                case "03d":
                    text = "Scattered Clouds Day";
                    break;
                case "03n":
                    text = "Scattered Clouds Night";
                    break;
                case "04d":
                    text = "Broken Clouds Day";
                    break;
                case "04n":
                    text = "Broken Clouds Night";
                    break;
                case "09d":
                    text = "Shower Rain Day";
                    break;
                case "09n":
                    text = "Shower Rain Night";
                    break;
                case "10d":
                    text = "Rain Day";
                    break;
                case "10n":
                    text = "Rain Night";
                    break;
                case "11d":
                    text = "Thunderstorm Day";
                    break;
                case "11n":
                    text = "Thunderstorm Night";
                    break;
                case "13d":
                    text = "Snow Day";
                    break;
                case "13n":
                    text = "Snow Night";
                    break;
                case "50d":
                    text = "Mist Day";
                    break;
                case "50n":
                    text = "Mist Night";
                    break;
                default:
                    text = "";
                    break;
            }
            return text;
        }

        public View GetViewFromIcon(string icon)
        {
            switch (icon)
            {
                case "01d":
                    return new SunView(context);
                case "01n":
                    return new MoonView(context);
                case "02d":
                    return new CloudSunView(context);
                case "02n":
                    return new CloudMoonView(context);
                case "03d":
                    return new CloudView(context);
                case "03n":
                    return new CloudView(context);
                case "04d":
                    return new CloudView(context);
                case "04n":
                    return new CloudView(context);
                case "09d":
                    return new CloudHvRainView(context);
                case "09n":
                    return new CloudHvRainView(context);
                case "10d":
                    return new CloudRainView(context);
                case "10n":
                    return new CloudRainView(context);
                case "11d":
                    return new CloudThunderView(context);
                case "11n":
                    return new CloudThunderView(context);
                case "13d":
                    return new CloudSnowView(context);
                case "13n":
                    return new CloudSnowView(context);
                case "50d":
                    return new CloudFogView(context);
                case "50n":
                    return new CloudFogView(context);
                default:
                    return new CloudView(context);
            }
        }
    }
}
