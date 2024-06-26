using Rocket.API;
using Rocket.API.Extensions;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using BVAtt;

namespace BVAtt.Translate
{
    public static class Extensions
    {
        public static string Translate(this string Translation, params object[] Args)
        {
            if (AttPlugin.Instance.Translations.Instance[Translation] != null)
            {
                return AttPlugin.Instance.Translate(Translation, placeholder: Args);
            }
            else
            {
                return AttPlugin.Instance.DefaultTranslations.Translate(Translation, placeholder: Args);
            }
        }

    }
}