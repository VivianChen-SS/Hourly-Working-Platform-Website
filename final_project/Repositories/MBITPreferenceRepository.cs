using final_project.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace final_project.Repositories
{
    public class MBITPreferenceRepository
    {
        private static Dictionary<string, bool> MBIT = new Dictionary<string, bool>();
        private static string requiredChatacter = "";
        private static MBITPreferenceViewModels MBITFormAnswers = new MBITPreferenceViewModels();

        //update MBIT & requiredChatacter
        public void updateAllContext(MBITPreferenceViewModels viewModels)
        {
            MBIT.Clear();
            requiredChatacter = ""; //其實可以不用這行，只是為了閱讀思考方便。

            switch (viewModels.attitudes)
            {
                case "I": MBIT.Add("I",true); MBIT.Add("E", false); break;
                case "E": MBIT.Add("I", false); MBIT.Add("E", true); break;
                default: MBIT.Add("I",true); MBIT.Add("E", true); break; //others
            }

            switch (viewModels.functions_SN)
            {
                case "S": MBIT.Add("S", true); MBIT.Add("N", false); break;
                case "N": MBIT.Add("S", false); MBIT.Add("N", true); break;
                default: MBIT.Add("S", true); MBIT.Add("N", true); break; //others
            }

            switch (viewModels.functions_TF)
            {
                case "T": MBIT.Add("T", true); MBIT.Add("F", false); break;
                case "F": MBIT.Add("T", false); MBIT.Add("F", true); break;
                default: MBIT.Add("T", true); MBIT.Add("F", true); break; //others
            }

            switch (viewModels.lifestylePreferences)
            {
                case "J": MBIT.Add("J", true); MBIT.Add("P", false); break;
                case "P": MBIT.Add("J", false); MBIT.Add("P", true); break;
                default: MBIT.Add("J", true); MBIT.Add("P", true); break; //others
            }
            requiredChatacter = viewModels.RequiredCharacter;
        }
        public void updateMBITFormAnswers(MBITPreferenceViewModels viewModels)
        {
            MBITFormAnswers = viewModels;
        }
        
        public Dictionary<string,bool> getMBITPreferences()
        {
            return MBIT;
        }
        public string getRequiredChatacter()
        {
            return requiredChatacter;
        }
        public MBITPreferenceViewModels getMBITFormAnswers()
        {
            return MBITFormAnswers;
        }
        
        public void clearMBITPreferenceRepository()
        {
            MBIT.Clear();
            requiredChatacter = "";
            MBITFormAnswers = new MBITPreferenceViewModels();
        }
    }
}