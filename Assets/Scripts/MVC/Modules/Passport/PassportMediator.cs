
using DotHass.Unity.Net;
using DotHass.Unity;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using SDGame.UITools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace DotHassUnity
{
    public class PassportMediator : UIMediator
    {
        #region 控件绑定变量声明，自动生成请勿手改
        [ControlBinding]
        private RectTransform LoginBlock;
        [ControlBinding]
        private Image RegistedBlock;
        [ControlBinding]
        private RectTransform PlatformBlock;
        [ControlBinding]
        private Button EnterGameButton;
        [ControlBinding]
        private Button LoginButton;
        [ControlBinding]
        private Button GoRegButton;
        [ControlBinding]
        private InputField LoginPidText;
        [ControlBinding]
        private InputField LoginPwdText;
        [ControlBinding]
        private RectTransform RegBlock;
        [ControlBinding]
        private Button RegButton;
        [ControlBinding]
        private Button BackButton;
        [ControlBinding]
        private InputField RegPidText;
        [ControlBinding]
        private InputField RegPwdText;
        [ControlBinding]
        private InputField RegPwd2Text;
        [ControlBinding]
        private RectTransform LoginShake;
        [ControlBinding]
        private RectTransform RegShake;
        [ControlBinding]
        private Button RegistedButton;

        #endregion




        public static string TypeName { get; } = typeof(PassportMediator).Name;

        private IAudioService audioService;
        private SaveService saveService;
        private AppSetting setting;
        private PassportVo passportVo;
        public bool loading = false;

        public PassportProxy proxy
        {
            get
            {
                return Facade.RetrieveProxy(PassportProxy.TypeName) as PassportProxy;
            }
        }


        public PassportMediator(
            AppSetting setting,
            SaveService saveService,
            IAudioService audioService,
        UIControlData viewComponent) : base(TypeName, viewComponent)
        {
            this.audioService = audioService;
            this.saveService = saveService;
            this.setting = setting;
            this.passportVo = new PassportVo();
        }



        /// <summary>
        /// Called by the View when the Mediator is registered
        /// </summary>
        public override void OnRegister()
        {

            if (setting.platform == AppConst.DefaultPlatform)
            {
                LoginButton.GetOrAddComponent<PointerEventTrigger>().onClick += OnEventHandler;
                GoRegButton.GetOrAddComponent<PointerEventTrigger>().onClick += OnEventHandler;
                BackButton.GetOrAddComponent<PointerEventTrigger>().onClick += OnEventHandler;
                RegButton.GetOrAddComponent<PointerEventTrigger>().onClick += OnEventHandler;
                RegistedButton.GetOrAddComponent<PointerEventTrigger>().onClick += OnEventHandler;

                //设置pid
                var pid = saveService.Load<string>("pid", "");
                LoginPidText.text = pid;
            }
            else
            {
                LoginBlock.gameObject.SetActive(false);
                PlatformBlock.gameObject.SetActive(true);
                EnterGameButton.GetOrAddComponent<PointerEventTrigger>().onClick += OnEventHandler;
            }

        }

        /// <summary>
        /// Called by the View when the Mediator is removed
        /// </summary>
        public override void OnRemove()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string[] ListNotificationInterests()
        {
            return new string[] {
                NoticeConst.LOGIN_FAILED,
                NoticeConst.LOGIN_SUCCESS,
                NoticeConst.REG_SUCCESS,
                NoticeConst.REG_FAILED,
            };
        }

        /// <summary>
        /// Handle <c>INotification</c>s.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Typically this will be handled in a switch statement,
        ///         with one 'case' entry per <c>INotification</c>
        ///         the <c>Mediator</c> is interested in.
        ///     </para>
        /// </remarks>
        /// <param name="notification"></param>
        public override void HandleNotification(INotification notification)
        {
            loading = false;
            if (notification.Name == NoticeConst.LOGIN_SUCCESS)
            {
                //TODO:这里平台可以处理一些东西
                EnterGame();
            }
            else if (notification.Name == NoticeConst.LOGIN_FAILED)
            {
                var errorInfo = notification.Body as ErrorInfo;

                Debug.Log($"STATUCODE:{errorInfo.StateCode}");
            }
            else if (notification.Name == NoticeConst.REG_SUCCESS)
            {
                RegBlock.gameObject.SetActive(false);
                RegistedBlock.gameObject.SetActive(true);
            }
            else if (notification.Name == NoticeConst.REG_FAILED)
            {
                Debug.Log(notification.Body.ToString());
            }
        }

        public void EnterGame()
        {
            Facade.SendNotification(NoticeConst.PassportExit);
        }


        private void OnEventHandler(PointerEventData eventData)
        {
            if (loading == true)
            {
                Debug.Log("大侠，您慢一点...");
                return;
            }
            loading = true;

            if (eventData.pointerPress == LoginButton.gameObject) //登陆
            {
                passportVo.username = LoginPidText.text;
                passportVo.password = LoginPwdText.text;

                if (validate(passportVo.username) == false || validate(passportVo.password) == false)
                {
                    Debug.Log("大侠，你的通行文书有点问题哦");
                    loading = false;
                    return;
                }

                proxy.Login(passportVo);

            }
            else if (eventData.pointerPress == GoRegButton.gameObject)//切换到注册
            {
                this.LoginBlock.gameObject.SetActive(false);
                this.RegBlock.gameObject.SetActive(true);
                loading = false;
            }
            else if (eventData.pointerPress == RegButton.gameObject) //注册
            {
                passportVo.username = RegPidText.text;
                passportVo.password = RegPwdText.text;
                if (validate(passportVo.username) == false || validate(RegPwdText.text) == false || RegPwdText.text != RegPwd2Text.text)
                {
                    Debug.Log("大侠，你的通行文书有点问题哦");
                    loading = false;
                    return;
                }
                loading = false;
                proxy.Reg(passportVo);
            }
            else if (eventData.pointerPress == BackButton.gameObject) //切换回登陆
            {
                this.RegBlock.gameObject.SetActive(false);
                this.LoginBlock.gameObject.SetActive(true);
                loading = false;
            }
            else if (eventData.pointerPress == RegistedButton.gameObject)//注册完成进入游戏
            {
                loading = false;
                EnterGame();
            }
            else if (eventData.pointerPress == EnterGameButton.gameObject)//平台直接进入游戏
            {
                loading = false;
                passportVo.username = "";
                passportVo.password = "";
                proxy.Login(passportVo);
            }
        }


        public bool validate(string str)
        {
            return !string.IsNullOrEmpty(str) && str.Length >= 6 && str.Length <= 16;
        }

    }
}
