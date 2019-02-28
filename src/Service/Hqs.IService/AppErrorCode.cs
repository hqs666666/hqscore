using System.ComponentModel.DataAnnotations;

namespace Hqs.IService
{
    public enum AppErrorCode
    {
        [Display(Name = "成功")]
        Success = 0,

        [Display(Name = "失败")]
        Exception = 1,

        #region 10XX 

        [Display(Name = "缺少版本号")]
        VersionRequired = 1000,

        [Display(Name = "缺少 appid")]
        AppIdRequired = 1001,

        [Display(Name = "缺少 secret")]
        SecretRequired = 1002,

        [Display(Name = "缺少 access_token")]
        AccessTokenRequired = 1003,

        [Display(Name = "缺少 refresh_token")]
        RefreshTokenRequired = 1004,

        [Display(Name = "缺少 oauth_code")]
        OAuthCodeRequired = 1005,

        [Display(Name = "缺少 timestamp")]
        TimestampRequired = 1006,

        [Display(Name = "缺少 nonce")]
        NonceRequired = 1007,

        [Display(Name = "缺少 signature")]
        SignatureRequired = 1008,

        [Display(Name = "缺少 stage")]
        StageRequired = 1009,

        [Display(Name = "缺少 content-md5")]
        ContentMd5Required = 1010,

        #endregion

        #region 11XX

        [Display(Name = "Invalid Url")]
        InvalidUrl = 1100,

        [Display(Name = "不合法的请求格式")]
        InvalidRequestFormat = 1101,

        [Display(Name = "不合法的请求参数")]
        InvalidRequestParams = 1102,

        [Display(Name = "不合法的凭证类型")]
        InvalidGrantType = 1103,

        [Display(Name = "不合法的 appid")]
        InvalidAppId = 1104,

        [Display(Name = "不合法的 secret")]
        InvalidSecret = 1105,

        [Display(Name = "不合法的 access_token")]
        InvalidAccessToken = 1106,

        [Display(Name = "不合法的 refresh_token")]
        InvalidRefreshToken = 1107,

        [Display(Name = "不合法的 oauth_code")]
        InvalidOAuthCode = 1108,

        [Display(Name = "不合法的 timestamp")]
        InvalidTimestamp = 1109,

        [Display(Name = "不合法的 nonce")]
        InvalidNonce = 1110,

        [Display(Name = "签名验证失败")]
        InvalidSignature = 1111,

        [Display(Name = "无效的签名密钥")]
        InvalidSignToken = 1112,

        [Display(Name = "没有操作权限")]
        PermissionDenied = 1113,

        #endregion
    }
}
