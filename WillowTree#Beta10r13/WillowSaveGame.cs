using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using X360.IO;
using X360.STFS;
using System.IO;

namespace WindowsFormsApplication1
{
   
    // Fun fun fun time. Okay, well after I decided to ditch using DJsIO I ran into a number of problems.
    // 90% of them revolved around Microsofts total lack of Big Endian support. So I had to add my own, 
    // and by that I mean I snatched some code off of Google and twisted it to work for WillowTree. Well,
    // it works doesn't it? Aaaaaaaanyway, I really want to fix up the opening/saving process so that you
    // can edit specific parts of the save without requiring a complete rebuild. For example, if you just 
    // (want to) edit a weapon, it rebuilds the weapon list only.


    public class WillowSaveGame
    {

        //Yay, code from Google.
        public enum ByteOrder : int
        {
            LittleEndian,
            BigEndian
        }

        public static byte[] ReadBytes(BinaryReader reader, int fieldSize, ByteOrder byteOrder)
        {
            byte[] bytes = new byte[fieldSize];
            if (byteOrder == ByteOrder.LittleEndian)
                return reader.ReadBytes(fieldSize);
            else
            {
                for (int i = fieldSize - 1; i > -1; i--)
                    bytes[i] = reader.ReadByte();
                return bytes;
            }
        }
        public static byte[] ReadBytes(byte[] inBytes, int fieldSize, ByteOrder byteOrder)
        {
            byte[] bytes = new byte[fieldSize];
            if (byteOrder == ByteOrder.LittleEndian)
                return inBytes;
            else
            {
                int f = 0;
                for (int i = fieldSize - 1; i > -1; i--)
                {
                    bytes[i] = inBytes[f];
                    f++;
                }
                return bytes;
            }
        }

        public static int ReadInt32(BinaryReader reader, ByteOrder Endian)
        {
                return BitConverter.ToInt32(ReadBytes(reader, 4, Endian),0);
        
        }
        public static int ReadInt16(BinaryReader reader, ByteOrder Endian)
        {
                return BitConverter.ToInt16(ReadBytes(reader, 2, Endian),0);
            
        }
        public static void WriteInt32(int inInt, BinaryWriter writer, ByteOrder Endian)
        {
            
            writer.Write(BitConverter.ToInt32(ReadBytes(BitConverter.GetBytes(inInt), 4, Endian), 0));

        }
        public static void WriteInt16(short inInt, BinaryWriter writer, ByteOrder Endian)
        {

            writer.Write(ReadBytes(BitConverter.GetBytes((short)inInt), 2, Endian));
            
        }
        public static byte[] Int32ToBtyes(int inInt, ByteOrder Endian)
        {
            return ReadBytes(BitConverter.GetBytes(inInt), 4, Endian);
        }
        public static byte[] Int16ToBtyes(int inInt, ByteOrder Endian)
        {
            return ReadBytes(BitConverter.GetBytes((short)inInt), 2, Endian);
        }
        public static void Write(BinaryWriter writer, int outInt, ByteOrder Endian)
        {
            writer.Write(Int32ToBtyes(outInt, Endian));
        }
        public static void Write(BinaryWriter writer, byte[] outBytes, ByteOrder Endian)
        {
            writer.Write(outBytes);
        }
        public static void Write16(BinaryWriter writer, int outInt, ByteOrder Endian)
        {
            writer.Write(Int16ToBtyes(outInt, Endian));
        }

        //Checks if the byte/array is null.
        public static bool isNotNull(byte[] array)
        {
            try
            {
                if (array.Length > 0)
                    return true;
                else return false;
            }
            catch { return false; }
        }
        public static bool isNotNull(byte inByte)
        {
            try
            {
                if (inByte != null)
                    return true;
                else return false;
            }
            catch { return false; }
        }

        public bool WSGEndian;
        public ByteOrder EndianWSG;
        public BinaryReader WSGReader;
        
        
        public string Platform;
        public string OpenedWSG;
        //General Info
        public string MagicHeader; 
        public int VersionNumber;
        public string PLYR;
        public int RevisionNumber;
        public string Class;
        public int Level;
        public int Experience;
        public int SkillPoints;
        public int unknown1;
        public int Cash;
        public int FinishedPlaythrough1;
        public int NumberOfSkills;
        
        //Skill Arrays
        public string[] SkillNames;
        public int[] LevelOfSkills;
        public int[] ExpOfSkills;
        public int[] InUse;


        //Unknown, obviously.
        public int unknown2;
        public int unknown3;
        public int unknown4;
        public int unknown5;
        public int NumberOfPools;

        //Ammo Pool Arrays
        public string[] ResourcePools;
        public string[] AmmoPools;
        public float[] RemainingPools;
        public int[] PoolLevels;

        public int NumberOfItems;

        //Item Arrays
        public List<List<string>> ItemStrings = new List<List<string>>();
        public List<List<int>> ItemValues = new List<List<int>>();

        //Backpack Info
        public int BackpackSize;
        public int EquipSlots;
        public int NumberOfWeapons;

        //Weapon Arrays
        public List<List<string>> WeaponStrings = new List<List<string>>();
        public List<List<int>> WeaponValues = new List<List<int>>();


        //Look, stuff!
        public byte[] ChallengeData;
        public int TotalLocations;
        public string[] LocationStrings;
        public string CurrentLocation;
        public int[] SaveInfo1to5 = new int[5];
        public int SaveNumber;
        public int[] SaveInfo7to10 = new int[4];
        public int SometimesNull;
        
        //Quest Arrays/Info
        public string CurrentQuest = "None";
        public int TotalPT1Quests;
        public string[] PT1Strings;
        public int[,] PT1Values;
        public int UnknownPT1Value;
        public string[,] PT1Subfolders;
        public bool HasOddNullsInQuest;
        public int TotalPT2Quests;
        public string SecondaryQuest = "None";
        public string[] PT2Strings;
        public int[,] PT2Values;
        public string[,] PT2Subfolders;
        public int UnknownQuestValue;
        
        
        //More unknowns and color info.
        public int unknownSaveValue;
        public string LastPlayedDate;
        public string CharacterName;
        public byte[] Color1;
        public byte[] Color2;
        public byte[] Color3;
        public int unknown6;
        public int unknown7;
        public int unknown8;
        public int unknown9;
        public int unknown10;
        //Echo Info
        public int SometimesNullsRevenge = 0;
        public bool wasNull = false;
        public int NumberOfEchos;
        public string[] EchoStrings;
        public int[,] EchoValues;
        public int NumberOfEchosPT2;
        public string[] EchoStringsPT2;
        public int[,] EchoValuesPT2;
        public int unknown11;
        //public byte[] MiscData;

