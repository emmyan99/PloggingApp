﻿namespace PloggingAPI.Models;

public class PloggingDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string RankingCollectionName { get; set; } = null!;
}
