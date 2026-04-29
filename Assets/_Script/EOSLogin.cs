using UnityEngine;
using Epic.OnlineServices;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.UserInfo;
using PlayEveryWare.EpicOnlineServices;
using TMPro;

public class EOSLogin : MonoBehaviour
{
    public TMP_Text statusText;

    void Start()
    {
        var platform = EOSManager.Instance.GetEOSPlatformInterface();
        var auth = platform.GetAuthInterface();

        var loginOptions = new LoginOptions()
        {
            Credentials = new Credentials()
            {
                Type = LoginCredentialType.AccountPortal
            }
        };

        statusText.text = "Logging in...";

        auth.Login(ref loginOptions, null, (ref LoginCallbackInfo data) =>
        {
            Debug.Log("Login result: " + data.ResultCode);

            if (data.ResultCode == Result.Success)
            {
                statusText.text = "Login Success";
                var localUserId = data.LocalUserId;
                GetUserInfo(localUserId);
            }
            else
            {
                statusText.text = "Login Failed";
            }
        });
    }

    void GetUserInfo(EpicAccountId userId)
    {
        var platform = EOSManager.Instance.GetEOSPlatformInterface();
        var userInfo = platform.GetUserInfoInterface();

        var queryOptions = new QueryUserInfoOptions()
        {
            LocalUserId = userId,
            TargetUserId = userId
        };

        userInfo.QueryUserInfo(ref queryOptions, null, (ref QueryUserInfoCallbackInfo info) =>
        {
            if (info.ResultCode == Result.Success)
            {
                var copyOptions = new CopyUserInfoOptions()
                {
                    LocalUserId = userId,
                    TargetUserId = userId
                };

                if (userInfo.CopyUserInfo(ref copyOptions, out UserInfoData? userData) == Result.Success)
                {
                    Debug.Log("User Name: " + userData.Value.DisplayName);
                    statusText.text = "Hello, " + userData.Value.DisplayName;
                }
            }
            else
            {
                statusText.text = "Login ok but can't get name";
            }
        });
    }
}