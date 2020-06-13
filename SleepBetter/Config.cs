using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiteeDrive;

namespace SleepBetter
{

    class Config
    {
        const string path = "SleepBetter.ini";
        readonly AccessToken accessToken;
        readonly Repo repo;
        readonly Item item;

        public UInt32 Days { get; set; }

        public Config()
        {
            accessToken = GiteeDrive.AccessToken.NewAccessToken(SleepBetter.Personal.GiteeRepo.GiteeAccessToken);
            repo = new Repo(SleepBetter.Personal.GiteeRepo.GiteeRepoOwner, SleepBetter.Personal.GiteeRepo.GiteeRepoName, "master");
            var root = 
                from i in GiteeDrive.RepoModule.getRoot(accessToken,false , repo)
                where i.IsFile
                where (i as Item.File).Item1.path == path
                select i;
            item = root.First();

            
            var text = ItemModule.downloadString(accessToken, item);
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

        public void Upload()
        {
            var cfg = new StringBuilder();

            cfg
                .Append("Days = ")
                .Append(Days);

            var blob =
                Encoding.UTF8.GetBytes(cfg.ToString());

            ItemModule.updateString(
                accessToken, "更新了睡眠记录", cfg.ToString(),item);
        }
    }
}
