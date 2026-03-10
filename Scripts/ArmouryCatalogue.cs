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
                        new ArmouryElement("cf3ba0d76633406ba9876633ebf3cfa6", DPAsset.Runeblade), //Mindshard
                        new ArmouryElement("4b697b99238849e6a18a0b9ce9e1a0f8", DPAsset.Aquila), //FrozenVigil
                        new ArmouryElement("3b1cdbdec35f4d249c5d0eaf57778046", DPAsset.DarktideA), //new asset [Kiava Gamma Pattern] Power Sword
                        new ArmouryElement("5f3b9e68aa78405ab23ade27846b6b2f", DPAsset.Runeblade_On), //hexagrammic greatsword
                        new ArmouryElement("1deb94cffbda4aafa01e44d1b19d0f59", DPAsset.Holy), //santiel's heart
                        new ArmouryElement("728f49c5e09941f9be18aa3697069dc4", DPAsset.Sunblade), //ravensword
                        new ArmouryElement("e11922e693164c0895e415b76a7ba809", DPAsset.Sunblade), //act1 base alternative
                        

                        //Ranged
                        new ArmouryElement("e4570c06ba5c4dd9bb0734d85ad81ce4", DPAsset.Plasmacannon_Green), //Act1 plasma cannon
                        new ArmouryElement("d8a45ba1f0b7478d91eb3a1998e00729", DPAsset.Plasmacannon_Red), //Act2 plasma cannon                     
                        new ArmouryElement("f43775fe14454b31bed48e5d26e44077", DPAsset.Plasmacannon_Yellow), //starstorm

                        new ArmouryElement("40d8a98cd786488c87103c97cbdb8631", DPAsset.Lascannon_Green) //Act1 base


                    };
                }
                Main.log.Log($"Armoury of the Expanse - ArmouryCatalogue found");
                return _catalogue;               
            }
            catch (Exception)
            {
                Main.log.Log($"[ERROR] Armoury of the Expanse - ArmouryCatalogue crashed");
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
