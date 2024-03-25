using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICredentialProvider
{
    string GetAccessToken();
    void StoreAccessToken(string token);
}
