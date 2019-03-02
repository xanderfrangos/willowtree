using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using X360;
using X360.IO;
using X360.STFS;
using X360.FATX;
using X360.Other;
using X360.Media; //Yeah, I don't need most of these. So sue me.
using X360.Profile;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using System.Resources;
using Microsoft.VisualBasic;
using System.Net;
using System.Threading;
using System.IO;
using System.Reflection;
//using System.Collections.Specialized;




namespace WindowsFormsApplication1 
{


    public partial class WillowTree : DevComponents.DotNetBar.Office2007RibbonForm
    {
        public bool Clicked = false; //Goes with the quest stuff. Really...ineffective.
        public static WebClient Updater = new WebClient();
        string VersionFromServer;

        //Grabs the name of a weapon/item from an INI.
        public string GetName(Ini.IniFile INI, string[,] PartArray, int DesiredPart, int NumberOfSubParts, string INIValueToRetrieve)
        {
            string Name = "";
            for (int build = 0; build < NumberOfSubParts; build++)
            {





                if (Name == "" && INI.IniReadValue(PartArray[DesiredPart, build], INIValueToRetrieve) != null)
                    Name = (INI.IniReadValue(PartArray[DesiredPart, build], INIValueToRetrieve));
                else if (INI.IniReadValue(PartArray[DesiredPart, build], INIValueToRetrieve) != null && INI.IniReadValue(PartArray[DesiredPart, build], INIValueToRetrieve) != "")
                    Name = (Name + " " + INI.IniReadValue(PartArray[DesiredPart, build], INIValueToRetrieve));
            }

            return Name;
        }
        public string GetName(Ini.IniFile INI, string[] PartArray, int DesiredPart, string INIValueToRetrieve)
        {
            string Name = "";

            Name = (INI.IniReadValue(PartArray[DesiredPart], INIValueToRetrieve));


            return Name;
        }
        public string GetName(Ini.IniFile INI, List<List<string>> PartArray, int DesiredPart, int NumberOfSubParts, string INIValueToRetrieve)
        {
            string Name = "";
            for (int build = 0; build < NumberOfSubParts; build++)
            {

                if (Name == "" && INI.IniReadValue(PartArray[DesiredPart][build], INIValueToRetrieve) != null)
                    Name = (INI.IniReadValue(PartArray[DesiredPart][build], INIValueToRetrieve));
                else if (INI.IniReadValue(PartArray[DesiredPart][build], INIValueToRetrieve) != null && INI.IniReadValue(PartArray[DesiredPart][build], INIValueToRetrieve) != "")
                    Name = (Name + " " + INI.IniReadValue(PartArray[DesiredPart][build], INIValueToRetrieve));
            }

            return Name;
        }
        public string GetName(List<List<string>> PartArray, int DesiredPart, int NumberOfSubParts, string INIValueToRetrieve)
        {
            string Name = "";
            for (int build = 0; build < NumberOfSubParts; build++)
            {
                Ini.IniFile INI;
                if(PartArray[DesiredPart][build].IndexOf(".") > 0)
                    INI = new Ini.IniFile(AppDir + "\\Data\\" + PartArray[DesiredPart][build].Remove(PartArray[DesiredPart][build].IndexOf(".")) + ".txt");
                else INI = new Ini.IniFile(AppDir + "\\Data\\titles.ini");
                    if (Name == "" && INI.IniReadValue(PartArray[DesiredPart][build].Substring(PartArray[DesiredPart][build].IndexOf(".") + 1), INIValueToRetrieve) != null)
                        Name = (INI.IniReadValue(PartArray[DesiredPart][build].Substring(PartArray[DesiredPart][build].IndexOf(".") + 1), INIValueToRetrieve));
                    else if (INI.IniReadValue(PartArray[DesiredPart][build], INIValueToRetrieve) != null && INI.IniReadValue(PartArray[DesiredPart][build].Substring(PartArray[DesiredPart][build].IndexOf(".") + 1), INIValueToRetrieve) != "")
                        Name = (Name + " " + INI.IniReadValue(PartArray[DesiredPart][build].Substring(PartArray[DesiredPart][build].IndexOf(".") + 1), INIValueToRetrieve));
                
            }

            return Name;
        }
        string OpenedLocker;
        
        //Generate each advTree
        public void DoQuestTree()
        {
            QuestTree.Nodes.Clear();
            Ini.IniFile Quests = new Ini.IniFile(AppDir + "\\Data\\Quests.ini");
            Node PT1 = new Node();
            PT1.Text = "Playthrough 1 Quests";
            PT1.Name = "PT1";
            Node PT2 = new Node();
            PT2.Text = "Playthrough 2 Quests";
            PT2.Name = "PT2";
            QuestTree.Nodes.Add(PT1);
            QuestTree.Nodes.Add(PT2);
            for (int build = 0; build < CurrentWSG.TotalPT1Quests; build++)
            {
                Node TempNode = new Node();
                TempNode.Name = "PT1";
                TempNode.Text = GetName(Quests, CurrentWSG.PT1Strings, build, "MissionName");
                if (TempNode.Text == null || TempNode.Text == "") TempNode.Text = "Unknown Quest";
                PT1.Nodes.Add(TempNode);


                //for(int sub_build = 0; sub_build < (CurrentWSG.PT1Values[build,3] + 4); sub_build++)
                //{
                //    Node Sub = new Node();
                //    Sub.Text = "Value: " + CurrentWSG.PT1Values[build, sub_build];
                //    TempNode.Nodes.Add(Sub);
                //     + CurrentWSG.TotalPT1Quests
                //}

            }

            for (int build2 = 0; build2 < CurrentWSG.TotalPT2Quests; build2++)
            {
                Node TempNode = new Node();
                TempNode.Name = "PT2";
                TempNode.Text = GetName(Quests, CurrentWSG.PT2Strings, build2, "MissionName");
                if (TempNode.Text == null || TempNode.Text == "") TempNode.Text = "Unknown Quest";

                PT2.Nodes.Add(TempNode);


                //for(int sub_build = 0; sub_build < (CurrentWSG.PT1Values[build,3] + 4); sub_build++)
                //{
                //    Node Sub = new Node();
                //    Sub.Text = "Value: " + CurrentWSG.PT1Values[build, sub_build];
                //    TempNode.Nodes.Add(Sub);
                //    
                //}

            }

        }
        public void DoLocationTree()
        {
            Ini.IniFile PartList = new Ini.IniFile(AppDir + "\\Data\\Locations.ini");
            LocationTree.Nodes.Clear();

            for (int build = 0; build < CurrentWSG.TotalLocations; build++)
            {
                string name = PartList.IniReadValue(CurrentWSG.LocationStrings[build], "OutpostDisplayName");
                Node TempNode = new Node();
                if (name != "")
                    TempNode.Text = name;
                else TempNode.Text = CurrentWSG.LocationStrings[build];


                LocationTree.Nodes.Add(TempNode);

            }





        }
        public void DoSkillTree()
        {
            Ini.IniFile Common = new Ini.IniFile(AppDir + "\\Data\\gd_skills_common.txt");
            Ini.IniFile Roland = new Ini.IniFile(AppDir + "\\Data\\gd_Skills2_Roland.txt");
            Ini.IniFile Lilith = new Ini.IniFile(AppDir + "\\Data\\gd_Skills2_Lilith.txt");
            Ini.IniFile Mordecai = new Ini.IniFile(AppDir + "\\Data\\gd_skills2_Mordecai.txt");
            Ini.IniFile Brick = new Ini.IniFile(AppDir + "\\Data\\gd_Skills2_Brick.txt");
            SkillTree.Nodes.Clear();
            SkillLevel.Value = 0;
            SkillExp.Value = 0;
            SkillActive.SelectedItem = "No";
            for (int build = 0; build < CurrentWSG.NumberOfSkills; build++)
            {
                Node TempNode = new Node();
                if (Common.IniReadValue(CurrentWSG.SkillNames[build], "SkillName") != "")
                    TempNode.Text = Common.IniReadValue(CurrentWSG.SkillNames[build], "SkillName");
                else if (Roland.IniReadValue(CurrentWSG.SkillNames[build], "SkillName") != "")
                    TempNode.Text = Roland.IniReadValue(CurrentWSG.SkillNames[build], "SkillName");
                else if (Lilith.IniReadValue(CurrentWSG.SkillNames[build], "SkillName") != "")
                    TempNode.Text = Lilith.IniReadValue(CurrentWSG.SkillNames[build], "SkillName");
                else if (Mordecai.IniReadValue(CurrentWSG.SkillNames[build], "SkillName") != "")
                    TempNode.Text = Mordecai.IniReadValue(CurrentWSG.SkillNames[build], "SkillName");
                else if (Brick.IniReadValue(CurrentWSG.SkillNames[build], "SkillName") != "")
                    TempNode.Text = Brick.IniReadValue(CurrentWSG.SkillNames[build], "SkillName");
                else
                    TempNode.Text = CurrentWSG.SkillNames[build];

                SkillTree.Nodes.Add(TempNode);


            }



        }
        public void DoEchoTree()
        {
            EchoTree.Nodes.Clear();
            Ini.IniFile Echos = new Ini.IniFile(AppDir + "\\Data\\Echos.ini");
            Node PT1 = new Node();
            PT1.Text = "Playthrough 1 Echo Logs";
            Node PT2 = new Node();
            PT2.Text = "Playthrough 2 Echo Logs";
            EchoTree.Nodes.Add(PT1);
            EchoTree.Nodes.Add(PT2);
            for (int build = 0; build < CurrentWSG.NumberOfEchos; build++)
            {
                Node TempNode = new Node();
                TempNode.Name = "PT1";
                TempNode.Text = GetName(Echos, CurrentWSG.EchoStrings, build, "Subject");
                if (TempNode.Text == null || TempNode.Text == "") TempNode.Text = "Unknown Echo";
                PT1.Nodes.Add(TempNode);
            }
            if (CurrentWSG.NumberOfEchosPT2 > 0 && CurrentWSG.TotalPT2Quests > 1 && CurrentWSG.NumberOfEchosPT2 < 300)
            for (int build2 = 0; build2 < CurrentWSG.NumberOfEchosPT2; build2++)
            {
                Node TempNode = new Node();
                TempNode.Name = "PT2";
                TempNode.Text = GetName(Echos, CurrentWSG.EchoStringsPT2, build2, "Subject");
                if (TempNode.Text == null || TempNode.Text == "") TempNode.Text = "Unknown Echo";

                PT2.Nodes.Add(TempNode);
            }
        }
        public void DoAmmoTree()
        {
            AmmoTree.Nodes.Clear();

            for (int build = 0; build < CurrentWSG.NumberOfPools; build++)
            {
                Node TempNode = new Node();

                TempNode.Text = GetAmmoName(CurrentWSG.ResourcePools[build]);

                AmmoTree.Nodes.Add(TempNode);

            }



        }
        public void DoWeaponTree()
        {
            WeaponTree.Nodes.Clear();
            Ini.IniFile Titles = new Ini.IniFile(AppDir + "\\Data\\Titles.ini");

            for (int build = 0; build < CurrentWSG.WeaponStrings.Count; build++)
            {
                Node TempNode = new Node();
                TempNode.Text = GetName(Titles, CurrentWSG.WeaponStrings, build, 14, "PartName");
                //TempNode.Text = GetName(CurrentWSG.WeaponStrings, build, 14, "PartName");
                //TempNode.Name = "" + build;
                //for (int search_name = 0; search_name < 14; search_name++)
                //{

                    //Node Sub = new Node();
                    //Sub.Text = "Part " + (search_name + 1) + ": " + CurrentWSG.WeaponStrings[build][search_name];
                    //TempNode.Nodes.Add(Sub);

                    //if(TempNode.Text == "" && Titles.IniReadValue(CurrentWSG.WeaponStrings[build, search_name], "PartName") != null)
                    //TempNode.Text = (Titles.IniReadValue(CurrentWSG.WeaponStrings[build, search_name], "PartName"));
                    //else if(Titles.IniReadValue(CurrentWSG.WeaponStrings[build, search_name], "PartName") != null) 
                    //TempNode.Text = (TempNode.Text + " " + Titles.IniReadValue(CurrentWSG.WeaponStrings[build, search_name], "PartName"));
                //}
                if (TempNode.Text == "") TempNode.Text = "Unknown Weapon";
                WeaponTree.Nodes.Add(TempNode);
            }

        }
        public void DoItemTree()
        {
            ItemTree.Nodes.Clear();
            Ini.IniFile Titles = new Ini.IniFile(AppDir + "\\Data\\Titles.ini");

            for (int build = 0; build < CurrentWSG.NumberOfItems; build++)
            {
                Node TempNode = new Node();
                TempNode.Text = GetName(Titles, CurrentWSG.ItemStrings, build, 9, "PartName");
                //TempNode.Name = "" + build;
                //for (int search_name = 0; search_name < 9; search_name++)
                //{

                    //Node Sub = new Node();
                    //Sub.Text = "Part " + (search_name + 1) + ": " + CurrentWSG.ItemStrings[build][search_name];
                    //TempNode.Nodes.Add(Sub);

                   // if (TempNode.Text == "" && Titles.IniReadValue(CurrentWSG.ItemStrings[build][search_name], "PartName") != null)
                   //     TempNode.Text = (Titles.IniReadValue(CurrentWSG.ItemStrings[build][search_name], "PartName"));
                   // else if (Titles.IniReadValue(CurrentWSG.ItemStrings[build][search_name], "PartName") != null)
                   //     TempNode.Text = (TempNode.Text + " " + Titles.IniReadValue(CurrentWSG.ItemStrings[build][search_name], "PartName"));

               // }
               if (TempNode.Text == "") TempNode.Text = "Unknown Item";
                //Node Sub_Ammo = new Node();
                //Node Sub_Quality = new Node();
                //Node Sub_Slot = new Node();
                //Sub_Ammo.Text = "Quantity: " + CurrentWSG.ItemValues[build][0];
                //Sub_Quality.Text = "Quality Level: " + CurrentWSG.ItemValues[build][1];
                //Sub_Slot.Text = "Equipped: " + CurrentWSG.ItemValues[build][2];
                //TempNode.Nodes.Add(Sub_Ammo);
                //TempNode.Nodes.Add(Sub_Quality);
                //TempNode.Nodes.Add(Sub_Slot);
                ItemTree.Nodes.Add(TempNode);
            }

        }
        public void DoLockerTree(string InputFile)
        {
            LockerTree.Nodes.Clear();
            Ini.IniFile Locker = new Ini.IniFile(InputFile);
            OpenedLocker = InputFile;
            for (int Progress = 0; Progress < Locker.ListSectionNames().Length; Progress++)
            {
                int Count = 0;
                for (int CheckName = 0; CheckName < LockerTree.Nodes.Count; CheckName++)
                {
                    if (LockerTree.Nodes[CheckName].Text == Locker.ListSectionNames()[Progress] || LockerTree.Nodes[CheckName].Text.Contains(Locker.ListSectionNames()[Progress] + " (Copy ") == true)
                        Count = Count + 1;
                }
                if (Count == 0)
                {
                    Node temp = new Node();
                    temp.Text = Locker.ListSectionNames()[Progress];
                    LockerTree.Nodes.Add(temp);
                }
                else
                {

                    Node temp = new Node();
                    temp.Text = Locker.ListSectionNames()[Progress] + " (Copy " + Count + ")";
                    LockerTree.Nodes.Add(temp);
                }
            }

        }
        public void DoPartsCategory(string Category, AdvTree Tree)
        {
            Ini.IniFile PartList = new Ini.IniFile(AppDir + "\\Data\\" + Category + ".txt");
            Node TempNode = new Node();
            TempNode.Name = Category;
            TempNode.Text = Category;
            for (int Progress = 0; Progress < PartList.ListSectionNames().Length; Progress++)
            {
                Node Part = new Node();
                Part.Name = PartList.ListSectionNames()[Progress];
                Part.Text = PartList.ListSectionNames()[Progress];
                TempNode.Nodes.Add(Part);

            }
            Tree.Nodes.Add(TempNode);

        }
        public void DoTabs()
        {
            string TabsLine = System.IO.File.ReadAllText(AppDir + "\\Data\\weapon_tabs.txt");
            string[] TabsList = TabsLine.Split(new char[] { (char)';' });
            for (int Progress = 0; Progress < TabsList.Length; Progress++) DoPartsCategory(TabsList[Progress], PartCategories);
            string TabsLine2 = System.IO.File.ReadAllText(AppDir + "\\Data\\item_tabs.txt");
            string[] TabsList2 = TabsLine2.Split(new char[] { (char)';' });
            for (int Progress = 0; Progress < TabsList2.Length; Progress++) DoPartsCategory(TabsList2[Progress], ItemPartsSelector);
            
        }
        public void DoQuestList()
        {
            Ini.IniFile PartList = new Ini.IniFile(AppDir + "\\Data\\Quests.ini");

            for (int Progress = 0; Progress < PartList.ListSectionNames().Length; Progress++)
                QuestList.Items.Add(PartList.IniReadValue(PartList.ListSectionNames()[Progress], "MissionName"));

        }
        public void DoEchoList()
        {
            Ini.IniFile PartList = new Ini.IniFile(AppDir + "\\Data\\Echos.ini");

            for (int Progress = 0; Progress < PartList.ListSectionNames().Length; Progress++)
            {
                string name = PartList.IniReadValue(PartList.ListSectionNames()[Progress], "Subject");
                if (name != "")
                    EchoList.Items.Add(name);
                else EchoList.Items.Add(PartList.ListSectionNames()[Progress]);
            }
        }
 
