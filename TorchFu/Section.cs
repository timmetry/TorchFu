using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TorchFu
{
    public class TorchSec
    {
        protected string secType;

        public TorchSec(string secType)
        {
            this.secType = secType;
        }

        protected List<TorchLine> lines = new List<TorchLine>();
        protected List<TorchSec> subsecs = new List<TorchSec>();
        protected void Add(TorchLine line)
        { lines.Add(line); }
        protected void Add(TorchSec subsec)
        { subsecs.Add(subsec); }

        protected string firstLine;
        protected string lastLine;

        protected virtual void Init()
        {
            if (secType != "")
            {
                firstLine = '[' + secType.ToUpper() + ']';
                lastLine = "[/" + secType.ToUpper() + ']';
            }
        }

        protected void Write(TextWriter tw)
        {
            Init();
            tw.WriteLine(firstLine);
            foreach (TorchLine line in lines)
                tw.WriteLine(line.ToString());
            foreach (TorchSec sec in subsecs)
                sec.Write(tw);
            tw.WriteLine(lastLine);
        }
    }

    public class PointSec : TorchSec
    {
        public double x;
        public double y;

        public PointSec() : base("point") { }
        public PointSec(double x, double y)
            : this()
        {
            this.x = x;
            this.y = y;
        }

        protected override void Init()
        {
            base.Init();
            Add(new FloatLine("x", x));
            Add(new FloatLine("y", y));
        }
    }

    public class AffixesSec : TorchSec
    {
        public int affixLevel;
        public string target;
        protected List<string> affixes;
        public bool remove;

        protected AffixesSec() : base("affixes")
        {
            affixes = new List<string>();
            remove = false;
            target = "";
        }
        public AffixesSec(string affix)
            : this()
        {
            AddAffix(affix);
        }
        public AffixesSec(string affix1, string affix2)
            : this(affix1)
        {
            AddAffix(affix2);
        }
        public AffixesSec(string affix1, string affix2, string affix3)
            : this(affix1, affix2)
        {
            AddAffix(affix3);
        }
        public AffixesSec(string[] affixes)
            : this()
        {
            this.affixes = new List<string>(affixes);
        }

        public void AddAffix(string affix)
        { affixes.Add(affix); }

        protected override void Init()
        {
            if (remove)
                secType += "remove";
            base.Init();
            if (affixLevel > 0)
                Add(new IntLine("affixlevel", affixLevel));
            if (target != "")
                Add(new StringLine("target", target));
            foreach (string affix in affixes)
                Add(new StringLine("affix", affix));
        }
    }

    public class UnitTypesSec : TorchSec
    {
        protected List<string> unitTypes;

        protected UnitTypesSec() : base("unittypes") { }
        public UnitTypesSec(string unitType)
            : this()
        {
            unitTypes = new List<string>();
            unitTypes.Add(unitType);
        }
        public UnitTypesSec(string unitType1, string unitType2)
            : this(unitType1)
        {
            unitTypes.Add(unitType2);
        }
        public UnitTypesSec(string unitType1, string unitType2, string unitType3)
            : this(unitType1, unitType2)
        {
            unitTypes.Add(unitType3);
        }
        public UnitTypesSec(string[] unitTypes)
            : this()
        {
            this.unitTypes = new List<string>(unitTypes);
        }
        public UnitTypesSec(List<string> unitTypes)
            : this()
        {
            this.unitTypes = unitTypes;
        }

        protected override void Init()
        {
            base.Init();
            foreach (string unitType in unitTypes)
                Add(new StringLine("unittypes", unitType.ToUpper()));
        }
    }

    public class EffectSec : TorchSec
    {
        public string name = "";
        public string activation = "";
        public string duration = "";
        public string type = "";
        public string damageType = "";
        public string statModifyName = "";
        public string statModifyPercent = "";
        public string graphOverride = "";
        public double min = -1;
        public double max = -1;
        public double force = -1;
        public double minForce = -1;
        public double maxForce = -1;
        public double dmgPctMin = -1;
        public double dmgPctMax = -1;

        public EffectSec(string activation, string type, double min, double max)
            : base("effect")
        {
            this.activation = activation;
            this.type = type;
            if (type == "knock back")
            {
                force = max;
            }
            else
            {
                this.min = min;
                this.max = max;
            }
        }
        public EffectSec(string activation, string type, string damageType, double min, double max)
            : this(activation, type, min, max)
        {
            this.damageType = damageType;
        }

        protected override void Init()
        {
            base.Init();
            if (name != "")
                Add(new StringLine("name", name));
            if (activation != "")
                Add(new StringLine("activation", activation.ToUpper()));
            if (duration != "")
                Add(new StringLine("duration", duration.ToUpper()));
            if (type != "")
                Add(new StringLine("type", type.ToUpper()));
            if (damageType != "")
                Add(new StringLine("damage_type", damageType.ToUpper()));
            if (statModifyName != "")
                Add(new StringLine("statModifyName", statModifyName.ToUpper()));
            if (statModifyPercent != "")
                Add(new StringLine("statModifyName", statModifyPercent.ToUpper()));
            if (graphOverride != "")
                Add(new StringLine("graphOverride", graphOverride));
            if (min != -1)
                Add(new FloatLine("min", min));
            if (max != -1)
                Add(new FloatLine("max", max));
            if (force != -1)
                Add(new FloatLine("force", force));
            if (minForce != -1)
                Add(new FloatLine("force", minForce));
            if (maxForce != -1)
                Add(new FloatLine("force", maxForce));
            if (dmgPctMin != -1)
                Add(new FloatLine("dmgPctMin", dmgPctMin));
            if (dmgPctMax != -1)
                Add(new FloatLine("dmgPctMax", dmgPctMax));
        }
    }

    public class IngredientSec : TorchSec
    {
        public string type;
        public string name;
        public int count;

        public IngredientSec(string type, string name, int count)
            : base("ingredient")
        {
            this.type = type;
            this.name = name;
            this.count = count;
        }

        protected override void Init()
        {
            base.Init();
            Add(new StringLine(type, name));
            Add(new IntLine("count", count));
        }
    }

    public class ResultSec : TorchSec
    {
        public string type;
        public string name;

        public ResultSec(string type, string name)
            : base("result")
        {
            this.type = type;
            this.name = name;
        }

        protected override void Init()
        {
            base.Init();
            Add(new StringLine(type, name));
        }
    }

    public class LevelSec : TorchSec
    {
        public double manaCost;
        public int levelRequired;

        public LevelSec(int level)
            : base("level"+level.ToString())
        { }
        public LevelSec(int level, double manaCost)
            : this(level)
        {
            this.manaCost = manaCost;
        }
        public LevelSec(int level, double manaCost, int levelRequired)
            : this(level, manaCost)
        {
            this.levelRequired = levelRequired;
        }

        public void AddEvent(EventSec eventSec)
        {
            Add(eventSec);
        }

        protected override void Init()
        {
            base.Init();

            if (manaCost > 0)
                Add(new FloatLine("manacost", manaCost));
            if (levelRequired > 0)
                Add(new IntLine("level_required", levelRequired));
        }
    }

    public class EventSec : TorchSec
    {
        public double weaponDamagePercent;
        public string layoutFile;
        public bool attaches;
        public bool useDPS;

        public EventSec(string eventType)
            : base("event_" + eventType)
        {
            attaches = false;
            useDPS = false;
        }
        public EventSec(string eventType, string layoutFile)
            : this(eventType)
        {
            this.layoutFile = layoutFile;
        }
        public EventSec(string eventType, string layoutFile, double weaponDamagePercent)
            : this(eventType, layoutFile)
        {
            this.weaponDamagePercent = weaponDamagePercent;
            attaches = true;
            useDPS = true;
        }

        public void AddAffixes(AffixesSec affixesSec)
        {
            Add(affixesSec);
        }

        protected override void Init()
        {
            base.Init();
            if (layoutFile != "")
                Add(new StringLine("file", layoutFile));
            if (attaches)
                Add(new BoolLine("attaches", attaches));
            if (weaponDamagePercent > 0)
                Add(new FloatLine("WeaponDamagePCT", weaponDamagePercent));
            if (useDPS)
                Add(new BoolLine("useDPS", useDPS));

        }

        public static string EVENT_START { get { return "start"; } }
        public static string EVENT_END { get { return "end"; } }
        public static string EVENT_TRIGGER { get { return "trigger"; } }
    }
}