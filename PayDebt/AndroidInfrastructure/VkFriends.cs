using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Util;
using Infrastructure;
using Org.Json;
using VKontakte.API;

namespace PayDebt.AndroidInfrastructure
{
    internal static class VkFriends
    {
        public static async Task<List<Tuple<int, string>>> GetVKFriends()
        {
            var parameters = new VKParameters();
            parameters.Put("order", "hints");
            //parameters.Put(VKApiConst.UserId, VKApiConst.OwnerId);
            parameters.Put(VKApiConst.Fields, "first_name,last_name");
            var request = VKApi.Friends.Get(parameters);
            request.Attempts = 5;
            return ParseVKGetFriendsRequest(await request.ExecuteAsync(null, AttemptFailed));
        }

        private static void AttemptFailed(VKRequest arg1, int arg2, int arg3)
        {
            throw new SystemException();
        }

        private static List<Tuple<int, string>> ParseVKGetFriendsRequest(VKResponse response)
        {
            var json = response.Json;
            if (!json.Has("response"))
            {
                Log.Error("PayDebt", "Invalid API response: " + response.Json);
                return new List<Tuple<int, string>>();
            }

            json = json.GetJSONObject("response");

            var list = new List<Tuple<int, string>>(json.Has("count") ? json.GetInt("count") : 10);

            if (json.Has("items"))
            {
                var jsonArray = json.GetJSONArray("items");
                jsonArray.Length();
                for (var i = 0; i < jsonArray.Length(); i++)
                    list.Add(ParseIdNamePair(jsonArray.GetJSONObject(i)));
            }

            return list;
        }

        private static Tuple<int, string> ParseIdNamePair(JSONObject obj)
        {
            if (!obj.Has("id")) return null;

            var firstName = obj.Has("first_name") ? obj.GetString("first_name") : "";
            var lastName = obj.Has("last_name") ? obj.GetString("last_name") : "";

            var name = string.Join(" ", new[] { firstName, lastName }.GetAllNonEmpty());

            return Tuple.Create(obj.GetInt("id"), name);
        }
    }
}