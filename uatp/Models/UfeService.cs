using System;

public class UfeService
{
    private  static readonly Lazy<UfeService>  _instance = new Lazy<UfeService>(() => new UfeService());
    public static UfeService Instance => _instance.Value;

    private decimal _currentFeeMultiplier;
    private DateTime _lastUpdate;

    private UfeService()
    {
        _currentFeeMultiplier = 1m; // Initial fee multiplier
        _lastUpdate = DateTime.UtcNow;
        UpdateFeeMultiplier();
    }

    public decimal GetFeeMultiplier()
    {
        if ((DateTime.UtcNow - _lastUpdate).TotalHours >= 1)
        {
            UpdateFeeMultiplier();
        }

        return _currentFeeMultiplier;
    }

    private void UpdateFeeMultiplier()
    {
        var random = new Random();
        _currentFeeMultiplier = (decimal)random.NextDouble() * 2; // Random decimal between 0 and 2
        _lastUpdate = DateTime.UtcNow;
    }
}
