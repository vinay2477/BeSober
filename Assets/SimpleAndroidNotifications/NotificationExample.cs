using System;
using UnityEngine;

namespace Assets.SimpleAndroidNotifications
{
    public class NotificationExample : MonoBehaviour
    {
        private void Start()
        {
            ScheduleNormal();
            ScheduleNormal2();
            ScheduleNormal3();
        }

        public void Rate()
        {
            Application.OpenURL("http://u3d.as/y6r");
        }

        public void OpenWiki()
        {
            Application.OpenURL("https://github.com/hippogamesunity/SimpleAndroidNotificationsPublic/wiki");
        }

        public void ScheduleSimple()
        {
            NotificationManager.Send(TimeSpan.FromSeconds(5), "Simple notification", "Customize icon and color", new Color(1, 0.3f, 0.15f));
        }

        public void ScheduleNormal()
        {
            NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(5), "Alert Zone", "A lot can happen over coffee! Try Sugar Brown's Coffee.", new Color(0, 0.6f, 1), NotificationIcon.Message);
        }

        public void ScheduleNormal2()
        {
           NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(15), "Bed Time", "Its almost time for bed. How was your day?", new Color(0, 0.6f, 1), NotificationIcon.Message);
        }

        public void ScheduleNormal3()
        {
            NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(10), "Good Morning", "Drinking water is essential to a healthy lifestyle.", new Color(0, 0.6f, 1), NotificationIcon.Message);
        }

        public void ScheduleCustom()
        {
            var notificationParams = new NotificationParams
            {
                Id = UnityEngine.Random.Range(0, int.MaxValue),
                Delay = TimeSpan.FromSeconds(5),
                Title = "Custom notification",
                Message = "Message",
                Ticker = "Ticker",
                Sound = true,
                Vibrate = true,
                Light = true,
                SmallIcon = NotificationIcon.Heart,
                SmallIconColor = new Color(0, 0.5f, 0),
                LargeIcon = "app_icon"
            };

            NotificationManager.SendCustom(notificationParams);
        }

        public void CancelAll()
        {
            NotificationManager.CancelAll();
        }
    }
}