        public class DLC_Data
        {
            public int DLC_Size;
            public int DLC_Header;
            public int DLC_Unknown;
            public byte DLC_Unknown2;
            public int BankSize;
            public byte[] Bank;
            public byte SecondaryPackEnabled;
            public int BackpackSize;
            public int TotalItems;
            public int TotalWeapons;
            
            
            public List<List<string>> ItemParts = new List<List<string>>();
            public List<int> ItemQuantity = new List<int>();
            public List<int> ItemLevel = new List<int>();
            public List<int> ItemQuality = new List<int>();
            public List<int> ItemEquipped = new List<int>();


            public List<List<string>> WeaponParts = new List<List<string>>();
            public List<int> WeaponAmmo = new List<int>();
            public List<int> WeaponLevel = new List<int>();
            public List<int> WeaponQuality = new List<int>();
            public List<int> WeaponEquippedSlot = new List<int>();

            
            
            }


         public DLC_Data DLC = new DLC_Data();

        //Xbox 360 only
        public long ProfileID;
        public byte[] DeviceID;
        public byte[] CON_Image;
        public string Title_Display;
        public string Title_Package;
        public uint TitleID = 1414793191;



        ///<summary>Reports back the expected platform this WSG was created on.</summary>
        public string WSGType(byte[] InputWSG)
        {
            DJsIO SaveReader = new DJsIO(InputWSG, true);
            string Magic = SaveReader.ReadString(3, StringForm.ASCII, StringRead.Defined);
            




            if (Magic == "CON")
            {
                SaveReader.ReadBytes(53245);
                string SecondaryMagic = SaveReader.ReadString(3, StringForm.ASCII, StringRead.Defined);
                SaveReader.Close();

                if (SecondaryMagic == "WSG")
                {


                    return "X360";
                }
                else
                {

                    return "Not WSG";
                }
            }
            else if (Magic == "WSG")
            {
                int WSG_Version = SaveReader.ReadInt32();
                SaveReader.Close();
                if (WSG_Version == 2)
                {

                    return "PS3";
                }
                else if (WSG_Version == 33554432)
                {

                    return "PC";
                }
                else
                {

                    return "unknown";
                }
            }
            else
            {
                SaveReader.Close();
                return "Not WSG";
            }
            
        }
        ///<summary>Extracts a WSG from a CON.</summary>
        public byte[] WSGExtract(string InputX360File)
        {
            try
            {
                STFSPackage CON = new STFSPackage(new DJsIO(InputX360File, DJFileMode.Open, true), new X360.Other.LogRecord());
                DJsIO Extract = new DJsIO(true);
                //CON.FileDirectory[0].Extract(Extract);
                ProfileID = CON.Header.ProfileID;
                DeviceID = CON.Header.DeviceID;
                //DJsIO Save = new DJsIO("C:\\temp.sav", DJFileMode.Create, true);
                //Save.Write(Extract.ReadStream());
                //Save.Close();
                //byte[] nom = CON.GetFile("SaveGame.sav").GetEntryData(); 
                return CON.GetFile("SaveGame.sav").GetTempIO(true).ReadStream();
            }
            catch
            {
                try
                {
                    DJsIO Manual = new DJsIO(InputX360File, DJFileMode.Open, true);
                    Manual.ReadBytes(881);
                    ProfileID = Manual.ReadInt64();
                    Manual.ReadBytes(132);
                    DeviceID = Manual.ReadBytes(20);
                    Manual.ReadBytes(48163);
                    int size = Manual.ReadInt32();
                    Manual.ReadBytes(4040);
                    return Manual.ReadBytes(size);
                }
                catch { return null; }
            }
        }
        ///<summary>Converts a file to a byte[] for use with ReadWSG</summary>
        public void OpenWSG(string InputFile)
        {
            //try
            //{
                DJsIO Reader = new DJsIO(InputFile, DJFileMode.Open, true);
                Platform = WSGType(Reader.ReadStream());
                
                if (Platform == "X360")
                {
                    Reader.Close();
                    ReadWSG(WSGExtract(InputFile), true, ByteOrder.BigEndian);

                }
                else if (Platform == "PS3")
                {
                    ReadWSG(Reader.ReadStream(), true, ByteOrder.BigEndian);
                    Reader.Close();
                }
                else if (Platform == "PC")
                {
                    ReadWSG(Reader.ReadStream(), false, ByteOrder.LittleEndian);
                    Reader.Close();
                }
                OpenedWSG = InputFile;
            //}

            //catch { }
            
        }
        ///<summary>Reads and decompiles the contents of a WSG</summary>
        private void ReadWSG(byte[] FileArray, bool isBigEndian, ByteOrder EndianFormat)
        {
            
            //TestStream.Write(FileArray, 0, FileArray.Length);
            BinaryReader TestReader = new BinaryReader(new MemoryStream(FileArray));
            
            DJsIO SaveReader = new DJsIO(FileArray, isBigEndian);
            WSGEndian = isBigEndian;
            EndianWSG = EndianFormat;
            MagicHeader = new string(TestReader.ReadChars(3));
            VersionNumber = ReadInt32(TestReader,EndianFormat);
            PLYR = new string(TestReader.ReadChars(4));
            RevisionNumber = ReadInt32(TestReader,EndianFormat);
            Class = ReadString(TestReader);
            Level = ReadInt32(TestReader,EndianFormat);
            Experience = ReadInt32(TestReader,EndianFormat);
            SkillPoints = ReadInt32(TestReader,EndianFormat);
            unknown1 = ReadInt32(TestReader,EndianFormat);
            Cash = ReadInt32(TestReader,EndianFormat);
            FinishedPlaythrough1 = ReadInt32(TestReader,EndianFormat);
            NumberOfSkills = ReadInt32(TestReader,EndianFormat);
            Skills(TestReader, NumberOfSkills);
            unknown2 = ReadInt32(TestReader,EndianFormat);
            unknown3 = ReadInt32(TestReader,EndianFormat);
            unknown4 = ReadInt32(TestReader,EndianFormat);
            unknown5 = ReadInt32(TestReader,EndianFormat);
            NumberOfPools = ReadInt32(TestReader,EndianFormat);
            Ammo(TestReader, NumberOfPools);
            NumberOfItems = ReadInt32(TestReader,EndianFormat);
            Items(TestReader, NumberOfItems);
            BackpackSize = ReadInt32(TestReader,EndianFormat);
            EquipSlots = ReadInt32(TestReader,EndianFormat);
            NumberOfWeapons = ReadInt32(TestReader,EndianFormat);
            Weapons(TestReader, NumberOfWeapons);
            ChallengeData = TestReader.ReadBytes(1330);
            TotalLocations = ReadInt32(TestReader,EndianFormat);
            Locations(TestReader, TotalLocations);
            CurrentLocation = ReadString(TestReader);
            SaveInfo1to5[0] = ReadInt32(TestReader,EndianFormat);
            SaveInfo1to5[1] = ReadInt32(TestReader,EndianFormat);
            SaveInfo1to5[2] = ReadInt32(TestReader,EndianFormat);
            SaveInfo1to5[3] = ReadInt32(TestReader,EndianFormat);
            SaveInfo1to5[4] = ReadInt32(TestReader,EndianFormat);
            SaveNumber = ReadInt32(TestReader,EndianFormat);
            SaveInfo7to10[0] = ReadInt32(TestReader,EndianFormat);
            SaveInfo7to10[1] = ReadInt32(TestReader,EndianFormat);
            SaveInfo7to10[2] = ReadInt32(TestReader,EndianFormat);
            SaveInfo7to10[3] = ReadInt32(TestReader,EndianFormat);
            // Story time! Once upon a time Xander found this pesky bit of the save that sometimes had 
            // a null int32 followed by the number of PT1 Quests (as opposed to it being after the 
            // in-progress quest). Well it turned out that it only showed up when PT2 was unlocked. 
            // Well Xander quickly found that tons of people unlocked PT2 through WillowTree 
            // and it was no longer a good indicator of which format it should be. So Xander decided 
            // to check if the long was null, even though it was the lazy way out. Now Xander 
            // waits lazily for the code that checks the format to improve itself. The end. 
            //  Cool story, bro?
            SometimesNull = ReadInt32(TestReader,EndianFormat); //Don't write unless 0
            if (SometimesNull == 0)
            {
                TotalPT1Quests = ReadInt32(TestReader,EndianFormat);
                
            }
            else
            {
                CurrentQuest = new string(TestReader.ReadChars(SometimesNull - 1));
                TestReader.ReadByte();
                TotalPT1Quests = ReadInt32(TestReader,EndianFormat);
            }
            PT1Quests(TestReader, TotalPT1Quests);
            UnknownPT1Value = ReadInt32(TestReader,EndianFormat);
            int tempCheckValue = ReadInt32(TestReader,EndianFormat);
            if (tempCheckValue == 0) //Checks for the odd extra null bytes.
            {
                HasOddNullsInQuest = true;
                TotalPT2Quests = ReadInt32(TestReader,EndianFormat);
            }
            else
            {
                HasOddNullsInQuest = false;

                SecondaryQuest = new string(TestReader.ReadChars(tempCheckValue - 1)); //Could be current PT2 quest or last online quest, don't know.
                TestReader.ReadByte();
                TotalPT2Quests = ReadInt32(TestReader,EndianFormat);
            }
                PT2Quests(TestReader, TotalPT2Quests); //Saves playthrough 2 quests as arrays of strings and ints (15 values, -1 for none if not used)
            UnknownQuestValue = ReadInt32(TestReader,EndianFormat); //Is either 2 or 0
            ReadString(TestReader); //Z0_Missions.Missions.M_IntroStateSaver
            ReadInt32(TestReader,EndianFormat); //1
            ReadString(TestReader); //Z0_Missions.Missions.M_IntroStateSaver
            ReadInt32(TestReader,EndianFormat); //2
            ReadInt32(TestReader,EndianFormat); //0
            ReadInt32(TestReader,EndianFormat); //0
            ReadInt32(TestReader,EndianFormat); //0
            unknownSaveValue = ReadInt32(TestReader,EndianFormat);
            LastPlayedDate = ReadString(TestReader);
            CharacterName = ReadString(TestReader);
            Color1 = TestReader.ReadBytes(4); //ABGR Big (X360, PS3), RGBA Little (PC)
            Color2 = TestReader.ReadBytes(4); //ABGR Big (X360, PS3), RGBA Little (PC)
            Color3 = TestReader.ReadBytes(4); //ABGR Big (X360, PS3), RGBA Little (PC)
            unknown6 = ReadInt32(TestReader,EndianFormat);
            unknown7 = ReadInt32(TestReader,EndianFormat);
            unknown8 = ReadInt32(TestReader,EndianFormat);
            unknown9 = ReadInt32(TestReader,EndianFormat);
            unknown10 = ReadInt32(TestReader,EndianFormat);
            NumberOfEchos = ReadInt32(TestReader,EndianFormat);
            if (NumberOfEchos == 0)
            {
                SometimesNullsRevenge = NumberOfEchos;
                NumberOfEchos = ReadInt32(TestReader, EndianFormat);
                wasNull = true;
            }
            Echos(TestReader, NumberOfEchos); //One string, two identical int32s
            unknown11 = ReadInt32(TestReader,EndianFormat);
            if ((TestReader.BaseStream.Length - TestReader.BaseStream.Position) > 0)
            NumberOfEchosPT2 = ReadInt32(TestReader,EndianFormat);
            if (NumberOfEchosPT2 > 0 && TotalPT2Quests > 1 && NumberOfEchosPT2 < 300) //I doubt there will ever be 300+ echo logs...
            {
                EchosPT2(TestReader, NumberOfEchosPT2);
            }
            
            if ((TestReader.BaseStream.Length - TestReader.BaseStream.Position) > 0)
                    DLC.DLC_Size = ReadInt32(TestReader, EndianWSG);
            if ((TestReader.BaseStream.Length - TestReader.BaseStream.Position) > 0)
            {
                DLC.DLC_Header = ReadInt32(TestReader, EndianWSG);
                DLC.DLC_Unknown = ReadInt32(TestReader, EndianWSG);
                DLC.DLC_Unknown2 = TestReader.ReadByte();
                DLC.BankSize = ReadInt32(TestReader, EndianWSG);
                List<byte> tempBytes = new List<byte>();
                tempBytes.Add(TestReader.ReadByte());
                //int p = (int)TestReader.BaseStream.Position;
                for (int i = 1; (TestReader.BaseStream.Length - TestReader.BaseStream.Position) > 0; i++)
                {

                    if (tempBytes.Count > 10 && tempBytes[i - 1] == 1 && tempBytes[i - 2] == 169 && tempBytes[i - 3] == 75 && tempBytes[i - 4] == 35)
                    {
                        DLC.BackpackSize = ReadInt32(TestReader, EndianWSG);
                        if (DLC.BackpackSize > 0)
                        {
                            DLC.SecondaryPackEnabled = TestReader.ReadByte();
                            //DLC.TotalItems = ReadInt32(TestReader, EndianWSG);
                            GetDLCItems(TestReader);
                            //DLC.TotalWeapons = ReadInt32(TestReader, EndianWSG);
                            GetDLCWeapons(TestReader);
                        }
                        TestReader.ReadBytes((int)TestReader.BaseStream.Length - (int)TestReader.BaseStream.Position);
                    }
                    else if (tempBytes.Count > 10 && tempBytes[i - 1] == 35 && tempBytes[i - 2] == 75 && tempBytes[i - 3] == 169 && tempBytes[i - 4] == 1)
                    {
                        DLC.BackpackSize = ReadInt32(TestReader, EndianWSG);
                        if (DLC.BackpackSize > 0)
                        {
                            
                            DLC.SecondaryPackEnabled = TestReader.ReadByte();
                            //DLC.TotalItems = ReadInt32(TestReader, EndianWSG);
                            GetDLCItems(TestReader);

                            GetDLCWeapons(TestReader);
                        }
                        if(((int)TestReader.BaseStream.Length - (int)TestReader.BaseStream.Position) > 0)
                        TestReader.ReadBytes((int)TestReader.BaseStream.Length - (int)TestReader.BaseStream.Position);
                    }
                    else
                    {
                        tempBytes.Add(TestReader.ReadByte());
                    }
                }
                DLC.Bank = (byte[])tempBytes.ToArray();
                //MiscData = new byte[TestReader.BaseStream.Length - TestReader.BaseStream.Position];
                //MiscData = TestReader.ReadBytes((int)(TestReader.BaseStream.Length - TestReader.BaseStream.Position));
            }
            TestReader.Close();
            

        }
        private void Skills(BinaryReader DJsIO, int NumOfSkills)
        {


            string[] TempSkillNames = new string[NumOfSkills];
            int[] TempLevelOfSkills = new int[NumOfSkills];
            int[] TempExpOfSkills = new int[NumOfSkills];
            int[] TempInUse = new int[NumOfSkills];

            for (int Progress = 0; Progress < NumOfSkills; Progress++)
            {
                TempSkillNames[Progress] = ReadString(DJsIO);
                TempLevelOfSkills[Progress] = ReadInt32(DJsIO,EndianWSG);
                TempExpOfSkills[Progress] = ReadInt32(DJsIO,EndianWSG);
                TempInUse[Progress] = ReadInt32(DJsIO,EndianWSG);
            }
            SkillNames = TempSkillNames;
            LevelOfSkills = TempLevelOfSkills;
            ExpOfSkills = TempExpOfSkills;
            InUse = TempInUse;
        } //Ignore all of the "DJsIO", I was just too lazy to rename them after I removed most of my dependence on the X360 DLL.
        private void Ammo(BinaryReader DJsIO, int NumOfPools)
        {


            string[] TempResourcePools = new string[NumOfPools];
            string[] TempAmmoPools = new string[NumOfPools];
            float[] TempRemainingPools = new float[NumOfPools];
            int[] TempPoolLevels = new int[NumOfPools];

            for (int Progress = 0; Progress < NumOfPools; Progress++)
            {
                TempResourcePools[Progress] = ReadString(DJsIO);
                TempAmmoPools[Progress] = ReadString(DJsIO);
                TempRemainingPools[Progress] = DJsIO.ReadSingle();
                TempPoolLevels[Progress] = ReadInt32(DJsIO,EndianWSG);
            }
            ResourcePools = TempResourcePools;
            AmmoPools = TempAmmoPools;
            RemainingPools = TempRemainingPools;
            PoolLevels = TempPoolLevels;
        }
        private void Items(BinaryReader DJsIO, int NumOfItems)
        {
            for (int Progress = 0; Progress < NumOfItems; Progress++)
            {
                ItemStrings.Add(new List<string>());
                ItemValues.Add(new List<int>());
                for (int TotalStrings = 0; TotalStrings < 9; TotalStrings++)
                    ItemStrings[Progress].Add(ReadString(DJsIO));
                int tempLevel;
                    ItemValues[Progress].Add(ReadInt32(DJsIO,EndianWSG));
                    if (EndianWSG == ByteOrder.BigEndian)
                    {
                        tempLevel = ReadInt16(DJsIO, EndianWSG);
                        ItemValues[Progress].Add(ReadInt16(DJsIO, EndianWSG));
                        ItemValues[Progress].Add(ReadInt32(DJsIO, EndianWSG));
                        ItemValues[Progress].Add(tempLevel);
                    }
                    else
                    {
                        ItemValues[Progress].Add(ReadInt16(DJsIO, EndianWSG));
                        tempLevel = ReadInt16(DJsIO, EndianWSG);
                        ItemValues[Progress].Add(ReadInt32(DJsIO, EndianWSG));
                        ItemValues[Progress].Add(tempLevel);
                    }
            }
        }
        private void Weapons(BinaryReader DJsIO, int NumOfWeapons)
        {


            for (int Progress = 0; Progress < NumOfWeapons; Progress++)
            {
                WeaponStrings.Add(new List<string>());
                WeaponValues.Add(new List<int>());
                int tempLevel;
                for (int TotalStrings = 0; TotalStrings < 14; TotalStrings++)
                    WeaponStrings[Progress].Add(ReadString(DJsIO));
                //for (int TotalValues = 0; TotalValues < 3; TotalValues++)
                    WeaponValues[Progress].Add(ReadInt32(DJsIO,EndianWSG));
                 if (EndianWSG == ByteOrder.BigEndian)
                {
                    tempLevel = ReadInt16(DJsIO, EndianWSG);
                    WeaponValues[Progress].Add(ReadInt16(DJsIO, EndianWSG));
                    WeaponValues[Progress].Add(ReadInt32(DJsIO, EndianWSG));
                    WeaponValues[Progress].Add(tempLevel);
                }
                else
                {
                    WeaponValues[Progress].Add(ReadInt16(DJsIO, EndianWSG));
                    tempLevel = ReadInt16(DJsIO, EndianWSG);
                    WeaponValues[Progress].Add(ReadInt32(DJsIO, EndianWSG));
                    WeaponValues[Progress].Add(tempLevel);
                }
            }

        }
        private void PT1Quests(BinaryReader DJsIO, int NumOfQuests)
        {


            string[] TempQuestStrings = new string[NumOfQuests];
            int[,] TempQuestValues = new int[NumOfQuests, 9];
            string[,] TempQuestSubfolders = new string[NumOfQuests,5];
            
            for (int Progress = 0; Progress < NumOfQuests; Progress++)
            {

                TempQuestStrings[Progress] = ReadString(DJsIO);
                
                

                for (int TotalValues = 0; TotalValues < 4; TotalValues++)
                {
                    TempQuestValues[Progress,TotalValues] = ReadInt32(DJsIO,EndianWSG);
                }





                if (TempQuestValues[Progress, 3] > 0)
                {


                    for (int ExtraValues = 0; ExtraValues < TempQuestValues[Progress, 3]; ExtraValues++)
                    {
                        TempQuestSubfolders[Progress, ExtraValues] = ReadString(DJsIO);
                        TempQuestValues[Progress, ExtraValues + 4] = ReadInt32(DJsIO,EndianWSG);

                    }
                }
                    for (int FillGaps = TempQuestValues[Progress, 3] + 4; FillGaps < 9; FillGaps++)
                    {
                        TempQuestValues[Progress, FillGaps] = 0;
                    }
                    
                
                }
            if (CurrentQuest == "None") CurrentQuest = TempQuestStrings[0];
            PT1Strings = TempQuestStrings;
            PT1Values = TempQuestValues;
            PT1Subfolders = TempQuestSubfolders;
        }
        private void PT2Quests(BinaryReader DJsIO, int NumOfQuests)
        {


            string[] TempQuestStrings = new string[NumOfQuests];
            int[,] TempQuestValues = new int[NumOfQuests, 9];
            string[,] TempQuestSubfolders = new string[NumOfQuests, 5];

            for (int Progress = 0; Progress < NumOfQuests; Progress++)
            {

                TempQuestStrings[Progress] = ReadString(DJsIO);



                for (int TotalValues = 0; TotalValues < 4; TotalValues++)
                {
                    TempQuestValues[Progress, TotalValues] = ReadInt32(DJsIO,EndianWSG);
                }





                if (TempQuestValues[Progress, 3] > 0)
                {


                    for (int ExtraValues = 0; ExtraValues < TempQuestValues[Progress, 3]; ExtraValues++)
                    {
                        TempQuestSubfolders[Progress, ExtraValues] = ReadString(DJsIO);
                        TempQuestValues[Progress, ExtraValues + 4] = ReadInt32(DJsIO,EndianWSG);

                    }
                    for (int FillGaps = TempQuestValues[Progress, 3] + 4; FillGaps < 9; FillGaps++)
                    {
                        TempQuestValues[Progress, FillGaps] = 0;
                    }

                }
            }

            if (SecondaryQuest == "None") SecondaryQuest = TempQuestStrings[0];
            PT2Strings = TempQuestStrings;
            PT2Values = TempQuestValues;
            PT2Subfolders = TempQuestSubfolders;
        }
        private void Locations(BinaryReader DJsIO, int NumOfLocations)
        {


            string[] TempLocationStrings = new string[NumOfLocations];


            for (int Progress = 0; Progress < NumOfLocations; Progress++)
            {
                TempLocationStrings[Progress] = ReadString(DJsIO);

            }
            LocationStrings = TempLocationStrings;

        }
        private void Echos(BinaryReader DJsIO, int NumOfEchos)
        {


            string[] TempEchoStrings = new string[NumOfEchos];
            int[,] TempEchoValues = new int[NumOfEchos, 2];

            for (int Progress = 0; Progress < NumOfEchos; Progress++)
            {

                TempEchoStrings[Progress] = ReadString(DJsIO);



                for (int TotalValues = 0; TotalValues < 2; TotalValues++)
                {
                    TempEchoValues[Progress, TotalValues] = ReadInt32(DJsIO,EndianWSG);
                }


            }
                EchoStrings = TempEchoStrings;
                EchoValues = TempEchoValues;
            
        }
        private void EchosPT2(BinaryReader DJsIO, int NumOfEchos)
        {


            string[] TempEchoStrings = new string[NumOfEchos];
            int[,] TempEchoValues = new int[NumOfEchos, 2];

            for (int Progress = 0; Progress < NumOfEchos; Progress++)
            {

                TempEchoStrings[Progress] = ReadString(DJsIO);



                for (int TotalValues = 0; TotalValues < 2; TotalValues++)
                {
                    TempEchoValues[Progress, TotalValues] = ReadInt32(DJsIO,EndianWSG);
                }


            }
            EchoStringsPT2 = TempEchoStrings;
            EchoValuesPT2 = TempEchoValues;

        }
        private void GetDLCItems(BinaryReader DJsIO)
        {
            try
            {
                DLC.TotalItems = ReadInt32(DJsIO, EndianWSG);
                int tempItems = NumberOfItems;
                if (DLC.TotalItems > 0)
                for (int Progress = tempItems; Progress < DLC.TotalItems + tempItems; Progress++)
                {
                    ItemStrings.Add(new List<string>());
                    ItemValues.Add(new List<int>());
                    int tempLevel;
                    for (int TotalStrings = 0; TotalStrings < 9; TotalStrings++)
                        ItemStrings[Progress].Add(ReadString(DJsIO));
                    //for (int TotalValues = 0; TotalValues < 3; TotalValues++)
                    ItemValues[Progress].Add(ReadInt32(DJsIO, EndianWSG));
                    if (EndianWSG == ByteOrder.BigEndian)
                    {
                        tempLevel = ReadInt16(DJsIO, EndianWSG);
                        ItemValues[Progress].Add(ReadInt16(DJsIO, EndianWSG));
                        ItemValues[Progress].Add(ReadInt32(DJsIO, EndianWSG));
                        ItemValues[Progress].Add(tempLevel);
                    }
                    else
                    {
                        ItemValues[Progress].Add(ReadInt16(DJsIO, EndianWSG));
                        tempLevel = ReadInt16(DJsIO, EndianWSG);
                        ItemValues[Progress].Add(ReadInt32(DJsIO, EndianWSG));
                        ItemValues[Progress].Add(tempLevel);
                    }
                    NumberOfItems++;
                }
            }
            catch { 
            ItemStrings.RemoveRange(NumberOfItems,ItemStrings.Count - NumberOfItems);
            ItemValues.RemoveRange(NumberOfItems, ItemStrings.Count - NumberOfItems);
            }
        }
        private void GetDLCWeapons(BinaryReader DJsIO)

