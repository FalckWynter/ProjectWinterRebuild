using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.Linq;
namespace PlentyFishFramework
{
    public class MentionSystem : AbstractSystem
    {
        protected override void OnInit()
        {
        }

        public static void ShowMessage(string title,string description,bool isPermitSameMention = true)
        {
            float duration = (title.Length + description.Length) / 5; //average reading speed in English is c. 15 characters a second

            GameObject ob = UtilSystem.CreateMentionPrefab();
            ob.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            // If no duplicates are allowed, then find any duplicates and hide them
            if (!isPermitSameMention)
            {
                foreach (var window in GetDuplicateNotificationWindow(title, description))
                    window.Hide();
            }
            var mono = ob.GetComponent<NotificationWindow>();
            mono.SetDuration(duration);
            mono.Show();
            mono.SetDetails(title, description);
        }

        private static IEnumerable<NotificationWindow> GetDuplicateNotificationWindow(string title, string description)
        {
            return UtilModel.notifyHolder
                .GetComponentsInChildren<NotificationWindow>()
                .Where(window => window.Title == title && window.Description == description);
        }
    }
}