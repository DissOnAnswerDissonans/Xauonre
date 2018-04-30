using MiniXauonre.Controller;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    class Skill
    {
        public List<SkillType> SkillTypes { get; protected set; }
        public string Explanation { get; set; }
        public string Name { get; set; }

        public Action<Map, Player, Hero> Work  { get; set; }

        public Skill()
        {
            SkillTypes = new List<SkillType>();
            Name = "Base";
            Explanation = "Nothing";
            Work = (m, p, h) => { };
        }
    }

    enum SkillType
    {
        Attack,
        Move,
    }
}