        public void SaveChangesToLocker()
        {
            try
            {
                string tempINI = System.IO.File.ReadAllText(OpenedLocker);
                tempINI = tempINI.Replace("\r\n\r\n\r\n", "\r\n"); // Clean up extra lines.
                System.IO.File.WriteAllText(OpenedLocker, tempINI);
                string search = "[" + LockerTree.SelectedNode.Text + "]";

                string[] tempINI2 = System.IO.File.ReadAllLines(OpenedLocker);
                int NumberOfFoundLines = 0;
                int DiscoveredLine = -1;
                bool FoundOriginal = false;
                for (int Progress = 0; Progress < tempINI2.Length; Progress++)
                {
                    //string tewst = tempINI2[Progress].Substring(0, tempINI2[Progress].Length - 1);

                    if (tempINI2[Progress] == search)
                        DiscoveredLine = Progress;


                    else if (tempINI2[Progress] == "[" + NameLocker.Text + "]")
                    {
                        if (LockerTree.SelectedNode.Text != NameLocker.Text)
                        {
                            NumberOfFoundLines = NumberOfFoundLines + 1;
                            FoundOriginal = true;
                        }
                    }
                    else if (tempINI2[Progress].Contains("[" + NameLocker.Text + " (Copy ") == true)
                        if (LockerTree.SelectedNode.Text != NameLocker.Text)
                            NumberOfFoundLines = NumberOfFoundLines + 1;
                }
                if (NumberOfFoundLines == 0)
                    tempINI2[DiscoveredLine] = "[" + NameLocker.Text + "]";
                else if (NumberOfFoundLines > 0 && FoundOriginal == false)
                    tempINI2[DiscoveredLine] = "[" + NameLocker.Text + "]";
                else if (NumberOfFoundLines > 0 && FoundOriginal == true)
                    tempINI2[DiscoveredLine] = "[" + NameLocker.Text + " (Copy " + (NumberOfFoundLines) + ")" + "]";


                //tempINI.Replace("[" + LockerTree.SelectedNode.Text + "]", "[" + NameLocker.Text + "]");
                System.IO.File.WriteAllLines(OpenedLocker, tempINI2);

                string Description = "";
                Ini.IniFile Locker = new Ini.IniFile(OpenedLocker);
                Locker.ListSectionNames()[LockerTree.SelectedNode.Index] = NameLocker.Text;
                Locker.IniWriteValue(Locker.ListSectionNames()[LockerTree.SelectedNode.Index], "Rating", "" + RatingLocker.Rating);
                for (int Progress = 0; Progress < DescriptionLocker.Lines.Length; Progress++)
                    Description = Description + DescriptionLocker.Lines[Progress] + "$LINE$";
                for (int Progress = 0; Progress < 14; Progress++)
                    Locker.IniWriteValue(Locker.ListSectionNames()[LockerTree.SelectedNode.Index], "Part" + (Progress + 1), (string)PartsLocker.Items[Progress]);

                Locker.IniWriteValue(Locker.ListSectionNames()[LockerTree.SelectedNode.Index], "Description", Description);
                Locker.IniWriteValue(Locker.ListSectionNames()[LockerTree.SelectedNode.Index], "Type", "" + ItemTypeLocker.SelectedItem);
                //Locker.IniReadValue(Locker.ListSectionNames()[LockerTree.SelectedNode.Index] = "";

                //DoLockerTree(OpenedLocker);
                LockerTree.SelectedNode.Text = Locker.ListSectionNames()[LockerTree.SelectedNode.Index];
                NameLocker.Text = Locker.ListSectionNames()[LockerTree.SelectedNode.Index];
            }
            catch { MessageBox.Show("Couldn't save changes."); }
        }
        public void DoLocationsList()
        {
            LocationsList.Items.Clear();
            CurrentLocation.Items.Clear();
            Ini.IniFile Locations = new Ini.IniFile(AppDir + "\\Data\\Locations.ini");
            for (int Progress = 0; Progress < Locations.ListSectionNames().Length; Progress++)
            {
                LocationsList.Items.Add(Locations.IniReadValue(Locations.ListSectionNames()[Progress], "OutpostDisplayName"));
                CurrentLocation.Items.Add(Locations.IniReadValue(Locations.ListSectionNames()[Progress], "OutpostDisplayName"));
            }
        }
        public void DoSkillList()
        {
            SkillList.Items.Clear();
            if (Class.SelectedItem == "Soldier")
            {
                Ini.IniFile SkillINI = new Ini.IniFile(AppDir + "\\Data\\gd_skills2_Roland.txt");
                for (int Progress = 0; Progress < SkillINI.ListSectionNames().Length; Progress++)
                    SkillList.Items.Add((string)SkillINI.IniReadValue(SkillINI.ListSectionNames()[Progress], "SkillName"));
            }
            else if (Class.SelectedItem == "Siren")
            {
                Ini.IniFile SkillINI = new Ini.IniFile(AppDir + "\\Data\\gd_Skills2_Lilith.txt");
                for (int Progress = 0; Progress < SkillINI.ListSectionNames().Length; Progress++)
                    SkillList.Items.Add((string)SkillINI.IniReadValue(SkillINI.ListSectionNames()[Progress], "SkillName"));
            }
            else if (Class.SelectedItem == "Hunter")
            {
                Ini.IniFile SkillINI = new Ini.IniFile(AppDir + "\\Data\\gd_skills2_Mordecai.txt");
                for (int Progress = 0; Progress < SkillINI.ListSectionNames().Length; Progress++)
                    SkillList.Items.Add((string)SkillINI.IniReadValue(SkillINI.ListSectionNames()[Progress], "SkillName"));
            }
            else if (Class.SelectedItem == "Berserker")
            {
                Ini.IniFile SkillINI = new Ini.IniFile(AppDir + "\\Data\\gd_Skills2_Brick.txt");
                for (int Progress = 0; Progress < SkillINI.ListSectionNames().Length; Progress++)
                    SkillList.Items.Add((string)SkillINI.IniReadValue(SkillINI.ListSectionNames()[Progress], "SkillName"));
            }
            else
            {
                Ini.IniFile SkillINI = new Ini.IniFile(AppDir + "\\Data\\gd_skills_common.txt");
                for (int Progress = 0; Progress < SkillINI.ListSectionNames().Length; Progress++)
                    SkillList.Items.Add((string)SkillINI.IniReadValue(SkillINI.ListSectionNames()[Progress], "SkillName"));
            }
        }
        public string GetAmmoName(string d_resources)
        {
            if (d_resources == "d_resources.AmmoResources.Ammo_Sniper_Rifle")
                return "Sniper Rifle";
            else if (d_resources == "d_resources.AmmoResources.Ammo_Repeater_Pistol")
                return "Repeater Pistol";
            else if (d_resources == "d_resources.AmmoResources.Ammo_Grenade_Protean")
                return "Protean Grenades";
            else if (d_resources == "d_resources.AmmoResources.Ammo_Patrol_SMG")
                return "Patrol SMG";
            else if (d_resources == "d_resources.AmmoResources.Ammo_Combat_Shotgun")
                return "Combat Shotgun";
            else if (d_resources == "d_resources.AmmoResources.Ammo_Combat_Rifle")
                return "Combat Rifle";
            else if (d_resources == "d_resources.AmmoResources.Ammo_Revolver_Pistol")
                return "Revolver Pistol";
            else if (d_resources == "d_resources.AmmoResources.Ammo_Rocket_Launcher")
                return "Rocket Launcher";
            else
                return d_resources;
        }
        public void StartUpFunctions()
        {
            DoTabs();
            if (System.IO.File.Exists(AppDir + "\\Data\\default.wtl") == false)
                System.IO.File.WriteAllText(AppDir + "\\Data\\default.wtl", "\r\n[New Weapon]\r\nType=Weapon\r\nRating=0\r\nDescription=\"Type in a description for the weapon here.\"\r\nPart1=None\r\nPart2=None\r\nPart3=None\r\nPart4=None\r\nPart5=None\r\nPart6=None\r\nPart7=None\r\nPart8=None\r\nPart9=None\r\nPart10=None\r\nPart11=None\r\nPart12=None\r\nPart13=None\r\nPart14=None");
            DoLockerTree(AppDir + "\\Data\\default.wtl");
            DoLocationsList();
            DoQuestList();
            DoEchoList();
            setXPchart();
        }
        public bool CheckIfNull(string[] Array)
        {
            try
            {
                if (Array.Length > 0)
                    return false;
                else
                    return true;
            }
            catch { return true; }
        }
        public int[] XPChart = new int[63];
        public void setXPchart()
        {
            XPChart[0] = 0;
XPChart[1] = 0;
XPChart[2] = 358;
XPChart[3] = 1241;
XPChart[4] = 2850;
XPChart[5] = 5376;
XPChart[6] = 8997;
XPChart[7] = 13886;
XPChart[8] = 20208;
XPChart[9] = 28126;
XPChart[10] = 37798;
XPChart[11] = 49377;
XPChart[12] = 63016;
XPChart[13] = 78861;
XPChart[14] = 97061;
XPChart[15] = 117757;
XPChart[16] = 141092;
XPChart[17] = 167207;
XPChart[18] = 196238;
XPChart[19] = 228322;
XPChart[20] = 263595;
XPChart[21] = 302190;
XPChart[22] = 344238;
XPChart[23] = 389873;
XPChart[24] = 439222;
XPChart[25] = 492414;
XPChart[26] = 549578;
XPChart[27] = 610840;
XPChart[28] = 676325;
XPChart[29] = 746158;
XPChart[30] = 820463;
XPChart[31] = 899363;
XPChart[32] = 982980;
XPChart[33] = 1071436;
XPChart[34] = 1164850;
XPChart[35] = 1263343;
XPChart[36] = 1367034;
XPChart[37] = 1476041;
XPChart[38] = 1590483;
XPChart[39] = 1710476;
XPChart[40] = 1836137;
XPChart[41] = 1967582;
XPChart[42] = 2104926;
XPChart[43] = 2248285;
XPChart[44] = 2397772;
XPChart[45] = 2553561;
XPChart[46] = 2715586;
XPChart[47] = 2884139;
XPChart[48] = 3059273;
XPChart[49] = 3241098;
XPChart[50] = 3429728;
XPChart[51] = 3628272;
XPChart[52] = 3827841;
XPChart[53] = 4037544;
XPChart[54] = 4254492;
XPChart[55] = 4478793;
XPChart[56] = 4710557;
XPChart[57] = 4949891;
XPChart[58] = 5196904;
XPChart[59] = 5451702;
XPChart[60] = 5714395;
XPChart[61] = 5985087;
XPChart[62] = 2147483647;
        }
        public bool MultipleIntroStateSaver(int Playthrough)
        {
            int TotalFound = 0;
            string[] PT;
            if (Playthrough == 1)
                PT = CurrentWSG.PT1Strings;
            else
                PT = CurrentWSG.PT2Strings;
            for (int Progress = 0; Progress < PT.Length; Progress++)
                if (PT[Progress] == "Z0_Missions.Missions.M_IntroStateSaver")
                    TotalFound = TotalFound + 1;
            if (TotalFound > 1)
                return true;
            else
                return false;
        }
        public int GetExtraStats(string[] WeaponParts, string StatName)
        {
            try
            {
                


                double ExtraDamage = 0;
                for (int i = 3; i < 14; i++)
                    if (WeaponParts[i].Contains("."))
                        ExtraDamage = ExtraDamage + Conversion.Val(new Ini.IniFile(AppDir + "\\Data\\" + WeaponParts[i].Substring(0, WeaponParts[i].IndexOf(".")) + ".txt").IniReadValue(WeaponParts[i].Substring(WeaponParts[i].IndexOf(".") + 1), StatName));

                if (StatName == "TechLevelIncrease")
                return (int)ExtraDamage;
                else
                return (int)((ExtraDamage)*100);
            }
            catch
            {
                return -1;
            }
        }
        public int GetWeaponDamage(string[] WeaponParts)
        {
            try
            {
            string ItemGradeFile = WeaponParts[0].Substring(0,WeaponParts[0].IndexOf(".")) + ".txt";
            string ItemGradePart = WeaponParts[0].Substring(WeaponParts[0].IndexOf(".")+1);
            string Manufacturer = WeaponParts[1].Substring(WeaponParts[1].LastIndexOf(".")+1);
            Ini.IniFile ItemGrade = new Ini.IniFile(AppDir + "\\Data\\" + ItemGradeFile);
            double ExtraDamage = 0;
            double Multiplier = Conversion.Val(new Ini.IniFile(AppDir + "\\Data\\" + WeaponParts[2].Substring(0, WeaponParts[2].IndexOf(".")) + ".txt").IniReadValue(WeaponParts[2].Substring(WeaponParts[2].IndexOf(".") + 1), "WeaponDamageFormulaMultiplier")); ;
            double Level = Conversion.Val(ItemGrade.IniReadValue(ItemGradePart,Manufacturer+"("+WeaponQuality.Value+")"));
            double Power = 1.3;
            double Offset = 9;
            for (int i = 3; i < 14; i++)
                if (WeaponParts[i].Contains("."))
                ExtraDamage = ExtraDamage + Conversion.Val(new Ini.IniFile(AppDir + "\\Data\\" + WeaponParts[i].Substring(0, WeaponParts[i].IndexOf(".")) + ".txt").IniReadValue(WeaponParts[i].Substring(WeaponParts[i].IndexOf(".") + 1), "WeaponDamage"));


            return (int)(ExtraDamage * (Multiplier * (Math.Pow(Level, Power) + Offset))) + (int)(Multiplier * (Math.Pow(Level, Power) + Offset));
            }
            catch
            {
                return 1337;
            }
        }
        public int GetWeaponDamage(string[] WeaponParts, double Itemgrade)
        {
            try
            {
                string ItemGradeFile = WeaponParts[0].Substring(0, WeaponParts[0].IndexOf(".")) + ".txt";
                string ItemGradePart = WeaponParts[0].Substring(WeaponParts[0].IndexOf(".") + 1);
                string Manufacturer = WeaponParts[1].Substring(WeaponParts[1].LastIndexOf(".") + 1);
                Ini.IniFile ItemGrade = new Ini.IniFile(AppDir + "\\Data\\" + ItemGradeFile);
                double ExtraDamage = 0;
                double Multiplier = Conversion.Val(new Ini.IniFile(AppDir + "\\Data\\" + WeaponParts[2].Substring(0, WeaponParts[2].IndexOf(".")) + ".txt").IniReadValue(WeaponParts[2].Substring(WeaponParts[2].IndexOf(".") + 1), "WeaponDamageFormulaMultiplier")); ;
                double Level = Itemgrade;
                double Power = 1.3;
                double Offset = 9;
                for (int i = 3; i < 14; i++)
                    if (WeaponParts[i].Contains("."))
                        ExtraDamage = ExtraDamage + Conversion.Val(new Ini.IniFile(AppDir + "\\Data\\" + WeaponParts[i].Substring(0, WeaponParts[i].IndexOf(".")) + ".txt").IniReadValue(WeaponParts[i].Substring(WeaponParts[i].IndexOf(".") + 1), "WeaponDamage"));


                return (int)(ExtraDamage * (Multiplier * (Math.Pow(Level, Power) + Offset))) + (int)(Multiplier * (Math.Pow(Level, Power) + Offset));
            }
            catch
            {
                return 1337;
            }
        }
        public bool IsDLCWeaponMode = false;
        public bool IsDLCItemMode = false;

        //No longer used. Just kept in case the need arises to have the DLC/Normal backpacks seperated.
        public void DoDLCWeaponTree()
        {
            WeaponTree.Nodes.Clear();
            Ini.IniFile Titles = new Ini.IniFile(AppDir + "\\Data\\Titles.ini");

            for (int build = 0; build < CurrentWSG.DLC.WeaponParts.Count; build++)
            {
                Node TempNode = new Node();
                TempNode.Text = GetName(Titles, CurrentWSG.DLC.WeaponParts, build, 14, "PartName");
                TempNode.Name = "" + build;
                for (int search_name = 0; search_name < 14; search_name++)
                {

                    //Node Sub = new Node();
                    //Sub.Text = "Part " + (search_name + 1) + ": " + CurrentWSG.WeaponStrings[build][search_name];
                    //TempNode.Nodes.Add(Sub);

                    //if(TempNode.Text == "" && Titles.IniReadValue(CurrentWSG.WeaponStrings[build, search_name], "PartName") != null)
                    //TempNode.Text = (Titles.IniReadValue(CurrentWSG.WeaponStrings[build, search_name], "PartName"));
                    //else if(Titles.IniReadValue(CurrentWSG.WeaponStrings[build, search_name], "PartName") != null) 
                    //TempNode.Text = (TempNode.Text + " " + Titles.IniReadValue(CurrentWSG.WeaponStrings[build, search_name], "PartName"));
                }
                if (TempNode.Text == "") TempNode.Text = "Unknown Weapon";
                WeaponTree.Nodes.Add(TempNode);
            }

        }
        public void DoDLCItemTree()
        {
            ItemTree.Nodes.Clear();
            Ini.IniFile Titles = new Ini.IniFile(AppDir + "\\Data\\Titles.ini");

            for (int build = 0; build < CurrentWSG.DLC.ItemParts.Count; build++)
            {
                Node TempNode = new Node();
                TempNode.Text = "";
                TempNode.Name = "" + build;
                for (int search_name = 0; search_name < 9; search_name++)
                {

                    //Node Sub = new Node();
                    //Sub.Text = "Part " + (search_name + 1) + ": " + CurrentWSG.ItemStrings[build][search_name];
                    //TempNode.Nodes.Add(Sub);

                    if (TempNode.Text == "" && Titles.IniReadValue(CurrentWSG.DLC.ItemParts[build][search_name], "PartName") != null)
                        TempNode.Text = (Titles.IniReadValue(CurrentWSG.DLC.ItemParts[build][search_name], "PartName"));
                    else if (Titles.IniReadValue(CurrentWSG.DLC.ItemParts[build][search_name], "PartName") != null)
                        TempNode.Text = (TempNode.Text + " " + Titles.IniReadValue(CurrentWSG.DLC.ItemParts[build][search_name], "PartName"));

                }
                if (TempNode.Text == "") TempNode.Text = "Unknown Item";
                //Node Sub_Ammo = new Node();
                //Node Sub_Quality = new Node();
                //Node Sub_Slot = new Node();
                //Sub_Ammo.Text = "Quantity: " + CurrentWSG.ItemValues[build][0];
                //Sub_Quality.Text = "Quality Level: " + CurrentWSG.ItemValues[build][1];
                //Sub_Slot.Text = "Equipped: " + CurrentWSG.ItemValues[build][2];
                //TempNode.Nodes.Add(Sub_Ammo);
                //TempNode.Nodes.Add(Sub_Quality);
                //TempNode.Nodes.Add(Sub_Slot);
                ItemTree.Nodes.Add(TempNode);
            }

        }

        //Don't ask.
        public string AmIACookie(bool passedCookieClass)
        {
            if (passedCookieClass)
                return "Good job, Mr. Cookie!";
            else
                return "Bad Mr. Cookie! You're actually a Brownie!";
        }

        //Recovers the latest version from my server.
        public void CheckVersion()
        {
            try
            {
                VersionFromServer = Updater.DownloadString("http://xander.x-fusion.co.uk/WillowTree/version.txt");
            }
            catch { }
            }

        private void TrySelectedNode(AdvTree DesiredTree, int SelectedIndex)
        {

            try
            {
                DesiredTree.SelectedNode = DesiredTree.Nodes[SelectedIndex];
            }
            catch { DesiredTree.SelectedNode = DesiredTree.Nodes[SelectedIndex - 1];
            }
            DesiredTree.SelectedNode.EnsureVisible();
        }
        private void TrySelectedNode(AdvTree DesiredTree, int RootNodeIndex , int SelectedIndex)
        {
            try { DesiredTree.SelectedNode = DesiredTree.Nodes[RootNodeIndex].Nodes[SelectedIndex]; }
            catch { DesiredTree.SelectedNode = DesiredTree.Nodes[RootNodeIndex].Nodes[SelectedIndex - 1]; }
            DesiredTree.SelectedNode.EnsureVisible();
        }

        //Resizes arrays. I really should just convert everything to lists...
        private void ResizeArrayLarger(ref string[,] Input, int rows, int cols)
        {
            string[,] newArray = new string[rows, cols];
            Array.Copy(Input, newArray, Input.Length);
            Input = newArray;
        }
        private void ResizeArraySmaller(ref string[,] Input, int rows, int cols)
        {
            string[,] newArray = new string[rows, cols];
            Array.Copy(Input, 0, newArray, 0, (long)(rows * cols));
            Input = newArray;
        }
        private void ResizeArrayLarger(ref string[] Input, int rows)
        {
            string[] newArray = new string[rows];
            Array.Copy(Input, newArray, Input.Length);
            Input = newArray;
        }
        private void ResizeArraySmaller(ref string[] Input, int rows)
        {
            string[] newArray = new string[rows];
            Array.Copy(Input, 0, newArray, 0, (long)rows);
            Input = newArray;
        }
        private void ResizeArrayLarger(ref int[,] Input, int rows, int cols)
        {
            int[,] newArray = new int[rows, cols];
            Array.Copy(Input, newArray, Input.Length);
            Input = newArray;
        }
        private void ResizeArraySmaller(ref int[,] Input, int rows, int cols)
        {
            int[,] newArray = new int[rows, cols];
            Array.Copy(Input, 0, newArray, 0, (long)((rows) * cols));
            Input = newArray;
        }
        private void ResizeArrayLarger(ref int[] Input, int rows)
        {
            int[] newArray = new int[rows];
            Array.Copy(Input, newArray, Input.Length);
            Input = newArray;
        }
        private void ResizeArraySmaller(ref int[] Input, int rows)
        {
            int[] newArray = new int[rows];
            Array.Copy(Input, 0, newArray, 0, (long)rows);
            Input = newArray;
        }
        private void ResizeArrayLarger(ref float[] Input, int rows)
        {
            float[] newArray = new float[rows];
            Array.Copy(Input, newArray, Input.Length);
            Input = newArray;
        }
        private void ResizeArraySmaller(ref float[] Input, int rows)
        {
            float[] newArray = new float[rows];
            Array.Copy(Input, 0, newArray, 0, (long)rows);
            Input = newArray;
        }

        WillowSaveGame CurrentWSG;

        private string AppDir = Path.GetDirectoryName(Application.ExecutablePath);
        public WillowTree()
        {
            InitializeComponent();

            UpdateBar.Hide();

            MainTab.Select();
            if (System.IO.Directory.Exists(AppDir + "\\Data") == false)
            {
                MessageBox.Show("Couldn't find the 'Data' folder! Please make sure that WillowTree# and the 'Data' folder are in the same directory.");
                Application.Exit();
            }
            //DoTabs();
//if (System.IO.File.Exists(AppDir + "\\Data\\default.wtl") == false)
                //System.IO.File.WriteAllText(AppDir + "\\Data\\default.wtl", "\r\n[New Weapon]\r\nType=Weapon\r\nRating=0\r\nDescription=\"Type in a description for the weapon here.\"\r\nPart1=None\r\nPart2=None\r\nPart3=None\r\nPart4=None\r\nPart5=None\r\nPart6=None\r\nPart7=None\r\nPart8=None\r\nPart9=None\r\nPart10=None\r\nPart11=None\r\nPart12=None\r\nPart13=None\r\nPart14=None");

            //DoLockerTree(AppDir + "\\Data\\default.wtl");
            GeneralTab.Enabled = false;
            SkillsTab.Enabled = false;
            AmmoTab.Enabled = false;
            ItemsTab.Enabled = false;
            WeaponsTab.Enabled = false;
            QuestsTab.Enabled = false;
            EchosTab.Enabled = false;
            //WTLTab.Enabled = false;
            ExportToBackpack.Enabled = false;
            ImportAllFromItems.Enabled = false;
            ImportAllFromWeapons.Enabled = false;
            ribbonControl2.SelectedRibbonTabItem = MainTab;
            if (System.Diagnostics.Debugger.IsAttached == true) DebugTab.Enabled = true;
            else
            {
                DebugTab.Enabled = false;
                DebugTab.Visible = false;
            }
            //DoLocationsList();
            //DoQuestList();
            //DoEchoList();


        }
                
        private void Open_Click(object sender, EventArgs e)
        {
            textBox1.Clear();


            OpenFileDialog tempOpen = new OpenFileDialog();
            tempOpen.DefaultExt = "*.sav";
            tempOpen.Filter = "WillowSaveGame(*.sav)|*.sav";

            

            if (tempOpen.ShowDialog() == DialogResult.OK)
            try
            {
                BankSpace.Enabled = false;
                BankSpace.Value = 0;

                CurrentWSG = new WillowSaveGame();
                CurrentWSG.OpenWSG(tempOpen.FileName);
                Ini.IniFile Locations = new Ini.IniFile(AppDir + "\\Data\\Locations.ini");

                textBox1.AppendText(CurrentWSG.Platform);
                CharacterName.Text = CurrentWSG.CharacterName;
                
                if(CurrentWSG.Level <= Level.Maximum)
                Level.Value = CurrentWSG.Level;
                else
                    Level.Value = Level.Maximum;
                if (CurrentWSG.Experience < Experience.Maximum && CurrentWSG.Experience > Experience.Minimum)
                    Experience.Value = CurrentWSG.Experience;
                else
                    Experience.Value = Experience.Maximum;
                if (CurrentWSG.SkillPoints <= SkillPoints.Maximum)
                    SkillPoints.Value = CurrentWSG.SkillPoints;
                else
                    SkillPoints.Value = SkillPoints.Maximum;
                PT2Unlocked.SelectedIndex = CurrentWSG.FinishedPlaythrough1;
                Cash.Value = CurrentWSG.Cash;
                BackpackSpace.Value = CurrentWSG.BackpackSize;
                EquipSlots.Value = CurrentWSG.EquipSlots;
                SaveNumber.Value = CurrentWSG.SaveNumber;
                CurrentLocation.SelectedItem = Locations.IniReadValue(CurrentWSG.CurrentLocation, "OutpostDisplayName");
                if (CurrentWSG.Class == "gd_Roland.Character.CharacterClass_Roland") Class.SelectedIndex = 0;
                else if (CurrentWSG.Class == "gd_lilith.Character.CharacterClass_Lilith") Class.SelectedIndex = 1;
                else if (CurrentWSG.Class == "gd_mordecai.Character.CharacterClass_Mordecai") Class.SelectedIndex = 2;
                else if (CurrentWSG.Class == "gd_Brick.Character.CharacterClass_Brick") Class.SelectedIndex = 3;
                textBox1.AppendText("\r\nHeader: " + CurrentWSG.MagicHeader + "\r\nVersion: " + CurrentWSG.VersionNumber + "\r\nPlyr String: " + CurrentWSG.PLYR + "\r\nRevision Number: " + CurrentWSG.RevisionNumber + "\r\nClass: " + CurrentWSG.Class + "\r\nLevel: " + CurrentWSG.Level + "\r\nExp: " + CurrentWSG.Experience + "\r\nSkill Points: " + CurrentWSG.SkillPoints + "\r\nunknown1: " + CurrentWSG.unknown1 + "\r\nMoney: " + CurrentWSG.Cash + "\r\nFinished Game: " + CurrentWSG.FinishedPlaythrough1 + "\r\nNumber of skills: " + CurrentWSG.NumberOfSkills + "\r\nCurrent Quest: " + CurrentWSG.CurrentQuest + "\r\nTotal PT1 Quests: " + CurrentWSG.TotalPT1Quests + "\r\nSecondary Quest: " + CurrentWSG.SecondaryQuest + "\r\nTotal PT2 Quests: " + CurrentWSG.TotalPT2Quests + "\r\nUnknown Quest Value: " + CurrentWSG.UnknownQuestValue + "\r\nSometimesNull: " + CurrentWSG.SometimesNull);
                ActivePT1Quest.Text = CurrentWSG.CurrentQuest;
                ActivePT2Quest.Text = CurrentWSG.SecondaryQuest;
                DoQuestTree();
                DoLocationTree();
                DoSkillTree();
                DoEchoTree();
                DoAmmoTree();
                DoWeaponTree();
                DoItemTree();
                DoSkillList();

                GeneralTab.Enabled = true;
                AmmoTab.Enabled = true;
                SkillsTab.Enabled = true;
                EchosTab.Enabled = true;
                WTLTab.Enabled = true;
                ExportToBackpack.Enabled = true;
                ImportAllFromItems.Enabled = true;
                ImportAllFromWeapons.Enabled = true;
                WeaponsTab.Enabled = true;
                ItemsTab.Enabled = true;
                QuestsTab.Enabled = true;
                Save.Enabled = true;
                SaveAs.Enabled = true;

                if (CurrentWSG.DLC.BankSize > 0)
                {
                    BankSpace.Value = CurrentWSG.DLC.BankSize;
                    BankSpace.Enabled = true;
                }
            }
            //if (VersionFromServer == "f")    
            //try {
              //  DoAmmoTree();
            //}
                catch
                {

                    if (CurrentWSG.EchoStringsPT2 == null && CurrentWSG.NumberOfEchosPT2 > 0 && CurrentWSG.TotalPT2Quests > 1 && CurrentWSG.NumberOfEchosPT2 < 300)
                        MessageBox.Show("Error reading PT2 echo logs.");
                    else
                    MessageBox.Show("Could not open save.");
                    BankSpace.Enabled = false;
                    BankSpace.Value = 0;
                    GeneralTab.Enabled = false;
                    AmmoTab.Enabled = false;
                   SkillsTab.Enabled = false;
                    EchosTab.Enabled = false;
                    //WTLTab.Enabled = false;
                    ExportToBackpack.Enabled = false;
                    ImportAllFromItems.Enabled = false;
                    ImportAllFromWeapons.Enabled = false;
                    WeaponsTab.Enabled = false;
                    ItemsTab.Enabled = false;
                    QuestsTab.Enabled = false;
                    Save.Enabled = false;
                    SaveAs.Enabled = false;
                    MainTab.Select();
                    textBox1.AppendText("\r\nHeader: " + CurrentWSG.MagicHeader + "\r\nVersion: " + CurrentWSG.VersionNumber + "\r\nPlyr String: " + CurrentWSG.PLYR + "\r\nRevision Number: " + CurrentWSG.RevisionNumber + "\r\nClass: " + CurrentWSG.Class + "\r\nLevel: " + CurrentWSG.Level + "\r\nExp: " + CurrentWSG.Experience + "\r\nSkill Points: " + CurrentWSG.SkillPoints + "\r\nunknown1: " + CurrentWSG.unknown1 + "\r\nMoney: " + CurrentWSG.Cash + "\r\nFinished Game: " + CurrentWSG.FinishedPlaythrough1 + "\r\nNumber of skills: " + CurrentWSG.NumberOfSkills + "\r\nCurrent Quest: " + CurrentWSG.CurrentQuest + "\r\nTotal PT1 Quests: " + CurrentWSG.TotalPT1Quests + "\r\nSecondary Quest: " + CurrentWSG.SecondaryQuest + "\r\nTotal PT2 Quests: " + CurrentWSG.TotalPT2Quests + "\r\nUnknown Quest Value: " + CurrentWSG.UnknownQuestValue + "\r\nSometimesNull: " + CurrentWSG.SometimesNull);
                }
        }


