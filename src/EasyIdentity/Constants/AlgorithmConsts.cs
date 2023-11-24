namespace EasyIdentity.Constants;

public static class AlgorithmConsts
{
    public const string RsaSigningAlgorithm_RS256 = "RS256";
    public const string RsaSigningAlgorithm_RS384 = "RS384";
    public const string RsaSigningAlgorithm_RS512 = "RS512";

    public const string ECDsaSigningAlgorithm_ES256 = "ES256";
    public const string ECDsaSigningAlgorithm_ES384 = "ES384";
    public const string ECDsaSigningAlgorithm_ES512 = "ES512";

    public enum RsaSigningAlgorithm
    {
        RS256 = 0,
        RS384 = 1,
        RS512 = 2,

        PS256 = 3,
        PS384 = 4,
        PS512 = 5
    }

    public enum ECDsaSigningAlgorithm
    {
        ES256 = 0,
        ES384 = 1,
        ES512 = 2
    }
}
