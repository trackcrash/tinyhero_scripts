using Firebase;
using Firebase.Auth;
using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Google;
using Firebase.Database;

public class LoginManager : MonoBehaviour
{
    // public class User{
    //     public user(){
    //         this.
    //     }
    // }
    public InputField EmailInput, PasswordInput;
    private FirebaseAuth auth = null;
    private FirebaseUser user = null;
    

    //firebase database init

    private DatabaseReference reference;

    // 기기 연동이 되어 있는 상태인지 체크한다.
    private bool signedIn = false;
    
    private void Awake()
    {
        // 초기화
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        // 유저의 로그인 정보에 어떠한 변경점이 생기면 실행되게 이벤트를 걸어준다.
        auth.StateChanged += AuthStateChanged;
    }
    void start(){
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    
    // 계정 로그인에 어떠한 변경점이 발생시 진입.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            // 연동된 계정과 기기의 계정이 같다면 true를 리턴한다. 
            signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                UnityEngine.Debug.Log("Signed out " + user.UserId);
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                UnityEngine.Debug.Log("Signed in " + user.UserId);
                LoadingManager.Instance.LoadScene("Home");
            }
        }
    }
    
    // 익명 로그인
    public void AnonyLogin()
    {
        // 익명 로그인 진행
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            // 익명 로그인 연동 결과
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }
    
    // 구글 로그인
    public void GoogleLoginProcessing()
    {
        if (GoogleSignIn.Configuration == null)
        {
            // 설정
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                RequestEmail = true,
                WebClientId = ""
            };
        }
        
        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();

        signIn.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Google Login task.IsCanceled");
            }
            else if (task.IsFaulted)
            {
                Debug.Log("Google Login task.IsFaulted");
            }
            else
            {
                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
                {
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                        Debug.Log("Google Login authTask.IsCanceled");
                        return;
                    }
                    if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                        Debug.Log("Google Login authTask.IsFaulted");
                        return;
                    }

                    user = authTask.Result;
                    Debug.LogFormat("Google User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
                    return;
                });
            }
        });
    }


    //이메일 로그인
    public void EmailLogin()
    {
        // 적당한 UGUI 를 만들어 email, pw 를 입력받는다.
        var email = EmailInput.text;
        var pw = PasswordInput.text;

        if (email.Length < 1 || pw.Length < 1)
        {
            Debug.Log("이메일 ID 나 PW 가 비어있습니다.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, pw).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                UnityEngine.Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                UnityEngine.Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // firebase email user create
            Firebase.Auth.FirebaseUser newUser = task.Result;
            UnityEngine.Debug.LogFormat("Firebase Email user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            return;
        });
    }

    // 연동 해제
    public void SignOut()
    {
        if (auth.CurrentUser != null)
            auth.SignOut();
    }

    // 연동 계정 삭제
    public void UserDelete()
    {
        if (auth.CurrentUser != null)
            auth.CurrentUser.DeleteAsync();
    }
}