namespace WifiWeather.Models;

public sealed class WeatherCodeConstants
{
    public const int THUNDERSTORM_LIGHT_RAIN = 200;
    public const int THUNDERSTORM_RAIN = 201;
    public const int THUNDERSTORM_HEAVY_RAIN = 202;
    public const int THUNDERSTORM_LIGHT = 210;
    public const int THUNDERSTORM = 211;
    public const int THUNDERSTORM_HEAVY = 212;
    public const int THUNDERSTORM_RAGGED = 221;
    public const int THUNDERSTORM_LIGHT_DRIZZLE = 230;
    public const int THUNDERSTORM_DRIZZLE = 231;
    public const int THUNDERSTORM_HEAVY_DRIZZLE = 232;

    public const int DRIZZLE_LIGHT = 300;
    public const int DRIZZLE = 301;
    public const int DRIZZLE_HEAVY = 302;
    public const int DRIZZLE_LIGHT_RAIN = 310;
    public const int DRIZZLE_RAIN = 311;
    public const int DRIZZLE_HEAVY_RAIN = 312;
    public const int DRIZZLE_SHOWER_RAIN = 313;
    public const int DRIZZLE_SHOWER_HEAVY = 314;
    public const int DRIZZLE_SHOWER = 321;

    public const int RAIN_LIGHT = 500;
    public const int RAIN_MODERATE = 501;
    public const int RAIN_HEAVY = 502;
    public const int RAIN_VERY_HEAVY = 503;
    public const int RAIN_EXTREME = 504;
    public const int RAIN_FREEZING = 511;
    public const int RAIN_SHOWER_LIGHT = 520;
    public const int RAIN_SHOWER = 521;
    public const int RAIN_SHOWER_HEAVY = 522;
    public const int RAIN_SHOWER_RAGGED = 531;

    public const int SNOW_LIGHT = 600;
    public const int SNOW = 601;
    public const int SNOW_HEAVY = 602;
    public const int SLEET = 611;
    public const int SNOW_SHOWER_SLEET_LIGHT = 612;
    public const int SNOW_SHOWER_SLEET = 613;
    public const int SNOW_RAIN_LIGHT = 615;
    public const int SNOW_RAIN = 621;
    public const int SNOW_SHOWER_LIGHT = 622;
    public const int SNOW_SHOWER = 631;
    public const int SNOW_SHOWER_HEAVY = 631;

    public const int MIST = 701;
    public const int SMOKE = 711;
    public const int HAZE = 721;
    public const int DUST_SAND = 731;
    public const int FOG = 741;
    public const int SAND = 751;
    public const int DUST = 761;
    public const int ASH = 762;
    public const int SQUALL = 771;
    public const int TORNADO = 781;

    public const int CLOUDS_CLEAR = 800;
    public const int CLOUDS_FEW = 801;
    public const int CLOUDS_SCATTERED = 802;
    public const int CLOUDS_BROKEN = 803;
    public const int CLOUDS_OVERCAST = 804;

    private static readonly WeatherCodeConstants instance = new WeatherCodeConstants();

    static WeatherCodeConstants() { }

    public static WeatherCodeConstants Instance
    {
        get
        {
            return instance;
        }
    }
}
