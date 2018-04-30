using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Controller
{
    class Command
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

    enum CommandType
    {
        Choose,
        Cancel,
        UseAbility,
        OpenShop,
    }
}
