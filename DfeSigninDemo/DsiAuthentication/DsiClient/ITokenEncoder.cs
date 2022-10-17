namespace DfeSigninDemo.DsiAuthentication.DsiClient
{
    public interface ITokenEncoder
    {
        string Base64Encode(byte[] stringInput);
    }
}