using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Controller
{
    public class Command
    {
        public CommandType Type { get; private set; }
        public List<int> Data { get; private set; }
        public Command(CommandType type, List<List<string>> metaData = null)
        {
            Type = type;
            MetaData = metaData;
        }
        public List<List<string>> MetaData { get; set; }

        public void FillWithData(List<int> data) => Data = data;
    }

    public enum CommandType
    {
        Choose,
        Cancel,
        UseAbility,
        OpenShop,
    }
}
