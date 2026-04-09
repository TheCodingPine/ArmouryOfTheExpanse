using System;
using System.Collections.Generic;


namespace ArmouryOfTheExpanse
{
    public static class ArmouryCatalogue
    {
        internal static List<ArmouryElement> _catalogue;

        public static List<ArmouryElement> Get()
        {
            try
            {
                if (_catalogue == null)
                {
                    _catalogue = new List<ArmouryElement>
                    {
                        //Melee
                            //Force 2Handed
                        new ArmouryElement("cf3ba0d76633406ba9876633ebf3cfa6", DPAsset.Runeblade), //Mindshard
                        new ArmouryElement("5f3b9e68aa78405ab23ade27846b6b2f", DPAsset.Runeblade_On), //hexagrammic greatsword
                            //Force 1Handed
                        new ArmouryElement("4b697b99238849e6a18a0b9ce9e1a0f8", DPAsset.Aquila), //FrozenVigil
                        new ArmouryElement("1720f627b780467aabef39690f0f69e8", DPAsset.Aquila), //whisper   
                        new ArmouryElement("1deb94cffbda4aafa01e44d1b19d0f59", DPAsset.Holy), //santiel's heart
                        new ArmouryElement("728f49c5e09941f9be18aa3697069dc4", DPAsset.Sunblade), //ravensword
                        new ArmouryElement("e11922e693164c0895e415b76a7ba809", DPAsset.Sunblade), //act1 base alternative
 
                            //Power 2Handed
                            //Power 1Handed
                        new ArmouryElement("3b1cdbdec35f4d249c5d0eaf57778046", DPAsset.DarktideA), //new asset [Kiava Gamma Pattern] Power Sword
                        new ArmouryElement("5f54e5c81b25449eaf410483c709d343", DPAsset.DarktideA), //Beastflayer
                        new ArmouryElement("ae22ac5586ac44f4bb2b2bda8667bdb3", DPAsset.Angelic), //Savant's Razor
                        new ArmouryElement("e9f3637e43ad4d7cabb73ea11c9bf365", DPAsset.Angelic), //Savant's Talon
                        
                                                                                                  
                        //Ranged
                            //Plasma Cannon
                        new ArmouryElement("e4570c06ba5c4dd9bb0734d85ad81ce4", DPAsset.Plasmacannon_Green), //Act1 plasma cannon
                        new ArmouryElement("d8a45ba1f0b7478d91eb3a1998e00729", DPAsset.Plasmacannon_Red), //Act2 plasma cannon                     
                        new ArmouryElement("f43775fe14454b31bed48e5d26e44077", DPAsset.Plasmacannon_Yellow), //plasma culverin
                            //Auto Cannon                            
                        new ArmouryElement("4cc6ec1168ef4c0fa0cf9ebdb98ad98b", DPAsset.Autocannon_Green), //Autocannon variant
                        new ArmouryElement("7ca9a0f43a2142769e2cc71a7a5d0429", DPAsset.Autocannon_Green), //Big Salvo
                        new ArmouryElement("70bb7e0af7394f58a5952eae9dd72402", DPAsset.Autocannon_Green), //Autocannon Act2
                        new ArmouryElement("f13eaaaa382747c5b0afb7267732a057", DPAsset.Autocannon_Green), //Autocannon Act4
                            //Las Cannon
                        new ArmouryElement("40d8a98cd786488c87103c97cbdb8631", DPAsset.Lascannon_Green), //Lascannon Act1
                        new ArmouryElement("857411fca50b4d33a3953bf87abd3c97", DPAsset.Lascannon_Blue), //Lascannon Act2
                        new ArmouryElement("e879f2d1d6634aac8326e24a55d49a58", DPAsset.Lascannon_Black), //Lascannon Act3
                            //Bolt Sniper
                        new ArmouryElement("697624ccaced41a28655058622ed2ac2", DPAsset.BoltSniper_SM),
                        new ArmouryElement("aa38dbb582be4b62b977c3fc8379d979", DPAsset.BoltSniper_HM)



                    };
                }
                int? count = _catalogue.Count;
                Main.log.Log($"ArmouryCatalogue found: {count} weapons with custom model");
                return _catalogue;               
            }
            catch (Exception)
            {
                Main.log.Log($"[ERROR] ArmouryCatalogue crashed");
                throw;
            }
        }
    }

    public class ArmouryElement
    {
        public string Guid { get; }
        public DPAsset NewVisual { get; }

        public ArmouryElement(string guid, DPAsset name)
        {
            Guid = guid;
            NewVisual = name;
        }
    }


}


//DOCS
// > New Asset by Darth?
//      - add a enum DPAsset entry with the asset name
//      - find the dummy blueprint ID in Darth's mod
//      - add a sprite and asset getter in DPGuids
//      - add the two getters and the enum to DPAssetsManager._guids
// > New blueprint?
//      - add the jbp guid in ArmouryCatalogue with the desired visual enum
