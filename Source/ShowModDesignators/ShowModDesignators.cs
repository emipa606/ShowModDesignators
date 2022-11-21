using System;
using System.Linq;
using RimWorld;
using Verse;

namespace ShowModDesignators;

[StaticConstructorOnStartup]
public class ShowModDesignators
{
    static ShowModDesignators()
    {
        foreach (var modContentPack in LoadedModManager.RunningMods.Where(mcp => !mcp.IsCoreMod))
        {
            foreach (var allDef in modContentPack.AllDefs)
            {
                try
                {
                    var text = $"\n({modContentPack.Name})".Replace('[', '(').Replace(']', ')');
                    if (!allDef.description.NullOrEmpty())
                    {
                        allDef.description += text;
                    }

                    if (allDef is not TraitDef traitDef)
                    {
                        if (allDef is not ThingDef { race: { } } thingDef)
                        {
                            continue;
                        }

                        if (thingDef.race.meatDef != null)
                        {
                            thingDef.race.meatDef.description += text;
                        }

                        if (thingDef.race.corpseDef != null)
                        {
                            thingDef.race.corpseDef.description += text;
                        }

                        if (thingDef.race.leatherDef != null)
                        {
                            thingDef.race.leatherDef.description += text;
                        }
                    }
                    else
                    {
                        foreach (var degreeData in traitDef.degreeDatas)
                        {
                            degreeData.description += text;
                        }
                    }
                }
                catch (Exception)
                {
                    Log.Error($"ModDesignator: {allDef.defName} of {allDef.GetType()} is evil");
                }
            }
        }
    }
}