        {
            try
            {
                DLC.TotalWeapons = ReadInt32(DJsIO, EndianWSG);
                int tempWeapons = NumberOfWeapons;
                if (DLC.TotalWeapons > 0)
                for (int Progress = tempWeapons; Progress < DLC.TotalWeapons + tempWeapons; Progress++)
                {
                    WeaponStrings.Add(new List<string>());
                    WeaponValues.Add(new List<int>());
                    int tempLevel;
                    for (int TotalStrings = 0; TotalStrings < 14; TotalStrings++)
                        WeaponStrings[Progress].Add(ReadString(DJsIO));
                    //for (int TotalValues = 0; TotalValues < 3; TotalValues++)
                    WeaponValues[Progress].Add(ReadInt32(DJsIO, EndianWSG));
                    if (EndianWSG == ByteOrder.BigEndian)
                    {
                        tempLevel = ReadInt16(DJsIO, EndianWSG);
                        WeaponValues[Progress].Add(ReadInt16(DJsIO, EndianWSG));
                        WeaponValues[Progress].Add(ReadInt32(DJsIO, EndianWSG));
                        WeaponValues[Progress].Add(tempLevel);
                    }
                    else
                    {
                        WeaponValues[Progress].Add(ReadInt16(DJsIO, EndianWSG));
                        tempLevel = ReadInt16(DJsIO, EndianWSG);
                        WeaponValues[Progress].Add(ReadInt32(DJsIO, EndianWSG));
                        WeaponValues[Progress].Add(tempLevel);
                    }
                    NumberOfWeapons++;
                }
            }
            catch {
                WeaponStrings.RemoveRange(NumberOfWeapons, WeaponStrings.Count - NumberOfWeapons);
                WeaponValues.RemoveRange(NumberOfWeapons, WeaponStrings.Count - NumberOfWeapons);
            }
            
        }
        ///<summary>OUTDATED. DO NOT USE.</summary>
        private void OLDGetDLCWeapons(BinaryReader DJsIO)
        {
            for (int Progress = 0; Progress < DLC.TotalWeapons; Progress++)
            {
                List<string> tempParts = new List<string>();
                for (int i = 0; i < 14; i++)
                {
                    tempParts.Add(ReadString(DJsIO));
                }
                DLC.WeaponParts.Add(tempParts);
                DLC.WeaponAmmo.Add(ReadInt32(DJsIO, EndianWSG));
                if (EndianWSG == ByteOrder.BigEndian)
                {
                    DLC.WeaponLevel.Add(ReadInt16(DJsIO, EndianWSG));
                    DLC.WeaponQuality.Add(ReadInt16(DJsIO, EndianWSG));
                }
                else
                {
                    DLC.WeaponQuality.Add(ReadInt16(DJsIO, EndianWSG));
                    DLC.WeaponLevel.Add(ReadInt16(DJsIO, EndianWSG));
                }
                DLC.WeaponEquippedSlot.Add(ReadInt32(DJsIO, EndianWSG));
            }
        }

        
        ///<summary>Reads a string in the format used by the WSG format</summary>
        public string ReadString(BinaryReader DJsIO)
        {
            int TempLengthValue = ReadInt32(DJsIO,EndianWSG);
            
            string TempOutput = new string(DJsIO.ReadChars(TempLengthValue - 1));
            DJsIO.ReadByte();
            return TempOutput;
            

        }
        ///<summary>Writes a string in the format used by the WSG format</summary>
        public byte[] WriteString(string InputString, ByteOrder Endian)
        {

            byte[] outBytes = new byte[new System.Text.ASCIIEncoding().GetBytes(InputString + "\0").Length + Int32ToBtyes(InputString.Length + 1, Endian).Length];
            System.Buffer.BlockCopy(Int32ToBtyes(InputString.Length + 1, Endian), 0 ,outBytes,0, Int32ToBtyes(InputString.Length + 1, Endian).Length);
            System.Buffer.BlockCopy(new System.Text.ASCIIEncoding().GetBytes(InputString + "\0"), 0, outBytes, Int32ToBtyes(InputString.Length + 1, Endian).Length, new System.Text.ASCIIEncoding().GetBytes(InputString + "\0").Length);
            return outBytes;
            //DJsIO tempByte = new DJsIO(Endian);
            
            //tempByte.Write((InputString.Length+1));
            //tempByte.WriteASCII(InputString);
            //tempByte.Write((byte)0);
            //return tempByte.ReadStream();

        }