        private void SkillTree_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            try
            {
                SkillName.Text = CurrentWSG.SkillNames[SkillTree.SelectedNode.Index];
                SkillLevel.Value = CurrentWSG.LevelOfSkills[SkillTree.SelectedNode.Index];
                SkillExp.Value = CurrentWSG.ExpOfSkills[SkillTree.SelectedNode.Index];
                if (CurrentWSG.InUse[SkillTree.SelectedNode.Index] == -1) SkillActive.SelectedItem = "No";
                else SkillActive.SelectedItem = "Yes";
            }
            catch { }
        }

        private void Save_Click(object sender, EventArgs e)
        {


            Ini.IniFile Locations = new Ini.IniFile(AppDir + "\\Data\\Locations.ini");
            SaveFileDialog tempSave = new SaveFileDialog();
            tempSave.DefaultExt = "*.sav";
            tempSave.Filter = "WillowSaveGame(*.sav)|*.sav";

            tempSave.FileName = CurrentWSG.OpenedWSG;

            if (tempSave.ShowDialog() == DialogResult.OK)
            {
                if (BankSpace.Enabled)
                    CurrentWSG.DLC.BankSize = (int)BankSpace.Value;

                if (Class.SelectedIndex == 0) CurrentWSG.Class = "gd_Roland.Character.CharacterClass_Roland";
                else if (Class.SelectedIndex == 1) CurrentWSG.Class = "gd_lilith.Character.CharacterClass_Lilith";
                else if (Class.SelectedIndex == 2) CurrentWSG.Class = "gd_mordecai.Character.CharacterClass_Mordecai";
                else if (Class.SelectedIndex == 3) CurrentWSG.Class = "gd_Brick.Character.CharacterClass_Brick";
                CurrentWSG.CharacterName = CharacterName.Text;
                CurrentWSG.Level = (int)Level.Value;
                CurrentWSG.Experience = (int)Experience.Value;
                CurrentWSG.SkillPoints = (int)SkillPoints.Value;
                CurrentWSG.FinishedPlaythrough1 = PT2Unlocked.SelectedIndex;
                CurrentWSG.Cash = (int)Cash.Value;
                CurrentWSG.BackpackSize = (int)BackpackSpace.Value;
                CurrentWSG.EquipSlots = (int)EquipSlots.Value;
                CurrentWSG.SaveNumber = (int)SaveNumber.Value;
                if (CurrentLocation.SelectedText != "" && CurrentLocation.SelectedText != null)
                    CurrentWSG.CurrentLocation = Locations.ListSectionNames()[CurrentLocation.SelectedIndex];

                if (CurrentWSG.Platform == "PS3" || CurrentWSG.Platform == "PC")
                {
                    DJsIO Save = new DJsIO(tempSave.FileName, DJFileMode.Create, CurrentWSG.WSGEndian);
                    Save.Write(CurrentWSG.SaveWSG());
                    Save.Close();
                }

                else if (CurrentWSG.Platform == "X360")
                {


                    DJsIO Save = new DJsIO(tempSave.FileName+".temp", DJFileMode.Create, CurrentWSG.WSGEndian);
                    Save.Write(CurrentWSG.SaveWSG());
                    Save.Close();

                    CreateSTFS Package = new CreateSTFS();
                    
                    Package.STFSType = STFSType.Type1;
                    Package.HeaderData.ProfileID = CurrentWSG.ProfileID;
                    Package.HeaderData.DeviceID = CurrentWSG.DeviceID;
                    Assembly newAssembly = Assembly.GetExecutingAssembly();
                    Stream WT_Icon = newAssembly.GetManifestResourceStream("WillowTree.WT_CON.png");
                    Package.HeaderData.ContentImage = System.Drawing.Image.FromStream(WT_Icon);
                    Package.HeaderData.Title_Display = CurrentWSG.CharacterName + " - Level " + CurrentWSG.Level + " - " + CurrentLocation.Text;
                    Package.HeaderData.Title_Package = "Borderlands";
                    Package.HeaderData.TitleID = 1414793191;
                    Package.AddFile(tempSave.FileName + ".temp", "SaveGame.sav");

                    STFSPackage CON = new STFSPackage(Package, new RSAParams(AppDir + "\\Data\\KV.bin"), tempSave.FileName, new X360.Other.LogRecord());


                    CON.FlushPackage(new RSAParams(AppDir + "\\Data\\KV.bin"));


                    CON.CloseIO();
                    File.Delete(tempSave.FileName + ".temp");

                }
                CurrentWSG.OpenedWSG = tempSave.FileName;
                MessageBox.Show("Saved WSG to: " + CurrentWSG.OpenedWSG);
            }
        }
        private void Save_Click_1(object sender, EventArgs e)
        {
            if(File.Exists(CurrentWSG.OpenedWSG + ".bak") == false)
            try { 
                File.Copy(CurrentWSG.OpenedWSG, CurrentWSG.OpenedWSG + ".bak"); 
            }
            catch { }

            try
            {
                Ini.IniFile Locations = new Ini.IniFile(AppDir + "\\Data\\Locations.ini");

                if (BankSpace.Enabled)
                    CurrentWSG.DLC.BankSize = (int)BankSpace.Value;

                if (Class.SelectedIndex == 0) CurrentWSG.Class = "gd_Roland.Character.CharacterClass_Roland";
                else if (Class.SelectedIndex == 1) CurrentWSG.Class = "gd_lilith.Character.CharacterClass_Lilith";
                else if (Class.SelectedIndex == 2) CurrentWSG.Class = "gd_mordecai.Character.CharacterClass_Mordecai";
                else if (Class.SelectedIndex == 3) CurrentWSG.Class = "gd_Brick.Character.CharacterClass_Brick";
                CurrentWSG.CharacterName = CharacterName.Text;
                CurrentWSG.Level = (int)Level.Value;
                CurrentWSG.Experience = (int)Experience.Value;
                CurrentWSG.SkillPoints = (int)SkillPoints.Value;
                CurrentWSG.FinishedPlaythrough1 = PT2Unlocked.SelectedIndex;
                CurrentWSG.Cash = (int)Cash.Value;
                CurrentWSG.BackpackSize = (int)BackpackSpace.Value;
                CurrentWSG.EquipSlots = (int)EquipSlots.Value;
                CurrentWSG.SaveNumber = (int)SaveNumber.Value;
                if (CurrentLocation.SelectedText != "" && CurrentLocation.SelectedText != null)
                CurrentWSG.CurrentLocation = Locations.ListSectionNames()[CurrentLocation.SelectedIndex];

                if (CurrentWSG.Platform == "PS3" || CurrentWSG.Platform == "PC")
                {
                    DJsIO Save = new DJsIO(CurrentWSG.OpenedWSG, DJFileMode.Create, CurrentWSG.WSGEndian);
                    Save.Write(CurrentWSG.SaveWSG());
                    MessageBox.Show("Saved WSG to: " + CurrentWSG.OpenedWSG);
                    Save.Close();
                }

                else if (CurrentWSG.Platform == "X360")
                {


                    //CreateContents Contents = new CreateContents();
                    //Contents.AddFile(new DJsIO(CurrentWSG.SaveWSG(), true), "SaveGame.sav");
                    //Contents.STFSType = STFSType.Type1;
                    //CreateSTFS Session = new CreateSTFS();

                    DJsIO Save = new DJsIO(CurrentWSG.OpenedWSG + ".temp", DJFileMode.Create, CurrentWSG.WSGEndian);
                    Save.Write(CurrentWSG.SaveWSG());
                    Save.Close();

                    CreateSTFS Package = new CreateSTFS();

                    Package.STFSType = STFSType.Type1;
                    Package.HeaderData.ProfileID = CurrentWSG.ProfileID;
                    Package.HeaderData.DeviceID = CurrentWSG.DeviceID;
                    Assembly newAssembly = Assembly.GetExecutingAssembly();
                    Stream WT_Icon = newAssembly.GetManifestResourceStream("WillowTree.WT_CON.png");
                    Package.HeaderData.ContentImage = System.Drawing.Image.FromStream(WT_Icon);
                    Package.HeaderData.Title_Display = CurrentWSG.CharacterName + " - Level " + CurrentWSG.Level + " - " + CurrentLocation.Text;
                    Package.HeaderData.Title_Package = "Borderlands";
                    Package.HeaderData.TitleID = 1414793191;
                    Package.AddFile(CurrentWSG.OpenedWSG + ".temp", "SaveGame.sav");

                    STFSPackage CON = new STFSPackage(Package, new RSAParams(AppDir + "\\Data\\KV.bin"), CurrentWSG.OpenedWSG, new X360.Other.LogRecord());


                    CON.FlushPackage(new RSAParams(AppDir + "\\Data\\KV.bin"));


                    CON.CloseIO();
                    File.Delete(CurrentWSG.OpenedWSG + ".temp");
                    MessageBox.Show("Saved WSG to: " + CurrentWSG.OpenedWSG);
                }
            }
            catch { MessageBox.Show("Couldn't save WSG"); }
        }
        private void ExitWT_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            //AliasToMyClass somevar = new AliasToMyClass();

