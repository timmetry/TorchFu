using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TorchFu
{
    /// <summary> A basic Torchlight ".dat" file. 
    /// Use this to build a file from scratch. 
    /// </summary>
    public class TorchFile : TorchSec
    {
        /// <summary> The character encoding format of the file (Encoding.Unicode by default). 
        /// I recommend you not tamper with this variable unless you REALLY know what you're doing.
        /// Torchlight will expect all ".dat" files to be in Unicode format, otherwise it won't generate the ".adm" files correctly.
        /// </summary>
        public Encoding encoding;
        /// <summary> The extension that the file will have upon creation (".dat" by default). 
        /// You won't need to change this unless you are creating a file type not supported by standard TorchFu. 
        /// Most Torchlight files will use ".dat" and be converted automatically to ".adm" when the game starts. 
        /// </summary>
        public string fileExt;
        /// <summary> The name that the file will have upon creation (minus the extension: "myfile" not "myfile.dat"). 
        /// This will default to the same as the [name] variable, but you can change it if you want to name the file differently.
        /// Don't include the directory (or path) where the file will be created; use the [filePath] variable for that. 
        /// </summary>
        public string fileName;
        /// <summary> The directory (or path) where the file will be created. 
        /// This will usually default to the right location, but you can add a custom folder if you want to group things differently. 
        /// Don't include the name or extension of the file here; use the [fileName] and [fileExt] variables for that. 
        /// </summary>
        public string filePath;
        /// <summary> The name of the thing this file is about.
        /// This is the name you'll need to use if you want to reference this thing from somewhere else. 
        /// Usually this will be the same as the [fileName], but you can make the two different if you want to. 
        /// </summary>
        public string name;

        /// <summary> This describes a basic Torchlight ".dat" file with a type and name at the location "media/". 
        /// </summary>
        /// <param name="type">The type of the thing the file is about (example: "unit").</param>
        /// <param name="name">The name of the file (don't include the ".dat" at the end).</param>
        public TorchFile(string type, string name)
            : base(type)
        {
            this.name = name;
            fileName = name;
            encoding = Encoding.Unicode;
            fileExt = ".dat";
            filePath = "media/";
        }
        /// <summary> This describes a basic Torchlight ".dat" file with a type, name and path (or directory).
        /// </summary>
        /// <param name="type">The type of the thing the file is about (example: "unit").</param>
        /// <param name="name">The name of the file (don't include the ".dat" at the end).</param>
        /// <param name="filePath">The directory where the file will be created (don't include "media/").</param>
        public TorchFile(string type, string name, string filePath)
            : this(type, name)
        {
            this.filePath += filePath;
        }

        protected override void Init()
        {
            base.Init();
            Add(new StringLine("name", name));
        }

        /// <summary> Creates the file and saves it in the specified directory (or path). 
        /// </summary>
        public void Create()
        {
            string fullPath = filePath + fileName + fileExt;
            Console.Out.WriteLine("Creating " + fullPath + "...");
            TextWriter tw = new StreamWriter(fullPath, false, encoding);
            Write(tw);
            tw.Close();
        }
    }

    /// <summary> A Torchlight graph file. 
    /// Use this to create point graphs to modify stat progressions or provide camera shakes.
    /// </summary>
    public class GraphFile : TorchFile
    {
        /// <summary> Whether or not the graph should follow gradually through the points to create a curve. 
        /// Defaults to false for a stats graph. 
        /// </summary>
        public bool curved;
        /// <summary> Whether or not the graph should keep increasing beyond the last point based on the last couple points. 
        /// Defaults to true for a stats graph. 
        /// </summary>
        public bool inferPastEnd;

        /// <summary> This describes a standard graph file in "media/graphs/stats/". 
        /// </summary>
        /// <param name="name">The name of the graph.</param>
        public GraphFile(string name) : this(name, "stats/", true) { }
        /// <summary> This describes a basic graph file with a name, directory, and whether it continues past the end. 
        /// </summary>
        /// <param name="name">The name of the graph.</param>
        /// <param name="directory">The directory (from within "media/graphs/") where the file will be created.</param>
        /// <param name="inferPastEnd">Whether or not the graph continues after the end based on the last couple points.</param>
        public GraphFile(string name, string directory, bool inferPastEnd)
            : base("line", name, "graphs/" + directory)
        {
            this.inferPastEnd = inferPastEnd;
        }
        public GraphFile(string name, string directory, int first, int last, int multiple, bool inferPastEnd, 
                         double initial, double increment, double modifier, double xmod)
            : this(name, directory, inferPastEnd)
        {
            AddCurve(first, last, multiple, initial, increment, modifier, xmod);
        }

        protected override void Init()
        {
            base.Init();
            Add(new BoolLine("curved", curved));
            if (inferPastEnd)
                Add(new BoolLine("infer_passed_end", inferPastEnd));
            Add(new StringLine("datatype", "LINE"));
        }

        /// <summary> Adds a single point to the graph. 
        /// </summary>
        /// <param name="x">the x-coordinate of the point</param>
        /// <param name="y">the y-coordinate of the point</param>
        public void AddPoint(double x, double y)
        {
            Add(new PointSec(x, y));
        }
        /// <summary> Adds a straight line to the graph. 
        /// The y-coordinate will increase by a fixed rate at each point along the line.
        /// </summary>
        /// <param name="first">the x-coordinate of the first point.</param>
        /// <param name="last">the x-coordinate of the last point.</param>
        /// <param name="multiple">the rate at which the x-coordinate will increase for each new point.</param>
        /// <param name="initial">the first y-coordinate.</param>
        /// <param name="increment">the rate at which the y-coordinate will increase for each new point.</param>
        public void AddLine(double first, double last, double multiple, double initial, double increment)
        {
            AddCurve(first, last, multiple, initial, increment, 0, 0);
        }
        /// <summary> Adds a curved line to the graph. 
        /// The y-coordinate will increase by a faster rate for each new point along the line.
        /// </summary>
        /// <param name="first">The x-coordinate of the first point.</param>
        /// <param name="last">The x-coordinate of the last point.</param>
        /// <param name="multiple">The rate at which the x-coordinate will increase for each new point.</param>
        /// <param name="initial">The first y-coordinate.</param>
        /// <param name="increment">The rate at which the y-coordinate will increase for each new point.</param>
        /// <param name="modifier">This will control how steep the line curves upward.
        /// The rate at which [increment] will increase after each new point.</param>
        /// <param name="xmod">This can cause the line to curve upward at a progressively faster rate.
        /// Set it to zero for a standard curve. This is the rate [modifier] will increase after each new point.</param>
        public void AddCurve(double first, double last, double multiple, double initial, double increment, double modifier, double xmod)
        {
            for (double i = first; i <= last; i += multiple)
            {
                Add(new PointSec(i, initial));
                initial += increment;
                increment += modifier;
                modifier += xmod;
            }
        }
    }

    public class UnitFile : TorchFile
    {
        public string unitType;
        public string unitGuid;
        public string baseFile;
        public string basePath;

        public UnitFile(string name, string directory, string unitType)
            : base("unit", name, "units/" + directory)
        {
            basePath = "media/units/";
            this.unitType = unitType;
        }
        public UnitFile(string name, string directory, string unitType, string baseFile, string basePath)
            : this(name, directory, unitType)
        {
            this.baseFile = baseFile;
            this.basePath += basePath;
        }

        protected override void Init()
        {
            base.Init();
            Add(new StringLine("unittype", unitType.ToUpper()));
            Add(new StringLine("unit_guid", unitGuid));
            Add(new StringLine("basefile", basePath + baseFile + ".dat"));
        }
    }

    public class ItemFile : UnitFile
    {
        public string displayName;
        public string iconName;
        public string description;
        public int rarity;
        public int value;
        public int level;
        public int minLevel;
        public int maxLevel;
        public int stackSize;
        public bool merchantInfinite;

        public ItemFile(string name, string directory, string unitType)
            : base(name, "items/" + directory, unitType)
        {
            displayName = name;
            basePath += "items/";
            baseFile = "base";

            stackSize = 1;
            merchantInfinite = false;
        }
        public ItemFile(string name, string directory, string displayName, string unitType, string iconName)
            : this(name, directory, unitType)
        {
            this.displayName = displayName;
            this.iconName = iconName;
        }
        public ItemFile(string name, string directory, string displayName, string unitType, string iconName,
                        int rarity, int value, int level, int minLevel, int maxLevel)
            : this(name, directory, displayName, unitType, iconName)
        {
            this.displayName = displayName;
            this.rarity = rarity;
            this.value = value;
            this.level = level;
            this.minLevel = minLevel;
            this.maxLevel = maxLevel;
        }
        public ItemFile(string name, string directory, string displayName, string unitType, string iconName,
                        int rarity, int value, int level, int minLevel, int maxLevel, string baseFile, string basePath)
            : this(name, directory, displayName, unitType, iconName, rarity, value, level, minLevel, maxLevel)
        {
            this.baseFile = baseFile;
            this.basePath += basePath;
        }

        protected override void Init()
        {
            base.Init();
            Add(new TranslateLine("displayname", displayName));
            Add(new StringLine("icon", iconName));
            Add(new IntLine("rarity", rarity));
            Add(new IntLine("value", value));
            Add(new IntLine("level", level));
            Add(new IntLine("minlevel", minLevel));
            Add(new IntLine("maxlevel", maxLevel));
            if (stackSize > 1)
                Add(new IntLine("maxstacksize", stackSize));
            if (merchantInfinite)
                Add(new BoolLine("merchantinfinite", merchantInfinite));
            if (description != string.Empty)
                Add(new TranslateLine("description", description));
        }
    }

    public class SocketableFile : ItemFile
    {
        public AffixesSec affixesSec;

        public SocketableFile(string name)
            : base(name, "socketables/", "socketable")
        {
            displayName = name;
            description = "Insert Gems into weapons and armor with empty slots to garner their effects.\\n\\n"
                    + "Transmute any two identical Gems to get one of the next higher grade.";
        }
        public SocketableFile(string name, string displayName, string iconName)
            : this(name)
        {
            this.displayName = displayName;
            this.iconName = iconName;
        }
        public SocketableFile(string name, string displayName, string iconName, AffixesSec affixesSec,
                              int rarity, int value, int level, int minLevel, int maxLevel)
            : this(name, displayName, iconName)
        {
            this.affixesSec = affixesSec;
            this.rarity = rarity;
            this.value = value;
            this.level = level;
            this.minLevel = minLevel;
            this.maxLevel = maxLevel;
        }
        public SocketableFile(string name, string displayName, string iconName, AffixesSec affixesSec, 
                              int rarity, int value, int level, int minLevel, int maxLevel, string baseFile, string basePath) :
            this(name, displayName, iconName, affixesSec, rarity, value, level, minLevel, maxLevel)
        {
            this.baseFile = baseFile;
            this.basePath = basePath;
        }

        protected override void Init()
        {
            base.Init();
            Add(new StringLine("resourcedirectory", "media/models/armor/gemdrop"));
            Add(new StringLine("meshfile", "ring"));
            Add(new BoolLine("shadows", false));
            Add(new StringLine("fall_sound", "Item Fall"));
            Add(new StringLine("take_sound", "GemDrop"));
            Add(new StringLine("land_sound", "GemDrop"));
            Add(affixesSec);
        }
    }

    public abstract class XMLFile : TorchFile
    {
        public XMLFile(string type, string fileName, string filePath)
            : base(type, fileName, filePath)
        {
            fileExt = ".xml";
            encoding = Encoding.Default;
        }

        protected override void Init()
        {
            //base.Init(); -> xml files are initialized differently
            firstLine = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            // need to add the opening tag as first TorchLine
            lastLine = "</" + secType + '>';
        }
    }

    public class ImagesetFile : XMLFile
    {
        public ImagesetFile(string fileName, string filePath, string imageName, int totalWidth, int totalHeight)
            : base("Imageset", fileName, "UI/")
        {
            fileExt = ".imageset";
            this.filePath += filePath;
            Add(new XMLLine(0, "Imageset Name=\"" + this.fileName + "\" Imagefile=\"" + this.filePath + imageName
                    + "\" NativeHorzRes=\"" + totalWidth.ToString() + "\" NativeVertRes=\"" + totalHeight.ToString()
                    + "\" AutoScaled=\"true\" "));
        }
        public ImagesetFile(string fileName, string filePath, string imageExt, int imageWidth, int imageHeight, int columns, int rows)
            : this(fileName, filePath, imageExt, imageWidth * columns, imageHeight * rows)
        {
            for (int j = 0; j < rows; ++j)
                for (int i = 0; i < columns; ++i)
                    AddImage(fileName + (i + 1 + j * columns).ToString(), i * imageWidth, j * imageHeight, imageWidth, imageHeight);
        }

        public void AddImage(string name, int xpos, int ypos, int width, int height)
        {
            Add(new XMLLine(1, "Image Name=\"" + name + "\" XPos=\"" + xpos.ToString() + "\" YPos=\"" + ypos.ToString()
                    + "\" Width=\"" + width + "\" Height=\"" + height + "\" /"));
        }
    }

    public class AffixFile : TorchFile
    {
        public int rank;
        public int minRange;
        public int maxRange;
        public double duration;
        public int weight;
        public int slotsOccupy;
        protected List<string> unitTypes;
        protected List<EffectSec> effects;

        public AffixFile(string name, string directory)
            : base("affix", name, "Affixes/" + directory)
        {
            this.duration = 0;
        }
        public AffixFile(string name, string directory, int rank, int weight, int slotsOccupy, int minRange, int maxRange)
            : this(name, directory)
        {
            this.rank = rank;
            this.minRange = minRange;
            this.maxRange = maxRange;
            this.weight = weight;
            this.slotsOccupy = slotsOccupy;
        }
        public AffixFile(string name, string directory, int rank, int weight, int slotsOccupy, int minRange, int maxRange,
                         string[] unitTypes, EffectSec[] effects)
            : this(name, directory, rank, weight, slotsOccupy, minRange, maxRange)
        {
            this.unitTypes = new List<string>(unitTypes);
            this.effects = new List<EffectSec>(effects);
        }

        protected override void Init()
        {
            base.Init();

            Add(new IntLine("rank", rank));
            Add(new IntLine("min_spawn_range", minRange));
            Add(new IntLine("max_spawn_range", maxRange));
            Add(new FloatLine("duration", duration));
            Add(new IntLine("weight", weight));
            Add(new IntLine("slots_occupy", slotsOccupy));

            Add(new UnitTypesSec(unitTypes));
            foreach (EffectSec effect in effects)
                Add(effect);
        }

        public void AddUnitTypes(string unitType)
        {
            unitTypes.Add(unitType);
        }
        public void AddUnitTypes(string unitType1, string unitType2)
        {
            unitTypes.Add(unitType1);
            unitTypes.Add(unitType2);
        }
        public void AddUnitTypes(string unitType1, string unitType2, string unitType3)
        {
            unitTypes.Add(unitType1);
            unitTypes.Add(unitType2);
            unitTypes.Add(unitType3);
        }

        public void AddEffects(EffectSec effect)
        {
            effects.Add(effect);
        }
        public void AddEffects(EffectSec effect1, EffectSec effect2)
        {
            effects.Add(effect1);
            effects.Add(effect2);
        }
        public void AddEffects(EffectSec effect1, EffectSec effect2, EffectSec effect3)
        {
            effects.Add(effect1);
            effects.Add(effect2);
            effects.Add(effect3);
        }
    }

    public class GemAffixFile : AffixFile
    {
        public GemAffixFile(string name)
            : base(name, "Gems/")
        { }
        public GemAffixFile(string name, int rank, int weight, string[] types, EffectSec effect)
            : base(name, "Gems/", rank, weight, 0, 0, 9999999, types, new EffectSec[] { effect })
        { }
        public GemAffixFile(string name, int rank, int weight, string type, EffectSec effect)
            : this(name, rank, weight, new string[] { "socketable", type }, effect)
        { }
    }

    public class RecipeFile : TorchFile
    {
        public RecipeFile(string name, string directory)
            : base("recipe", name, "recipes/" + directory)
        { }

        public void AddIngredient(string type, string name)
        { AddIngredient(type, name, 1); }
        public void AddIngredient(string type, string name, int count)
        {
            Add(new IngredientSec(type, name, count));
        }

        public void AddResult(string type, string name)
        {
            Add(new ResultSec(type, name));
        }
    }

    public class SkillFile : TorchFile
    {
        public string displayName;
        public string description;
        public string skillIconActive;
        public string skillIconInactive;
        public string activationType;
        public string targetAlignment;
        public string targetType;
        public string animation;
        public string animationDW;
        public double range;
        public string requirementRight;
        public string requirementLeft;
        public int cooldownMS;
        public int maxInvestLevel;
        public bool canLeftMap;
        public bool requiresTarget;
        public long uniqueGUID;

        public SkillFile(string name, string directory)
            : base("skill", name, "skills/"+directory)
        {
            activationType = "NORMAL";
            targetAlignment = "EVIL";
            maxInvestLevel = 10;
            canLeftMap = true;
            targetType = "";
            animationDW = "";
            requirementRight = "";
            requirementLeft = "";
        }

        public void AddLevel(LevelSec levelSec)
        {
            Add(levelSec);
        }

        protected override void Init()
        {
            base.Init();

            Add(new TranslateLine("displayname", displayName));
            Add(new TranslateLine("description", description));
            Add(new StringLine("skill_icon", skillIconActive));
            Add(new StringLine("skill_icon_inactive", skillIconInactive));
            Add(new StringLine("activation_type", activationType));
            Add(new StringLine("target_alignment", targetAlignment));
            if (targetType != "")
                Add(new StringLine("target_type", targetType));
            Add(new StringLine("animation", animation));
            if (animationDW != "")
                Add(new StringLine("animationDW", animationDW));
            if (range > 0)
                Add(new FloatLine("range", range));
            if (requirementRight != "")
                Add(new StringLine("requirement_right", requirementRight));
            if (requirementLeft != "")
                Add(new StringLine("requirement_left", requirementLeft));
            if (cooldownMS > 0)
                Add(new IntLine("cooldownMS", cooldownMS));
            Add(new IntLine("max_invest_level", maxInvestLevel));
            if (canLeftMap)
                Add(new BoolLine("can_left_map", canLeftMap));
            if (requiresTarget)
                Add(new BoolLine("requires_target", requiresTarget));
            if (uniqueGUID > 0)
                Add(new LongLine("unique_guid", uniqueGUID));
        }
    }
}