        ///<summary>Save the current data to a WSG as a byte[]</summary>
        public byte[] SaveWSG()
        {
            MemoryStream OutStream = new MemoryStream();
            //DJsIO Out = new DJsIO(Endian);
            BinaryWriter Out = new BinaryWriter(OutStream);
            
            Write(Out, new System.Text.ASCIIEncoding().GetBytes(MagicHeader), EndianWSG);
            Write(Out, VersionNumber, EndianWSG);
            Write(Out, new System.Text.ASCIIEncoding().GetBytes(PLYR), EndianWSG);
            Write(Out, RevisionNumber, EndianWSG);
            Write(Out, WriteString(Class, EndianWSG), EndianWSG);
            Write(Out, Level, EndianWSG);
            Write(Out, Experience, EndianWSG);
            Write(Out, SkillPoints, EndianWSG);
            Write(Out, unknown1, EndianWSG);
            Write(Out, Cash, EndianWSG);
            Write(Out, FinishedPlaythrough1, EndianWSG);
            Write(Out, NumberOfSkills, EndianWSG);

            for (int Progress = 0; Progress < NumberOfSkills; Progress++) //Write Skills
            {
                Write(Out, WriteString(SkillNames[Progress], EndianWSG), EndianWSG);
                Write(Out, LevelOfSkills[Progress], EndianWSG);
                Write(Out, ExpOfSkills[Progress], EndianWSG);
                Write(Out, InUse[Progress], EndianWSG);
            }

            Write(Out, unknown2, EndianWSG);
            Write(Out, unknown3, EndianWSG);
            Write(Out, unknown4, EndianWSG);
            Write(Out, unknown5, EndianWSG);
            Write(Out, NumberOfPools, EndianWSG);

            for (int Progress = 0; Progress < NumberOfPools; Progress++) //Write Ammo Pools
            {
                Write(Out, WriteString(ResourcePools[Progress], EndianWSG), EndianWSG);
                Write(Out, WriteString(AmmoPools[Progress], EndianWSG), EndianWSG);
                Write(Out, BitConverter.GetBytes((float)RemainingPools[Progress]), EndianWSG);
                Write(Out, PoolLevels[Progress], EndianWSG);
            }

            Write(Out, NumberOfItems, EndianWSG);
            
            for (int Progress = 0; Progress < ItemStrings.Count; Progress++) //Write Items
            {
                for (int TotalStrings = 0; TotalStrings < 9; TotalStrings++)
                {
                    Write(Out, WriteString(ItemStrings[Progress][TotalStrings], EndianWSG), EndianWSG);
                }
                Write(Out, ItemValues[Progress][0], EndianWSG);
                //Write(Out, ItemValues[Progress][1], EndianWSG);


                    if (EndianWSG == ByteOrder.BigEndian)
                    {
                        Write16(Out, ItemValues[Progress][3], EndianWSG);
                        Write16(Out, ItemValues[Progress][1], EndianWSG);
                    }
                    else
                    {

                        Write16(Out, ItemValues[Progress][1], EndianWSG);
                        Write16(Out, ItemValues[Progress][3], EndianWSG);

                    }
                    Write(Out, ItemValues[Progress][2], EndianWSG);
            }




            Write(Out, BackpackSize, EndianWSG);
            Write(Out, EquipSlots, EndianWSG);
            Write(Out, NumberOfWeapons, EndianWSG);
            

            for (int Progress = 0; Progress < WeaponStrings.Count; Progress++) //Write Weapons
            {
                for (int TotalStrings = 0; TotalStrings < 14; TotalStrings++)
                {
                    Write(Out, WriteString(WeaponStrings[Progress][TotalStrings], EndianWSG), EndianWSG);
                }
                Write(Out, WeaponValues[Progress][0], EndianWSG);
                if (EndianWSG == ByteOrder.BigEndian)
                {
                    Write16(Out, WeaponValues[Progress][3], EndianWSG);
                    Write16(Out, WeaponValues[Progress][1], EndianWSG);
                }
                else
                {

                    Write16(Out, WeaponValues[Progress][1], EndianWSG);
                    Write16(Out, WeaponValues[Progress][3], EndianWSG);

                }
                Write(Out, WeaponValues[Progress][2], EndianWSG);
            }

            Write(Out, ChallengeData, EndianWSG);
            Write(Out, TotalLocations, EndianWSG);

            for (int Progress = 0; Progress < TotalLocations; Progress++) //Write Locations
            {
                Write(Out, WriteString(LocationStrings[Progress], EndianWSG), EndianWSG);
            }

            Write(Out, WriteString(CurrentLocation, EndianWSG), EndianWSG);
            Write(Out, SaveInfo1to5[0], EndianWSG);
            Write(Out, SaveInfo1to5[1], EndianWSG);
            Write(Out, SaveInfo1to5[2], EndianWSG);
            Write(Out, SaveInfo1to5[3], EndianWSG);
            Write(Out, SaveInfo1to5[4], EndianWSG);
            Write(Out, SaveNumber, EndianWSG);
            Write(Out, SaveInfo7to10[0], EndianWSG);
            Write(Out, SaveInfo7to10[1], EndianWSG);
            Write(Out, SaveInfo7to10[2], EndianWSG);
            Write(Out, SaveInfo7to10[3], EndianWSG);
            if (SometimesNull == 0) //Check if Total Quests is written before or after Current Quest
            {
                Write(Out, 0, EndianWSG);
                Write(Out, TotalPT1Quests, EndianWSG);

            }
            else
            {
                Write(Out, WriteString(CurrentQuest, EndianWSG), EndianWSG);
                Write(Out, TotalPT1Quests, EndianWSG);
                
            }

            for (int Progress = 0; Progress < TotalPT1Quests; Progress++)  //Write Playthrough 1 Quests
            {
                Write(Out, WriteString(PT1Strings[Progress], EndianWSG), EndianWSG);
                
                for (int TotalValues = 0; TotalValues < 4; TotalValues++)
                    Write(Out, PT1Values[Progress, TotalValues], EndianWSG);
                
                if (PT1Values[Progress, 3] > 0) //Checks for subfolders
                {
                    for (int ExtraValues = 0; ExtraValues < PT1Values[Progress, 3]; ExtraValues++)
                    {
                        Write(Out, WriteString(PT1Subfolders[Progress, ExtraValues], EndianWSG), EndianWSG); //Write Subfolder
                        Write(Out, PT1Values[Progress, ExtraValues + 4], EndianWSG); //Write Subfolder value

                    }
                }
            
            }
            Write(Out, UnknownPT1Value, EndianWSG);
            if (HasOddNullsInQuest == true) //Checks for the odd extra null bytes.
            {
                //Write(Out, 0);
                Write(Out, 0, EndianWSG);
                Write(Out, TotalPT2Quests, EndianWSG);
            }
            else
            {
                Write(Out, WriteString(SecondaryQuest, EndianWSG), EndianWSG);
                Write(Out, TotalPT2Quests, EndianWSG);
            }

            for (int Progress = 0; Progress < TotalPT2Quests; Progress++)  //Write Playthrough 2 Quests
            {
                Write(Out, WriteString(PT2Strings[Progress], EndianWSG), EndianWSG);

                for (int TotalValues = 0; TotalValues < 4; TotalValues++)
                    Write(Out, PT2Values[Progress, TotalValues], EndianWSG);

                if (PT2Values[Progress, 3] > 0) //Checks for subfolders
                {
                    for (int ExtraValues = 0; ExtraValues < PT2Values[Progress, 3]; ExtraValues++)
                    {
                        Write(Out, WriteString(PT2Subfolders[Progress, ExtraValues], EndianWSG), EndianWSG); //Write Subfolder
                        Write(Out, PT2Values[Progress, ExtraValues + 4], EndianWSG); //Write Subfolder value

                    }
                }

            }

            Write(Out, UnknownQuestValue, EndianWSG); //Is either 2 or 0
            Write(Out, WriteString("Z0_Missions.Missions.M_IntroStateSaver", EndianWSG), EndianWSG);
            Write(Out, 1, EndianWSG);
            Write(Out, WriteString("Z0_Missions.Missions.M_IntroStateSaver", EndianWSG), EndianWSG);
            Write(Out, 2, EndianWSG);
            Write(Out, 0, EndianWSG);
            Write(Out, 0, EndianWSG);
            Write(Out, 0, EndianWSG);
            Write(Out, unknownSaveValue, EndianWSG);
            Write(Out, WriteString(LastPlayedDate, EndianWSG), EndianWSG);
            Write(Out, WriteString(CharacterName, EndianWSG), EndianWSG);
            Write(Out, Color1, EndianWSG); //ABGR Big (X360, PS3), RGBA Little (PC)
            Write(Out, Color2, EndianWSG); //ABGR Big (X360, PS3), RGBA Little (PC)
            Write(Out, Color3, EndianWSG); //ABGR Big (X360, PS3), RGBA Little (PC)
            Write(Out, unknown6, EndianWSG);
            Write(Out, unknown7, EndianWSG);
            Write(Out, unknown8, EndianWSG);
            Write(Out, unknown9, EndianWSG);
            Write(Out, unknown10, EndianWSG);
            if (wasNull)
                Write(Out, SometimesNullsRevenge, EndianWSG);
            Write(Out, NumberOfEchos, EndianWSG);

            for (int Progress = 0; Progress < NumberOfEchos; Progress++) //Write Locations
            {
                Write(Out, WriteString(EchoStrings[Progress], EndianWSG), EndianWSG);
                Write(Out, EchoValues[Progress, 0], EndianWSG);
                Write(Out, EchoValues[Progress, 1], EndianWSG);
            }
            Write(Out, unknown11, EndianWSG);
            Write(Out, NumberOfEchosPT2, EndianWSG);
            if (NumberOfEchosPT2 > 0 && TotalPT2Quests > 1 && NumberOfEchosPT2 < 300)
            for (int Progress = 0; Progress < NumberOfEchosPT2; Progress++) //Write Locations
            {
                Write(Out, WriteString(EchoStringsPT2[Progress], EndianWSG), EndianWSG);
                Write(Out, EchoValuesPT2[Progress, 0], EndianWSG);
                Write(Out, EchoValuesPT2[Progress, 1], EndianWSG);
            }
            // //Old format ends after this
            if (false) //Old DLC Backpack writing. This is no longer needed unless you want to re-enable the DLC Backpack.
            {
                MemoryStream tempStream = new MemoryStream();
                BinaryWriter DLC_Backpack = new BinaryWriter(tempStream);
                DLC_Backpack.Write(DLC.SecondaryPackEnabled);
                Write(DLC_Backpack, DLC.ItemParts.Count, EndianWSG);
                for (int Progress = 0; Progress < DLC.ItemParts.Count; Progress++)
                {
                    foreach (string IP in DLC.ItemParts[Progress])
                        Write(DLC_Backpack, WriteString(IP, EndianWSG), EndianWSG);
                    //Write(DLC_Backpack, DLC.ItemQuantity[Progress], EndianWSG);
                    WriteInt32(DLC.ItemQuantity[Progress], DLC_Backpack, EndianWSG);
                    if (EndianWSG == ByteOrder.BigEndian)
                    {
                        Write16(DLC_Backpack, DLC.ItemLevel[Progress], EndianWSG);
                        Write16(DLC_Backpack, DLC.ItemQuality[Progress], EndianWSG);
                        WriteInt32(DLC.ItemEquipped[Progress], DLC_Backpack, EndianWSG);
                    }
                    else
                    {

                        Write16(DLC_Backpack, DLC.ItemQuality[Progress], EndianWSG);
                        Write16(DLC_Backpack, DLC.ItemLevel[Progress], EndianWSG);
                        DLC_Backpack.Write(DLC.ItemEquipped[Progress]);
                    }

                    //

                }
                Write(DLC_Backpack, DLC.WeaponParts.Count, EndianWSG);
                for (int Progress = 0; Progress < DLC.WeaponParts.Count; Progress++)
                {
                    foreach (string WP in DLC.WeaponParts[Progress])
                        Write(DLC_Backpack, WriteString(WP, EndianWSG), EndianWSG);
                    //Write(DLC_Backpack, DLC.WeaponAmmo[Progress], EndianWSG);
                    WriteInt32(DLC.WeaponAmmo[Progress], DLC_Backpack, EndianWSG);
                    if (EndianWSG == ByteOrder.BigEndian)
                    {
                        Write16(DLC_Backpack, DLC.WeaponLevel[Progress], EndianWSG);
                        Write16(DLC_Backpack, DLC.WeaponQuality[Progress], EndianWSG);
                        Write(DLC_Backpack, DLC.WeaponEquippedSlot[Progress], EndianWSG);
                    }
                    else
                    {
                        Write16(DLC_Backpack, DLC.WeaponQuality[Progress], EndianWSG);
                        Write16(DLC_Backpack, DLC.WeaponLevel[Progress], EndianWSG);
                        DLC_Backpack.Write(DLC.WeaponEquippedSlot[Progress]);
                    }
                    //

                }
                Write(Out, (int)DLC.Bank.Length + (int)tempStream.Length + 4, EndianWSG);
                Write(Out, DLC.Bank, EndianWSG);
                Write(Out, (int)tempStream.Length, EndianWSG);
                Out.Write(tempStream.ToArray());
            }
            else if (isNotNull(DLC.Bank) && DLC.SecondaryPackEnabled > 0)
            {
                Write(Out, DLC.Bank.Length + 26, EndianWSG);
                Write(Out, DLC.DLC_Header, EndianWSG);
                Write(Out, DLC.DLC_Unknown, EndianWSG);
                Out.Write(DLC.DLC_Unknown2);
                Write(Out, DLC.BankSize, EndianWSG);
                Write(Out, DLC.Bank, EndianWSG);
                Write(Out, (int)9, EndianWSG);
                Out.Write((byte)1);
                Write(Out, (int)0, EndianWSG);
                Write(Out, (int)0, EndianWSG);
            }
            else if (isNotNull(DLC.Bank))
            {
                Write(Out, DLC.Bank.Length + 13, EndianWSG);
                Write(Out, DLC.DLC_Header, EndianWSG);
                Write(Out, DLC.DLC_Unknown, EndianWSG);
                Out.Write(DLC.DLC_Unknown2);
                Write(Out, DLC.BankSize, EndianWSG);
                Write(Out, DLC.Bank, EndianWSG);
            }
            else Write(Out, 0, EndianWSG);
            Out.Close();
            return OutStream.ToArray();
        }
    }
}
