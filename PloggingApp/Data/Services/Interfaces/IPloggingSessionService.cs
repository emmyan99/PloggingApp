﻿using Plogging.Core.Enums;
using Plogging.Core.Models;

namespace PloggingApp.Data.Services.Interfaces
{
    public interface IPloggingSessionService
    {
        Task SavePloggingSession(PloggingSession ploggingSession);
        string UserId { get; set; }
        Task<IEnumerable<PloggingSession>> GetUserSessions(string UserId, DateTime startDate, DateTime endDate);
    }
}