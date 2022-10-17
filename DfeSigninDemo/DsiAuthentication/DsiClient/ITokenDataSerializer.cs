namespace DfeSigninDemo.DsiAuthentication.DsiClient
{
    public interface ITokenDataSerializer
    {
        string Serialize(object obj);
    }
}