using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiteeFS;

namespace SleepBetter
{

    class Config
    {
        const string path = "SleepBetter.ini";
        readonly Authentication.AccessToken accessToken;
        readonly Utils.Repo repo;
        readonly FileSystem.Item item;

        public UInt32 Days { get; set; }

        public Config()
        {
            accessToken = Authentication.buildAccessToken(Properties.Resources.GiteeAccessToken);
            repo = new Utils.Repo(Properties.Resources.RepoOwner, Properties.Resources.RepoName);
            var downloaded = FileSystem.getFileByPath(
                Microsoft.FSharp.Core.FSharpOption<Authentication.AccessToken>.Some(accessToken), repo,path);

            if (downloaded.IsError)
                throw downloaded.ErrorValue;
            else
            {
                item = downloaded.ResultValue.Item1;

                var text = Encoding.UTF8.GetString(downloaded.ResultValue.Item2);
                var configsText = text.Split('\n');
                var config = new Dictionary<string, string>();
                foreach(var i in configsText)
                {
                    if (String.IsNullOrWhiteSpace(i)) continue;
                    var kv = i.Split('=');
                    config.Add(kv[0].Trim().Trim('\r').Trim(), kv[1].Trim().Trim('\r').Trim());
                }
                Days = uint.Parse(config["Days"]);
            }
        }

        public void Upload()
        {
            var cfg = new StringBuilder();

            cfg
                .Append("Days = ")
                .Append(Days);

            var blob =
                Encoding.UTF8.GetBytes(cfg.ToString());

            FileSystem.updateFile(
                accessToken, item, blob, "更新了睡眠记录");
        }
    }
}
