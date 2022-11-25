using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace final_project.Repositories
{
    public class ChosenTabsRepository
    {
        //到底是誰 bug de不出來，偷吃步，然後還把檔案放repository，堅持不是viewmodel
        private static List<string> workTabIDList = new List<string>(); //已經選好的
        private static List<string> skillTabIDList = new List<string>();
        private static List<string> requiredSkillTabIDList = new List<string>();
        private static List<string> workTabIDSearchList = new List<string>(); //搜出來的
        private static List<string> skillTabIDSearchList = new List<string>();
        private static string WorkTabSearchString = "";
        private static string SkillTabSearchString = "";

        /*名詞解釋一下：
         搜出來，按下確認時，被勾選的checkbox : selectedSkills
         按下search，搜尋出來的checkbox : skillTabIDSearchList
         已經選好的 : skillTabIDList
         必要技能：requiredSkillTabIDList*/

        #region workTab(eventTab) repository region
        //清空所有workTab的資源，在CreateEvent一開啟頁面時使用 (沒有HttpPost的ActionResult)
        public void clearWorkTabRepository()
        {
            if (workTabIDList != null)
            {
                workTabIDList.Clear();
            }
            workTabIDSearchList.Clear();
            WorkTabSearchString = "";
        }

        //【2 overloads】
        // 新增一個「新的標籤」存入DB時使用，以下此段負責存入「已選擇標籤」List
        public void updateWorkTabIDList(string newWorkTabID)
        {
            workTabIDList.Add(newWorkTabID);
        }

        //搜尋出多個標籤checkbox，把勾選/取消勾選的 checkbox 儲存/刪掉於「已選擇標籤」List
        public void updateWorkTabIDList(List<string> selectedWorks)
        {

            if (selectedWorks != null)
            {
                foreach (string workTabID in workTabIDSearchList)
                {
                    //勾選了，但是還沒有存進「已選擇標籤」
                    if (selectedWorks.Contains(workTabID) && !workTabIDList.Contains(workTabID))
                    {
                        workTabIDList.Add(workTabID);
                    }
                    //已經在「已選擇標籤」裡面了，但是user取消勾選
                    if (!selectedWorks.Contains(workTabID) && workTabIDList.Contains(workTabID))
                    {
                        workTabIDList.Remove(workTabID);
                    }
                }
            }
            else
            {
                foreach (string workTabID in workTabIDSearchList)
                {
                    if (workTabIDList.Contains(workTabID))
                    {
                        workTabIDList.Remove(workTabID);
                    }
                }
            }
        }

        public List<string> getWorkTabIDList()
        {
            return workTabIDList;
        }

        //先存一份搜出了什麼，到時候update才有東西比對
        public void setworkTabIDSearchList(List<string> searchList)
        {
            workTabIDSearchList = searchList;
        }

        //for按下「是，新曾這個標籤」時使用
        //必須在這裡另外存一個searchstring，是考量如果user在按下「是，新曾這個標籤」BUTTON時，
        //已把searchstring textbox裡的東西刪掉，會抓不到searchstring 就GG了
        public string getWorkTabSearchString()
        {
            return WorkTabSearchString;
        }
        public void setWorkTabSearchString(string searchString)
        {
            WorkTabSearchString = searchString;
        }

        #endregion

        #region skillTab repository region
        //清空所有skillTab的資源，在CreateEvent一開啟頁面時使用 (沒有HttpPost的ActionResult)
        public void clearSkillTabRepository()
        {
            skillTabIDList.Clear();
            if (requiredSkillTabIDList != null)
            {
                requiredSkillTabIDList.Clear();
            }
            skillTabIDSearchList.Clear();
            SkillTabSearchString = "";
        }

        //【2 overloads】
        // 新增一個「新的標籤」存入DB時使用，以下此段負責存入「已選擇標籤」List
        public void updateSkillTabIDList(string newSkillTabID)
        {
            skillTabIDList.Add(newSkillTabID);
        }

        //搜尋出多個標籤checkbox，把勾選/取消勾選的 checkbox 儲存/刪掉於「已選擇標籤」List
        public void updateSkillTabIDList(List<string> selectedSkills)
        {

            if (selectedSkills != null)
            {
                foreach (string skillTabID in skillTabIDSearchList)
                {
                    //勾選了，但是還沒有存進「已選擇標籤」
                    if (selectedSkills.Contains(skillTabID) && !skillTabIDList.Contains(skillTabID))
                    {
                        skillTabIDList.Add(skillTabID);
                    }
                    //已經在「已選擇標籤」裡面了，但是user取消勾選
                    if (!selectedSkills.Contains(skillTabID) && skillTabIDList.Contains(skillTabID))
                    {
                        skillTabIDList.Remove(skillTabID);
                    }
                }
            }
            else
            {
                foreach (string skillTabID in skillTabIDSearchList)
                {
                    if (skillTabIDList.Contains(skillTabID))
                    {
                        skillTabIDList.Remove(skillTabID);
                    }
                }
            }
        }

        public List<string> getSkillTabIDList()
        {
            return skillTabIDList;
        }

        //先存一份搜出了什麼，到時候update才有東西比對
        public void setskillTabIDSearchList(List<string> searchList)
        {
            skillTabIDSearchList = searchList;
        }

        //for按下「是，新曾這個標籤」時使用
        //必須在這裡另外存一個searchstring，是考量如果user在按下「是，新曾這個標籤」BUTTON時，
        //已把searchstring textbox裡的東西刪掉，會抓不到searchstring 就GG了
        public string getSkillTabSearchString()
        {
            return SkillTabSearchString;
        }
        public void setSkillTabSearchString(string searchString)
        {
            SkillTabSearchString = searchString;
        }

        #endregion

        public void clearRequiredSkillTabIDList()
        {
            if (requiredSkillTabIDList != null)
            {
                requiredSkillTabIDList.Clear();
            }
        }
        public List<string> getRequiredSkillTabIDList()
        {
            return requiredSkillTabIDList;
        }
        public void updateRequiredSkillTabIDList(List<string> requiredSkill)
        {
           requiredSkillTabIDList = requiredSkill;
        }
    }
}