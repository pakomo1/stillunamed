using System.Collections.Generic;
using System.Linq;
using System.Web;

public class OAuth2
{
    #region Class functionality

    /// <summary>
    /// Construct a OAuth2 forwarding URI to redirect with.
    /// </summary>  
    /// <param name="provider">OAuth2 provider wrapper.</param>
    /// <param name="redirectUri">URI to redirect back to the system.</param>
    /// <param name="locale">Language locale for provider interface.</param>
    /// <returns>URI to redirect system to, for user authorization.</returns>
    public static string CreateRedirect(OAuth2Provider provider, string redirectUri, string locale = "en")
    {
        var parameters = new Dictionary<string, string> {
            {"client_id", provider.ClientId},
            {"display", "page"},
            {"locale", locale},
            {"redirect_uri", redirectUri},
            {"response_type", "code"}
        };

        if (provider.Offline)
            parameters.Add(
                "access_type",
                "offline");

        if (!string.IsNullOrWhiteSpace(provider.Scope))
            parameters.Add(
                "scope",
                provider.Scope);

        if (!string.IsNullOrWhiteSpace(provider.State))
            parameters.Add(
                "state",
                provider.State);

        var qs = buildQueryString(parameters);
        var url =
            provider.AuthUri + "?" +
            qs;

        return url;
    }

    #endregion
    #region Helper functions
    /// <summary>
    /// Construct a query-string from dictionary.
    /// </summary>
    /// <param name="parameters">Set of parameters in dictionary form to construct from.</param>
    /// <returns>Query-string.</returns>
    private static string buildQueryString(Dictionary<string, string> parameters)
    {
        return parameters.Aggregate(
            "",
            (c, p) => c + ("&" + p.Key + "=" + HttpUtility.UrlEncode(p.Value)))
            .Substring(1);
    }
    #endregion
}

/// <summary>
/// OAuth2 provider wrapper.
/// </summary>
public class OAuth2Provider
{
    public string ClientId { get; set; }
    public string AuthUri { get; set; }
    public string AccessTokenUri { get; set; }
    public string Scope { get; set; }
    public string State { get; set; }
    public bool Offline = false;
}
