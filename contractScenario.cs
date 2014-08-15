﻿#region license
/*The MIT License (MIT)
Contract Scenario - Scenario Module To Store Save Specific Info

Copyright (c) 2014 DMagic

KSP Plugin Framework by TriggerAu, 2014: http://forum.kerbalspaceprogram.com/threads/66503-KSP-Plugin-Framework

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using UnityEngine;
using Contracts;

namespace ContractsWindow
{

	[KSPScenario(ScenarioCreationOptions.AddToExistingCareerGames | ScenarioCreationOptions.AddToNewCareerGames, GameScenes.FLIGHT, GameScenes.EDITOR, GameScenes.TRACKSTATION, GameScenes.SPACECENTER)] 
	class contractScenario: ScenarioModule
	{

		internal static contractScenario Instance
		{
			get
			{
				Game g = HighLogic.CurrentGame;
				try
				{
					var mod = g.scenarios.FirstOrDefault(m => m.moduleName == typeof(contractScenario).Name);
					if (mod != null)
						return (contractScenario)mod.moduleRef;
					else
						return null;
				}
				catch(Exception e)
				{
					print(string.Format("[Contracts+] Could not find Contracts Window Scenario Module: {0}", e));
					return null;
				}
			}
			private set { }
		}

		[KSPField(isPersistant = true)]
		public bool loaded = false;

		internal List<contractContainer> showList = new List<contractContainer>();
		internal List<contractContainer> hiddenList = new List<contractContainer>();

		//initialize data for each gamescene
		internal int[] orderMode = new int[4];
		internal int[] windowMode = new int[4];
		internal int[] showHideMode = new int[4];
		internal List<Guid> showIDList = new List<Guid>();
		internal List<Guid> hiddenIDList = new List<Guid>();
		internal bool[] windowVisible = new bool[4];
		internal bool[] toolTips = new bool[4] { true, true, true, true };
		internal sortClass[] sortMode = new sortClass[4] { sortClass.Difficulty, sortClass.Difficulty, sortClass.Difficulty, sortClass.Difficulty };
		internal Rect[] windowRects = new Rect[4] { new Rect(50, 80, 250, 300), new Rect(50, 80, 250, 300), new Rect(50, 80, 250, 300), new Rect(50, 80, 250, 300) };
		private int[] windowPos = new int[16] { 50, 80, 250, 300, 50, 80, 250, 300, 50, 80, 250, 300, 50, 80, 250, 300 };

		internal contractsWindow cWin;

		public override void OnLoad(ConfigNode node)
		{
			showList.Clear();
			hiddenList.Clear();
			ConfigNode scenes = node.GetNode("Contracts_Window_Parameters");
			if (scenes != null)
			{
				showIDList = stringSplitGuid(scenes.GetValue("DefaultListID"));
				hiddenIDList = stringSplitGuid(scenes.GetValue("HiddenListID"));
				showHideMode = stringSplit(scenes.GetValue("ShowListMode"));
				windowMode = stringSplit(scenes.GetValue("WindowMode"));
				orderMode = stringSplit(scenes.GetValue("SortOrder"));
				int[] sort = stringSplit(scenes.GetValue("SortMode"));
				sortMode = new sortClass[4] { (sortClass)sort[0], (sortClass)sort[1], (sortClass)sort[2], (sortClass)sort[3] };
				windowPos = stringSplit(scenes.GetValue("WindowPosition"));
				windowVisible = stringSplitBool(scenes.GetValue("WindowVisible"));
				toolTips = stringSplitBool(scenes.GetValue("ToolTips"));
				int[] winPos = new int[4] {windowPos[4 * currentScene(HighLogic.LoadedScene)], windowPos[(4 * currentScene(HighLogic.LoadedScene)) + 1], windowPos[(4 * currentScene(HighLogic.LoadedScene)) + 2], windowPos[(4 * currentScene(HighLogic.LoadedScene)) + 3]};
				loadWindow(winPos);
				loaded = true;
			}
			cWin = gameObject.AddComponent<contractsWindow>();
		}

		public override void OnSave(ConfigNode node)
		{
			Guid[] showListID = contractID(showList);
			Guid[] hiddenListID = contractID(hiddenList);
			saveWindow(windowRects[currentScene(HighLogic.LoadedScene)]);

			ConfigNode scenes = new ConfigNode("Contracts_Window_Parameters");
			scenes.AddValue("DefaultListID", stringConcat(showListID, showListID.Length));
			scenes.AddValue("HiddenListID", stringConcat(hiddenListID, hiddenListID.Length));
			scenes.AddValue("ShowListMode", stringConcat(showHideMode, showHideMode.Length));
			scenes.AddValue("WindowMode", stringConcat(windowMode, windowMode.Length));
			scenes.AddValue("SortOrder", stringConcat(orderMode, orderMode.Length));
			int[] sort = new int[4] { (int)sortMode[0], (int)sortMode[1], (int)sortMode[2], (int)sortMode[3] };
			scenes.AddValue("SortMode", stringConcat(sort, sort.Length));
			scenes.AddValue("WindowPosition", stringConcat(windowPos, windowPos.Length));
			scenes.AddValue("WindowVisible", stringConcat(windowVisible, windowVisible.Length));
			scenes.AddValue("ToolTips", stringConcat(toolTips, toolTips.Length));
			node.AddNode(scenes);
		}

		#region utilities

		internal static int currentScene(GameScenes s)
		{
			switch (s)
			{
				case GameScenes.FLIGHT:
					return 0;
				case GameScenes.EDITOR:
					return 1;
				case GameScenes.SPACECENTER:
					return 2;
				case GameScenes.TRACKSTATION:
					return 3;
				default:
					return 3;
			}
		}

		private string stringConcat(int[] source, int i)
		{
			if (i == 0)
				return "";
			string[] s = new string[i];
			for (int j = 0; j < i; j++)
			{
				s[j] = source[j].ToString() + ",";
			}
			return string.Concat(s).TrimEnd(',');
		}

		private string stringConcat(Guid[] source, int i)
		{
			if (i == 0)
				return "";
			string[] s = new string[i];
			for (int j = 0; j < i; j++)
			{
				s[j] = source[j].ToString() + ",";
			}
			return string.Concat(s).TrimEnd(',');
		}

		private string stringConcat(bool[] source, int i)
		{
			if (i == 0)
				return "";
			string[] s = new string[i];
			for (int j = 0; j < i; j++)
			{
				s[j] = source[j].ToString() + ",";
			}
			return string.Concat(s).TrimEnd(',');
		}

		private int[] stringSplit(string source)
		{
			string[] s = source.Split(',');
			int[] i = new int[s.Length];
			for (int j = 0; j < s.Length; j++)
			{
				i[j] = int.Parse(s[j]);
			}
			return i;
		}

		private List<Guid> stringSplitGuid(string source)
		{
			if (source == "")
				return new List<Guid>();
			string[] s = source.Split(',');
			List<Guid> i = new List<Guid>();
			for (int j = 0; j < s.Length; j++)
			{
				i.Add(new Guid(s[j]));
			}
			return i;
		}

		private bool[] stringSplitBool(string source)
		{
			string[] s = source.Split(',');
			bool[] b = new bool[s.Length];
			for (int j = 0; j < s.Length; j++)
			{
				b[j] = bool.Parse(s[j]);
			}
			return b;
		}

		private Guid[] contractID(List<contractContainer> c)
		{
			Guid[] id = new Guid[c.Count];
			for (int j = 0; j < c.Count; j++)
				id[j] = c[j].contract.ContractGuid;
			return id;
		}

		internal void addToList(Guid ID, List<contractContainer> cL)
		{
			Contract c = ContractSystem.Instance.Contracts.FirstOrDefault(n => n.ContractGuid == ID);
			if (c != null)
				cL.Add(new contractContainer(c));
		}

		#endregion

		#region save/load methods

		private void saveWindow(Rect source)
		{
			int i = currentScene(HighLogic.LoadedScene);
			windowPos[i * 4] = (int)source.x;
			windowPos[(i * 4) + 1] = (int)source.y;
			windowPos[(i * 4) + 2] = (int)source.width;
			windowPos[(i * 4) + 3] = (int)source.height;
		}

		private void loadWindow(int[] window)
		{
			windowRects[currentScene(HighLogic.LoadedScene)] = new Rect(window[0], window[1], window[2], window[3]);
		}

		#endregion

	}
}