            //MessageBox.Show( somevar);
        }
        private void WillowTree_Load(object sender, EventArgs e)
        {
            Thread t1 = new Thread(CheckVersion);
            t1.Start();
            StartUpFunctions();
            t1.Join();
            if (VersionFromServer != Version.Text && VersionFromServer != "" && VersionFromServer != null)
            {
                UpdateButton.Text = "Version " + VersionFromServer + " is now available! Click here to download.";
                UpdateBar.Show();
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }
        private void Breakpoint_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debugger.Break();
        }

        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< WEAPONS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  \\

        private void WeaponTree_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {

            try
            {

                if (IsDLCWeaponMode)
                {
                    CurrentWeaponParts.Items.Clear();
                    CurrentPartsGroup.Text = WeaponTree.SelectedNode.Text;

                    for (int build_list = 0; build_list < 14; build_list++)
                    {
                        CurrentWeaponParts.Items.Add(CurrentWSG.DLC.WeaponParts[WeaponTree.SelectedNode.Index][build_list]);
                    }

                    RemainingAmmo.Value = CurrentWSG.DLC.WeaponAmmo[WeaponTree.SelectedNode.Index];
                    WeaponQuality.Value = CurrentWSG.DLC.WeaponQuality[WeaponTree.SelectedNode.Index];
                    WeaponItemGradeSlider.Value = CurrentWSG.DLC.WeaponLevel[WeaponTree.SelectedNode.Index];
                    if (CurrentWSG.DLC.WeaponEquippedSlot[WeaponTree.SelectedNode.Index] == 0) EquippedSlot.SelectedItem = "Unequipped";
                    else if (CurrentWSG.DLC.WeaponEquippedSlot[WeaponTree.SelectedNode.Index] == 1) EquippedSlot.SelectedItem = "Slot 1 (Up)";
                    else if (CurrentWSG.DLC.WeaponEquippedSlot[WeaponTree.SelectedNode.Index] == 2) EquippedSlot.SelectedItem = "Slot 2 (Down)";
                    else if (CurrentWSG.DLC.WeaponEquippedSlot[WeaponTree.SelectedNode.Index] == 3) EquippedSlot.SelectedItem = "Slot 3 (Left)";
                    else if (CurrentWSG.DLC.WeaponEquippedSlot[WeaponTree.SelectedNode.Index] > 3) EquippedSlot.SelectedItem = "Slot 4 (Right)";
                }
                else
                {
                    CurrentWeaponParts.Items.Clear();
                    CurrentPartsGroup.Text = WeaponTree.SelectedNode.Text;

                    for (int build_list = 0; build_list < 14; build_list++)
                    {
                        CurrentWeaponParts.Items.Add(CurrentWSG.WeaponStrings[WeaponTree.SelectedNode.Index][build_list]);
                    }

                    RemainingAmmo.Value = CurrentWSG.WeaponValues[WeaponTree.SelectedNode.Index][0];
                    WeaponQuality.Value = CurrentWSG.WeaponValues[WeaponTree.SelectedNode.Index][1];
                    if (CurrentWSG.WeaponValues[WeaponTree.SelectedNode.Index][2] == 0) EquippedSlot.SelectedItem = "Unequipped";
                    else if (CurrentWSG.WeaponValues[WeaponTree.SelectedNode.Index][2] == 1) EquippedSlot.SelectedItem = "Slot 1 (Up)";
                    else if (CurrentWSG.WeaponValues[WeaponTree.SelectedNode.Index][2] == 2) EquippedSlot.SelectedItem = "Slot 2 (Down)";
                    else if (CurrentWSG.WeaponValues[WeaponTree.SelectedNode.Index][2] == 3) EquippedSlot.SelectedItem = "Slot 3 (Left)";
                    else if (CurrentWSG.WeaponValues[WeaponTree.SelectedNode.Index][2] > 3) EquippedSlot.SelectedItem = "Slot 4 (Right)";
                    WeaponItemGradeSlider.Value = CurrentWSG.WeaponValues[WeaponTree.SelectedNode.Index][3];
                }

            }
            catch { }

        }

        private void PartCategories_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            if (CurrentWeaponParts.SelectedItem != null && PartCategories.SelectedNode.HasChildNodes == false)
            {
                CurrentWeaponParts.Items[CurrentWeaponParts.SelectedIndex] = PartCategories.SelectedNode.Parent.Text + "." + PartCategories.SelectedNode.Text;
            }

        }

        private void NewWeapons_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsDLCWeaponMode)
                {

                    CurrentWSG.DLC.TotalWeapons++;
                    CurrentWSG.DLC.WeaponParts.Add(new List<string>());
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Item Grade");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Manufacturer");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Weapon Type");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Body");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Grip");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Mag");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Barrel");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Sight");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Stock");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Action");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Accessory");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Material");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Prefix");
                    CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add("Title");
                    CurrentWSG.DLC.WeaponLevel.Add(0);
                    CurrentWSG.DLC.WeaponQuality.Add(0);
                    CurrentWSG.DLC.WeaponAmmo.Add(0);
                    CurrentWSG.DLC.WeaponEquippedSlot.Add(0);


                    DoDLCWeaponTree();
                }
                else
                {
                    CurrentWSG.NumberOfWeapons++;
                    CurrentWSG.WeaponStrings.Add(new List<string>());
                    CurrentWSG.WeaponValues.Add(new List<int>());
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Item Grade");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Manufacturer");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Weapon Type");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Body");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Grip");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Mag");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Barrel");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Sight");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Stock");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Action");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Accessory");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Material");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Prefix");
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add("Title");
                    CurrentWSG.WeaponValues[(CurrentWSG.NumberOfWeapons - 1)].Add(0);
                    CurrentWSG.WeaponValues[(CurrentWSG.NumberOfWeapons - 1)].Add(0);
                    CurrentWSG.WeaponValues[(CurrentWSG.NumberOfWeapons - 1)].Add(0);
                    CurrentWSG.WeaponValues[(CurrentWSG.NumberOfWeapons - 1)].Add(0);

                    Node TempNode = new Node();
                    TempNode.Text = "New Weapon";
                    WeaponTree.Nodes.Add(TempNode);
                    //DoWeaponTree();
                }
            }
            catch { }
        }

        private void SaveChangesWeapon_Click(object sender, EventArgs e)
        {
            try
            {
                int Selected = WeaponTree.SelectedNode.Index;;

                    if (IsDLCWeaponMode)
                    {
                        for (int Progress = 0; Progress < 14; Progress++)
                            CurrentWSG.DLC.WeaponParts[WeaponTree.SelectedNode.Index][Progress] = (string)CurrentWeaponParts.Items[Progress];
                        CurrentWSG.DLC.WeaponAmmo[WeaponTree.SelectedNode.Index] = (int)RemainingAmmo.Value;
                        CurrentWSG.DLC.WeaponQuality[WeaponTree.SelectedNode.Index] = (int)WeaponQuality.Value;
                        CurrentWSG.DLC.WeaponEquippedSlot[WeaponTree.SelectedNode.Index] = EquippedSlot.SelectedIndex;
                        CurrentWSG.DLC.WeaponLevel[WeaponTree.SelectedNode.Index] = WeaponItemGradeSlider.Value;

                        //DoDLCWeaponTree();
                    }
                    else
                    {
                        for (int Progress = 0; Progress < 14; Progress++)
                            CurrentWSG.WeaponStrings[WeaponTree.SelectedNode.Index][Progress] = (string)CurrentWeaponParts.Items[Progress];
                        CurrentWSG.WeaponValues[WeaponTree.SelectedNode.Index][0] = (int)RemainingAmmo.Value;
                        CurrentWSG.WeaponValues[WeaponTree.SelectedNode.Index][1] = (int)WeaponQuality.Value;
                        CurrentWSG.WeaponValues[WeaponTree.SelectedNode.Index][2] = EquippedSlot.SelectedIndex;
                        CurrentWSG.WeaponValues[WeaponTree.SelectedNode.Index][3] = (int)WeaponItemGradeSlider.Value;
                        //DoWeaponTree();
                    }
                    WeaponTree.SelectedNode.Text = GetName(new Ini.IniFile(AppDir + "\\Data\\Titles.ini"), CurrentWSG.WeaponStrings, WeaponTree.SelectedIndex, 14, "PartName");
            }
            catch { }
        } // Save Changes

        private void ExportWeaponToClipboard_Click(object sender, EventArgs e)
        {
            InOutPartsBox.Clear();
            for (int Progress = 0; Progress < 14; Progress++)
            {
                if (Progress > 0) InOutPartsBox.AppendText("\r\n");
                InOutPartsBox.AppendText((string)CurrentWeaponParts.Items[Progress]);
            }
            InOutPartsBox.AppendText("\r\n" + RemainingAmmo.Value);
            InOutPartsBox.AppendText("\r\n" + WeaponQuality.Value);
            InOutPartsBox.AppendText("\r\n" + EquippedSlot.SelectedIndex);
            InOutPartsBox.AppendText("\r\n" + WeaponItemGradeSlider.Value);
            Clipboard.SetText(InOutPartsBox.Text);
        } // Export -> to Clipboard

        private void ImportWeaponsFromClipboard_Click(object sender, EventArgs e)
        {
            InOutPartsBox.Clear();
            InOutPartsBox.Text = Clipboard.GetText();
            InOutPartsBox.Text = InOutPartsBox.Text.Replace(" ", "");

            for (int Progress = 0; Progress < 14; Progress++)
                CurrentWeaponParts.Items[Progress] = InOutPartsBox.Lines[Progress];
            //CurrentWeaponParts.Items[0] = Cookie(true);
            try
            {
                RemainingAmmo.Value = Convert.ToInt32(InOutPartsBox.Lines[14]);
            }
            catch
            {
                RemainingAmmo.Value = 0;
            }
            try
            {
                WeaponQuality.Value = Convert.ToInt32(InOutPartsBox.Lines[15]);
            }
            catch
            {
                WeaponQuality.Value = 0;
            }
            try
            {
                EquippedSlot.SelectedIndex = Convert.ToInt32(InOutPartsBox.Lines[16]);
            }
            catch
            {
                EquippedSlot.SelectedIndex = 0;

            }
        }// Import -> from Clipboard

        private void DuplicateWeapons_Click(object sender, EventArgs e)
        {
            try
            {
                int Selected = WeaponTree.SelectedNode.Index;

                if (IsDLCWeaponMode)
                {
                    CurrentWSG.DLC.WeaponParts.Add(new List<string>());
                    foreach (string i in CurrentWSG.DLC.WeaponParts[Selected])
                        CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add(i);
                    CurrentWSG.DLC.WeaponAmmo.Add(CurrentWSG.DLC.WeaponAmmo[Selected]);
                    CurrentWSG.DLC.WeaponQuality.Add(CurrentWSG.DLC.WeaponQuality[Selected]);
                    CurrentWSG.DLC.WeaponLevel.Add(CurrentWSG.DLC.WeaponLevel[Selected]);
                    CurrentWSG.DLC.WeaponEquippedSlot.Add(0);
                    CurrentWSG.DLC.TotalWeapons++;

                    DoDLCWeaponTree();
                }
                else
                {
                    CurrentWSG.WeaponStrings.Add(new List<string>());
                    CurrentWSG.WeaponValues.Add(new List<int>());
                    foreach (string i in CurrentWSG.WeaponStrings[Selected])
                    CurrentWSG.WeaponStrings[CurrentWSG.WeaponStrings.Count - 1].Add(i);
                    foreach (int i in CurrentWSG.WeaponValues[Selected])
                        CurrentWSG.WeaponValues[CurrentWSG.WeaponStrings.Count - 1].Add(i);
                    //CurrentWSG.WeaponValues.Add(CurrentWSG.WeaponValues[Selected]);
                    CurrentWSG.WeaponValues[CurrentWSG.NumberOfWeapons][2] = 0;
                    CurrentWSG.NumberOfWeapons++;
                    WeaponTree.Nodes.Add(WeaponTree.SelectedNode.Copy());
                    //DoWeaponTree();
                }
            }
            catch { MessageBox.Show("Select a weapon to duplicate first."); }
        } // Duplicate Weapon

        private void DeleteWeapons_Click(object sender, EventArgs e)
        {
            try
            {
                int Selected = WeaponTree.SelectedIndex;
                    WeaponTree.DeselectNode(WeaponTree.SelectedNode, new eTreeAction());
                    WeaponTree.Nodes.RemoveAt(Selected);
                    CurrentWSG.WeaponStrings.RemoveAt(Selected);
                    CurrentWSG.WeaponValues.RemoveAt(Selected);
                    CurrentWSG.NumberOfWeapons--;
                    TrySelectedNode(WeaponTree, Selected);
                    //DoWeaponTree();

            }
            catch { MessageBox.Show("Select a weapon to delete first."); }
        } // Delete Weapon

        private void ExportWeaponsToFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog ToFile = new SaveFileDialog();
            ToFile.DefaultExt = "*.txt";
            ToFile.Filter = "Weapon Files(*.txt)|*.txt";
            ToFile.FileName = CurrentPartsGroup.Text + ".txt";
            if (ToFile.ShowDialog() == DialogResult.OK)
            {

                InOutPartsBox.Clear();
                for (int Progress = 0; Progress < 14; Progress++)
                {
                    if (Progress > 0) InOutPartsBox.AppendText("\r\n");
                    InOutPartsBox.AppendText((string)CurrentWeaponParts.Items[Progress]);
                }
                InOutPartsBox.AppendText("\r\n" + RemainingAmmo.Value);
                InOutPartsBox.AppendText("\r\n" + WeaponQuality.Value);
                InOutPartsBox.AppendText("\r\n" + EquippedSlot.SelectedIndex);
                InOutPartsBox.AppendText("\r\n" + WeaponItemGradeSlider.Value);



                System.IO.File.WriteAllLines(ToFile.FileName, InOutPartsBox.Lines);
            }
        } // Export -> to File

        private void ImportWeaponFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog FromFile = new OpenFileDialog();
            FromFile.DefaultExt = "*.txt";
            FromFile.Filter = "Weapon Files(*.txt)|*.txt";

            FromFile.FileName = CurrentPartsGroup.Text + ".txt";

            if (FromFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    InOutPartsBox.Clear();
                    InOutPartsBox.Text = System.IO.File.ReadAllText(FromFile.FileName);

                    for (int Progress = 0; Progress < 14; Progress++)
                        CurrentWeaponParts.Items[Progress] = InOutPartsBox.Lines[Progress];
                    if (InOutPartsBox.Lines[14] != null)
                        RemainingAmmo.Value = Convert.ToInt32(InOutPartsBox.Lines[14]);
                    else
                        RemainingAmmo.Value = 0;
                    if (InOutPartsBox.Lines[15] != null)
                        WeaponQuality.Value = Convert.ToInt32(InOutPartsBox.Lines[15]);
                    else WeaponQuality.Value = 0;
                    if (InOutPartsBox.Lines[16] != null)
                        EquippedSlot.SelectedIndex = Convert.ToInt32(InOutPartsBox.Lines[16]);
                    else
                        EquippedSlot.SelectedIndex = 0;
                    if (InOutPartsBox.Lines[17] != null)
                        WeaponItemGradeSlider.Value = Convert.ToInt32(InOutPartsBox.Lines[17]);
                    else 
                        WeaponItemGradeSlider.Value = 0;
                }
                catch { }
            }
        } // Import -> from File

        private void DeletePartWeapon_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentWeaponParts.Items[CurrentWeaponParts.SelectedIndex] = "None";
            }
            catch { }
        } // Delete Part

        private void WeaponsFromClipboard_Click(object sender, EventArgs e) // Insert -> from Clipboard
        {
           // try
           // {
                InOutPartsBox.Clear();
                InOutPartsBox.Text = Clipboard.GetText();
                InOutPartsBox.Text = InOutPartsBox.Text.Replace(" ", "");
                if (IsDLCWeaponMode)
                {
                    CurrentWSG.DLC.WeaponParts.Add(new List<string>());
                    CurrentWSG.DLC.TotalWeapons++;
                    for (int Progress = 0; Progress < 14; Progress++)
                    {
                        CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add(InOutPartsBox.Lines[Progress]);

                    }
                    CurrentWSG.DLC.WeaponAmmo.Add(Convert.ToInt32(InOutPartsBox.Lines[14]));
                    CurrentWSG.DLC.WeaponQuality.Add(Convert.ToInt32(InOutPartsBox.Lines[15]));
                    CurrentWSG.DLC.WeaponEquippedSlot.Add(Convert.ToInt32(InOutPartsBox.Lines[16]));
                    DoDLCWeaponTree();
                }
                else
                {
                    CurrentWSG.WeaponStrings.Add(new List<string>());
                    CurrentWSG.WeaponValues.Add(new List<int>());
                    CurrentWSG.NumberOfWeapons++;
                    for (int Progress = 0; Progress < 14; Progress++)
                        CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add(InOutPartsBox.Lines[Progress]);
                    for (int Progress = 0; Progress < 3; Progress++)
                        CurrentWSG.WeaponValues[CurrentWSG.NumberOfWeapons - 1].Add(Convert.ToInt32(InOutPartsBox.Lines[Progress + 14]));
                    if(InOutPartsBox.Lines.Length > 17)
                        CurrentWSG.WeaponValues[CurrentWSG.NumberOfWeapons - 1].Add(Convert.ToInt32(InOutPartsBox.Lines[17]));
                    else 
                        CurrentWSG.WeaponValues[CurrentWSG.NumberOfWeapons - 1].Add(0);
                    Node TempNode = new Node();
                    TempNode.Text = GetName(new Ini.IniFile(AppDir + "\\Data\\Titles.ini"), CurrentWSG.WeaponStrings, CurrentWSG.NumberOfWeapons - 1, 14, "PartName");
                    WeaponTree.Nodes.Add(TempNode);
                    //DoWeaponTree();
                }
           // }
          //  catch { }
        }

        private void WeaponFromFile_Click(object sender, EventArgs e) // Insert -> from File
        {
            OpenFileDialog FromFile = new OpenFileDialog();
            FromFile.DefaultExt = "*.txt";
            FromFile.Filter = "Weapon Files(*.txt)|*.txt";

            FromFile.FileName = CurrentPartsGroup.Text + ".txt";

            if (FromFile.ShowDialog() == DialogResult.OK)
            {

                InOutPartsBox.Clear();
                InOutPartsBox.Text = System.IO.File.ReadAllText(FromFile.FileName);
                if (IsDLCWeaponMode)
                {
                    CurrentWSG.DLC.WeaponParts.Add(new List<string>());
                    CurrentWSG.DLC.TotalWeapons++;
                    for (int Progress = 0; Progress < 14; Progress++)
                    {
                        CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add(InOutPartsBox.Lines[Progress]);

                    }
                    CurrentWSG.DLC.WeaponAmmo.Add(Convert.ToInt32(InOutPartsBox.Lines[14]));
                    CurrentWSG.DLC.WeaponQuality.Add(Convert.ToInt32(InOutPartsBox.Lines[15]));
                    CurrentWSG.DLC.WeaponEquippedSlot.Add(Convert.ToInt32(InOutPartsBox.Lines[16]));
                    DoDLCWeaponTree();
                }
                else
                {
                    CurrentWSG.WeaponStrings.Add(new List<string>());
                    CurrentWSG.WeaponValues.Add(new List<int>());
                    CurrentWSG.NumberOfWeapons++;
                    for (int Progress = 0; Progress < 14; Progress++)
                        CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add(InOutPartsBox.Lines[Progress]);
                    for (int Progress = 0; Progress < 3; Progress++)
                        CurrentWSG.WeaponValues[CurrentWSG.NumberOfWeapons - 1].Add(Convert.ToInt32(InOutPartsBox.Lines[Progress + 14]));
                    if (InOutPartsBox.Lines.Length > 17)
                        CurrentWSG.WeaponValues[CurrentWSG.NumberOfWeapons - 1].Add(Convert.ToInt32(InOutPartsBox.Lines[17]));
                    else CurrentWSG.WeaponValues[CurrentWSG.NumberOfWeapons - 1].Add(0);

                    Node TempNode = new Node();
                    TempNode.Text = GetName(new Ini.IniFile(AppDir + "\\Data\\Titles.ini"), CurrentWSG.WeaponStrings, CurrentWSG.NumberOfWeapons - 1, 14, "PartName");
                    WeaponTree.Nodes.Add(TempNode);
                    //DoWeaponTree();
                }
            }

        }

        private void ExportToLockerWeapon_Click(object sender, EventArgs e)
        {

            InOutPartsBox.Clear();
            for (int Progress = 0; Progress < 14; Progress++)
            {
                if (Progress > 0) InOutPartsBox.AppendText("\r\n");
                InOutPartsBox.AppendText((string)CurrentWeaponParts.Items[Progress]);
            }
            //New Weapon

            string tempINI = System.IO.File.ReadAllText(OpenedLocker);
            int Occurances = 0;
            Ini.IniFile Locker = new Ini.IniFile(OpenedLocker);
            for (int Progress = 0; Progress < Locker.ListSectionNames().Length; Progress++)
                if (Locker.ListSectionNames()[Progress] == "New Weapon" || Locker.ListSectionNames()[Progress].Contains("New Weapon (Copy "))
                    Occurances = Occurances + 1;


            if (Occurances > 0)
            {
                //MessageBox.Show("A new weapon already exists.");
                string NewWeaponEntry = "\r\n\r\n[New Weapon (Copy " + Occurances + ")]\r\nType=Weapon\r\nRating=0\r\nDescription=\"Type in a description for the weapon here.\"\r\nPart1=None\r\nPart2=None\r\nPart3=None\r\nPart4=None\r\nPart5=None\r\nPart6=None\r\nPart7=None\r\nPart8=None\r\nPart9=None\r\nPart10=None\r\nPart11=None\r\nPart12=None\r\nPart13=None\r\nPart14=None";
                tempINI = tempINI + NewWeaponEntry;
                System.IO.File.WriteAllText(OpenedLocker, tempINI);
                Node temp = new Node();
                temp.Text = "New Weapon (Copy " + Occurances + ")";
                LockerTree.Nodes.Add(temp);
            }
            else
            {

                string NewWeaponEntry = "\r\n\r\n[New Weapon]\r\nType=Weapon\r\nRating=0\r\nDescription=\"Type in a description for the weapon here.\"\r\nPart1=None\r\nPart2=None\r\nPart3=None\r\nPart4=None\r\nPart5=None\r\nPart6=None\r\nPart7=None\r\nPart8=None\r\nPart9=None\r\nPart10=None\r\nPart11=None\r\nPart12=None\r\nPart13=None\r\nPart14=None";
                tempINI = tempINI + NewWeaponEntry;
                System.IO.File.WriteAllText(OpenedLocker, tempINI);
                Node temp = new Node();
                temp.Text = "New Weapon";
                LockerTree.Nodes.Add(temp);
            }
            LockerTree.SelectedNode = LockerTree.Nodes[LockerTree.Nodes.Count - 1];

            //Insert From Clipboard
            InOutPartsBox.Text = InOutPartsBox.Text.Replace(" ", "");

            NameLocker.Text = "";
            Ini.IniFile Titles = new Ini.IniFile(AppDir + "\\Data\\Titles.ini");
            for (int Progress = 0; Progress < 14; Progress++)
            {
                PartsLocker.Items[Progress] = InOutPartsBox.Lines[Progress];
                if (GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != "" && GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != null && NameLocker.Text == "")
                    NameLocker.Text = GetName(Titles, InOutPartsBox.Lines, Progress, "PartName");
                else if (GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != "" && GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != null && NameLocker.Text != "")
                    NameLocker.Text = NameLocker.Text + " " + GetName(Titles, InOutPartsBox.Lines, Progress, "PartName");
                DescriptionLocker.Text = "Type in a description for the item here.";
                RatingLocker.Rating = 0;

            }
            SaveChangesToLocker();
        }

        private void CurrentWeaponParts_DoubleClick(object sender, EventArgs e)
        {
            //Interaction.InputBox TempBox = new Interaction.InputBox("Enter a new part", "Manual Edit", (string)CurrentWeaponParts.SelectedItem, 0, 0);
            //string test = Interaction.InputBox("Enter a new part", "Manual Edit", (string)CurrentWeaponParts.SelectedItem, 10, 10);
            string tempManualPart = Interaction.InputBox("Enter a new part", "Manual Edit", (string)CurrentWeaponParts.SelectedItem, 10, 10);
            if(tempManualPart != "")
            CurrentWeaponParts.Items[CurrentWeaponParts.SelectedIndex] = tempManualPart;
        }

        private void ImportWeapons_Click(object sender, EventArgs e)
        {
            OpenFileDialog tempImport = new OpenFileDialog();
            tempImport.DefaultExt = "*.wtl";
            tempImport.Filter = "WillowTree Locker(*.wtl)|*.wtl";

            tempImport.FileName = CurrentWSG.CharacterName + "'s Weapons.wtl";
            if (tempImport.ShowDialog() == DialogResult.OK)
            {
                Ini.IniFile ImportWTL = new Ini.IniFile(tempImport.FileName);
                if (IsDLCWeaponMode)
                {
                    CurrentWSG.DLC.WeaponParts.Add(new List<string>());

                    for (int Progress = 0; Progress < ImportWTL.ListSectionNames().Length; Progress++)
                    {
                        for (int ProgressStrings = 0; ProgressStrings < 14; ProgressStrings++)
                            CurrentWSG.DLC.WeaponParts[CurrentWSG.DLC.WeaponParts.Count - 1].Add(ImportWTL.IniReadValue(ImportWTL.ListSectionNames()[Progress], "Part" + (ProgressStrings + 1)));

                        CurrentWSG.DLC.WeaponAmmo.Add(0);
                        CurrentWSG.DLC.WeaponQuality.Add(0);
                        CurrentWSG.DLC.WeaponLevel.Add(0);
                        CurrentWSG.DLC.WeaponEquippedSlot.Add(0);
                        CurrentWSG.DLC.TotalWeapons++;
                    }
                    DoDLCWeaponTree();
                }

                else
                {

                    for (int Progress = 0; Progress < ImportWTL.ListSectionNames().Length; Progress++)
                    {
                        CurrentWSG.WeaponStrings.Add(new List<string>());
                        CurrentWSG.WeaponValues.Add(new List<int>());
                        for (int ProgressStrings = 0; ProgressStrings < 14; ProgressStrings++)
                            CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons].Add(ImportWTL.IniReadValue(ImportWTL.ListSectionNames()[Progress], "Part" + (ProgressStrings + 1)));
                        for (int i = 0; i < 4; i++)
                            CurrentWSG.WeaponValues[CurrentWSG.NumberOfWeapons].Add(0);
                        CurrentWSG.NumberOfWeapons++;
                    }
                    DoWeaponTree();
                }
            }
        }

        private void ExportWeapons_Click(object sender, EventArgs e)
        {
            SaveFileDialog tempExport = new SaveFileDialog();
            tempExport.DefaultExt = "*.wtl";
            tempExport.Filter = "WillowTree Locker(*.wtl)|*.wtl";

            tempExport.FileName = CurrentWSG.CharacterName + "'s Weapons.wtl";
            string ExportText = "";
            if (tempExport.ShowDialog() == DialogResult.OK)
            {
                if(IsDLCWeaponMode)
                    for (int Progress = 0; Progress < CurrentWSG.DLC.WeaponParts.Count; Progress++)
                    {
                        ExportText = ExportText + "[" + WeaponTree.Nodes[Progress].Text + "]\r\nType=Weapon\r\nRating=0\r\nDescription=Type in a description for the weapon here.\r\n";
                        for (int PartProgress = 0; PartProgress < 14; PartProgress++)
                            ExportText = ExportText + "Part" + (PartProgress + 1) + "=" + CurrentWSG.DLC.WeaponParts[Progress][PartProgress] + "\r\n";
                    }
                else
                for (int Progress = 0; Progress < CurrentWSG.NumberOfWeapons; Progress++)
                {
                    ExportText = ExportText + "[" + WeaponTree.Nodes[Progress].Text + "]\r\nType=Weapon\r\nRating=0\r\nDescription=Type in a description for the weapon here.\r\n";
                    for (int PartProgress = 0; PartProgress < 14; PartProgress++)
                        ExportText = ExportText + "Part" + (PartProgress + 1) + "=" + CurrentWSG.WeaponStrings[Progress][PartProgress] + "\r\n";
                }
                File.WriteAllText(tempExport.FileName, ExportText);
            }
        }

        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< QUESTS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  \\

        private void QuestTree_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            Clicked = false;
            try
            {
                SelectedQuestGroup.Text = QuestTree.SelectedNode.Text;
                //if (QuestTree.SelectedNode.Parent.Text == "Playthrough 1 Quests" && QuestTree.SelectedNode.HasChildNodes == false)

                //else if (QuestTree.SelectedNode.Parent.Text == "Playthrough 2 Quests" && QuestTree.SelectedNode.HasChildNodes == false)
                if (QuestTree.SelectedNode.Name == "PT1")
                {

                    QuestString.Text = CurrentWSG.PT1Strings[QuestTree.SelectedNode.Index];
                    if (CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, 0] > 2)
                        QuestProgress.SelectedIndex = 3;
                    else QuestProgress.SelectedIndex = CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, 0];
                    NumberOfObjectives.Value = CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, 3];
                    Objectives.Items.Clear();
                    if (NumberOfObjectives.Value > 0)
                        for (int Progress = 0; Progress < NumberOfObjectives.Value; Progress++)
                            Objectives.Items.Add(new Ini.IniFile(AppDir + "\\Data\\Quests.ini").IniReadValue(QuestString.Text, "Objectives[" + Progress + "]"));
                    ObjectiveValue.Value = 0;
                    QuestSummary.Text = new Ini.IniFile(AppDir + "\\Data\\Quests.ini").IniReadValue(QuestString.Text, "MissionSummary");
                    QuestDescription.Text = new Ini.IniFile(AppDir + "\\Data\\Quests.ini").IniReadValue(QuestString.Text, "MissionDescription");
                }
                else if (QuestTree.SelectedNode.Name == "PT2")
                {
                    QuestString.Text = CurrentWSG.PT2Strings[QuestTree.SelectedNode.Index];
                    if (CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, 0] > 2)
                        QuestProgress.SelectedIndex = 3;
                    else QuestProgress.SelectedIndex = CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, 0];
                    NumberOfObjectives.Value = CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, 3];
                    Objectives.Items.Clear();
                    if (NumberOfObjectives.Value > 0)
                        for (int Progress = 0; Progress < NumberOfObjectives.Value; Progress++)
                            Objectives.Items.Add(new Ini.IniFile(AppDir + "\\Data\\Quests.ini").IniReadValue(QuestString.Text, "Objectives[" + Progress + "]"));
                    ObjectiveValue.Value = 0;
                    QuestSummary.Text = new Ini.IniFile(AppDir + "\\Data\\Quests.ini").IniReadValue(QuestString.Text, "MissionSummary");
                    QuestDescription.Text = new Ini.IniFile(AppDir + "\\Data\\Quests.ini").IniReadValue(QuestString.Text, "MissionDescription");
                }
            }
            catch { QuestString.Text = ""; Objectives.Items.Clear(); NumberOfObjectives.Value = 0; ObjectiveValue.Value = 0; QuestProgress.SelectedIndex = 0; }
        }

        private void QuestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedItem = QuestList.SelectedIndex;
            NewQuest.ClosePopup();
            try
            {
                Ini.IniFile Quests = new Ini.IniFile(AppDir + "\\Data\\Quests.ini");
                if (QuestTree.SelectedNode.Name == "PT1" || QuestTree.SelectedNode == QuestTree.Nodes[0])
                {
                    int TotalObjectives = 0;
                    CurrentWSG.TotalPT1Quests = CurrentWSG.TotalPT1Quests + 1;
                    ResizeArrayLarger(ref CurrentWSG.PT1Strings, CurrentWSG.TotalPT1Quests);
                    ResizeArrayLarger(ref CurrentWSG.PT1Values, CurrentWSG.TotalPT1Quests, 9);
                    ResizeArrayLarger(ref CurrentWSG.PT1Subfolders, CurrentWSG.TotalPT1Quests, 5);
                    CurrentWSG.PT1Strings[CurrentWSG.TotalPT1Quests - 1] = Quests.ListSectionNames()[SelectedItem];
                    CurrentWSG.PT1Values[CurrentWSG.TotalPT1Quests - 1, 0] = 1;
                    for (int Progress = 0; Progress < 5; Progress++)
                        if (Quests.IniReadValue(Quests.ListSectionNames()[SelectedItem], "Objectives[" + Progress + "]") == "") break;
                        else TotalObjectives = Progress + 1;
                    CurrentWSG.PT1Values[CurrentWSG.TotalPT1Quests - 1, 3] = TotalObjectives;
                    for (int Progress = 0; Progress < 5; Progress++)
                        CurrentWSG.PT1Subfolders[CurrentWSG.TotalPT1Quests - 1, Progress] = "None";

                    DoQuestTree();
                    QuestTree.SelectedNode = QuestTree.Nodes[0].Nodes[CurrentWSG.TotalPT1Quests - 1];
                }
                if (QuestTree.SelectedNode.Name == "PT2" || QuestTree.SelectedNode.Text == "Playthrough 2 Quests")
                {
                    int TotalObjectives = 0;
                    CurrentWSG.TotalPT2Quests = CurrentWSG.TotalPT2Quests + 1;
                    ResizeArrayLarger(ref CurrentWSG.PT2Strings, CurrentWSG.TotalPT2Quests);
                    ResizeArrayLarger(ref CurrentWSG.PT2Values, CurrentWSG.TotalPT2Quests, 9);
                    ResizeArrayLarger(ref CurrentWSG.PT2Subfolders, CurrentWSG.TotalPT2Quests, 5);
                    CurrentWSG.PT2Strings[CurrentWSG.TotalPT2Quests - 1] = Quests.ListSectionNames()[SelectedItem];
                    CurrentWSG.PT2Values[CurrentWSG.TotalPT2Quests - 1, 0] = 1;
                    for (int Progress = 0; Progress < 5; Progress++)
                        if (Quests.IniReadValue(Quests.ListSectionNames()[SelectedItem], "Objectives[" + Progress + "]") == "") break;
                        else TotalObjectives = Progress + 1;
                    CurrentWSG.PT2Values[CurrentWSG.TotalPT2Quests - 1, 3] = TotalObjectives;
                    for (int Progress = 0; Progress < 5; Progress++)
                        CurrentWSG.PT2Subfolders[CurrentWSG.TotalPT2Quests - 1, Progress] = "None";

                    DoQuestTree();
                    QuestTree.SelectedNode = QuestTree.Nodes[1].Nodes[CurrentWSG.TotalPT2Quests - 1];
                }
            }
            catch { }
        }

        private void Objectives_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (QuestTree.SelectedNode.Name == "PT1" && Clicked == true)
                ObjectiveValue.Value = CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, Objectives.SelectedIndex + 4];
            else if (QuestTree.SelectedNode.Name == "PT2" && Clicked == true)
                ObjectiveValue.Value = CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, Objectives.SelectedIndex + 4];
        }

        private void ObjectiveValue_ValueChanged(object sender, EventArgs e)
        {
            if (Objectives.Items.Count > 0)
                if (QuestTree.SelectedNode.Name == "PT1" && Clicked == true)
                    CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, Objectives.SelectedIndex + 4] = (int)ObjectiveValue.Value;
                else if (QuestTree.SelectedNode.Name == "PT2" && Clicked == true)
                    CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, Objectives.SelectedIndex + 4] = (int)ObjectiveValue.Value;
        }

        private void QuestProgress_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Ini.IniFile Quests = new Ini.IniFile(AppDir + "\\Data\\Quests.ini");
                if (QuestTree.SelectedNode.Name == "PT1" && Clicked == true)
                {
                    if (CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, 0] == 4 && QuestProgress.SelectedIndex < 3)
                    {
                        Objectives.Items.Clear();
                        int TotalObjectives = 0;

                        //string curTxt;
                        for (int Progress = 0; Progress < 5; Progress++)
                        {
                            //curTxt = Quests.IniReadValue(QuestString.Text, "Objectives[" + Progress + "]");
                            if (Quests.IniReadValue(QuestString.Text, "Objectives[" + Progress + "]") == "") break;
                            else TotalObjectives = Progress + 1;
                        }
                        CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, 3] = TotalObjectives;
                        for (int Progress = 0; Progress < 5; Progress++)
                            CurrentWSG.PT1Subfolders[QuestTree.SelectedNode.Index, Progress] = "None";
                        NumberOfObjectives.Value = TotalObjectives;
                        if (TotalObjectives > 0)
                            for (int Progress = 0; Progress < TotalObjectives; Progress++)
                                Objectives.Items.Add(Quests.IniReadValue(QuestString.Text, "Objectives[" + Progress + "]"));
                        ObjectiveValue.Value = 0;
                        CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, 0] = QuestProgress.SelectedIndex;
                        CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, 3] = TotalObjectives;
                        for (int Progress = 0; Progress < 5; Progress++)
                            CurrentWSG.PT1Subfolders[QuestTree.SelectedNode.Index, Progress] = "None";
                    }
                    else if (CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, 0] != 4 && QuestProgress.SelectedIndex == 3)
                    {
                        Objectives.Items.Clear();
                        CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, 0] = 4;
                        CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, 3] = 0;
                    }
                    else if (CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, 0] != 4 && QuestProgress.SelectedIndex < 3)
                        CurrentWSG.PT1Values[QuestTree.SelectedNode.Index, 0] = QuestProgress.SelectedIndex;
                }
                else if (QuestTree.SelectedNode.Name == "PT2" && Clicked == true)
                {
                    if (CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, 0] == 4 && QuestProgress.SelectedIndex < 3)
                    {
                        Objectives.Items.Clear();
                        int TotalObjectives = 0;
                        for (int Progress = 0; Progress < 5; Progress++)
                            if (Quests.IniReadValue(QuestString.Text, "Objectives[" + Progress + "]") == "") break;
                            else TotalObjectives = Progress + 1;
                        CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, 3] = TotalObjectives;
                        for (int Progress = 0; Progress < 5; Progress++)
                            CurrentWSG.PT2Subfolders[QuestTree.SelectedNode.Index, Progress] = "None";
                        NumberOfObjectives.Value = TotalObjectives;
                        if (NumberOfObjectives.Value > 0)
                            for (int Progress = 0; Progress < NumberOfObjectives.Value; Progress++)
                                Objectives.Items.Add(Quests.IniReadValue(QuestString.Text, "Objectives[" + Progress + "]"));
                        ObjectiveValue.Value = 0;
                        CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, 0] = QuestProgress.SelectedIndex;
                        CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, 3] = TotalObjectives;
                    }
                    else if (CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, 0] != 4 && QuestProgress.SelectedIndex == 3)
                    {
                        Objectives.Items.Clear();
                        CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, 0] = 4;
                        CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, 3] = 0;
                        //for (int Progress = 0; Progress < 5; Progress++)
                        //    CurrentWSG.PT2Subfolders[QuestTree.SelectedNode.Index, Progress] = "None";
                    }
                    else if (CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, 0] != 4 && QuestProgress.SelectedIndex < 3)
                        CurrentWSG.PT2Values[QuestTree.SelectedNode.Index, 0] = QuestProgress.SelectedIndex;

                }
            }
            catch { }
        }

        private void QuestProgress_Click(object sender, EventArgs e)
        {
            Clicked = true;
        }

        private void DeleteQuest_Click(object sender, EventArgs e)
        {
            try
            {
                int Selected = QuestTree.SelectedNode.Index; ;
                if (QuestTree.SelectedNode.Name == "PT1")
                {
                    if (QuestTree.SelectedNode.Text != "Fresh Off The Bus" || MultipleIntroStateSaver(1) == true)
                    {
                    Selected = QuestTree.SelectedNode.Index;
                    CurrentWSG.TotalPT1Quests = CurrentWSG.TotalPT1Quests - 1;
                    for (int Position = Selected; Position < CurrentWSG.TotalPT1Quests; Position++)
                    {
                        CurrentWSG.PT1Strings[Position] = CurrentWSG.PT1Strings[Position + 1];
                        for (int Progress = 0; Progress < 4; Progress++)
                            CurrentWSG.PT1Values[Position,Progress] = CurrentWSG.PT1Values[Position + 1, Progress];
                        for (int Progress = 0; Progress < 5; Progress++)
                            CurrentWSG.PT1Subfolders[Position,Progress] = CurrentWSG.PT1Subfolders[Position + 1, Progress];
                    }
                    ResizeArraySmaller(ref CurrentWSG.PT1Strings, CurrentWSG.TotalPT1Quests);
                    ResizeArraySmaller(ref CurrentWSG.PT1Subfolders, CurrentWSG.TotalPT1Quests, 5);
                    ResizeArraySmaller(ref CurrentWSG.PT1Values, CurrentWSG.TotalPT1Quests, 9);
                    DoQuestTree();
                    QuestTree.SelectedNode = QuestTree.Nodes[0].Nodes[Selected];
                }
                    else if (MultipleIntroStateSaver(1) == false)
                        MessageBox.Show("You must have the default quest.");
                }
                else if (QuestTree.SelectedNode.Name == "PT2")
                {
                    if (QuestTree.SelectedNode.Text != "Fresh Off The Bus" || MultipleIntroStateSaver(2) == true)
                    {
                        Selected = QuestTree.SelectedNode.Index;
                        CurrentWSG.TotalPT2Quests = CurrentWSG.TotalPT2Quests - 1;
                        for (int Position = Selected; Position < CurrentWSG.TotalPT2Quests; Position++)
                        {
                            CurrentWSG.PT2Strings[Position] = CurrentWSG.PT2Strings[Position + 1];
                            for (int Progress = 0; Progress < 4; Progress++)
                                CurrentWSG.PT2Values[Position,Progress] = CurrentWSG.PT2Values[Position + 1, Progress];
                            for (int Progress = 0; Progress < 5; Progress++)
                                CurrentWSG.PT2Subfolders[Position,Progress] = CurrentWSG.PT2Subfolders[Position + 1, Progress];
                        }
                        ResizeArraySmaller(ref CurrentWSG.PT2Strings, CurrentWSG.TotalPT2Quests);
                        ResizeArraySmaller(ref CurrentWSG.PT2Subfolders, CurrentWSG.TotalPT2Quests, 5);
                        ResizeArraySmaller(ref CurrentWSG.PT2Values, CurrentWSG.TotalPT2Quests, 9);

                        DoQuestTree();

                        try
                        {
                            QuestTree.SelectedNode = QuestTree.Nodes[1].Nodes[Selected];
                        }
                        catch { }
                    }
                    else if (MultipleIntroStateSaver(2) == false)
                        MessageBox.Show("You must have the default quest.");
                }
            }
            catch { }
        }


        private void ExportQuests_Click(object sender, EventArgs e)
        {
            try
            {
                if (QuestTree.SelectedNode.Name == "PT2")
                {
                    SaveFileDialog tempExport = new SaveFileDialog();
                    string ExportText = "";
                    tempExport.DefaultExt = "*.quests";
                    tempExport.Filter = "Quest Data(*.quests)|*.quests";

                    tempExport.FileName = CurrentWSG.CharacterName + "'s PT2 Quests.quests";

                    if (tempExport.ShowDialog() == DialogResult.OK)
                        for (int Progress = 0; Progress < CurrentWSG.TotalPT2Quests; Progress++)
                        {
                            ExportText = ExportText + "[" + CurrentWSG.PT2Strings[Progress] + "]\r\nProgress=" + CurrentWSG.PT2Values[Progress, 0] + "\r\nDLCValue1=" + CurrentWSG.PT2Values[Progress, 1] + "\r\nDLCValue2=" + CurrentWSG.PT2Values[Progress, 2] + "\r\nObjectives=" + CurrentWSG.PT2Values[Progress, 3] + "\r\n";
                            for (int Folders = 0; Folders < CurrentWSG.PT2Values[Progress, 3]; Folders++)
                                ExportText = ExportText + "FolderName" + Folders + "=" + CurrentWSG.PT2Subfolders[Progress, Folders] + "\r\nFolderValue" + Folders + "=" + CurrentWSG.PT2Values[Progress, Folders + 4] + "\r\n";

                        }
                    File.WriteAllText(tempExport.FileName, ExportText);
                }
                else if (QuestTree.SelectedNode.Name == "PT1")
                {
                    SaveFileDialog tempExport = new SaveFileDialog();
                    string ExportText = "";
                    tempExport.DefaultExt = "*.quests";
                    tempExport.Filter = "Quest Data(*.quests)|*.quests";

                    tempExport.FileName = CurrentWSG.CharacterName + "'s PT1 Quests.quests";

                    if (tempExport.ShowDialog() == DialogResult.OK)
                        for (int Progress = 0; Progress < CurrentWSG.TotalPT1Quests; Progress++)
                        {
                            ExportText = ExportText + "[" + CurrentWSG.PT1Strings[Progress] + "]\r\nProgress=" + CurrentWSG.PT1Values[Progress, 0] + "\r\nDLCValue1=" + CurrentWSG.PT1Values[Progress, 1] + "\r\nDLCValue2=" + CurrentWSG.PT1Values[Progress, 2] + "\r\nObjectives=" + CurrentWSG.PT1Values[Progress, 3] + "\r\n";
                            for (int Folders = 0; Folders < CurrentWSG.PT1Values[Progress, 3]; Folders++)
                                ExportText = ExportText + "FolderName" + Folders + "=" + CurrentWSG.PT1Subfolders[Progress, Folders] + "\r\nFolderValue" + Folders + "=" + CurrentWSG.PT1Values[Progress, Folders + 4] + "\r\n";

                        }
                    File.WriteAllText(tempExport.FileName, ExportText);
                }


            }
            catch { MessageBox.Show("Select a playthrough to extract first."); }

        }

        private void ImportQuests_Click(object sender, EventArgs e)
        {
            try
            {
                if (QuestTree.SelectedNode.Name == "PT2")
                {
                    OpenFileDialog tempImport = new OpenFileDialog();
                    tempImport.DefaultExt = "*.quests";
                    tempImport.Filter = "Quest Data(*.quests)|*.quests";

                    tempImport.FileName = CurrentWSG.CharacterName + "'s PT2 Quests.quests";
                    if (tempImport.ShowDialog() == DialogResult.OK)
                    {
                        Ini.IniFile ImportQuests = new Ini.IniFile(tempImport.FileName);
                        string[] TempQuestStrings = new string[ImportQuests.ListSectionNames().Length];
                        int[,] TempQuestValues = new int[ImportQuests.ListSectionNames().Length, 10];
                        string[,] TempQuestSubfolders = new string[ImportQuests.ListSectionNames().Length, 7];
                        for (int Progress = 0; Progress < ImportQuests.ListSectionNames().Length; Progress++)
                        {
                            TempQuestStrings[Progress] = ImportQuests.ListSectionNames()[Progress];
                            TempQuestValues[Progress, 0] = Convert.ToInt32(ImportQuests.IniReadValue(ImportQuests.ListSectionNames()[Progress], "Progress"));
                            TempQuestValues[Progress, 1] = Convert.ToInt32(ImportQuests.IniReadValue(ImportQuests.ListSectionNames()[Progress], "DLCValue1"));
                            TempQuestValues[Progress, 2] = Convert.ToInt32(ImportQuests.IniReadValue(ImportQuests.ListSectionNames()[Progress], "DLCValue2"));
                            TempQuestValues[Progress, 3] = Convert.ToInt32(ImportQuests.IniReadValue(ImportQuests.ListSectionNames()[Progress], "Objectives"));
                            for (int Folders = 0; Folders < TempQuestValues[Progress, 3]; Folders++)
                            {
                                TempQuestValues[Progress, Folders + 4] = Convert.ToInt32(ImportQuests.IniReadValue(ImportQuests.ListSectionNames()[Progress], "FolderValue" + Folders));
                                TempQuestSubfolders[Progress, Folders] = ImportQuests.IniReadValue(ImportQuests.ListSectionNames()[Progress], "FolderName" + Folders);
                            }
                        }
                        CurrentWSG.PT2Strings = TempQuestStrings;
                        CurrentWSG.PT2Values = TempQuestValues;
                        CurrentWSG.PT2Subfolders = TempQuestSubfolders;
                        CurrentWSG.TotalPT2Quests = ImportQuests.ListSectionNames().Length;
                        DoQuestTree();
                    }
                }
                else if (QuestTree.SelectedNode.Name == "PT1")
                {
                    OpenFileDialog tempImport = new OpenFileDialog();
                    tempImport.DefaultExt = "*.quests";
                    tempImport.Filter = "Quest Data(*.quests)|*.quests";

                    tempImport.FileName = CurrentWSG.CharacterName + "'s PT1 Quests.quests";
                    if (tempImport.ShowDialog() == DialogResult.OK)
                    {
                        Ini.IniFile ImportQuests = new Ini.IniFile(tempImport.FileName);
                        string[] TempQuestStrings = new string[ImportQuests.ListSectionNames().Length];
                        int[,] TempQuestValues = new int[ImportQuests.ListSectionNames().Length, 10];
                        string[,] TempQuestSubfolders = new string[ImportQuests.ListSectionNames().Length, 7];
                        for (int Progress = 0; Progress < ImportQuests.ListSectionNames().Length; Progress++)
                        {
                            TempQuestStrings[Progress] = ImportQuests.ListSectionNames()[Progress];
                            TempQuestValues[Progress, 0] = Convert.ToInt32(ImportQuests.IniReadValue(ImportQuests.ListSectionNames()[Progress], "Progress"));
                            TempQuestValues[Progress, 1] = Convert.ToInt32(ImportQuests.IniReadValue(ImportQuests.ListSectionNames()[Progress], "DLCValue1"));
                            TempQuestValues[Progress, 2] = Convert.ToInt32(ImportQuests.IniReadValue(ImportQuests.ListSectionNames()[Progress], "DLCValue2"));
                            TempQuestValues[Progress, 3] = Convert.ToInt32(ImportQuests.IniReadValue(ImportQuests.ListSectionNames()[Progress], "Objectives"));
                            for (int Folders = 0; Folders < TempQuestValues[Progress, 3]; Folders++)
                            {
                                TempQuestValues[Progress, Folders + 4] = Convert.ToInt32(ImportQuests.IniReadValue(ImportQuests.ListSectionNames()[Progress], "FolderValue" + Folders));
                                TempQuestSubfolders[Progress, Folders] = ImportQuests.IniReadValue(ImportQuests.ListSectionNames()[Progress], "FolderName" + Folders);
                            }
                        }
                        CurrentWSG.PT1Strings = TempQuestStrings;
                        CurrentWSG.PT1Values = TempQuestValues;
                        CurrentWSG.PT1Subfolders = TempQuestSubfolders;
                        CurrentWSG.TotalPT1Quests = ImportQuests.ListSectionNames().Length;
                        DoQuestTree();
                    }
                }
            }
            catch { MessageBox.Show("Select a playthrough to replace first."); }




        }


        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< ITEMS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  \\

        private void ItemTree_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {

            try
            {
     


                    CurrentItemParts.Items.Clear();
                    CurrentPartsGroup.Text = ItemTree.SelectedNode.Text;

                    if (IsDLCItemMode)
                    {
                        for (int build_list = 0; build_list < 9; build_list++)
                        {
                            CurrentItemParts.Items.Add(CurrentWSG.DLC.ItemParts[ItemTree.SelectedNode.Index][build_list]);
                        }

                        Quantity.Value = CurrentWSG.DLC.ItemQuantity[ItemTree.SelectedNode.Index];
                        ItemQuality.Value = CurrentWSG.DLC.ItemQuality[ItemTree.SelectedNode.Index];
                        ItemItemGradeSlider.Value = CurrentWSG.DLC.ItemLevel[ItemTree.SelectedNode.Index];
                        if (CurrentWSG.DLC.ItemEquipped[ItemTree.SelectedNode.Index] == 0) Equipped.SelectedItem = "No";
                        else if (CurrentWSG.DLC.ItemEquipped[ItemTree.SelectedNode.Index] == 1) Equipped.SelectedItem = "Yes";
                    }
                    else
                    {
                        for (int build_list = 0; build_list < 9; build_list++)
                        {
                            CurrentItemParts.Items.Add(CurrentWSG.ItemStrings[ItemTree.SelectedNode.Index][build_list]);
                        }

                        Quantity.Value = CurrentWSG.ItemValues[ItemTree.SelectedNode.Index][0];
                        ItemQuality.Value = CurrentWSG.ItemValues[ItemTree.SelectedNode.Index][1];
                        if (CurrentWSG.ItemValues[ItemTree.SelectedNode.Index][2] == 0) Equipped.SelectedItem = "No";
                        else if (CurrentWSG.ItemValues[ItemTree.SelectedNode.Index][2] == 1) Equipped.SelectedItem = "Yes";
                        ItemItemGradeSlider.Value = CurrentWSG.ItemValues[ItemTree.SelectedNode.Index][3];
                    }

                
            }
            catch { }

        }

        private void NewItems_Click(object sender, EventArgs e)
        {
            try
            {

                if (IsDLCItemMode)
                {
                    CurrentWSG.DLC.TotalItems++;
                    CurrentWSG.DLC.ItemParts.Add(new List<string>());
                    CurrentWSG.DLC.ItemParts[CurrentWSG.DLC.ItemParts.Count - 1].Add("Item Grade");
                    CurrentWSG.DLC.ItemParts[CurrentWSG.DLC.ItemParts.Count - 1].Add("Item Type");
                    CurrentWSG.DLC.ItemParts[CurrentWSG.DLC.ItemParts.Count - 1].Add("Body");
                    CurrentWSG.DLC.ItemParts[CurrentWSG.DLC.ItemParts.Count - 1].Add("Left Side");
                    CurrentWSG.DLC.ItemParts[CurrentWSG.DLC.ItemParts.Count - 1].Add("Right Side");
                    CurrentWSG.DLC.ItemParts[CurrentWSG.DLC.ItemParts.Count - 1].Add("Material");
                    CurrentWSG.DLC.ItemParts[CurrentWSG.DLC.ItemParts.Count - 1].Add("Manufacturer");
                    CurrentWSG.DLC.ItemParts[CurrentWSG.DLC.ItemParts.Count - 1].Add("Prefix");
                    CurrentWSG.DLC.ItemParts[CurrentWSG.DLC.ItemParts.Count - 1].Add("Title");
                    CurrentWSG.DLC.ItemQuantity.Add(1);
                    CurrentWSG.DLC.ItemQuality.Add(0);
                    CurrentWSG.DLC.ItemEquipped.Add(0);


                    DoDLCItemTree();
                }
                else
                {
                    CurrentWSG.NumberOfItems++;
                    CurrentWSG.ItemStrings.Add(new List<string>());
                    CurrentWSG.ItemValues.Add(new List<int>());
                    CurrentWSG.ItemStrings[CurrentWSG.NumberOfItems - 1].Add("Item Grade");
                    CurrentWSG.ItemStrings[CurrentWSG.NumberOfItems - 1].Add("Item Type");
                    CurrentWSG.ItemStrings[CurrentWSG.NumberOfItems - 1].Add("Body");
                    CurrentWSG.ItemStrings[CurrentWSG.NumberOfItems - 1].Add("Left Side");
                    CurrentWSG.ItemStrings[CurrentWSG.NumberOfItems - 1].Add("Right Side");
                    CurrentWSG.ItemStrings[CurrentWSG.NumberOfItems - 1].Add("Material");
                    CurrentWSG.ItemStrings[CurrentWSG.NumberOfItems - 1].Add("Manufacturer");
                    CurrentWSG.ItemStrings[CurrentWSG.NumberOfItems - 1].Add("Prefix");
                    CurrentWSG.ItemStrings[CurrentWSG.NumberOfItems - 1].Add("Title");
                    CurrentWSG.ItemValues[(CurrentWSG.NumberOfItems - 1)].Add(1);
                    CurrentWSG.ItemValues[(CurrentWSG.NumberOfItems - 1)].Add(0);
                    CurrentWSG.ItemValues[(CurrentWSG.NumberOfItems - 1)].Add(0);
                    CurrentWSG.ItemValues[(CurrentWSG.NumberOfItems - 1)].Add(0);

                    Node TempNode = new Node();
                    TempNode.Text = "New Item";
                    ItemTree.Nodes.Add(TempNode);
                    //DoItemTree();
                }
            }
            catch { }
        }

        private void SaveChangesItem_Click(object sender, EventArgs e)
        {
            try
            {
                int Selected = ItemTree.SelectedNode.Index;


                if (IsDLCItemMode)
                {

                    for (int Progress = 0; Progress < 9; Progress++)
                        CurrentWSG.DLC.ItemParts[ItemTree.SelectedNode.Index][Progress] = (string)CurrentItemParts.Items[Progress];
                    CurrentWSG.DLC.ItemQuantity[ItemTree.SelectedNode.Index] = (int)Quantity.Value;
                    CurrentWSG.DLC.ItemQuality[ItemTree.SelectedNode.Index] = (int)ItemQuality.Value;
                    CurrentWSG.DLC.ItemEquipped[ItemTree.SelectedNode.Index] = Equipped.SelectedIndex;
                    CurrentWSG.DLC.ItemLevel[ItemTree.SelectedNode.Index] = ItemItemGradeSlider.Value;
                    //DoDLCItemTree();
                }

                else
                {
                    for (int Progress = 0; Progress < 9; Progress++)
                        CurrentWSG.ItemStrings[ItemTree.SelectedNode.Index][Progress] = (string)CurrentItemParts.Items[Progress];
                    CurrentWSG.ItemValues[ItemTree.SelectedNode.Index][0] = (int)Quantity.Value;
                    CurrentWSG.ItemValues[ItemTree.SelectedNode.Index][1] = (int)ItemQuality.Value;
                    CurrentWSG.ItemValues[ItemTree.SelectedNode.Index][2] = Equipped.SelectedIndex;
                    CurrentWSG.ItemValues[ItemTree.SelectedNode.Index][3] = (int)ItemItemGradeSlider.Value;
                    DoItemTree();
                }
                //DoItemTree();
                //ItemTree.SelectedNode = ItemTree.Nodes[Selected];
                ItemTree.SelectedNode.Text = GetName(new Ini.IniFile(AppDir + "\\Data\\Titles.ini"), CurrentWSG.ItemStrings, ItemTree.SelectedIndex, 14, "PartName");
            }
            catch { }
        } // Save Changes

        private void ExportItemToClipboard_Click(object sender, EventArgs e)
        {
            InOutPartsBox.Clear();
            for (int Progress = 0; Progress < 9; Progress++)
            {
                if (Progress > 0) InOutPartsBox.AppendText("\r\n");
                InOutPartsBox.AppendText((string)CurrentItemParts.Items[Progress]);
            }
            InOutPartsBox.AppendText("\r\n" + Quantity.Value);
            InOutPartsBox.AppendText("\r\n" + ItemQuality.Value);
            InOutPartsBox.AppendText("\r\n" + Equipped.SelectedIndex);
            InOutPartsBox.AppendText("\r\n" + ItemItemGradeSlider.Value);
            Clipboard.SetText(InOutPartsBox.Text);
        } // Export -> to Clipboard

        private void ImportItemsFromClipboard_Click(object sender, EventArgs e)
        {
            InOutPartsBox.Clear();
            InOutPartsBox.Text = Clipboard.GetText();
            InOutPartsBox.Text = InOutPartsBox.Text.Replace(" ", "");

            for (int Progress = 0; Progress < 9; Progress++)
                CurrentItemParts.Items[Progress] = InOutPartsBox.Lines[Progress];
            try
            {
                Quantity.Value = Convert.ToInt32(InOutPartsBox.Lines[9]);
            }
            catch
            {
                Quantity.Value = 0;
            }
            try
            {
                ItemQuality.Value = Convert.ToInt32(InOutPartsBox.Lines[10]);
            }
            catch
            {
                ItemQuality.Value = 0;
            }
            try
            {
                EquippedSlot.SelectedIndex = Convert.ToInt32(InOutPartsBox.Lines[11]);
            }
            catch
            {
                Equipped.SelectedIndex = 0;

            }
        }// Import -> from Clipboard

        private void DuplicateItems_Click(object sender, EventArgs e)
        {
            int Selected = ItemTree.SelectedNode.Index;

            if (IsDLCItemMode)
            {
                CurrentWSG.DLC.ItemParts.Add(new List<string>());
                foreach (string i in CurrentWSG.DLC.ItemParts[Selected])
                    CurrentWSG.DLC.ItemParts[CurrentWSG.DLC.ItemParts.Count - 1].Add(i);
                CurrentWSG.DLC.ItemQuantity.Add(CurrentWSG.DLC.ItemQuantity[Selected]);
                CurrentWSG.DLC.ItemQuality.Add(CurrentWSG.DLC.ItemQuality[Selected]);
                CurrentWSG.DLC.ItemLevel.Add(CurrentWSG.DLC.ItemLevel[Selected]);
                CurrentWSG.DLC.ItemEquipped.Add(0);
                CurrentWSG.DLC.TotalItems++;
                ItemTree.Nodes.Add(ItemTree.SelectedNode.Copy());
                //DoDLCItemTree();
            }
            else
            {
                CurrentWSG.ItemStrings.Add(new List<string>());
                CurrentWSG.ItemValues.Add(new List<int>());
                foreach (string i in CurrentWSG.ItemStrings[Selected])
                    CurrentWSG.ItemStrings[CurrentWSG.ItemStrings.Count - 1].Add(i);
                foreach (int i in CurrentWSG.ItemValues[Selected])
                    CurrentWSG.ItemValues[CurrentWSG.ItemStrings.Count - 1].Add(i);
                //CurrentWSG.ItemValues.Add(CurrentWSG.ItemValues[Selected]);
                CurrentWSG.ItemValues[CurrentWSG.NumberOfItems][2] = 0;
                CurrentWSG.NumberOfItems++;
                //DoItemTree();
                ItemTree.Nodes.Add(ItemTree.SelectedNode.Copy());
            }
            
        } // Duplicate Item

        private void DeleteItems_Click(object sender, EventArgs e)
        {
            try
            {

                int Selected = ItemTree.SelectedIndex;

                if (IsDLCItemMode)
                {
                    CurrentWSG.DLC.ItemParts.RemoveAt(Selected);
                    CurrentWSG.DLC.ItemQuantity.RemoveAt(Selected);
                    CurrentWSG.DLC.ItemQuality.RemoveAt(Selected);
                    CurrentWSG.DLC.ItemLevel.RemoveAt(Selected);
                    CurrentWSG.DLC.ItemEquipped.RemoveAt(Selected);
                    CurrentWSG.DLC.TotalItems++;
                    DoItemTree();
                }
                else
                {
                    ItemTree.DeselectNode(ItemTree.SelectedNode, new eTreeAction());
                    ItemTree.Nodes.RemoveAt(Selected);
                    CurrentWSG.ItemStrings.RemoveAt(Selected);
                    CurrentWSG.ItemValues.RemoveAt(Selected);
                    CurrentWSG.NumberOfItems--;
                    TrySelectedNode(ItemTree, Selected);
                    
                    //DoItemTree();
                }
                
            }
            catch { MessageBox.Show("Select an item to delete first."); }
        } // Delete Item

        private void ExportItemsToFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog ToFile = new SaveFileDialog();
            ToFile.DefaultExt = "*.txt";
            ToFile.Filter = "Item Files(*.txt)|*.txt";
            ToFile.FileName = CurrentPartsGroup.Text + ".txt";
            if (ToFile.ShowDialog() == DialogResult.OK)
            {

                InOutPartsBox.Clear();

   

               
                    for (int Progress = 0; Progress < 9; Progress++)
                    {
                        if (Progress > 0) InOutPartsBox.AppendText("\r\n");
                        InOutPartsBox.AppendText((string)CurrentItemParts.Items[Progress]);
                    }
                    InOutPartsBox.AppendText("\r\n" + Quantity.Value);
                    InOutPartsBox.AppendText("\r\n" + ItemQuality.Value);
                    InOutPartsBox.AppendText("\r\n" + Equipped.SelectedIndex);
                    InOutPartsBox.AppendText("\r\n" + ItemItemGradeSlider.Value);


                System.IO.File.WriteAllLines(ToFile.FileName, InOutPartsBox.Lines);
            }
        } // Export -> to File

        private void ImportItemFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog FromFile = new OpenFileDialog();
            FromFile.DefaultExt = "*.txt";
            FromFile.Filter = "Item Files(*.txt)|*.txt";

            FromFile.FileName = CurrentPartsGroup.Text + ".txt";

            if (FromFile.ShowDialog() == DialogResult.OK)
            {

                InOutPartsBox.Clear();
                InOutPartsBox.Text = System.IO.File.ReadAllText(FromFile.FileName);

                for (int Progress = 0; Progress < 9; Progress++)
                    CurrentItemParts.Items[Progress] = InOutPartsBox.Lines[Progress];
                if (InOutPartsBox.Lines[9] != null)
                    Quantity.Value = Convert.ToInt32(InOutPartsBox.Lines[9]);
                else
                    Quantity.Value = 0;
                if (InOutPartsBox.Lines[10] != null)
                    ItemQuality.Value = Convert.ToInt32(InOutPartsBox.Lines[15]);
                else ItemQuality.Value = 0;
                if (InOutPartsBox.Lines[11] != null)
                    Equipped.SelectedIndex = Convert.ToInt32(InOutPartsBox.Lines[16]);
                else
                    Equipped.SelectedIndex = 0;
                if (InOutPartsBox.Lines[12] != null)
                    ItemItemGradeSlider.Value = Convert.ToInt32(InOutPartsBox.Lines[16]);
                else
                    ItemItemGradeSlider.Value = 0;
            }
        } // Import -> from File

        private void DeletePartItem_Click(object sender, EventArgs e)
        {
            CurrentItemParts.Items[CurrentItemParts.SelectedIndex] = "None";
        } // Delete Part

        private void ItemsFromClipboard_Click(object sender, EventArgs e) // Insert -> from Clipboard
        {
            try
            {
                InOutPartsBox.Clear();
                InOutPartsBox.Text = Clipboard.GetText();
                InOutPartsBox.Text = InOutPartsBox.Text.Replace(" ", "");

                if (IsDLCItemMode)
                {
                    CurrentWSG.DLC.ItemParts.Add(new List<string>());
                    CurrentWSG.DLC.TotalItems++;
                    for (int Progress = 0; Progress < 9; Progress++)
                    {
                        CurrentWSG.DLC.ItemParts[CurrentWSG.DLC.ItemParts.Count - 1].Add(InOutPartsBox.Lines[Progress]);

                    }
                    CurrentWSG.DLC.ItemQuantity.Add(Convert.ToInt32(InOutPartsBox.Lines[9]));
                    CurrentWSG.DLC.ItemQuality.Add(Convert.ToInt32(InOutPartsBox.Lines[10]));
                    CurrentWSG.DLC.ItemEquipped.Add(Convert.ToInt32(InOutPartsBox.Lines[11]));
                    CurrentWSG.DLC.ItemLevel.Add(0);
                    DoDLCItemTree();
                }
                else
                {
                    CurrentWSG.ItemStrings.Add(new List<string>());
                    CurrentWSG.ItemValues.Add(new List<int>());
                    CurrentWSG.NumberOfItems++;
                    for (int Progress = 0; Progress < 9; Progress++)
                        CurrentWSG.ItemStrings[CurrentWSG.NumberOfItems - 1].Add(InOutPartsBox.Lines[Progress]);
                    for (int Progress = 0; Progress < 3; Progress++)
                        CurrentWSG.ItemValues[CurrentWSG.NumberOfItems - 1].Add(Convert.ToInt32(InOutPartsBox.Lines[Progress + 9]));
                    if (InOutPartsBox.Lines.Length > 13)
                        CurrentWSG.ItemValues[CurrentWSG.NumberOfItems - 1].Add(Convert.ToInt32(InOutPartsBox.Lines[13]));
                    else CurrentWSG.ItemValues[CurrentWSG.NumberOfItems - 1].Add(0);
                    Node TempNode = new Node();
                    TempNode.Text = GetName(new Ini.IniFile(AppDir + "\\Data\\Titles.ini"), CurrentWSG.ItemStrings, CurrentWSG.NumberOfItems - 1, 9, "PartName");
                    ItemTree.Nodes.Add(TempNode);
                }
            }
            catch { }
        }

        private void ItemFromFile_Click(object sender, EventArgs e) // Insert -> from File
        {
            OpenFileDialog FromFile = new OpenFileDialog();
            FromFile.DefaultExt = "*.txt";
            FromFile.Filter = "Item Files(*.txt)|*.txt";

            FromFile.FileName = CurrentPartsGroup.Text + ".txt";

            if (FromFile.ShowDialog() == DialogResult.OK)
            {

                InOutPartsBox.Clear();
                InOutPartsBox.Text = System.IO.File.ReadAllText(FromFile.FileName);

                if (IsDLCItemMode)
                {
                    CurrentWSG.DLC.ItemParts.Add(new List<string>());
                    CurrentWSG.NumberOfItems++;
                    for (int Progress = 0; Progress < 9; Progress++)
                    
                        CurrentWSG.DLC.ItemParts[CurrentWSG.NumberOfItems - 1].Add(InOutPartsBox.Lines[Progress]);
                        CurrentWSG.DLC.ItemQuantity.Add(Convert.ToInt32(InOutPartsBox.Lines[9]));
                        CurrentWSG.DLC.ItemQuality.Add(Convert.ToInt32(InOutPartsBox.Lines[10]));
                        CurrentWSG.DLC.ItemEquipped.Add(Convert.ToInt32(InOutPartsBox.Lines[11]));
                        CurrentWSG.DLC.ItemLevel.Add(0);
                        Node TempNode = new Node();
                        TempNode.Text = GetName(new Ini.IniFile(AppDir + "\\Data\\Titles.ini"), CurrentWSG.ItemStrings, CurrentWSG.NumberOfItems - 1, 9, "PartName");
                        ItemTree.Nodes.Add(TempNode);
                    //DoDLCItemTree();
                }
                else
                {
                    CurrentWSG.ItemStrings.Add(new List<string>());
                    CurrentWSG.ItemValues.Add(new List<int>());
                    CurrentWSG.NumberOfItems++;
                    for (int Progress = 0; Progress < 9; Progress++)
                        CurrentWSG.ItemStrings[CurrentWSG.ItemStrings.Count - 1].Add(InOutPartsBox.Lines[Progress]);
                    for (int Progress = 0; Progress < 3; Progress++)
                        CurrentWSG.ItemValues[CurrentWSG.ItemStrings.Count - 1].Add(Convert.ToInt32(InOutPartsBox.Lines[Progress + 9]));
                    if (InOutPartsBox.Lines.Length > 12)
                        CurrentWSG.ItemValues[CurrentWSG.ItemStrings.Count - 1].Add(Convert.ToInt32(InOutPartsBox.Lines[13]));
                    else CurrentWSG.ItemValues[CurrentWSG.ItemStrings.Count - 1].Add(0);
                    Node TempNode = new Node();
                    TempNode.Text = GetName(new Ini.IniFile(AppDir + "\\Data\\Titles.ini"), CurrentWSG.ItemStrings, CurrentWSG.NumberOfItems - 1, 9, "PartName");
                    ItemTree.Nodes.Add(TempNode);
                }
            }

        }

        private void ExportToLockerItem_Click(object sender, EventArgs e)
        {

            InOutPartsBox.Clear();
            for (int Progress = 0; Progress < 9; Progress++)
            {
                if (Progress > 0) InOutPartsBox.AppendText("\r\n");
                InOutPartsBox.AppendText((string)CurrentItemParts.Items[Progress]);
            }
            //New Item

            string tempINI = System.IO.File.ReadAllText(OpenedLocker);
            int Occurances = 0;
            Ini.IniFile Locker = new Ini.IniFile(OpenedLocker);
            for (int Progress = 0; Progress < Locker.ListSectionNames().Length; Progress++)
                if (Locker.ListSectionNames()[Progress] == "New Item" || Locker.ListSectionNames()[Progress].Contains("New Item (Copy "))
                    Occurances++;


            if (Occurances > 0)
            {
                //MessageBox.Show("A new Item already exists.");
                string NewItemEntry = "\r\n\r\n[New Item (Copy " + Occurances + ")]\r\nType=Item\r\nRating=0\r\nDescription=\"Type in a description for the Item here.\"\r\nPart1=None\r\nPart2=None\r\nPart3=None\r\nPart4=None\r\nPart5=None\r\nPart6=None\r\nPart7=None\r\nPart8=None\r\nPart9=None\r\nPart10=\r\nPart11=\r\nPart12=\r\nPart13=\r\nPart14=";
                tempINI = tempINI + NewItemEntry;
                System.IO.File.WriteAllText(OpenedLocker, tempINI);
                Node temp = new Node();
                temp.Text = "New Item (Copy " + Occurances + ")";
                LockerTree.Nodes.Add(temp);
            }
            else
            {

                string NewItemEntry = "\r\n\r\n[New Item]\r\nType=Item\r\nRating=0\r\nDescription=\"Type in a description for the Item here.\"\r\nPart1=None\r\nPart2=None\r\nPart3=None\r\nPart4=None\r\nPart5=None\r\nPart6=None\r\nPart7=None\r\nPart8=None\r\nPart9=None\r\nPart10=\r\nPart11=\r\nPart12=\r\nPart13=\r\nPart14=";
                tempINI = tempINI + NewItemEntry;
                System.IO.File.WriteAllText(OpenedLocker, tempINI);
                Node temp = new Node();
                temp.Text = "New Item";
                LockerTree.Nodes.Add(temp);
            }
            LockerTree.SelectedNode = LockerTree.Nodes[LockerTree.Nodes.Count - 1];

            //Insert From Clipboard
            InOutPartsBox.Text = InOutPartsBox.Text.Replace(" ", "");

            NameLocker.Text = "";
            Ini.IniFile Titles = new Ini.IniFile(AppDir + "\\Data\\Titles.ini");
            for (int Progress = 0; Progress < 9; Progress++)
            {
                PartsLocker.Items[Progress] = InOutPartsBox.Lines[Progress];
                if (GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != "" && GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != null && NameLocker.Text == "")
                    NameLocker.Text = GetName(Titles, InOutPartsBox.Lines, Progress, "PartName");
                else if (GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != "" && GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != null && NameLocker.Text != "")
                    NameLocker.Text = NameLocker.Text + " " + GetName(Titles, InOutPartsBox.Lines, Progress, "PartName");
                DescriptionLocker.Text = "Type in a description for the item here.";
                RatingLocker.Rating = 0;

            }
            SaveChangesToLocker();
        }

        private void ItemPartCategories_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            if (CurrentItemParts.SelectedItem != null && ItemPartsSelector.SelectedNode.HasChildNodes == false)
            {
                CurrentItemParts.Items[CurrentItemParts.SelectedIndex] = ItemPartsSelector.SelectedNode.Parent.Text + "." + ItemPartsSelector.SelectedNode.Text;
            }
        }

 

        private void ExportItems_Click(object sender, EventArgs e)
        {
            SaveFileDialog tempExport = new SaveFileDialog();
            tempExport.DefaultExt = "*.wtl";
            tempExport.Filter = "WillowTree Locker(*.wtl)|*.wtl";

            tempExport.FileName = CurrentWSG.CharacterName + "'s Items.wtl";
            string ExportText = "";
            if (tempExport.ShowDialog() == DialogResult.OK)
            {
                if (IsDLCItemMode)
                {
                    for (int Progress = 0; Progress < CurrentWSG.DLC.ItemParts.Count; Progress++)
                    {
                        ExportText = ExportText + "[" + ItemTree.Nodes[Progress].Text + "]\r\nType=Item\r\nRating=0\r\nDescription=Type in a description for the Item here.\r\n";
                        for (int PartProgress = 0; PartProgress < 9; PartProgress++)
                            ExportText = ExportText + "Part" + (PartProgress + 1) + "=" + CurrentWSG.DLC.ItemParts[Progress][PartProgress] + "\r\n";
                        ExportText = ExportText + "Part10=\r\nPart11=\r\nPart12=\r\nPart13=\r\nPart14=\r\n";
                    }
                }
                else
                {
                    for (int Progress = 0; Progress < CurrentWSG.NumberOfItems; Progress++)
                    {
                        ExportText = ExportText + "[" + ItemTree.Nodes[Progress].Text + "]\r\nType=Item\r\nRating=0\r\nDescription=Type in a description for the Item here.\r\n";
                        for (int PartProgress = 0; PartProgress < 9; PartProgress++)
                            ExportText = ExportText + "Part" + (PartProgress + 1) + "=" + CurrentWSG.ItemStrings[Progress][PartProgress] + "\r\n";
                        ExportText = ExportText + "Part10=\r\nPart11=\r\nPart12=\r\nPart13=\r\nPart14=\r\n";
                    }
                }
                File.WriteAllText(tempExport.FileName, ExportText);
            }
        }

        private void CurrentItemParts_DoubleClick(object sender, EventArgs e)
        {
            string tempManualPart = Interaction.InputBox("Enter a new part", "Manual Edit", (string)CurrentItemParts.SelectedItem, 10, 10);
            if (tempManualPart != "")
                CurrentItemParts.Items[CurrentItemParts.SelectedIndex] = tempManualPart;

        }

        private void ImportItems_Click(object sender, EventArgs e)
        {
            OpenFileDialog tempImport = new OpenFileDialog();
            tempImport.DefaultExt = "*.wtl";
            tempImport.Filter = "WillowTree Locker(*.wtl)|*.wtl";

            tempImport.FileName = CurrentWSG.CharacterName + "'s Items.wtl";
            if (tempImport.ShowDialog() == DialogResult.OK)
            {
                Ini.IniFile ImportWTL = new Ini.IniFile(tempImport.FileName);

                if (IsDLCItemMode)
                {
                    CurrentWSG.DLC.ItemParts.Add(new List<string>());
                    
                    for (int Progress = 0; Progress < ImportWTL.ListSectionNames().Length; Progress++)
                    {
                        for (int ProgressStrings = 0; ProgressStrings < 9; ProgressStrings++)
                            CurrentWSG.DLC.ItemParts[CurrentWSG.DLC.ItemParts.Count - 1].Add(ImportWTL.IniReadValue(ImportWTL.ListSectionNames()[Progress], "Part" + (ProgressStrings + 1)));
                        
                            CurrentWSG.DLC.ItemQuantity.Add(1);
                            CurrentWSG.DLC.ItemQuality.Add(0);
                            CurrentWSG.DLC.ItemLevel.Add(0);
                            CurrentWSG.DLC.ItemEquipped.Add(0);
                        CurrentWSG.DLC.TotalItems++;
                    }
                    DoDLCItemTree();
                }

                else
                {
                    CurrentWSG.ItemStrings.Add(new List<string>());
                    CurrentWSG.ItemValues.Add(new List<int>());
                    for (int Progress = 0; Progress < ImportWTL.ListSectionNames().Length; Progress++)
                    {
                        for (int ProgressStrings = 0; ProgressStrings < 9; ProgressStrings++)
                            CurrentWSG.ItemStrings[CurrentWSG.NumberOfItems].Add(ImportWTL.IniReadValue(ImportWTL.ListSectionNames()[Progress], "Part" + (ProgressStrings + 1)));
                        for (int i = 0; i < 4; i++)
                            CurrentWSG.ItemValues[CurrentWSG.NumberOfItems].Add(0);
                        CurrentWSG.NumberOfItems++;
                    }
                    DoItemTree();
                }
            }
        }

        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< ECHOS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  \\

        private void DeleteEcho_Click(object sender, EventArgs e)
        {
            try
            {
                int Selected = EchoTree.SelectedNode.Index; ;
                if (EchoTree.SelectedNode.Name == "PT1")
                {
                    Selected = EchoTree.SelectedNode.Index;
                    CurrentWSG.NumberOfEchos = CurrentWSG.NumberOfEchos - 1;
                    for (int Position = Selected; Position < CurrentWSG.NumberOfEchos; Position++)
                    {
                        CurrentWSG.EchoStrings[Position] = CurrentWSG.EchoStrings[Position + 1];
                        for (int Progress = 0; Progress < 2; Progress++)
                            CurrentWSG.EchoValues[Position,Progress] = CurrentWSG.EchoValues[Position + 1, Progress];

                    }
                    ResizeArraySmaller(ref CurrentWSG.EchoStrings, CurrentWSG.NumberOfEchos);
                    ResizeArraySmaller(ref CurrentWSG.EchoValues, CurrentWSG.NumberOfEchos, 2);
                    DoEchoTree();
                    EchoTree.SelectedNode = EchoTree.Nodes[0].Nodes[Selected];
                }
                else if (EchoTree.SelectedNode.Name == "PT2")
                {
                    Selected = EchoTree.SelectedNode.Index;
                    CurrentWSG.NumberOfEchosPT2 = CurrentWSG.NumberOfEchosPT2 - 1;
                    for (int Position = Selected; Position < CurrentWSG.NumberOfEchosPT2; Position++)
                    {
                        CurrentWSG.EchoStringsPT2[Position] = CurrentWSG.EchoStringsPT2[Position + 1];
                        for (int Progress = 0; Progress < 2; Progress++)
                            CurrentWSG.EchoValuesPT2[Position,Progress] = CurrentWSG.EchoValuesPT2[Position + 1, Progress];

                    }
                    ResizeArraySmaller(ref CurrentWSG.EchoStringsPT2, CurrentWSG.NumberOfEchosPT2);
                    ResizeArraySmaller(ref CurrentWSG.EchoValuesPT2, CurrentWSG.NumberOfEchosPT2, 2);

                    DoEchoTree();

                    try
                    {
                        EchoTree.SelectedNode = EchoTree.Nodes[1].Nodes[Selected];
                    }
                    catch { }

                }
            }
            catch { }
        }
        private void EchoList_Click(object sender, EventArgs e)
        {
            
                Ini.IniFile Echoes = new Ini.IniFile(AppDir + "\\Data\\Echos.ini");
                if (EchoTree.SelectedNode.Name == "PT1" || EchoTree.SelectedNode.Text == "Playthrough 1 Echo Logs")
                {
                
                    if (CheckIfNull(CurrentWSG.EchoStrings) == true)
                    {
                        CurrentWSG.EchoStrings = new string[1];
                        CurrentWSG.EchoStrings[0] = Echoes.ListSectionNames()[EchoList.SelectedIndex];
                        CurrentWSG.EchoValues = new int[1, 2];
                        CurrentWSG.NumberOfEchos = 1;
                    }
                    else
                    {
                    CurrentWSG.NumberOfEchos = CurrentWSG.NumberOfEchos + 1;
                    ResizeArrayLarger(ref CurrentWSG.EchoStrings, CurrentWSG.NumberOfEchos);
                    ResizeArrayLarger(ref CurrentWSG.EchoValues, CurrentWSG.NumberOfEchos, 2);
                    CurrentWSG.EchoStrings[CurrentWSG.NumberOfEchos - 1] = Echoes.ListSectionNames()[EchoList.SelectedIndex];
                    
                    }
                    DoEchoTree();
                    EchoTree.Nodes[0].Expand();
                    EchoTree.SelectedNode = EchoTree.Nodes[0];
                }
                else if (EchoTree.SelectedNode.Name == "PT2" || EchoTree.SelectedNode.Text == "Playthrough 2 Echo Logs")
                {

                    if (CheckIfNull(CurrentWSG.EchoStringsPT2) == true)
                    {
                        if (CurrentWSG.TotalPT2Quests > 1)
                        {
                            CurrentWSG.EchoStringsPT2 = new string[1];
                            CurrentWSG.EchoStringsPT2[0] = Echoes.ListSectionNames()[EchoList.SelectedIndex];
                            CurrentWSG.EchoValuesPT2 = new int[1, 2];
                            CurrentWSG.NumberOfEchosPT2 = 1;
                        }
                        else
                            MessageBox.Show("You must have more than just the default quest.");
                    }
                    else
                    {
                        CurrentWSG.NumberOfEchosPT2 = CurrentWSG.NumberOfEchosPT2 + 1;
                        ResizeArrayLarger(ref CurrentWSG.EchoStringsPT2, CurrentWSG.NumberOfEchosPT2);
                        ResizeArrayLarger(ref CurrentWSG.EchoValuesPT2, CurrentWSG.NumberOfEchosPT2, 2);
                        CurrentWSG.EchoStringsPT2[CurrentWSG.NumberOfEchosPT2 - 1] = Echoes.ListSectionNames()[EchoList.SelectedIndex];
                        
                    }
                    DoEchoTree();
                    EchoTree.Nodes[1].Expand();
                    EchoTree.SelectedNode = EchoTree.Nodes[1];
                }
                //try
                //{ }
            //catch { MessageBox.Show("Could not add new echo log"); }
        }

        private void ExportEchoes_Click(object sender, EventArgs e)
        {
            try
            {
                if (EchoTree.SelectedNode.Name == "PT2" || EchoTree.SelectedNode.Text == "Playthrough 2 Echo Logs")
                {
                    SaveFileDialog tempExport = new SaveFileDialog();
                    string ExportText = "";
                    tempExport.DefaultExt = "*.echologs";
                    tempExport.Filter = "Echo Logs(*.echologs)|*.echologs";

                    tempExport.FileName = CurrentWSG.CharacterName + "'s PT2 Echo Logs.echologs";

                    if (tempExport.ShowDialog() == DialogResult.OK)
                        for (int Progress = 0; Progress < CurrentWSG.NumberOfEchosPT2; Progress++)
                        {
                            ExportText = ExportText + "[" + CurrentWSG.EchoStringsPT2[Progress] + "]" + "\r\nDLCValue1=" + CurrentWSG.EchoValuesPT2[Progress, 0] + "\r\nDLCValue2=" + CurrentWSG.EchoValuesPT2[Progress, 1] + "\r\n";


                        }
                    File.WriteAllText(tempExport.FileName, ExportText);
                }
                else if (EchoTree.SelectedNode.Name == "PT1" || EchoTree.SelectedNode.Text == "Playthrough 1 Echo Logs")
                {
                    SaveFileDialog tempExport = new SaveFileDialog();
                    string ExportText = "";
                    tempExport.DefaultExt = "*.echologs";
                    tempExport.Filter = "Echo Logs(*.echologs)|*.echologs";

                    tempExport.FileName = CurrentWSG.CharacterName + "'s PT1 Echo Logs.echologs";

                    if (tempExport.ShowDialog() == DialogResult.OK)
                        for (int Progress = 0; Progress < CurrentWSG.NumberOfEchos; Progress++)
                        {
                            ExportText = ExportText + "[" + CurrentWSG.EchoStrings[Progress] + "]" + "\r\nDLCValue1=" + CurrentWSG.EchoValues[Progress, 0] + "\r\nDLCValue2=" + CurrentWSG.EchoValues[Progress, 1] + "\r\n";


                        }
                    File.WriteAllText(tempExport.FileName, ExportText);
                }
            }
            catch { MessageBox.Show("Select a playthrough to export first."); }
        }

        private void ImportEchoes_Click(object sender, EventArgs e)
        {
            //try
            //{
                OpenFileDialog tempImport = new OpenFileDialog();
                if (EchoTree.SelectedNode.Name == "PT2" || EchoTree.SelectedNode.Text == "Playthrough 2 Echo Logs")
                {

                    tempImport.DefaultExt = "*.echologs";
                    tempImport.Filter = "Echo Logs(*.echologs)|*.echologs";

                    tempImport.FileName = CurrentWSG.CharacterName + "'s PT2 Echo Logs.echologs";
                    if (tempImport.ShowDialog() == DialogResult.OK)
                    {
                        if (EchoTree.SelectedNode.Name == "PT2" || EchoTree.SelectedNode.Text == "Playthrough 2 Echo Logs")
                        {
                            Ini.IniFile ImportLogs = new Ini.IniFile(tempImport.FileName);
                            string[] TempEchoStrings = new string[ImportLogs.ListSectionNames().Length];
                            int[,] TempEchoValues = new int[ImportLogs.ListSectionNames().Length, 10];
                            for (int Progress = 0; Progress < ImportLogs.ListSectionNames().Length; Progress++)
                            {
                                TempEchoStrings[Progress] = ImportLogs.ListSectionNames()[Progress];
                                TempEchoValues[Progress, 0] = Convert.ToInt32(ImportLogs.IniReadValue(ImportLogs.ListSectionNames()[Progress], "DLCValue1"));
                                TempEchoValues[Progress, 1] = Convert.ToInt32(ImportLogs.IniReadValue(ImportLogs.ListSectionNames()[Progress], "DLCValue2"));


                            }
                            CurrentWSG.EchoStringsPT2 = TempEchoStrings;
                            CurrentWSG.EchoValuesPT2 = TempEchoValues;
                            CurrentWSG.NumberOfEchosPT2 = ImportLogs.ListSectionNames().Length;
                            DoEchoTree();
                        }
                    }
                }
                        else if (EchoTree.SelectedNode.Name == "PT1" || EchoTree.SelectedNode.Text == "Playthrough 1 Echo Logs")
                        {
                            tempImport.DefaultExt = "*.echologs";
                            tempImport.Filter = "Echo Logs(*.echologs)|*.echologs";

                            tempImport.FileName = CurrentWSG.CharacterName + "'s PT1 Echo Logs.echologs";
                            
                    
                            
                            if (tempImport.ShowDialog() == DialogResult.OK)
                            {
                                Ini.IniFile ImportLogs = new Ini.IniFile(tempImport.FileName);
                                string[] TempEchoStrings = new string[ImportLogs.ListSectionNames().Length];
                                int[,] TempEchoValues = new int[ImportLogs.ListSectionNames().Length, 10];
                                for (int Progress = 0; Progress < ImportLogs.ListSectionNames().Length; Progress++)
                                {
                                    TempEchoStrings[Progress] = ImportLogs.ListSectionNames()[Progress];
                                    TempEchoValues[Progress, 0] = Convert.ToInt32(ImportLogs.IniReadValue(ImportLogs.ListSectionNames()[Progress], "DLCValue1"));
                                    TempEchoValues[Progress, 1] = Convert.ToInt32(ImportLogs.IniReadValue(ImportLogs.ListSectionNames()[Progress], "DLCValue2"));


                                }
                                CurrentWSG.EchoStrings = TempEchoStrings;
                                CurrentWSG.EchoValues = TempEchoValues;
                                CurrentWSG.NumberOfEchos = ImportLogs.ListSectionNames().Length;
                                DoEchoTree();
                            }
                            
                        }
                    
                
            try{
            }
            catch { MessageBox.Show("Select a playthrough to import first."); }


        }

        private void EchoDLCValue1_ValueChanged(object sender, EventArgs e)
        {
            if (EchoTree.SelectedNode.Name == "PT2")
                CurrentWSG.EchoValuesPT2[EchoTree.SelectedIndex, 0] = (int)EchoDLCValue1.Value;
            else if (EchoTree.SelectedNode.Name == "PT1")
                CurrentWSG.EchoValues[EchoTree.SelectedIndex, 0] = (int)EchoDLCValue1.Value;
        }

        private void DLCValue2_ValueChanged(object sender, EventArgs e)
        {
            if (EchoTree.SelectedNode.Name == "PT2")
                CurrentWSG.EchoValuesPT2[EchoTree.SelectedIndex, 1] = (int)EchoDLCValue2.Value;
            else if (EchoTree.SelectedNode.Name == "PT1")
                CurrentWSG.EchoValues[EchoTree.SelectedIndex, 1] = (int)EchoDLCValue2.Value;
            
        }

        private void EchoTree_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            try
            {
                if (EchoTree.SelectedNode.Name == "PT2")
                {
                    EchoDLCValue1.Value = CurrentWSG.EchoValuesPT2[EchoTree.SelectedIndex, 0];
                    EchoDLCValue2.Value = CurrentWSG.EchoValuesPT2[EchoTree.SelectedIndex, 1];
                }
                else if (EchoTree.SelectedNode.Name == "PT1")
                {
                    EchoDLCValue1.Value = CurrentWSG.EchoValues[EchoTree.SelectedIndex, 0];
                    EchoDLCValue2.Value = CurrentWSG.EchoValues[EchoTree.SelectedIndex, 1];
                }
            }
            catch { }
        }


        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< GENERAL/LOCATIONS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  \\


        private void DeleteLocation_Click(object sender, EventArgs e)
        {
            try
            {
                int Selected = LocationTree.SelectedNode.Index;
                CurrentWSG.TotalLocations = CurrentWSG.TotalLocations - 1;
                for (int Position = Selected; Position < CurrentWSG.TotalLocations; Position++)
                {
                    CurrentWSG.LocationStrings[Position] = CurrentWSG.LocationStrings[Position + 1];
                }
                ResizeArraySmaller(ref CurrentWSG.LocationStrings, CurrentWSG.TotalLocations);
                DoLocationTree();

                LocationTree.SelectedIndex = Selected;
            }
            catch { }
        }

        private void LocationsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                Ini.IniFile Locations = new Ini.IniFile(AppDir + "\\Data\\Locations.ini");
                int SelectedItem = LocationsList.SelectedIndex;
                CurrentWSG.TotalLocations = CurrentWSG.TotalLocations + 1;
                ResizeArrayLarger(ref CurrentWSG.LocationStrings, CurrentWSG.TotalLocations);
                CurrentWSG.LocationStrings[CurrentWSG.TotalLocations - 1] = Locations.ListSectionNames()[SelectedItem];
                DoLocationTree();

                
            }
            catch { }
        }
        private void Level_ValueChanged(object sender, EventArgs e)
        {
            if (Level.Value < 61)
            {
                Experience.Minimum = XPChart[(int)Level.Value];
                Experience.Maximum = XPChart[(int)Level.Value + 1] - 1;
            }
            else
            {
                Experience.Minimum = XPChart[61];
                Experience.Maximum = XPChart[62];
            }

        }

        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< SKILLS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  \\

        private void SkillLevel_ValueChanged(object sender, EventArgs e)
        {
            try
            { CurrentWSG.LevelOfSkills[SkillTree.SelectedIndex] = (int)SkillLevel.Value; }
            catch { }
        }

        private void SkillExp_ValueChanged(object sender, EventArgs e)
        {
            try
            { CurrentWSG.ExpOfSkills[SkillTree.SelectedIndex] = (int)SkillExp.Value; }
            catch { }
        }

        private void SkillActive_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (SkillActive.SelectedIndex == 1)
                    CurrentWSG.InUse[SkillTree.SelectedIndex] = 1;
                else
                    CurrentWSG.InUse[SkillTree.SelectedIndex] = -1;
            }
            catch { }
        }

        private void DeleteSkill_Click(object sender, EventArgs e)
        {
            try
            {
                int Selected = SkillTree.SelectedNode.Index;
                CurrentWSG.NumberOfSkills = CurrentWSG.NumberOfSkills - 1;
                for (int Position = Selected; Position < CurrentWSG.NumberOfSkills; Position++)
                {
                    CurrentWSG.SkillNames[Position] = CurrentWSG.SkillNames[Position + 1];
                    CurrentWSG.InUse[Position] = CurrentWSG.InUse[Position + 1];
                    CurrentWSG.ExpOfSkills[Position] = CurrentWSG.ExpOfSkills[Position + 1];
                    CurrentWSG.LevelOfSkills[Position] = CurrentWSG.LevelOfSkills[Position + 1];

                }
                ResizeArraySmaller(ref CurrentWSG.SkillNames, CurrentWSG.NumberOfSkills);
                ResizeArraySmaller(ref CurrentWSG.InUse, CurrentWSG.NumberOfSkills);
                ResizeArraySmaller(ref CurrentWSG.ExpOfSkills, CurrentWSG.NumberOfSkills);
                ResizeArraySmaller(ref CurrentWSG.LevelOfSkills, CurrentWSG.NumberOfSkills);
                DoSkillTree();

                SkillTree.SelectedIndex = Selected;
            }
            catch { }
        }

        private void ExportSkills_Click(object sender, EventArgs e)
        {
            SaveFileDialog tempExport = new SaveFileDialog();
            string ExportText = "";
            tempExport.DefaultExt = "*.skills";
            tempExport.Filter = "Skills Data(*.skills)|*.skills";

            tempExport.FileName = CurrentWSG.CharacterName + "'s Skills.skills";

            if (tempExport.ShowDialog() == DialogResult.OK)
            {
                for (int Progress = 0; Progress < CurrentWSG.NumberOfSkills; Progress++)
                    ExportText = ExportText + "[" + CurrentWSG.SkillNames[Progress] + "]\r\nLevel=" + CurrentWSG.LevelOfSkills[Progress] + "\r\nExperience=" + CurrentWSG.ExpOfSkills[Progress] + "\r\nInUse=" + CurrentWSG.InUse[Progress] + "\r\n";
            }
            File.WriteAllText(tempExport.FileName, ExportText);
        }

        private void ImportSkills_Click(object sender, EventArgs e)

        {
            OpenFileDialog tempImport = new OpenFileDialog();
            tempImport.DefaultExt = "*.skills";
            tempImport.Filter = "Skills Data(*.skills)|*.skills";

            tempImport.FileName = CurrentWSG.CharacterName + "'s Skills.skills";
            if (tempImport.ShowDialog() == DialogResult.OK)
            {

                Ini.IniFile ImportSkills = new Ini.IniFile(tempImport.FileName);
                string[] TempSkillNames = new string[ImportSkills.ListSectionNames().Length];
                int[] TempSkillLevels = new int[ImportSkills.ListSectionNames().Length];
                int[] TempSkillExp = new int[ImportSkills.ListSectionNames().Length];
                int[] TempSkillInUse = new int[ImportSkills.ListSectionNames().Length];
                for (int Progress = 0; Progress < ImportSkills.ListSectionNames().Length; Progress++)
                {
                    TempSkillNames[Progress] = ImportSkills.ListSectionNames()[Progress];
                    TempSkillLevels[Progress] = Convert.ToInt32(ImportSkills.IniReadValue(ImportSkills.ListSectionNames()[Progress], "Level"));
                    TempSkillExp[Progress] = Convert.ToInt32(ImportSkills.IniReadValue(ImportSkills.ListSectionNames()[Progress], "Experience"));
                    TempSkillInUse[Progress] = Convert.ToInt32(ImportSkills.IniReadValue(ImportSkills.ListSectionNames()[Progress], "InUse"));
                }
                CurrentWSG.SkillNames = TempSkillNames;
                CurrentWSG.LevelOfSkills = TempSkillLevels;
                CurrentWSG.ExpOfSkills = TempSkillExp;
                CurrentWSG.InUse = TempSkillInUse;
                CurrentWSG.NumberOfSkills = ImportSkills.ListSectionNames().Length;
                DoSkillTree();
            }
        }
        private void SkillList_Click(object sender, EventArgs e)
        {
            try
            {
                Ini.IniFile SkillINI = new Ini.IniFile(AppDir + "\\Data\\gd_skills_common.txt");
                if (Class.SelectedItem == "Soldier")
                    SkillINI = new Ini.IniFile(AppDir + "\\Data\\gd_skills2_Roland.txt");

                else if (Class.SelectedItem == "Siren")
                    SkillINI = new Ini.IniFile(AppDir + "\\Data\\gd_Skills2_Lilith.txt");

                else if (Class.SelectedItem == "Hunter")
                    SkillINI = new Ini.IniFile(AppDir + "\\Data\\gd_skills2_Mordecai.txt");

                else if (Class.SelectedItem == "Berserker")
                    SkillINI = new Ini.IniFile(AppDir + "\\Data\\gd_Skills2_Brick.txt");

                CurrentWSG.NumberOfSkills = CurrentWSG.NumberOfSkills + 1;
                ResizeArrayLarger(ref CurrentWSG.SkillNames, CurrentWSG.NumberOfSkills);
                ResizeArrayLarger(ref CurrentWSG.LevelOfSkills, CurrentWSG.NumberOfSkills);
                ResizeArrayLarger(ref CurrentWSG.ExpOfSkills, CurrentWSG.NumberOfSkills);
                ResizeArrayLarger(ref CurrentWSG.InUse, CurrentWSG.NumberOfSkills);
                CurrentWSG.InUse[CurrentWSG.NumberOfSkills - 1] = -1;
                CurrentWSG.SkillNames[CurrentWSG.NumberOfSkills - 1] = SkillINI.ListSectionNames()[SkillList.SelectedIndex];
                DoSkillTree();
            }
            catch { MessageBox.Show("Could not add new Skill."); }
        }

        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< AMMO POOLS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  \\

        private void AmmoTree_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            try
            {
                AmmoPoolRemaining.Value = (decimal)CurrentWSG.RemainingPools[AmmoTree.SelectedIndex];
                AmmoSDULevel.Value = CurrentWSG.PoolLevels[AmmoTree.SelectedIndex];
            }
            catch { }
        }

        private void AmmoPoolRemaining_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CurrentWSG.RemainingPools[AmmoTree.SelectedIndex] = (float)AmmoPoolRemaining.Value;
            }
            catch { }
        }

        private void AmmoSDULevel_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CurrentWSG.PoolLevels[AmmoTree.SelectedIndex] = (int)AmmoSDULevel.Value;
            }

            catch { }
        }

        private void AmmoDelete_Click(object sender, EventArgs e)
        {
            if (AmmoTree.SelectedIndex != -1)
            {
                CurrentWSG.NumberOfPools = CurrentWSG.NumberOfPools - 1;
                ResizeArraySmaller(ref CurrentWSG.AmmoPools, CurrentWSG.NumberOfPools);
                ResizeArraySmaller(ref CurrentWSG.ResourcePools, CurrentWSG.NumberOfPools);
                ResizeArraySmaller(ref CurrentWSG.RemainingPools, CurrentWSG.NumberOfPools);
                ResizeArraySmaller(ref CurrentWSG.PoolLevels, CurrentWSG.NumberOfPools);
                DoAmmoTree();
            }
        }

        private void NewAmmo_Click(object sender, EventArgs e)
        {
            try
            {
                string New_d_resources = Interaction.InputBox("Enter the 'd_resources' for the new Ammo Pool", "New Ammo Pool", "", 10, 10);
                string New_d_resourcepools = Interaction.InputBox("Enter the 'd_resourcepools' for the new Ammo Pool", "New Ammo Pool", "", 10, 10);
                if (New_d_resourcepools != "" && New_d_resources != "")
                {
                    CurrentWSG.NumberOfPools = CurrentWSG.NumberOfPools + 1;
                    ResizeArrayLarger(ref CurrentWSG.AmmoPools, CurrentWSG.NumberOfPools);
                    ResizeArrayLarger(ref CurrentWSG.ResourcePools, CurrentWSG.NumberOfPools);
                    ResizeArrayLarger(ref CurrentWSG.RemainingPools, CurrentWSG.NumberOfPools);
                    ResizeArrayLarger(ref CurrentWSG.PoolLevels, CurrentWSG.NumberOfPools);
                    CurrentWSG.AmmoPools[CurrentWSG.NumberOfPools - 1] = New_d_resourcepools;
                    CurrentWSG.ResourcePools[CurrentWSG.NumberOfPools - 1] = New_d_resources;
                    DoAmmoTree();
                }
            }
            catch { MessageBox.Show("Couldn't add new ammo pool."); }
        }

        //  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< WILLOWTREE LOCKER >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  \\

        private void LockerTree_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            try
            {
                PartsLocker.Items.Clear();
                Ini.IniFile Locker = new Ini.IniFile(OpenedLocker);
                NameLocker.Text = Locker.ListSectionNames()[LockerTree.SelectedNode.Index];

                RatingLocker.Rating = Convert.ToInt32(Locker.IniReadValue(Locker.ListSectionNames()[LockerTree.SelectedNode.Index], "Rating"));
                DescriptionLocker.Text = Locker.IniReadValue(Locker.ListSectionNames()[LockerTree.SelectedNode.Index], "Description");
                DescriptionLocker.Text = DescriptionLocker.Text.Replace("$LINE$", "\r\n");
                ItemTypeLocker.SelectedItem = Locker.IniReadValue(Locker.ListSectionNames()[LockerTree.SelectedNode.Index], "Type");

                for (int Progress = 0; Progress < 14; Progress++)
                    PartsLocker.Items.Add(Locker.IniReadValue(Locker.ListSectionNames()[LockerTree.SelectedNode.Index], "Part" + (Progress + 1)));
            }
            catch { }
        }

        private void SaveChangesLocker_Click(object sender, EventArgs e)
        {

            SaveChangesToLocker();
        }

        private void NewWeaponLocker_Click(object sender, EventArgs e)
        {
            string tempINI = System.IO.File.ReadAllText(OpenedLocker);
            int Occurances = 0;
            Ini.IniFile Locker = new Ini.IniFile(OpenedLocker);
            for (int Progress = 0; Progress < Locker.ListSectionNames().Length; Progress++)
                if (Locker.ListSectionNames()[Progress] == "New Weapon" || Locker.ListSectionNames()[Progress].Contains("New Weapon (Copy "))
                    Occurances = Occurances + 1;


            if (Occurances > 0)
            {
                //MessageBox.Show("A new weapon already exists.");
                string NewWeaponEntry = "\r\n[New Weapon (Copy " + Occurances + ")]\r\nType=Weapon\r\nRating=0\r\nDescription=\"Type in a description for the weapon here.\"\r\nPart1=None\r\nPart2=None\r\nPart3=None\r\nPart4=None\r\nPart5=None\r\nPart6=None\r\nPart7=None\r\nPart8=None\r\nPart9=None\r\nPart10=None\r\nPart11=None\r\nPart12=None\r\nPart13=None\r\nPart14=None";
                tempINI = tempINI + NewWeaponEntry;
                System.IO.File.WriteAllText(OpenedLocker, tempINI);
                Node temp = new Node();
                temp.Text = "New Weapon (Copy " + Occurances + ")";
                LockerTree.Nodes.Add(temp);
            }
            else
            {

                string NewWeaponEntry = "\r\n[New Weapon]\r\nType=Weapon\r\nRating=0\r\nDescription=\"Type in a description for the weapon here.\"\r\nPart1=None\r\nPart2=None\r\nPart3=None\r\nPart4=None\r\nPart5=None\r\nPart6=None\r\nPart7=None\r\nPart8=None\r\nPart9=None\r\nPart10=None\r\nPart11=None\r\nPart12=None\r\nPart13=None\r\nPart14=None";
                tempINI = tempINI + NewWeaponEntry;
                System.IO.File.WriteAllText(OpenedLocker, tempINI);
                Node temp = new Node();
                temp.Text = "New Weapon";
                LockerTree.Nodes.Add(temp);
            }
        }

        private void NewItemLocker_Click(object sender, EventArgs e)
        {
            string tempINI = System.IO.File.ReadAllText(OpenedLocker);
            int Occurances = 0;
            Ini.IniFile Locker = new Ini.IniFile(OpenedLocker);
            for (int Progress = 0; Progress < Locker.ListSectionNames().Length; Progress++)
                if (Locker.ListSectionNames()[Progress] == "New Item" || Locker.ListSectionNames()[Progress].Contains("New Item (Copy "))
                    Occurances = Occurances + 1;


            if (Occurances > 0)
            {
                //MessageBox.Show("A new weapon already exists.");
                string NewWeaponEntry = "\r\n[New Item (Copy " + Occurances + ")]\r\nType=Item\r\nRating=0\r\nDescription=\"Type in a description for the item here.\"\r\nPart1=None\r\nPart2=None\r\nPart3=None\r\nPart4=None\r\nPart5=None\r\nPart6=None\r\nPart7=None\r\nPart8=None\r\nPart9=None\r\nPart10=\r\nPart11=\r\nPart12=\r\nPart13=\r\nPart14=";
                tempINI = tempINI + NewWeaponEntry;
                System.IO.File.WriteAllText(OpenedLocker, tempINI);
                Node temp = new Node();
                temp.Text = "New Item (Copy " + Occurances + ")";
                LockerTree.Nodes.Add(temp);
            }
            else
            {

                string NewWeaponEntry = "\r\n[New Item]\r\nType=Item\r\nRating=0\r\nDescription=\"Type in a description for the item here.\"\r\nPart1=None\r\nPart2=None\r\nPart3=None\r\nPart4=None\r\nPart5=None\r\nPart6=None\r\nPart7=None\r\nPart8=None\r\nPart9=None\r\nPart10=\r\nPart11=\r\nPart12=\r\nPart13=\r\nPart14=";
                tempINI = tempINI + NewWeaponEntry;
                System.IO.File.WriteAllText(OpenedLocker, tempINI);
                Node temp = new Node();
                temp.Text = "New Item";
                LockerTree.Nodes.Add(temp);
            }
        }

        private void ItemTypeLocker_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ItemTypeLocker.SelectedIndex == 0)
                for (int Progress = 9; Progress < 14; Progress++)
                {

                    PartsLocker.Items[Progress] = "None";
                }

            else if (ItemTypeLocker.SelectedIndex == 1)
                for (int Progress = 9; Progress < 14; Progress++)
                {

                    PartsLocker.Items[Progress] = "";
                }
        }

        private void ImportFromClipboardLocker_Click(object sender, EventArgs e)
        {
            try
            {
                InOutPartsBox.Clear();
                InOutPartsBox.Text = Clipboard.GetText();
                InOutPartsBox.Text = InOutPartsBox.Text.Replace(" ", "");
                //PartsLocker.Items.Clear();
                NameLocker.Text = "";
                Ini.IniFile Titles = new Ini.IniFile(AppDir + "\\Data\\Titles.ini");
                for (int Progress = 0; Progress < 14; Progress++)
                {
                    PartsLocker.Items[Progress] = InOutPartsBox.Lines[Progress];
                    if (GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != "" && GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != null && NameLocker.Text == "")
                        NameLocker.Text = GetName(Titles, InOutPartsBox.Lines, Progress, "PartName");
                    else if (GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != "" && GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != null && NameLocker.Text != "")
                        NameLocker.Text = NameLocker.Text + " " + GetName(Titles, InOutPartsBox.Lines, Progress, "PartName");
                }
            }
            catch { MessageBox.Show("Couldn't insert from clipboard."); }
        }

        private void ImportFromFileLocker_Click(object sender, EventArgs e)
        {
            OpenFileDialog FromFile = new OpenFileDialog();
            FromFile.DefaultExt = "*.txt";
            FromFile.Filter = "Item Files(*.txt)|*.txt";

            FromFile.FileName = CurrentPartsGroup.Text + ".txt";
            try
            {
                if (FromFile.ShowDialog() == DialogResult.OK)
                {

                    InOutPartsBox.Clear();
                    InOutPartsBox.Text = System.IO.File.ReadAllText(FromFile.FileName);
                    InOutPartsBox.Text = InOutPartsBox.Text.Replace(" ", "");
                    //PartsLocker.Items.Clear();
                    NameLocker.Text = "";
                    Ini.IniFile Titles = new Ini.IniFile(AppDir + "\\Data\\Titles.ini");
                    for (int Progress = 0; Progress < 14; Progress++)
                    {
                        PartsLocker.Items[Progress] = InOutPartsBox.Lines[Progress];
                        if (GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != "" && GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != null && NameLocker.Text == "")
                            NameLocker.Text = GetName(Titles, InOutPartsBox.Lines, Progress, "PartName");
                        else if (GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != "" && GetName(Titles, InOutPartsBox.Lines, Progress, "PartName") != null && NameLocker.Text != "")
                            NameLocker.Text = NameLocker.Text + " " + GetName(Titles, InOutPartsBox.Lines, Progress, "PartName");
                    }
                }
            }
            catch { MessageBox.Show("Couldn't insert from file."); }
        }

        private void ExportToFileLocker_Click(object sender, EventArgs e)
        {
            SaveFileDialog ToFile = new SaveFileDialog();
            ToFile.DefaultExt = "*.txt";
            ToFile.Filter = "Item Files(*.txt)|*.txt";
            ToFile.FileName = NameLocker.Text + ".txt";
            if (ToFile.ShowDialog() == DialogResult.OK)
            {
                int Loops = 0;
                InOutPartsBox.Clear();
                if (ItemTypeLocker.SelectedIndex == 0) Loops = 14;
                else if (ItemTypeLocker.SelectedIndex == 1) Loops = 9;
                for (int Progress = 0; Progress < Loops; Progress++)
                {
                    if (Progress > 0) InOutPartsBox.AppendText("\r\n");
                    InOutPartsBox.AppendText((string)PartsLocker.Items[Progress]);
                }
                InOutPartsBox.AppendText("\r\n0\r\n0\r\n0\r\n0");
                System.IO.File.WriteAllLines(ToFile.FileName, InOutPartsBox.Lines);
            }




        }

        private void ExportToClipboardLocker_Click(object sender, EventArgs e)
        {
            int Loops = 0;
            InOutPartsBox.Clear();
            if (ItemTypeLocker.SelectedIndex == 0) Loops = 14;
            else if (ItemTypeLocker.SelectedIndex == 1) Loops = 9;
            for (int Progress = 0; Progress < Loops; Progress++)
            {
                if (Progress > 0) InOutPartsBox.AppendText("\r\n");
                InOutPartsBox.AppendText((string)PartsLocker.Items[Progress]);
            }
            InOutPartsBox.AppendText("\r\n0\r\n0\r\n0\r\n0");
            Clipboard.SetText(InOutPartsBox.Text);
        }

        private void ExportToBackpack_Click(object sender, EventArgs e)
        {
            if (ItemTypeLocker.SelectedIndex == 0)
            {

                InOutPartsBox.Clear();

                for (int Progress = 0; Progress < 14; Progress++)
                {
                    if (Progress > 0) InOutPartsBox.AppendText("\r\n");
                    InOutPartsBox.AppendText((string)PartsLocker.Items[Progress]);
                }
                InOutPartsBox.AppendText("\r\n0\r\n0\r\n0\r\n0");


                CurrentWSG.NumberOfWeapons = CurrentWSG.NumberOfWeapons + 1;
                CurrentWSG.WeaponStrings.Add(new List<string>());
                CurrentWSG.WeaponValues.Add(new List<int>());


                for (int Progress = 0; Progress < 14; Progress++)
                    CurrentWSG.WeaponStrings[CurrentWSG.NumberOfWeapons - 1].Add(InOutPartsBox.Lines[Progress]);
                for (int Progress = 0; Progress < 4; Progress++)
                    CurrentWSG.WeaponValues[CurrentWSG.NumberOfWeapons - 1].Add(Convert.ToInt32(InOutPartsBox.Lines[Progress + 14]));
                Node TempNode = new Node();
                TempNode.Text = GetName(new Ini.IniFile(AppDir + "\\Data\\Titles.ini"), CurrentWSG.WeaponStrings, CurrentWSG.NumberOfWeapons - 1, 14, "PartName");
                WeaponTree.Nodes.Add(TempNode);
                //DoWeaponTree();
            }
            else if (ItemTypeLocker.SelectedIndex == 1)
            {

                InOutPartsBox.Clear();

                for (int Progress = 0; Progress < 9; Progress++)
                {
                    if (Progress > 0) InOutPartsBox.AppendText("\r\n");
                    InOutPartsBox.AppendText((string)PartsLocker.Items[Progress]);
                }
                InOutPartsBox.AppendText("\r\n0\r\n0\r\n0\r\n0");



                CurrentWSG.ItemStrings.Add(new List<string>());
                CurrentWSG.ItemValues.Add(new List<int>());
                CurrentWSG.NumberOfItems = CurrentWSG.NumberOfItems + 1;
                for (int Progress = 0; Progress < 9; Progress++)
                    CurrentWSG.ItemStrings[CurrentWSG.NumberOfItems - 1].Add(InOutPartsBox.Lines[Progress]);
                for (int Progress = 0; Progress < 4; Progress++)
                    CurrentWSG.ItemValues[CurrentWSG.NumberOfItems - 1].Add(Convert.ToInt32(InOutPartsBox.Lines[Progress + 9]));
                Node TempNode = new Node();
                TempNode.Text = GetName(new Ini.IniFile(AppDir + "\\Data\\Titles.ini"), CurrentWSG.ItemStrings, CurrentWSG.NumberOfItems - 1, 9, "PartName");
                ItemTree.Nodes.Add(TempNode);
                //DoItemTree();
            }
        }

        private void DeleteLocker_Click(object sender, EventArgs e)
        {
            try
            {
                int Selected = LockerTree.SelectedIndex;
                string search = "[" + LockerTree.SelectedNode.Text + "]";

                string[] tempINI2 = System.IO.File.ReadAllLines(OpenedLocker);

                int DiscoveredLine = -1;
                for (int Progress = 0; Progress < tempINI2.Length; Progress++)
                {
                    //string tewst = tempINI2[Progress].Substring(0, tempINI2[Progress].Length - 1);
                    if (tempINI2[Progress] == search)
                        DiscoveredLine = Progress;


                }
                for (int Progress = 0; Progress < 18; Progress++)
                    tempINI2[DiscoveredLine + Progress] = "";
                System.IO.File.WriteAllLines(OpenedLocker, tempINI2);
                DoLockerTree(OpenedLocker);
                try
                {
                    LockerTree.SelectedIndex = Selected;
                }
                catch { }
            }
            catch { }
        }

        private void OpenLocker_Click(object sender, EventArgs e)
        {
            OpenFileDialog FromFile = new OpenFileDialog();
            FromFile.DefaultExt = "*.wtl";
            FromFile.Filter = "WillowTree Locker(*.wtl)|*.wtl";

            FromFile.FileName = CurrentPartsGroup.Text + ".wtl";
            try
            {
                if (FromFile.ShowDialog() == DialogResult.OK)
                    DoLockerTree(FromFile.FileName);

            }
            catch { MessageBox.Show("Could not load the selected WillowTree Locker."); }
        }

        private void SaveAsLocker_Click(object sender, EventArgs e)
        {
            SaveFileDialog ToFile = new SaveFileDialog();
            ToFile.DefaultExt = "*.wtl";
            ToFile.Filter = "WillowTree Locker(*.wtl)|*.wtl";

            ToFile.FileName = "default.wtl";
            try
            {
                if (ToFile.ShowDialog() == DialogResult.OK)
                    File.Copy(OpenedLocker, ToFile.FileName);
                DoLockerTree(ToFile.FileName);
            }
            catch { MessageBox.Show("Could not save the selected WillowTree Locker."); }
        }

        private void ImportAllFromWeapons_Click(object sender, EventArgs e)
        {
            string tempINI = System.IO.File.ReadAllText(OpenedLocker);

            for (int Progress = 0; Progress < CurrentWSG.NumberOfWeapons; Progress++)
            {
                int Occurances = 0;
                Ini.IniFile Locker = new Ini.IniFile(OpenedLocker);
                for (int Search = 0; Search < Locker.ListSectionNames().Length; Search++)
                    if (Locker.ListSectionNames()[Search] == WeaponTree.Nodes[Progress].Text || Locker.ListSectionNames()[Search].Contains(WeaponTree.Nodes[Progress].Text + " (Copy "))
                        Occurances = Occurances + 1;

                if (Occurances > 0)
                {

                    string NewWeaponEntry = "\r\n[" + WeaponTree.Nodes[Progress].Text + " (Copy " + Occurances + ")]\r\nType=Weapon\r\nRating=0\r\nDescription=\"Type in a description for the weapon here.\"";
                    for (int PartProgress = 0; PartProgress < 14; PartProgress++)
                        NewWeaponEntry = NewWeaponEntry + "\r\nPart" + (PartProgress + 1) + "=" + CurrentWSG.WeaponStrings[Progress][PartProgress];

                    tempINI = tempINI + NewWeaponEntry;

                    Node temp = new Node();
                    temp.Text = WeaponTree.Nodes[Progress].Text + " (Copy " + Occurances + ")";
                    LockerTree.Nodes.Add(temp);
                }
                else
                {

                    string NewWeaponEntry = "\r\n[" + WeaponTree.Nodes[Progress].Text + "]\r\nType=Weapon\r\nRating=0\r\nDescription=\"Type in a description for the weapon here.\"";
                    for (int PartProgress = 0; PartProgress < 14; PartProgress++)
                        NewWeaponEntry = NewWeaponEntry + "\r\nPart" + (PartProgress + 1) + "=" + CurrentWSG.WeaponStrings[Progress][PartProgress];

                    tempINI = tempINI + NewWeaponEntry;

                    Node temp = new Node();
                    temp.Text = WeaponTree.Nodes[Progress].Text;
                    LockerTree.Nodes.Add(temp);
                }
                
                System.IO.File.WriteAllText(OpenedLocker, tempINI);

            }
        }

        private void ImportAllFromItems_Click(object sender, EventArgs e)
        {
            string tempINI = System.IO.File.ReadAllText(OpenedLocker);

            for (int Progress = 0; Progress < CurrentWSG.NumberOfItems; Progress++)
            {
                int Occurances = 0;
                Ini.IniFile Locker = new Ini.IniFile(OpenedLocker);
                for (int Search = 0; Search < Locker.ListSectionNames().Length; Search++)
                    if (Locker.ListSectionNames()[Search] == ItemTree.Nodes[Progress].Text || Locker.ListSectionNames()[Search].Contains(ItemTree.Nodes[Progress].Text + " (Copy "))
                        Occurances = Occurances + 1;

                if (Occurances > 0)
                {

                    string NewItemEntry = "\r\n[" + ItemTree.Nodes[Progress].Text + " (Copy " + Occurances + ")]\r\nType=Item\r\nRating=0\r\nDescription=\"Type in a description for the Item here.\"";
                    for (int PartProgress = 0; PartProgress < 9; PartProgress++)
                        NewItemEntry = NewItemEntry + "\r\nPart" + (PartProgress + 1) + "=" + CurrentWSG.ItemStrings[Progress][PartProgress];
                    NewItemEntry = NewItemEntry + "\r\nPart10=\r\nPart11=\r\nPart12=\r\nPart13=\r\nPart14=";

                    tempINI = tempINI + NewItemEntry;

                    Node temp = new Node();
                    temp.Text = ItemTree.Nodes[Progress].Text + " (Copy " + Occurances + ")";
                    LockerTree.Nodes.Add(temp);
                }
                else
                {

                    string NewItemEntry = "\r\n[" + ItemTree.Nodes[Progress].Text + "]\r\nType=Item\r\nRating=0\r\nDescription=\"Type in a description for the Item here.\"";
                    for (int PartProgress = 0; PartProgress < 9; PartProgress++)
                        NewItemEntry = NewItemEntry + "\r\nPart" + (PartProgress + 1) + "=" + CurrentWSG.ItemStrings[Progress][PartProgress];
                    NewItemEntry = NewItemEntry + "\r\nPart10=\r\nPart11=\r\nPart12=\r\nPart13=\r\nPart14=";

                    tempINI = tempINI + NewItemEntry;

                    Node temp = new Node();
                    temp.Text = ItemTree.Nodes[Progress].Text;
                    LockerTree.Nodes.Add(temp);
                }
                System.IO.File.WriteAllText(OpenedLocker, tempINI);
            }
        }

        private void ImportAllFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog tempRead = new OpenFileDialog();
            if (tempRead.ShowDialog() == DialogResult.OK)
                System.IO.File.WriteAllText(OpenedLocker, File.ReadAllText(OpenedLocker) + "\r\n" + File.ReadAllText(tempRead.FileName));
            DoLockerTree(OpenedLocker);
        }


        private void UpdateButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://xander.x-fusion.co.uk/WillowTree/WillowTree%23" + VersionFromServer + ".zip");
        }

        private void CurrentWeaponParts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void WeaponInfo_Click(object sender, EventArgs e)
        {
            string[] Parts = new string[14];
            
            int prog = 0;
            foreach (string i in CurrentWeaponParts.Items)
            {
                Parts[prog] = i;
                prog = prog + 1;
            }
            string WeaponInfo;
            DamageText.Text = "Expected Weapon Damage: " + GetWeaponDamage(Parts);
            if(WeaponItemGradeSlider.Value > 0)
                WeaponInfo = "Expected Damage: " + GetWeaponDamage(Parts, WeaponItemGradeSlider.Value);
            else
            WeaponInfo = "Expected Damage: " + GetWeaponDamage(Parts);
            if (GetExtraStats(Parts, "TechLevelIncrease") != 0)
                WeaponInfo = WeaponInfo + "\r\nElemental Tech Level: " + GetExtraStats(Parts, "TechLevelIncrease");
            if (GetExtraStats(Parts, "WeaponDamage") != 0)
                WeaponInfo = WeaponInfo + "\r\n" + GetExtraStats(Parts, "WeaponDamage") + "% Damage";
            if (GetExtraStats(Parts, "WeaponFireRate") != 0)
                WeaponInfo = WeaponInfo + "\r\n" + GetExtraStats(Parts, "WeaponFireRate") + "% Rate of Fire";
            if (GetExtraStats(Parts, "WeaponCritBonus") != 0)
                WeaponInfo = WeaponInfo + "\r\n" + GetExtraStats(Parts, "WeaponCritBonus") + "% Critical Damage";
            if (GetExtraStats(Parts, "WeaponReloadSpeed") != 0)
                WeaponInfo = WeaponInfo + "\r\n" + GetExtraStats(Parts, "WeaponReloadSpeed") + "% Reload Speed";
            if (GetExtraStats(Parts, "WeaponSpread") != 0)
                WeaponInfo = WeaponInfo + "\r\n" + GetExtraStats(Parts, "WeaponSpread") + "% Spread";
            if (GetExtraStats(Parts, "MaxAccuracy") != 0)
                WeaponInfo = WeaponInfo + "\r\n" + GetExtraStats(Parts, "MaxAccuracy") + "% Max Accuracy";
            if (GetExtraStats(Parts, "MinAccuracy") != 0)
                WeaponInfo = WeaponInfo + "\r\n" + GetExtraStats(Parts, "MinAccuracy") + "% Min Accuracy";
            if (Experience.Value == 1337)
                WeaponInfo = AmIACookie(false);
            if (Experience.Value == 1337 && BackpackSpace.Value == 1337)
                WeaponInfo = AmIACookie(true);
            MessageBox.Show(WeaponInfo);
        }

        private void PartCategories_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            WeaponPartInfo.Clear();
            try
            {
                if (File.Exists(AppDir + "\\Data\\" + PartCategories.SelectedNode.Parent.Text + ".txt"))
                {
                    string[] InText = File.ReadAllLines(AppDir + "\\Data\\" + PartCategories.SelectedNode.Parent.Text + ".txt");
                    int start = 0;
                    int end = 1;
                    string search = "[" + PartCategories.SelectedNode.Text + "]";
                    ArrayList Lines = new ArrayList();
                    for (int i1 = 0; i1 < InText.Length; i1++)
                    {
                        if (InText[i1].Contains(search))
                        {
                            start = i1;
                            i1 = InText.Length;
                        }
                    }
                    for (int i2 = start + 1; i2 < InText.Length; i2++)
                    {
                        if (InText[i2].Contains("[") == true && InText[i2].Contains("=") == false)
                        {
                            end = i2;
                            i2 = InText.Length;
                        }
                    }
                    if(start > end)
                        for (start++; start < InText.Length; start++)
                        {
                            
                                InText[start].Replace("=", ": ");
                                Lines.Add(InText[start]);
                            
                        }
                    else
                    for (start++; start < end; start++)
                    {
                        
                            InText[start].Replace("=", ": ");
                            Lines.Add(InText[start]);
                        
                    }
                    WeaponPartInfo.Lines = (string[])Lines.ToArray(typeof(string));

                }
            }
            catch { }
        }


        private void DLCWeaponButton_Click(object sender, EventArgs e)
        {
            DoDLCWeaponTree();
            IsDLCWeaponMode = true;
            WeaponItemGradeSlider.Enabled = true;
            WeaponPanel1.Text = "DLC Backpack";
        }

        private void MainWeaponButton_Click(object sender, EventArgs e)
        {
            DoWeaponTree();
            IsDLCWeaponMode = false;
            WeaponItemGradeSlider.Value = 0;
            WeaponItemGradeSlider.Enabled = false;
            WeaponPanel1.Text = "Main Backpack";
        }

        private void ItemGradeSlider_ValueChanged(object sender, EventArgs e)
        {
            if (WeaponItemGradeSlider.Value > 0)
                ItemGradeLabel.Text = "Level: " + (WeaponItemGradeSlider.Value - 2);
            else
            {
                ItemGradeLabel.Text = "Level: Disabled";
            }
        }

        private void DLCItemButton_Click(object sender, EventArgs e)
        {
            DoDLCItemTree();
            IsDLCItemMode = true;
            ItemItemGradeSlider.Enabled = true;
            ItemPanel.Text = "DLC Backpack";
        }

        private void MainItemButton_Click(object sender, EventArgs e)
        {
            DoItemTree();
            IsDLCItemMode = false;
            ItemItemGradeSlider.Value = 0;
            ItemItemGradeSlider.Enabled = false;
            ItemPanel.Text = "Main Backpack";
        }

        private void ItemItemGradeSlider_ValueChanged(object sender, EventArgs e)
        {
            if (ItemItemGradeSlider.Value > 0)
                ItemItemGradeLabel.Text = "Level: " + (ItemItemGradeSlider.Value - 2);
            else
            {
                ItemItemGradeLabel.Text = "Level: Disabled";
            }
        }



        private void EditAll_Click(object sender, EventArgs e)
        {
            string tempNewLevels = Interaction.InputBox("All of the guns in your backpack will be adjusted to the following level:", "Edit All Levels", "", 10, 10);
            if (tempNewLevels != "" && tempNewLevels == "" + Convert.ToInt32(tempNewLevels))
            {
                foreach (List<int> item in CurrentWSG.WeaponValues)
                    item[3] = Convert.ToInt32(tempNewLevels) + 2;
                WeaponItemGradeSlider.Value = Convert.ToInt32(tempNewLevels) + 2;
            }
        }




        }

    }

