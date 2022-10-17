using System.Collections.Generic;

namespace DfeSigninDemo.DsiAuthentication.DsiClient
{
    public interface ITokenData
    {
        IDictionary<string, object> Header { get; set; }
        IDictionary<string, object> Payload { get; set; }
    }
}