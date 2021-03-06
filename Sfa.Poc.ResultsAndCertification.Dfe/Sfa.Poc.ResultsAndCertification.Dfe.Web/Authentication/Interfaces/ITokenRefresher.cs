﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Web.Authentication.Interfaces
{
    /// <summary>
    /// Provides an easy way to ensure the user's access token is up to date. 
    /// </summary>
    public interface ITokenRefresher
    {
        /// <summary>
        /// Tries to refresh the current user's access token if required.
        /// </summary>
        /// <param name="refreshToken">The current refresh token.</param>
        /// <param name="expiresAt">The current token expiration information.</param>
        /// <param name="ct">The async cancellation token.</param>
        /// <returns><code>True</code> if refresh is not needed or executed successfully, <code>False</code> otherwise.</returns>
        Task<TokenRefreshResult> TryRefreshTokenIfRequiredAsync(
            string refreshToken,
            string expiresAt,
            CancellationToken ct);
    }
}
